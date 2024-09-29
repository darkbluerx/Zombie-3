using UnityEngine;
using Pathfinding;
using System.Collections;
using System;

public class Enemy : MonoBehaviour
{
    // Events
    public event Action OnAttack; // play attack animation -> ZombieAnimations.cs
    public static event Action OnCountZombieKillEvent; // count zombie kill -> GameInformationWiever.cs
    public event Action OnTakingDamage; // play take damage animation -> ZombieAnimations.cs

    AIDestinationSetter destinationSetter; // A* Pathfinding Project
    public Health enemyHealth;
    [Space]

    [Header("Drag the first AudioSource here")]
    [Tooltip ("Using SFX mixer -15 dB")]
    [SerializeField] AudioSource SFXAudioSource_Quiter; // play shout sound
    [Space]

    [Header("Drag the second AudioSource here")]
    [Tooltip("Using Enemies mixer -13 dB")]
    [SerializeField] AudioSource SFXAudioSource; // play death sound

    [Header("Stats"), Tooltip("Drag Enemy Stats scriptable object here")]
    [SerializeField] UnitsStatsSO enemyStats; // enemy stats scriptable object
    [Space]

    [Header("Enemy Settings")]
    [Tooltip("Enemy damage")]
    [SerializeField] int damage = 100;

    [Tooltip("How often enemy deals damage")]
    [SerializeField] int timeInterval = 10;
    [Space]

    [Header("Enemy Colliders")]
    Collider characterController;
    [SerializeField, Tooltip("Drag trigger collider here")] Collider triggerCollider;

    private void Awake()
    {
        enemyHealth = GetComponent<Health>();
        characterController = GetComponent<Collider>();
        destinationSetter = GetComponent<AIDestinationSetter>();

        if (enemyHealth == null)
        {
            Debug.LogError("Health component not found!");
        }
    }

    private void OnEnable() // Subscribe to events
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
        enemyStats.deathAudioEvent.Play(SFXAudioSource); // play death sound

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
            enemyStats.shoutAudioEvent.Play(SFXAudioSource_Quiter); // play shout sound
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
                OnAttack?.Invoke(); // play attack animation -> ZombieAnimations.cs

                // Call the TakeDamage method and pass the damage amount
                int remainingHealth = health.TakeDamage(damage);
            }
        } 
    }

    private void OnDisable() // Unsubscribe from events
    {
        enemyHealth.OnDeadEvent -= Dead;
    }
}