using UnityEngine;
using System;
using weapon;

//[RequireComponent(typeof(AudioSource))]
public class Player : MonoBehaviour
{
    public event Action OnHitPlayerEvent; //play hit sound

    public Health playerHealth;
    [SerializeField] GameObject player;

    [Header("Audio, automatically assign audio source")]
    [SerializeField] AudioSource audioSource;
    [Space]

    [Header("Player Stats, needed")]
    [SerializeField] UnitsStats unitsStats;

    private void Awake()
    {
        playerHealth = GetComponent<Health>();
        //audioSource = GetComponent<AudioSource>();
        player = GameObject.Find("Player");
    }

    private void OnEnable()
    {
        playerHealth.OnDeadEvent += Dead;
        playerHealth.OnTakeDamage += PainResponse;      
    }

    private void Dead()
    {
        //unitsStats.deathAudioEvent.Play(audioSource);
        AudioManager.Instance.PlayCorrectSound(audioEvent:unitsStats.deathAudioEvent); // call AudioManager to play game over sound

        TopDownPlayerController playerController = GetComponent<TopDownPlayerController>();
        playerController.enabled = false; // disable player movement

        Gun gun = GetComponentInChildren<Gun>();
        gun.enabled = false;

        Invoke("LoadGameOver", 3f);
        //Destroy(gameObject, 4f);
        Invoke("DisablePlayer", 4f);
    }

    private void DisablePlayer()
    {
        player.SetActive(false);
    }

    private void LoadGameOver()
    {
       LevelManager.Instance.StartingGameOver(); 
    }

    private void PainResponse(int damage)
    {
       AudioManager.Instance.PlayPlayerTakeDamage(); // call AudiManager to play hit sound
    }

    private void OnDisable()
    {
        playerHealth.OnDeadEvent -= Dead;
        playerHealth.OnTakeDamage -= PainResponse;
        //StopCoroutine(DealDamageOverTime());
    }
}   
