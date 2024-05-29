using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    [SerializeField] float smoothSpeed = 0.125f;
    [SerializeField] Vector3 offset;
    [SerializeField] float cameraXRotation = 45f;

    ObjectFader _fader;

    private void Awake()
    {
        if (playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void LateUpdate()
    {
        if (playerTransform == null) return;
    
        // Calculate the target location based on the player's location and offset
        Vector3 targetPosition = playerTransform.position + offset;

        // The Lerp method softens the movement of the camera
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);

        //Zero along. angle around the camera
        transform.rotation = Quaternion.Euler(cameraXRotation, 0f, 0f);
    }
}
