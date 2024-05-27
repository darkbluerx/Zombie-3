using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ZombieController : MonoBehaviour
{
    //[Header("Zombie Animations")]
    public event Action OnZombieIdle; //play idle animation -> ZombieAnimations.cs
    public event Action OnZombieWalk; //play walk animation -> ZombieAnimations.cs

    public event Action OnZombieAttack;

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
        if (characterController.velocity.magnitude > 0)
        {
            OnZombieWalk?.Invoke();
        }
        if (characterController.velocity.magnitude == 0)
        {
            OnZombieIdle?.Invoke();
        }
    }
}
