using UnityEngine;
using TMPro;
using System;

public class GuidanceManager : MonoBehaviour
{
    public static GuidanceManager Instance { get; private set; } //Singleton

    public static event Action OnLevelComplete; // Event to trigger when the level is complete

    [Header("MainStory for each level (SO)")]
    public GuidanceSO[] MainStory; // Main story for each level
    public GuidanceSO[] EndStory; // End story for each level

    [Header("Drag the hintText object here")]
    public TMP_Text hintText;

    [Header("Drag the closeTextWindow/button object here")]
    [SerializeField] UnityEngine.UI.Button closeTextButton;

    [Header("Drag the TextCanvas object here (Story text)")]
    [SerializeField] GameObject TextCanvas;

    [Header("Set the end game collider")]
    [SerializeField] Collider endGameCollider; // The collider that triggers the end of the level

    [Header("Set the Level number")]
    [SerializeField] int levelNumber;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("More than one instance of GuidanceManager found!");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        ShowGuindanceText(); //shows the guidance text when the collider is hit
    }

    private void OnEnable()
    {
        OnLevelComplete += ShowEndText; //shows the end text when the collider is hit
        closeTextButton.onClick.AddListener(CloseTextCanvasIfAnyKeyPressed);
    }

    private void CloseTextCanvasIfAnyKeyPressed()
    {
        if (Input.anyKey) // Close the window if any key is pressed
        {
            Time.timeScale = 1f; // Resume the game
            CloseWindow();
        }
    }

    public void ShowGuindanceText()
    {
        Time.timeScale = 0f; // Pause the game

        levelNumber = GetLevelNumber();
        for (int i = 0; i < MainStory.Length; i++)
        {
            if (MainStory[i].level == levelNumber)
            {
                hintText.text = MainStory[i].guindanceText + "\n \n Close the Text window by pressing Left Mouse and Space button.";
                return;
            }
        }
        hintText.text = "Hint text not found for this level"; 
    }

    int GetLevelNumber()
    {
        //check the game state and return the level number based on it
        if (LevelManager.Instance != null)
        {
            //Assume that the game state contains information about the current level
            return LevelManager.Instance.currentLevel;
        }
        else
        {
            Debug.LogWarning("Game state not set. Return default level number"); 
            return 1; // Return the default level number if the game state is not set
        }
    }
   
    private void ShowEndText()
    {
        SetActiveTextCanvas();
        Time.timeScale = 1f; // Pause the game

        levelNumber = GetLevelNumber();
        //Debug.Log("Level number: " + levelNumber);
        for (int x = 0; x < EndStory.Length; x++)
        {
            if (EndStory[x].level == levelNumber)
            {
                hintText.text = EndStory[x].guindanceText + "\n \n Close the Text window by pressing Left Mouse and Space button.";
                return;
            }
        }
        hintText.text = "EndHint text not found for this level";
    }

    private void SetActiveTextCanvas()
    {
        TextCanvas.SetActive(true);
    }

    private void CloseWindow()
    {
        hintText.text = "";
        closeTextButton.onClick.RemoveListener(CloseWindow);
        TextCanvas.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Triggered");
        if (other != null)
        {
            if (other.CompareTag("Player"))
            {
                OnLevelComplete?.Invoke();
                //Debug.Log("Level complete");
            }
        }
    }

    private void OnDisable()
    {
        OnLevelComplete -= ShowEndText;
    }
}
