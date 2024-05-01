using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ZombieAnimations : MonoBehaviour
{
    [SerializeField] Animator animator;
    [Space]

    [SerializeField] ZombieController zombieController;
    [SerializeField] Health health;

    private void Awake()
    {
        zombieController = GetComponent<ZombieController>();
        ////health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        //Zombie Animations
        //zombieController.OnZombieIdle += UnitAnimations_OnIdleZombie;
        zombieController.OnZombieWalk += UnitAnimations_OnWalkZombie;
        health.OnDeadEvent += Health_OnDeadEvent;
    }

    private void Health_OnDeadEvent()
    {
        animator.applyRootMotion = true;
        animator.SetTrigger("isDead");
        animator.SetBool("isWalk", false);
    }

    private void UnitAnimations_OnWalkZombie()
    {
        animator.SetBool("isWalk", true);
        ////animator.SetBool("isIdle", false);
    }

    private void OnDisable()
    {
        //Zombie Animations
        //zombieController.OnZombieIdle -= UnitAnimations_OnIdleZombie;
        zombieController.OnZombieWalk -= UnitAnimations_OnWalkZombie;
        health.OnDeadEvent -= Health_OnDeadEvent;
    }
}
