using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ZombieAnimations : MonoBehaviour
{
    [SerializeField] Animator animator;
    [Space]

    [SerializeField] ZombieController zombieController;
    [SerializeField] Health health;
    [SerializeField] Enemy enemy;

    int eatingAnimationHash = Animator.StringToHash("isEating");
    int idleAnimationHash = Animator.StringToHash("isIdle");
    int takingDmgAnimationHash = Animator.StringToHash("isTakingDmg");
    int attackAnimationHash = Animator.StringToHash("isAttack");
   
    private void Awake()
    {
        zombieController = GetComponent<ZombieController>();
    }

    private void OnEnable()
    {
        //Zombie Animations
        zombieController.OnZombieWalk += ZombieController_OnWalk;
        health.OnDeadEvent += Health_OnDeadEvent;
        health.OnEat += ZombieController_OnEat;
        zombieController.OnZombieIdle += ZombieController_OnIdle;
        enemy.OnTakingDamage += Enemy_OnTakeDamage;
        enemy.OnAttack += Enemy_OnAttack;
    }

    private void Enemy_OnAttack()
    {
       if(animator != null) animator.SetBool(attackAnimationHash,true);
    }

    private void Enemy_OnTakeDamage()
    {
        if (animator != null)
        {
            animator.SetBool("isIdle", false);
            animator.SetBool(idleAnimationHash, false);
            animator.SetTrigger(takingDmgAnimationHash);
        }
    }

    private void Health_OnDeadEvent()
    {
        animator.applyRootMotion = true;
        animator.SetTrigger("isDead");
        animator.SetBool("isWalk", false);
    }

    private void ZombieController_OnWalk()
    {
        animator.SetBool("isWalk", true);
        if (animator != null) animator.SetBool(idleAnimationHash, false);
    }

    private void ZombieController_OnIdle()
    {
        animator.SetBool("isWalk", false);
        if (animator != null) animator.SetBool(idleAnimationHash,true);
    }

    private void ZombieController_OnEat()
    {
        if (animator != null) animator.SetTrigger(eatingAnimationHash);
    }

    private void OnDisable()
    {
        //Zombie Animations
        //zombieController.OnZombieIdle -= UnitAnimations_OnIdleZombie;
        zombieController.OnZombieWalk -= ZombieController_OnWalk;
        health.OnDeadEvent -= Health_OnDeadEvent;
    }
}
