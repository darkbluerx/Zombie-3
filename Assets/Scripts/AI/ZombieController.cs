using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ZombieController : MonoBehaviour
{
    //[Header("Zombie Animations")]
    public event Action OnZombieIdle; //play idle animation -> ZombieAnimations.cs
    public event Action OnZombieWalk; //play walk animation -> ZombieAnimations.cs

    CharacterController characterController; 

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
       HandleAnimations();
    }

    private void HandleAnimations()
    {
        if (characterController.velocity.magnitude > 0) // If the zombie is moving play walk animation
        {
            OnZombieWalk?.Invoke();
        }
        if (characterController.velocity.magnitude == 0) // If the zombie is not moving play idle animation
        {
            OnZombieIdle?.Invoke();
        }
    }
}
