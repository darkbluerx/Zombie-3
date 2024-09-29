using UnityEngine;
using Pathfinding;
using System.Collections;
using System;

public class Enemy : MonoBehaviour
{
    //Events
    public event Action OnAttack; // play attack animation -> ZombieAnimations.cs
    public static event Action OnCountZombieKillEvent; //count zombie kill -> GameInformationWiever.cs
    public event Action OnTakingDamage; //play take damage animation -> ZombieAnimations.cs

    AIDestinationSetter destinationSetter; // A* Pathfinding Project
    public Health enemyHealth;
    [Space]

    [Header("Drag, audios -20 db in mixer")]
    [SerializeField] AudioSource sfxAudioSource; //play shout sound
    [Header("Drag, audios -30 db in mixer")] //play death sound
    [SerializeField] AudioSource sfxEnemyAudioSource;

    [Header("Stats"), Tooltip("Drag Enemy Stats scriptable object here")]
    [SerializeField] UnitsStatsSO unitsStats;
    [Space]

    [Header("Enemy Settings")]
    [SerializeField] int damage = 100; // enemy damage
    [SerializeField] int timeInterval = 10; // how often enemy deals damage
    [Space]

    [Header("Enemy Colliders")]
    Collider characterController;
    [SerializeField, Tooltip("Drag trigger collider here")] Collider triggerCollider;

    private void Awake()
    {
        enemyHealth = GetComponent<Health>();
        //sfxAudioSource = GetComponent<AudioSource>();
        //sfxEnemyAudioSource = GetComponent<AudioSource>();
        characterController = GetComponent<Collider>();
        destinationSetter = GetComponent<AIDestinationSetter>();

        if (enemyHealth == null)
        {
            Debug.LogError("Health component not found!");
        }
    }

    private void OnEnable()
    {
        if(enemyHealth != null)
        {
            enemyHealth.OnDeadEvent += Dead;
            enemyHealth.OnTakeDamage += PainResponse;
        }
        else
        {
            Debug.LogError("EnemyHealth is null, cannot subsribe to events!");
        }
    
        StartCoroutine(ShoutOverTime());
    }

    private void Dead()
    {
        //Debug.Log("Enemy is dead!");
        unitsStats.deathAudioEvent.Play(sfxEnemyAudioSource); // play death sound
        //AudioManager.Instance.PlayCorrectSound(audioEvent: unitsStats.deathAudioEvent);
        StopCoroutine(ShoutOverTime());
        
        destinationSetter.enabled = false; // disable zombie movement
        transform.GetComponent<AIPath>().enabled = false; // disable AIPath

        characterController.enabled = false; // disable ChracterCotroller
        triggerCollider.enabled = false; // disable trigger collider
        OnCountZombieKillEvent?.Invoke(); // count zombie kill -> GameInformationWiever

        Destroy(gameObject, 2f);
    }

    private void PainResponse(int damage)
    {
        //Debug.Log("Zombie take dmg!");
        OnTakingDamage?.Invoke(); // play take damage animation -> ZombieAnimations.cs
        AudioManager.Instance.PlayEnemyTakeDamage(); // play take damage sound
    }

    public void SetDamage(int newDamage)
    {
        damage = newDamage;
    }

    private IEnumerator ShoutOverTime()
    {
        while (true)
        {
            int randomTime = UnityEngine.Random.Range(7, timeInterval);

            yield return new WaitForSeconds(timeInterval);
            unitsStats.shoutAudioEvent.Play(sfxAudioSource);
            //AudioManager.Instance.PlayCorrectSound(audioEvent: unitsStats.shoutAudioEvent);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Check if the bullet hit a gameobject with health component
            Health health = other.gameObject.GetComponent<Health>();

            if (health != null)
            {
                OnAttack?.Invoke(); // play attack animatin -> ZombieAnimations.cs

                // Call the TakeDamage method and pass the damage amount
                int remainingHealth = health.TakeDamage(damage);
                //Debug.Log($"{collision.gameObject.name} take damage: {damage}. Jäljellä oleva terveys: {remainingHealth}");
            }
        } 
    }

    private void OnDisable()
    {
        enemyHealth.OnDeadEvent -= Dead;
        //enemyHealth.OnTakeDamage -= PainResponse;       
    }
}