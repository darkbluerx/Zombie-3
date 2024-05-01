using System;
using System.Collections;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [Header("System On/Off")]
    [SerializeField] bool rotate = true;
    [Space]

    bool isRotating = false;

    [Header("Time in seconds required for rotation.")]
    [SerializeField, Range(0.1f, 10f)] float rotationDuration = 3.0f; // Time in seconds required for a 180-degree rotation.
    [Space]

    [Header("Rotation Controls")]
    [SerializeField, Range(1,360)] float rotateAngle = 180f; // How many degrees to rotate at a time.
    [SerializeField, Range(-1, -360)] float negativeRotateAngle = -180f; // How many degrees to rotate at a time.
    [Space]

    [SerializeField, Range(0.1f, 10f)] float stoppingTime = 0.2f; // How many seconds to wait before rotating back.

    private void OnEnable()
    {
        if (rotate) StartCoroutine(RotateContinuously());
    }

    private IEnumerator RotateContinuously()
    {
        while (rotate)
        {
            if (!isRotating)
            {
                isRotating = true;
                StartCoroutine(RotateObject90Degrees());
            }
            yield return null;
        }
    }

    private IEnumerator RotateObject90Degrees()
    {
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = transform.rotation * Quaternion.Euler(0, 0, rotateAngle);
        float elapsedTime = 0.0f;

        while (elapsedTime < rotationDuration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / rotationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure that exactly a 180-degree angle is achieved.
        transform.rotation = targetRotation;

        // Wait for a moment in place.
        yield return new WaitForSeconds(stoppingTime);

        // Rotate back -180 degrees.
        startRotation = transform.rotation;
        targetRotation = transform.rotation * Quaternion.Euler(0, 0, negativeRotateAngle);
        elapsedTime = 0.0f;

        while (elapsedTime < rotationDuration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / rotationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure that exactly a -90-degree angle is achieved.
        transform.rotation = targetRotation;

        isRotating = false;
    }

    public void StopRotation()
    {
        rotate = false;
        if (!rotate) StopCoroutine(RotateContinuously());
    }

    public void StartRotation()
    {
        rotate = true;
        if (rotate) StartCoroutine(RotateContinuously());
    }
}
