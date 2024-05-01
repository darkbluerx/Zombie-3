using System;
using System.Collections;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    [Header("How far you can open the door")]
    [SerializeField] float interactionDistance = 2f;
    [Space]

    //[SerializeField] float doorAngle = 90f;
    Transform doorTranform;


    [Header("Door animation speeds")]
    [SerializeField, Range(10f, 800f)] float doorOpenSpeed = 200f;
    [SerializeField, Range(10f, 800f)] float doorCloseSpeed = 200f;
    [Space]

    [Header("Tooltip"), Tooltip("Can open door pressing F, if it x")]
    [SerializeField] bool ButtonDoor = false;

    bool isOpen = false;


    [SerializeField]

    private void Start()
    {
        doorTranform = GetComponent<Transform>();

        //doorTranform.rotation = Quaternion.Euler(0, doorAngle, 0);

        StartCoroutine(DoorAnimationClose());
    }

    private void Update()
    {
        if (ButtonDoor == true)
        {
            if (Input.GetKeyDown(KeyCode.F) && IsPlayerNearby())
            {
                ToggleDoor();
            }
        }
    }

    private bool IsPlayerNearby()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);

            return distance <= interactionDistance;
        }
        return false;
    }

    public void Open()
    {
        if (!isOpen) StartCoroutine(DoorAnimationOpen());
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;
        if (isOpen)
        {
            StartCoroutine(DoorAnimationOpen());
            Debug.Log("Door Opened");
        }
        else
        {
            StartCoroutine(DoorAnimationClose());
            Debug.Log("Door Closed");
        }
    }

    //ovi on auki -90 asteen kulmassa

    IEnumerator DoorAnimationOpen()
    {
        isOpen = true;
   
        // tässä vaiheessa ovi on -1 asteen kulmassa
        float orginalYangle = transform.rotation.eulerAngles.y;

        while (transform.rotation.eulerAngles.y > orginalYangle + 91f) // viimeinen luku on kuinka plajon halutaan avata ovi
        {
            transform.Rotate(Vector3.up * Time.deltaTime * doorCloseSpeed);
            yield return null;
        }
    }

    IEnumerator DoorAnimationClose()
    {
        //door is open 90 angle
        float orginalYangle = transform.rotation.eulerAngles.y; //90 is the orginal angle of the door when door is open

        while (transform.rotation.eulerAngles.y < orginalYangle + 90) //ha
        {
            transform.Rotate(Vector3.down * Time.deltaTime * doorOpenSpeed);
            yield return null;
        }
    }
}
