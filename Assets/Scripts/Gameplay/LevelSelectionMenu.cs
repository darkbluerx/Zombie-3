using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class LevelSelectionMenu : MonoBehaviour
{
    public static event Action OnShowMapSelectionCanvas; // Event to show the map selection canvas

    [SerializeField] UnityEngine.UI.Button[] levelButtons; // Nappulat
    [SerializeField] GameObject buttonsParent;
    [SerializeField] List<GameObject> levelPictures; // List of level pictures

    int currentLevel;
    int nextLevel;

    private void Awake()
    {
        ButtonToArray(); // Assign the buttons to the array
    }

    private void ButtonToArray()
    {
        int childCount = buttonsParent.transform.childCount;
        levelButtons = new UnityEngine.UI.Button[childCount];

        for (int i = 0; i < childCount; i++)
        {
            levelButtons[i] = buttonsParent.transform.GetChild(i).gameObject.GetComponent<UnityEngine.UI.Button>(); //Get the buttons automatically from the parent object in the inspector
        }
    }

    private void Start()
    {
        FinishPoint.OnLevelComplete += UnlockNextLevel;
        if (currentLevel > 1) OnShowMapSelectionCanvas?.Invoke(); // Show the map selection canvas if the player has completed at least one level -> MainMenu.cs

        //check if levelButtons array is assigned correctly
        if (levelButtons != null && levelButtons.Length > 0)
        {
            currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
            nextLevel = PlayerPrefs.GetInt("NextLevel", 2);

            for (int i = 0; i < levelButtons.Length; i++)
            {
                if (i < currentLevel - 1) // -1 because level numbers start from 1 and array index from 0
                {
                    //check if the button exists and activate it if needed
                    if (levelButtons[i] != null) levelButtons[i].interactable = true;
                }
                else if (i == currentLevel - 1) // -1 because level numbers start from 1 and array index from 0
                {
                    // Tarkista, että nappi on olemassa ja aktivoi se tarvittaessa
                    if (levelButtons[i] != null)
                    {
                        levelButtons[i].interactable = true;
                        int sceneIndex = i + 1; // +1 because level numbers start from 1 and scene index from 0
                        levelButtons[i].onClick.AddListener(() => LoadLevel(sceneIndex));

                        // Show the picture for the active level
                        ShowLevelPicture(i);
                    }
                }
                else
                {
                    //Check if the button exists and deactivate it if needed
                    if (levelButtons[i] != null) levelButtons[i].interactable = false;
                }
            }
        }
        else
        {
            Debug.LogWarning("Level buttons not assigned or empty.");
        }
    }

    private void LoadLevel(int levelindex)
    {
        SceneManager.LoadScene(levelindex);
    }

    public void UnlockNextLevel()
    {
        currentLevel = nextLevel;
        nextLevel = Mathf.Min(nextLevel + 1, levelButtons.Length); // Prevents from going over the number of buttons

        PlayerPrefs.SetInt("CurrentLevel", currentLevel);
        PlayerPrefs.SetInt("NextLevel", nextLevel);

        Start(); // Update the state of the buttons again
    }

    public void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        //Debug.Log("PlayerPrefs nollattu.");
    }

    private void ShowLevelPicture(int activeLevelIndex)
    {
        for (int i = 0; i < levelPictures.Count; i++)
        {
            if (levelPictures[i] != null)
            {
                levelPictures[i].SetActive(i == activeLevelIndex);
            }
        }
    }
}