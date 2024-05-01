using System;
using UnityEngine;

public class LevelEvent : MonoBehaviour
{
    public event Action OnLevelComplete;

    private void OnEnable()
    {
        if(OnLevelComplete != null) OnLevelComplete += LevelComplete;
    }
    private void OnDisable()
    {
        if (OnLevelComplete != null) OnLevelComplete -= LevelComplete;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Player"))
        {
           LevelComplete();
        }
    }

    public void LevelComplete()
    {
        OnLevelComplete?.Invoke();
    }
}
