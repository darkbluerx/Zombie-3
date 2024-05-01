using UnityEngine;
using System;

[RequireComponent(typeof(CharacterController))]
public class TopDownPlayerController : MonoBehaviour
{
    public static TopDownPlayerController Instance { get; private set; } // Singleton

    public static event Action OnStaminaChanged; //Invoke the event to update the StaminaBar UI-> UIManaker script
    public static event Action OnCanWalk; //play walk animation -> PlayerAnimation script

    [Header("Gun Barrel")]
    public Transform _firePoint; // The gun barrel

    [Header("Inputs")]
    [SerializeField] KeyCode runKey = KeyCode.LeftShift;

    [Header("Movement Speed")]
    [SerializeField] float currentSpeed;
    [Space]
    [SerializeField] float walkSpeed = 3f;
    [SerializeField] float runSpeed = 5f; 
    [Space]

    [Header("Layers")]
    [SerializeField] LayerMask obtacleMask; // Assign the layer containing your obstacles, obtacle layer
    [SerializeField] LayerMask aiming; // aiming layer
    [SerializeField] LayerMask groundMask; // ground layer

    Camera mainCamera; // Assign the main camera in the inspector.
    CharacterController characterController; // Replace Rigidbody with CharacterController.
    Vector3 moveDirection;

    [Header("Atributes")]
    float gravity = -9.81f;
    float maxStamina = 100f;
    float maxStaminaRegen = 5f;
    float currentmaxStamina;

    bool runPressed;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        if (Instance != null)
        {
            Debug.LogError("There can only be one TopDownPlayerControler");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
       // If you haven't assigned the main camera, try to find it in the scene
        if (mainCamera == null) mainCamera = Camera.main;

        currentmaxStamina = maxStamina; // Set the current maxStamina to the max maxStamina = 100
    }

    private void Update()
    {
        Aim(); // Aim the gun barrel at the mouse position
        HandleMovementInput(); // Get input for movement
       
        Run(walkSpeed,runSpeed);

        // Move the player
        Vector3 move = moveDirection * currentSpeed * Time.deltaTime;

        //Check for collisions with obstacles, if there are no obstacles, move the player
        if (characterController != null)
        {
            if (!Physics.SphereCast(transform.position, characterController.radius, move.normalized, out RaycastHit hit, move.magnitude, obtacleMask))
            {
                characterController.Move(move);
            }
        }
    }

    private void Aim()
    {
        var(success, position) = GetMousePosition();

        if (success)
        {
            // Turn the player towards the mouse
            Vector3 direction = position - transform.position;
            direction.y = 0;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Euler(0f, lookRotation.eulerAngles.y, 0f);

            // Turn the gun in the shooting direction
            direction = position - _firePoint.position; // Set the direction from the gun barrel to the point indicated by the mouse
            lookRotation = Quaternion.LookRotation(direction);
            _firePoint.rotation = Quaternion.Euler(0f, lookRotation.eulerAngles.y, 0f);
        }
    }

    private (bool succes, Vector3 position) GetMousePosition()
    {
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        // Add a transparent layer to a certain height from the ground
        Vector3 raycastOrigin = new Vector3(ray.origin.x, ray.origin.y, ray.origin.z);

        if (Physics.Raycast(raycastOrigin, ray.direction, out hitInfo, Mathf.Infinity, aiming))
        {
            // Check if the player is not hit by the ray
            if(hitInfo.collider.gameObject != gameObject)
            {
                // Check if the enemy is not hit
                if(hitInfo.collider.gameObject.layer != LayerMask.NameToLayer("Enemy"))
                {
                    // Draw the raycast beam
                    Debug.DrawRay(raycastOrigin, ray.direction * hitInfo.distance, Color.green);
                    return (succes: true, position: hitInfo.point);
                }
            }
        }

        // Draw the raycast beam, if the ray does not hit anything
        Debug.DrawRay(raycastOrigin, ray.direction * 1000f, Color.red);
        return (succes: false, Position: Vector3.zero);
    }

    private void FixedUpdate()
    {
        ApplyGravity(); 
    }

    private void HandleMovementInput()
    {
        // Get input for movement
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        // Calculate movement direction
        moveDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;
    }

    private void ApplyGravity()
    {
        if(characterController != null && !characterController.isGrounded)
        {
            // Apply gravity
            Vector3 gravityVector = Vector3.up * gravity * Time.deltaTime;
            characterController.Move(gravityVector);
        }
    }

    public void Run(float newWalkSpeed, float newRunSpeed)
    {
        runPressed = Input.GetKey(runKey); // If the run key is pressed, set the run speed

        if (runPressed && currentmaxStamina > 0)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, newRunSpeed, 2f * Time.deltaTime); // movement speed increases over time
            currentmaxStamina -= 10f * Time.deltaTime;

            if (currentmaxStamina <= 0 || currentmaxStamina <=1)
            {
                currentmaxStamina = 0;
                currentSpeed = newWalkSpeed;
                runPressed = false;

               OnCanWalk?.Invoke();  //player can't run anymore and play walk animation -> PlayerAnimation script
            }

            // Invoke the event to update the StaminaBar UI-> UIManaker script
            OnStaminaChanged?.Invoke();
        }

        if (!runPressed)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, newWalkSpeed, 2f * Time.deltaTime);

            if (currentmaxStamina < maxStamina)
            {
                currentmaxStamina += maxStaminaRegen * Time.deltaTime;

                // Invoke the event to update the StaminaBar UI -> UIManaker script
                OnStaminaChanged?.Invoke();
            }
        }
    }

    public float GetCurrentMaxStamina()
    {
        return currentmaxStamina;
    }

    public float GetMaxStamina()
    {
        return maxStamina;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue; 
        Gizmos.DrawWireSphere(_firePoint.position, 0.1f); //Draw a wire sphere at the gun barrel
    }
}