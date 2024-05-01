using UnityEngine;
using weapon;

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    public static PlayerAnimation Instance { get; private set; } //Singleton

    [SerializeField] Animator animator;
    [Space]

    [SerializeField] TopDownPlayerController playerController;
    [SerializeField] Health health;

    int shootAnimationHash = Animator.StringToHash("isShoot");
    //increase performance by caching the hash
    int VelocityZHash = Animator.StringToHash("velocityZ"); //Take a reference to the animator float velocityZ
    int VelocityXHash = Animator.StringToHash("velocityX"); //Take a reference to the animator float velocityX

    float velocityZ = 0f;
    float velocityX = 0f;

    float acceleration = 2f;
    float deceleration = 2f;

    float maxWalkVelocity = 0.5f;
    float maxRunVelocity = 2f;

    float currentMaxVelocity;
    bool runPressed;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<TopDownPlayerController>();

        if (Instance != null)
        {
            Debug.LogError("There can only be one PlayerAnimation");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        //animator.SetBool("gun", true); //set the gun animation to true and it will play right "Gun Blend Tree"
    }

    private void OnEnable() //subscribe to events
    {
        Gun.OnShootAnimation += ShootAnimation;

        health.OnDeadEvent += Health_OnDeadEvent2;
        Gun.OnGetGun += HaveGun;
        TopDownPlayerController.OnCanWalk += CanWalk;

        //WeapongSwitching.OnGetGun += HaveGun;

        WeaponSwitching.OnGetGun += HaveGun;
    }

    private void FixedUpdate()
    {
        Inputs();
    }

    private void CanWalk()
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

        if(TopDownPlayerController.Instance.GetCurrentMaxStamina() < 1) //if stamina is less than 1, don't play the run animation
        {
            currentMaxVelocity = maxWalkVelocity;
            //TopDownPlayerController.Instance.SetCanRun();
        }

        //handle current velocity
        ChangeVelocity(forwardPressed, leftPressed, rightPressed, runPressed, backPressed, currentMaxVelocity);
        LockOrResetVelocity(forwardPressed, leftPressed, rightPressed, runPressed, backPressed, currentMaxVelocity);

       if(animator != null) animator.SetFloat(VelocityZHash, velocityZ);
       if (animator != null) animator.SetFloat(VelocityXHash, velocityX);
    }

    //handes acceleration and deceleration
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

    private void HaveGun(bool haveGun)
    {         
        if (haveGun && (animator != null))
        {
            animator.SetBool("gun", true);
            animator.SetBool("unarmed", false);
        }
        if (!haveGun && (animator != null))
        {
            animator.SetBool("unarmed", true);
            animator.SetBool("gun", false);
        }
    }

    private void Health_OnDeadEvent2()
    {
        if(animator != null)
        {
            animator.applyRootMotion = true; //stop the player from moving when dead
            animator.SetTrigger("isDead");
            animator.SetBool("isRunWithGun", false);
            animator.SetBool("isRunWithNothing", false);
        }   
    }

    public void ShootAnimation()
    {
       // int animationSpeed = 1;
     
        //animator.speed = 1 * Time.deltaTime;

        if (animator != null) animator.SetTrigger(shootAnimationHash);
       // animator.SetFloat("isShoot2", animationSpeed * Time.deltaTime);

       // animationSpeed = 0;
    }

    private void OnDisable()
    {
        health.OnDeadEvent += Health_OnDeadEvent2;
        Gun.OnShootAnimation -= ShootAnimation;
    }
}
