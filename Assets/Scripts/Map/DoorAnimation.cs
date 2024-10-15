using UnityEngine;
using System;

//Door opening and closing feature + animation.
[RequireComponent(typeof(Animator))]
public class DoorAnimation : MonoBehaviour
{
    public event Action OnDoorOpen; // Play open door animation
    public event Action OnDoorClose; // Play close door animation

    [Header("AudioEvents")]
    public AudioEvent doorOpeningAudioEvent; // play open door sound
    public AudioEvent doorLockAudioEvent; // play locked door sound

    [Header("How far you can open the door")]
    [SerializeField] float interactionDistance = 2f;
    [Space]

    [SerializeField] Animator animator;
    [Space]

    [Header("Is the door open or closed at the start")]
    [SerializeField] bool isOpen = false;

    [Header("Controls if the door can be opened, On/Off")]
    [SerializeField] bool On = true;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (!isOpen) Close();
        else Open();
    }

    private void OnEnable() // Subscribe to events
    {
        OnDoorOpen += Open;
        OnDoorClose += Close;
    }

    private void Update()
    {
        if (On && Input.GetKeyDown(KeyCode.F) && IsPlayerNearby())
        {
            ToggleDoor();
        }
        if (!On)
        {
            if (Input.GetKeyDown(KeyCode.F) && IsPlayerNearby())
            {
                AudioManager.Instance.PlayCorrectSound(audioEvent: doorLockAudioEvent); // Play the locked door sound
            }
        }
    }

    private void ToggleDoor()
    {
        if (isOpen)
        {
            OnDoorClose?.Invoke(); // Play the close door animation
            AudioManager.Instance.PlayCorrectSound(audioEvent: doorOpeningAudioEvent); // Play the open door sound
            isOpen = false;
        }
        else
        {
            OnDoorOpen?.Invoke(); // Play the open door animation
            AudioManager.Instance.PlayCorrectSound(audioEvent: doorOpeningAudioEvent);
            isOpen = true;
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
        animator.SetBool("isOpen", true);
        animator.SetBool("isClose", false);
    }

    public void Close()
    {
        animator.SetBool("isOpen", false);
        animator.SetBool("isClose", true);
    }

    private void OnDisable() // Unsubscribe from events
    {
        OnDoorOpen -= Open;
        OnDoorClose -= Close;
    }
}
