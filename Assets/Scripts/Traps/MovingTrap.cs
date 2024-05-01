using System;
using System.Collections;
using UnityEngine;

public class MovingTrap : MonoBehaviour
{
    [Header("System On/Off")]
    [SerializeField] bool isWork = false;
    [Space]

    [Header("The amount of movement")]
    [SerializeField, Range(0.1f, 10f)] float movingHeight = 1.5f;
    [Space]

    [SerializeField, Range(0.1f, 10f)] float movingSpeed = 0.5f;

    public enum MovingAxis { X, UpY, DownY, Z };
    [SerializeField] MovingAxis movingAxis = MovingAxis.X;

    public void ChooseAxis()
    {
        switch (movingAxis)
        {
            case MovingAxis.X:
                
                StartCoroutine(MovesTowards_X());
                break;
            case MovingAxis.UpY:
                StartCoroutine(MovesTowards_UpY());
                break;
            case MovingAxis.DownY:
                StartCoroutine(MovesTowards_DownY());
                break;
            case MovingAxis.Z:
                StartCoroutine(MovesTowards_Z());
                break;
        }
    }

    public void StopTrap()
    {
        isWork = true;
        if (isWork) StartCoroutine(TheTrapStopsAnimation());
    }

    public void MovingDown()
    {
        StartCoroutine(MovesTowards_DownY());
    }

    public IEnumerator MovesTowards_X()
    {
        //isWork = true;
        float originalXpos = transform.position.x; // Save the original position
        while (transform.position.x < originalXpos + movingHeight)
        {
            transform.Translate(-Vector3.right * Time.deltaTime * movingSpeed); // Move the object to the right, x axis
            yield return null;
        }
    }

    public IEnumerator MovesTowards_UpY()
    {
        //isWork = true;
        float originalYpos = transform.position.y;
        while (transform.position.y < originalYpos + movingHeight)
        {
            transform.Translate(Vector3.up * Time.deltaTime * movingSpeed); // Move the object up, y axis
            yield return null;
        }
    }

    public IEnumerator MovesTowards_DownY()
    {
        //isWork = true;
        float originalYpos = transform.position.y;
        while (transform.position.y > originalYpos + -movingHeight)
        {
            transform.Translate(Vector3.down * Time.deltaTime * movingSpeed); // Move the object down, y axis
            yield return null;
        }
    }

    public IEnumerator MovesTowards_Z()
    {
        //isWork = true;
        float originalZpos = transform.position.z;
        while (transform.position.z < originalZpos + movingHeight)
        {
            transform.Translate(-Vector3.forward * Time.deltaTime * movingSpeed); // Move the object forward, z axis
            yield return null;
        }
    }

    public IEnumerator TheTrapStopsAnimation()
    {
        float originalYpos = transform.position.y + -2;

        while (transform.position.y > originalYpos + 0f)
        {
            transform.Translate(Vector3.down * Time.deltaTime / 2);
            yield return null;
        }
    }
}

