using UnityEngine;

//[RequireComponent(typeof(Animator))]
public class TestAnimation : MonoBehaviour
{
    Animator animator;
    float velocityZ = 0f;
    float velocityX = 0f;

    public float acceleration = 2f;
    public float deceleration = 2f;

    public float maxWalkVelocity = 0.5f;
    public float maxRunVelocity = 2f;

    //increase performance by caching the hash
    int VelocityZHash;
    int VelocityXHash;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        VelocityZHash = Animator.StringToHash("velocityZ");
        VelocityXHash = Animator.StringToHash("velocityX");
    }

    private void FixedUpdate()
    {
       Inputs();
    }

    private void Inputs()
    {
        bool forwardPressed = Input.GetKey(KeyCode.W);
        bool leftPressed = Input.GetKey(KeyCode.A);
        bool rightPressed = Input.GetKey(KeyCode.D);
        bool backPressed = Input.GetKey(KeyCode.S);

        bool runPressed = Input.GetKey(KeyCode.LeftShift);

        //set current max velocity
        float currentMaxVelocity = runPressed ? maxRunVelocity : maxWalkVelocity;

        //handle current velocity
        ChangeVelocity(forwardPressed, leftPressed, rightPressed, runPressed, backPressed, currentMaxVelocity);
        LockOrResetVelocity(forwardPressed, leftPressed, rightPressed, runPressed, backPressed, currentMaxVelocity);

        animator.SetFloat(VelocityZHash, velocityZ);
        animator.SetFloat(VelocityXHash, velocityX);
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

}
