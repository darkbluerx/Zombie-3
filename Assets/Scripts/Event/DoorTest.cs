using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DoorTest : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioEvent doorOpenAudioEvent;
    [Space]

    [Header("How far you can open the door")]
    [SerializeField] float interactionDistance = 2f;
    [Space]

    [Header("Door animation speeds")]
    [SerializeField, Range(10f, 800f)] float doorOpenSpeed = 200f;
    [SerializeField, Range(10f, 800f)] float doorCloseSpeed = 200f;
    [Space]

    [Header("Tooltip"), Tooltip("Can open door pressing F")]
    [SerializeField] bool canOpenDoor = false;

    bool isOpen = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        StartCoroutine(DoorAnimationOpen());
    }

    private void Update()
    {
        if(canOpenDoor == true)
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

    public void ToggleDoor()
    {
        doorOpenAudioEvent.Play(GetComponent<AudioSource>());

        isOpen = !isOpen;
        if (isOpen)
        {
            StartCoroutine(DoorAnimationOpen());
            //Debug.Log("Door Opened");
        }
        else
        {
            StartCoroutine(DoorAnimationClose());
            //Debug.Log("Door Closed");
        }
    }

   public IEnumerator DoorAnimationOpen()
   {
        isOpen = true;
        float orginalYangle = transform.rotation.eulerAngles.y; //0 is the orginal angle of the door when door is open

        while (transform.rotation.eulerAngles.y < orginalYangle + 90) //0 + 90, sillä aikaa kun oven y kulma on pienempi kuin 90,
        {
            // ovi pyörii y akselin ympäri kohti 90 astetta. pyörii kellon suuntaisesti
            transform.Rotate(Vector3.up * Time.deltaTime * doorOpenSpeed); 
            yield return null;
        }
    }

    IEnumerator DoorAnimationClose()
    {
        //isOpen = false;   //ovi on tässävaiheessa 90 asteen kulmassa
        float orginalYangle = transform.rotation.eulerAngles.y + -90; // -90 kuinka paljon ovea halutaan sulkea

        while (transform.rotation.eulerAngles.y > orginalYangle + 0.1f) // sillä aikaa kun oven y kulma on suurempi kuin 0.1,
        {
            // ovi pyörii y akselin ympäri kohti 0 astetta. pyörii vastapäivään
            transform.Rotate(Vector3.down * Time.deltaTime * doorCloseSpeed); 
            yield return null;
        }
    }
}
