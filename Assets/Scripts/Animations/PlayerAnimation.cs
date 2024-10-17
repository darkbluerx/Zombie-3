using UnityEngine;
using weapon;

//The player's animations are controlled here

[RequireComponent(typeof(Animator))] //Require the animator component
public class PlayerAnimation : MonoBehaviour
{
    public static PlayerAnimation Instance { get; private set; } //Singleton

    [SerializeField] Animator animator;
    [Space]

    [SerializeField] TopDownPlayerController playerController;
    [SerializeField] Health health;

    int shootAnimationHash = Animator.StringToHash("isShoot"); //Take a reference to the animator trigger isShoot
    //increase performance by caching the hash
    int VelocityZHash = Animator.StringToHash("velocityZ"); //Take a reference to the animator float velocityZ
    int VelocityXHash = Animator.StringToHash("velocityX"); //Take a reference to the animator float velocityX

    float velocityZ = 0f; //The speed of the player in the Z axis
    float velocityX = 0f; //The speed of the player in the X axis

    float acceleration = 2f; //The acceleration of the player
    float deceleration = 2f; //The deceleration of the player

    float maxWalkVelocity = 0.5f; //The maximum speed of the player when walking
    float maxRunVelocity = 2f; //The maximum speed of the player when running

    float currentMaxVelocity; //The current maximum speed of the player
    bool runPressed;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<TopDownPlayerController>();

