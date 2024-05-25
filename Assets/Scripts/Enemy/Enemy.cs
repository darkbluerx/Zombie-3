using UnityEngine;
using Pathfinding;
using System.Collections;
using System;

[RequireComponent(typeof(AudioSource))]
public class Enemy : MonoBehaviour
{
    //public event Action OnHitEnemyEvent; //play hit sound
    public static event Action OnCountZombieKillEvent; //count zombie kill

    AIDestinationSetter destinationSetter; // A* Pathfinding Project
    public Health enemyHealth;

    [SerializeField] AudioSource audioSource;
    [Header("Stats"), Tooltip("Drag Enemy Stats scriptable object here")]
    [SerializeField] UnitsStats unitsStats;
    [Space]

    [Header("Enemy Settings")]
    [SerializeField] int damage = 100; // enemy damage
    [SerializeField] int timeInterval = 20; // how often enemy deals damage
    [Space]

    [Header("Enemy Colliders")]
    Collider characterController;
    [SerializeField, Tooltip("Drag trigger collider here")] Collider triggerCollider;

    private void Awake()
    {
        enemyHealth = GetComponent<Health>();
        audioSource = GetComponent<AudioSource>();
        characterController = GetComponent<Collider>();
        destinationSetter = GetComponent<AIDestinationSetter>();

        if (enemyHealth == null)
        {
            Debug.LogError("Health component not found!");
        }

        if (audioSource == null)
        {
            Debug.LogError("AudioSource component not found!");
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
    
        StartCoroutine(DealDamageOverTime());
    }

    private void Dead()
    {
        //Debug.Log("Enemy is dead!");
        unitsStats.deathAudioEvent.Play(audioSource); // play death sound
        //AudioManager.Instance.PlayCorrectSound(audioEvent: unitsStats.deathAudioEvent);
        StopCoroutine(DealDamageOverTime());
        
        destinationSetter.enabled = false; // disable zombie movement
        transform.GetComponent<AIPath>().enabled = false; // disable AIPath

        characterController.enabled = false; // disable ChracterCotroller
        triggerCollider.enabled = false; // disable trigger collider
        OnCountZombieKillEvent?.Invoke(); // count zombie kill -> GameInformationWiever

        Destroy(gameObject, 2f);
    }

    private void PainResponse(int damage)
    {
        //Debug.Log("Ouch!");
        AudioManager.Instance.PlayEnemyTakeDamage(); // call AudiManager to play hit sound
    }

    public void SetDamage(int newDamage)
    {
        damage = newDamage;
    }

    private IEnumerator DealDamageOverTime()
    {
        while (true)
        {
            int randomTime = UnityEngine.Random.Range(10, timeInterval);

            yield return new WaitForSeconds(timeInterval);
            unitsStats.shoutAudioEvent.Play(audioSource);
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
                IEnumerator coroutine = DealDamageOverTime();

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