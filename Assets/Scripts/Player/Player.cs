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
    //[SerializeField] AudioSource audioSource;
    [Space]

    [Header("Player Stats, needed")]
    [SerializeField] UnitsStatsSO unitsStats;

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

        Invoke("LoadGameOver", 3f); // load game over scene after 3 seconds
        Invoke("DisablePlayer", 4f); // disable player after 4 seconds
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
