using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ZombieController : MonoBehaviour
{
    //[Header("Zombie Animations")]
    //public event Action OnZombieIdle;
    public event Action OnZombieWalk;
    //public event Action<int> OnTakeDamage;
    //public event Action OnDeadEvent;

    //public event Action OnZombieAttack;

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
    }
}
