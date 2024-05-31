using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishPoint : MonoBehaviour
{
    static public event Action OnLevelComplete;
    [SerializeField] bool lastLevel = false; 
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LevelComplete();
            OnLevelComplete?.Invoke(); // Invoke the event what shows the next level button -> MapSelection.cs
        }
    }

    private void LevelComplete()
    {
        Time.timeScale = 0f;
        Cursor.visible = true;
        if (lastLevel) LoadGameCopmleted();
        else SceneManager.LoadScene(0); // Load the level choose scene
        //SceneManager.LoadScene(0); 
    }

    private void LoadGameCopmleted()
    {
        LevelManager.Instance.GameCompleted();
    }
}
