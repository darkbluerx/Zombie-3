using UnityEngine;

/*
Here we do camera tracking with the player and we play 
the transparency of the walls when we are behind the walls
*/
public class CameraFollow : MonoBehaviour
{
    [Tooltip("Automatically added player object")]
    [SerializeField] Transform playerTransform;
    [Space]

    [Header("Camera Settings")]
    [Tooltip("The speed of the camera")]
    [SerializeField] float smoothSpeed = 0.125f;

    [Tooltip("The offset of the camera")]
    [SerializeField] Vector3 offset;

    [Tooltip("The rotation of the camera")]
    [SerializeField] float cameraRotationX = 45f;

    ObjectFader _fader; //Added ObjectFader variable

    private void Awake()
    {
        if (playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform; //Automatically added player object
        }
    }

    private void FixedUpdate()
    {
        CameraTracking();
        ControlObjectFading();
    }

    private void CameraTracking()
    {
        if (playerTransform == null) return;

        // Calculate the target location based on the player's location and offset
        Vector3 targetPosition = playerTransform.position + offset;

        // The Lerp method softens the movement of the camera
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);

        //Zero along. angle around the camera
        transform.rotation = Quaternion.Euler(cameraRotationX, 0f, 0f);
    }

    private void ControlObjectFading()
    {
        if (playerTransform != null)
        {
            Vector3 direction = playerTransform.transform.position - transform.position;

            //Create a new LayerMask that contains the LayerMask.IgnoreRaycast layer
            LayerMask layerMask = 1 << LayerMask.NameToLayer("Ignore Raycast");

            //Add LayerMask.IgnoreRaycast layer to LayerMask
            layerMask |= 1 << LayerMask.NameToLayer("Aiming");

            Ray ray = new Ray(transform.position, direction);
            RaycastHit hit;

            //Use Physics.Raycast method and give LayerMask as parameter
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~layerMask))
            {
                if (hit.collider == null) return;

                //Check if we hit the "Aiming" layer
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Aiming"))
                {
                    //Make the necessary actions here when you hit the "Aiming" layer
                    return;
                }

                if (hit.collider.gameObject == playerTransform)
                {
                    if (_fader != null)
                    {
                        _fader.DoFade = false;
                        _fader.ResetFade(); //Added ResetFade call here
                    }

                    //Debug.DrawRay(ray.origin, ray.direction * 1000f, Color.red);
                }
                else
                {
                    _fader = hit.collider.gameObject.GetComponent<ObjectFader>();
                    if (_fader != null) _fader.DoFade = true;

                    //Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green);
                }
            }
        }
    }
}