        if (Instance != null) //Singleton
        {
            Debug.LogError("There can only be one PlayerAnimation");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void OnEnable() //Subscribe to events
    {
        Gun.OnShootAnimation += Gun_ShootAnimation; //Call the ShootAnimation method when the OnShootAnimation event is triggered

        health.OnDeadEvent += Health_OnDeadEvent;
       
        TopDownPlayerController.OnCanWalk += TopDownPlayerController_CanWalk;

        Gun.OnGetGun += Gun_HaveGun;
        WeaponSwitching.OnGetGun += Gun_HaveGun;
    }

    private void FixedUpdate()
    {
        Inputs(); //Handle inputs
    }

    private void TopDownPlayerController_CanWalk()
    {
        currentMaxVelocity = maxWalkVelocity;
    }

    public float GetCurrentMaxVelocity()
    {
        return velocityX;
    }

    private void Inputs()
    {
        //Bools and Inputs
        bool forwardPressed = Input.GetKey(KeyCode.W);
        bool leftPressed = Input.GetKey(KeyCode.A);
        bool rightPressed = Input.GetKey(KeyCode.D);
        bool backPressed = Input.GetKey(KeyCode.S);

        runPressed = Input.GetKey(KeyCode.LeftShift);

        currentMaxVelocity = runPressed ? maxRunVelocity : maxWalkVelocity;

        if(TopDownPlayerController.Instance.GetCurrentMaxStamina() < 1) //If stamina is less than 1, don't play the run animation
        {
            currentMaxVelocity = maxWalkVelocity;
        }

        //Handle current velocity
        ChangeVelocity(forwardPressed, leftPressed, rightPressed, runPressed, backPressed, currentMaxVelocity);
        LockOrResetVelocity(forwardPressed, leftPressed, rightPressed, runPressed, backPressed, currentMaxVelocity);

       if(animator != null) animator.SetFloat(VelocityZHash, velocityZ); //Set the float velocityZ in the animator
       if (animator != null) animator.SetFloat(VelocityXHash, velocityX);
    }

    //Handes acceleration and deceleration
    private void ChangeVelocity(bool forwardPressed, bool leftPressed, bool rightPressed, bool runPressed, bool backPressed, float currentMaxVelocity)
    {
        // Walk forward
        if (forwardPressed && velocityZ < currentMaxVelocity)
        {
            velocityZ += Time.deltaTime * acceleration;
        }
        // Walk backward
        else if (backPressed && velocityZ > -currentMaxVelocity)
        {
            velocityZ += Time.deltaTime * acceleration;
        }

        if (leftPressed && velocityX > -currentMaxVelocity)
        {
            velocityX -= Time.deltaTime * acceleration;
        }
        if (rightPressed && velocityX < currentMaxVelocity)
        {
            velocityX += Time.deltaTime * acceleration;
        }

        // Decrease velocityZ when not moving forward or backward
        if (!forwardPressed && !backPressed && velocityZ != 0.0f)
        {
            float decelerationValue = (velocityZ > 0.0f) ? deceleration : -deceleration;
            velocityZ -= Time.deltaTime * decelerationValue;
        }

        if (!leftPressed && velocityX < 0.0f)
        {
            velocityX += Time.deltaTime * deceleration;
        }

        // Decrease velocityX if right is not pressed and velocityX > 0
        if (!rightPressed && velocityX > 0.0f)
        {
            velocityX -= Time.deltaTime * deceleration;
        }
    }

    private void LockOrResetVelocity(bool forwardPressed, bool leftPressed, bool rightPressed, bool runPressed, bool backPressed, float currentMaxVelocity)
    {
        // Reset velocityZ
        if (!forwardPressed && !backPressed && velocityZ != 0.0f)
        {
            float resetValue = (velocityZ > 0.0f) ? -deceleration : deceleration;
            velocityZ += Time.deltaTime * resetValue;
        }
        else if (!forwardPressed && velocityZ < 0.0f)
        {
            velocityZ = 0f;
        }

        // Reset velocityX
        if (!leftPressed && !rightPressed && velocityX != 0.0f && Mathf.Abs(velocityX) < 0.05f)
        {
            velocityX = 0f;
        }

        // Lock forward
        if ((forwardPressed || backPressed) && runPressed && Mathf.Abs(velocityZ) > currentMaxVelocity)
        {
            velocityZ = Mathf.Sign(velocityZ) * currentMaxVelocity;
        }
        // Decelerate to the max walk velocity if the player is not holding shift
        else if ((forwardPressed || backPressed) && Mathf.Abs(velocityZ) > currentMaxVelocity)
        {
            float decelerationValue = (velocityZ > 0.0f) ? deceleration : -deceleration;
            velocityZ = Mathf.MoveTowards(velocityZ, Mathf.Sign(velocityZ) * currentMaxVelocity, Time.deltaTime * decelerationValue);
        }
        // Round to the current velocity if within offset
        else if ((forwardPressed || backPressed) && Mathf.Abs(velocityZ) < currentMaxVelocity && Mathf.Abs(velocityZ) > (currentMaxVelocity - 0.05f))
        {
            velocityZ += Time.deltaTime * acceleration * Mathf.Sign(velocityZ);
        }
    }

    private void Gun_HaveGun(bool Gun_HaveGun)
    {         
        if (Gun_HaveGun && (animator != null)) //If the player has a gun, play the gun animation
        {
            animator.SetBool("gun", true);
            animator.SetBool("unarmed", false);
        }
        if (!Gun_HaveGun && (animator != null)) //If the player doesn't have a gun, play the unarmed animation
        {
            animator.SetBool("unarmed", true);
            animator.SetBool("gun", false);
        }
    }

    private void Health_OnDeadEvent() //When the player dies play the dead animation
    { 
        if(animator != null)
        {
            animator.applyRootMotion = true; //Stop the player from moving when dead
            animator.SetTrigger("isDead");
            animator.SetBool("isRunWithGun", false);
            animator.SetBool("isRunWithNothing", false);
        }   
    }

    public void Gun_ShootAnimation() //When the player shoots, play the shoot animation
    {
        if (animator != null) animator.SetTrigger(shootAnimationHash);
    }

    private void OnDisable() //Unsubscribe from events
    {
        Gun.OnShootAnimation -= Gun_ShootAnimation;

        health.OnDeadEvent += Health_OnDeadEvent;

        TopDownPlayerController.OnCanWalk -= TopDownPlayerController_CanWalk;

        Gun.OnGetGun -= Gun_HaveGun;
        WeaponSwitching.OnGetGun -= Gun_HaveGun;
    }
}
