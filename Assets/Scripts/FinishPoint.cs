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
            //UnlockNewLevel();
            //StartCoroutine(LevelComplete());
            LevelComplete();
            OnLevelComplete?.Invoke(); // Invoke the event what shows the next level button -> MapSelection.cs
        }
    }

    //private IEnumerator LevelComplete()
    //{
    //    AudioManager.Instance.StopBackgroundMusic(); ; // Stop background music
    //    yield return new WaitForSecondsRealtime(1f); // Wait for 1 second
    //    Time.timeScale = 0f;
    //    Cursor.visible = true;

    //    AudioManager.Instance.PlayCorrectSound(audioEvent: AudioManager.Instance.levelComplete);
    //    yield return new WaitForSecondsRealtime(2f);
    //    SceneManager.LoadScene(0); // Load the level choose scene
    //}

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
