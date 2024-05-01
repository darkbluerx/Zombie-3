using audio;
using System.Collections;
using UnityEngine;
using System;

[RequireComponent(typeof (AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; } //Singleton

    public event Action OnPlayBackgroundMusicEvent;
   
    AudioSource audioSource;
    [Header("Automatically assign audio source")]
    [SerializeField] AudioSource audioSource2;

    [Header("Traps")]
    public AudioEvent[] trapAudioEvents;
    [Space]

    [Header("Player")]
    public AudioEvent playerHitAudioEvents;
    bool isPlayerHitAudioPlaying = false;
    [Space]

    [Header("Enemy")]
    public AudioEvent enemyHitAudioEvents;
    bool isEnemyHitAudioPlaying = false;
    [Space]

    [Header("Trap Buttons")]
    [SerializeField] AudioEvent buttonTrapAudioEvent;

    [Header("Level Audios")]
    public AudioEvent levelComplete;
    public AudioEvent gameOver;
    public AudioEvent backgroundAudioEvent;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There can only be one AudioManager");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        audioSource = GetComponent<AudioSource>();
        audioSource2 = GameObject.Find("BackgroundMusic").GetComponent<AudioSource>(); // Find the background music audio source
    }

    private void OnEnable()
    {
        OnPlayBackgroundMusicEvent += PlayBackgroundMusic;    
    }

    private void Start()
    {
        OnPlayBackgroundMusicEvent?.Invoke();
    }

    private void PlayBackgroundMusic() // Play background music
    {
        //backgroundAudioEvent.Play(audioSource2);
    }

    public void StopBackgroundMusic()
    {
        audioSource2.Stop(); // Stop background music
    }

    public void GetTrapAudioEvent() // Play trap button sound
    {
        if(buttonTrapAudioEvent != null)
        {
            buttonTrapAudioEvent.Play(audioSource);
        }
    }

    public void PlayPlayerTakeDamage()
    {
        if (playerHitAudioEvents != null && !isPlayerHitAudioPlaying)
        {
            playerHitAudioEvents.Play(audioSource);
            isPlayerHitAudioPlaying = true;
            StartCoroutine(ResetPlayerHitAudioFlag()); // play the sound once
        }
    }

    public void PlayEnemyTakeDamage()
    {
        if (enemyHitAudioEvents != null && !isEnemyHitAudioPlaying)
        {
            enemyHitAudioEvents.Play(audioSource);
            isEnemyHitAudioPlaying = true;
            StartCoroutine(ResetEnemyHitAudioFlag()); // play the sound once
        }
    }
    
    private IEnumerator ResetPlayerHitAudioFlag() // play the sound once
    {
        yield return new WaitForSeconds(playerHitAudioEvents.audioClips.Length);
        isPlayerHitAudioPlaying = false;
    }

    private IEnumerator ResetEnemyHitAudioFlag() // play the sound once
    {
        yield return new WaitForSeconds(enemyHitAudioEvents.audioClips.Length);
        isEnemyHitAudioPlaying = false;
    }

    public void GetSound(TrapType trapType) // Choose the sound based on the trapType
    {
        switch (trapType)
        {
            case TrapType.Axe:
                PlaySound(trapType);
                break;
            case TrapType.Spike:
                PlaySound(trapType);
                break;
            case TrapType.Cutter:
                PlaySound(trapType);
                break;
            case TrapType.Saw:
                PlaySound(trapType);
                break;
            default:
                break;
        }     
    }

    public void PlaySound(TrapType trapType) //Play the sound related to the trapType
    {
        trapAudioEvents[(int)trapType].Play(audioSource); 
    }

    public void PlayCorrectSound(AudioEvent audioEvent) //you can call this method from anywhere and pass the audioEvent you want to play
    {
        audioEvent.Play(audioSource);  //script what calls this method, remember to add audioEvent parameter to it
        //examble: AudioManager.Instance.PlayCorrectSound(audioEvent:openDoorAudioEvent);
    }

    private void OnDisable()
    {
       //OnPlayBackgroundMusicEvent -= PlayBackgroundMusic;
    }  
}
