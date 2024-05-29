using audio;
using System.Collections;
using UnityEngine;
using System;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; } //Singleton

    //public event Action OnPlayBackgroundMusicEvent;

    public Slider musicVolumeSlider; // Slider for music volume
    public Slider sfxVolumeSlider; // Slider for sound volume

    [Header("Automatically assign audio sources")]
    [SerializeField] AudioSource sfxAudioSource;   
    [SerializeField] AudioSource musicAudioSource;

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
    //public AudioEvent backgroundAudioEvent;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There can only be one AudioManager");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        musicAudioSource = GameObject.Find("BackgroundMusic").GetComponent<AudioSource>(); // Find the background music audio source
        sfxAudioSource = GameObject.Find("SFXMusic").GetComponent<AudioSource>(); // Find the background music audio source
    }

    private void Start()
    {
        //OnPlayBackgroundMusicEvent?.Invoke(); // Play background music

        if (PlayerPrefs.HasKey("MusicVolume")) // If the player has set the volume before
        {
            float savedMusicVolume = PlayerPrefs.GetFloat("MusicVolume"); // Get the saved volume
            musicVolumeSlider.value = savedMusicVolume; // Set the volume to the saved volume
            SetMusicVolume(savedMusicVolume); // Set the volume to the saved volume
        }
        else
        {
            musicVolumeSlider.value = 0.5f; // Set the volume to the default volume
            SetMusicVolume(0.5f); // Set the volume to the default volume
        }

        if(PlayerPrefs.HasKey("SFXVolume")) // If the player has set the volume before
        {
            float savedSFXVolume = PlayerPrefs.GetFloat("SFXVolume"); // Get the saved volume
            sfxVolumeSlider.value = savedSFXVolume; // Set the volume to the saved volume
            SetSFXVolume(savedSFXVolume); // Set the volume to the saved volume
        }
        else
        {
            sfxVolumeSlider.value = 0.5f; // Set the volume to the default volume
            SetSFXVolume(0.5f); // Set the volume to the default volume
        }

        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume); // Set the volume of the music
        sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume); // Set the volume of the sound
    }

    public void SetMusicVolume(float volume) // Set the volume of the music
    {
        musicAudioSource.volume = volume; // Set the volume of the music
        PlayerPrefs.SetFloat("MusicVolume", volume); // Save the volume
    }

    public void SetSFXVolume(float volume) // Set the volume of the sfx
    {
        sfxAudioSource.volume = volume; // Set the volume of the sfx
        PlayerPrefs.SetFloat("SFXVolume", volume ); // Save the volume
    }

    public float GetSFXVolume() // Get the volume of the music
    {
        return sfxAudioSource.volume; // Get the volume of the music             
    }

    public void StopBackgroundMusic()
    {
        musicAudioSource.Stop(); // Stop background music
    }

    public void GetTrapAudioEvent() // Play trap button sound
    {
        if(buttonTrapAudioEvent != null)
        {
            buttonTrapAudioEvent.Play(sfxAudioSource);
        }
    }

    public void PlayPlayerTakeDamage()
    {
        if (playerHitAudioEvents != null && !isPlayerHitAudioPlaying)
        {
            playerHitAudioEvents.Play(sfxAudioSource);
            isPlayerHitAudioPlaying = true;
            StartCoroutine(ResetPlayerHitAudioFlag()); // play the sound once
        }
    }

    public void PlayEnemyTakeDamage()
    {
        if (enemyHitAudioEvents != null && !isEnemyHitAudioPlaying)
        {
            enemyHitAudioEvents.Play(sfxAudioSource);
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
        trapAudioEvents[(int)trapType].Play(sfxAudioSource); 
    }

    public void PlayCorrectSound(AudioEvent audioEvent) //you can call this method from anywhere and pass the audioEvent you want to play
    {
        audioEvent.Play(sfxAudioSource);  //script what calls this method, remember to add audioEvent parameter to it
        //examble: AudioManager.Instance.PlayCorrectSound(audioEvent:openDoorAudioEvent);
    }   
}
