using System;
using UnityEngine;

public class RotateTrap : MonoBehaviour
{
    [Header("System On/Off")]
    [SerializeField] bool isRotating = false;
    [Space]

    [SerializeField, Range(-350,350)] float rotationSpeed = -200f;
   
    [Tooltip("Select the axis of rotation")]
    public enum RotationAxis { X, Y, Z };
    [SerializeField] RotationAxis rotationAxis;

    private void FixedUpdate()
    {
        if (isRotating) ChooseRotate();
    }

    //private void OnEnable()
    //{
    //    if (isRotating) ChooseRotate();
    //}

    private void ChooseRotate()
    {
        switch (rotationAxis)
        {
            case RotationAxis.X:
                RotateX();
                break;
            case RotationAxis.Y:
                RotateY();
                break;
            case RotationAxis.Z:
                RotateZ();
                break;
        }
    }

    private void RotateX()
    {
        transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
    }

    private void RotateY()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    private void RotateZ()
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        //transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }

    public void StopRotation()
    {
          isRotating = false;
    }

    public void StartRotation()
    {
        isRotating = true;
    }
}
