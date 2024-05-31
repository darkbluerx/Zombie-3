using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager: MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    //public static event Action<int> OnLevelLoaded; // Event for showing the next level button

    [Header("Needed gameOverCanvas & levelCompleteCanvas")]
    [SerializeField] GameObject gameOverCanvas;
    [SerializeField] GameObject levelCompleteCanvas;

    [Header("Set Current level number")]
    public int currentLevel = 1;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There can only be one LevelManager" + transform + " " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void StartingGameOver()
    {
        Cursor.visible = true; // Show the cursor
        gameOverCanvas.SetActive(true);
        StartCoroutine(LoadGameOver());
    }

    public void LoadCurrentLevel()
    {
        SceneManager.LoadScene(currentLevel);
    }

    private IEnumerator LoadGameOver()
    {
        AudioManager.Instance.StopBackgroundMusic(); // Stop background music
        Time.timeScale = 0f; // Stop the game

        yield return new WaitForSecondsRealtime(2f); // wait for seconds
        AudioManager.Instance.PlayBackGroundMusic(audioEvent: AudioManager.Instance.gameOverAudioEvent); // Play game over music
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void GameCompleted()
    {
        levelCompleteCanvas.SetActive(true);
        Settings.Instance.CallOnDisableGun2(); // Disable the gun
        Cursor.visible = true; // Show the cursor
        StartCoroutine(LoadNextLevel());
    }

    private IEnumerator LoadNextLevel()
    {
       

        AudioManager.Instance.StopBackgroundMusic(); // Stop background music
        Time.timeScale = 0f; // Stop the game

        yield return new WaitForSecondsRealtime(1f); // wait for seconds
        AudioManager.Instance.PlayBackGroundMusic(audioEvent: AudioManager.Instance.levelCompleteAudioEvent); // Play level complete music
    }
}
