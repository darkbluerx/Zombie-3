using System.Collections;
using UnityEngine;
using UnityEngine.UI;

//Control the playback of sounds such as music, sfx, game, trap and enemy sounds
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; } //Singleton

    public Slider musicVolumeSlider; // Slider for music volume
    public Slider sfxVolumeSlider; // Slider for sound volume

    [Header("Automatically assign audio sources")]
    [SerializeField] AudioSource sfxAudioSource;
    [SerializeField] AudioSource musicAudioSource;

    [Header("Trap audios")]
    public AudioEvent[] trapAudioEvents;
    [Space]

    [Header("Player audios")]
    public AudioEvent playerHitAudioEvent;
    bool isPlayingPlayerHitAudio = false;
    [Space]

    [Header("Enemy audios")]
    public AudioEvent enemyHitAudioEvent;
    bool isPlayingEnemyHitAudio = false;
    [Space]

    [Header("Trap Button audios")]
    [SerializeField] AudioEvent buttonTrapAudioEvent;

    [Header("Level audios")]
    public AudioEvent levelCompleteAudioEvent;
    public AudioEvent gameOverAudioEvent;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There can only be one AudioManager");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        musicAudioSource = GameObject.Find("BackgroundMusic").GetComponent<AudioSource>(); // Find autiomatically the background music audio source
        sfxAudioSource = GameObject.Find("SFXMusic").GetComponent<AudioSource>(); // Find autiomatically the sfx audio source
    }

    private void Start()
    {
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

    public void GetTrapAudioEvent() // Play trap button sound
    {
        if(buttonTrapAudioEvent != null)
        {
            buttonTrapAudioEvent.Play(sfxAudioSource);
        }
    }

    public void PlayPlayerTakeDamage()
    {
        if (playerHitAudioEvent != null && !isPlayingPlayerHitAudio)
        {
            playerHitAudioEvent.Play(sfxAudioSource);
            isPlayingPlayerHitAudio = true;
            StartCoroutine(ResetPlayerHitAudioFlag()); // Play the sound once
        }
    }

    public void PlayEnemyTakeDamage()
    {
        if (enemyHitAudioEvent != null && !isPlayingEnemyHitAudio)
        {
            enemyHitAudioEvent.Play(sfxAudioSource);
            isPlayingEnemyHitAudio = true;
            StartCoroutine(ResetEnemyHitAudioFlag()); // Play the sound once
        }
    }
    
    private IEnumerator ResetPlayerHitAudioFlag() // Play the sound once
    {
        yield return new WaitForSeconds(playerHitAudioEvent.audioClips.Length);
        isPlayingPlayerHitAudio = false;
    }

    private IEnumerator ResetEnemyHitAudioFlag() // Play the sound once
    {
        yield return new WaitForSeconds(enemyHitAudioEvent.audioClips.Length);
        isPlayingEnemyHitAudio = false;
    }

    public void StopBackgroundMusic()
    {
        musicAudioSource.Stop(); // Stop background music
    }

    public void PlayBackGroundMusic(AudioEvent audioEvent)
    {
        audioEvent.Play(musicAudioSource); // Play background music
    }

    public void PlayCorrectSound(AudioEvent audioEvent) //You can call this method from anywhere and pass the audioEvent you want to play
    {
        audioEvent.Play(sfxAudioSource);  //script what calls this method, remember to add audioEvent parameter to it
        //examble: AudioManager.Instance.PlayCorrectSound(audioEvent:openDoorAudioEvent);
    }   
}
