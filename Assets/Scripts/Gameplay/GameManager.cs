using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // singleton

    public Action OnLevel1Complete; //this activate level 2
    public Action OnLevel2Complete; //this activate level 2


    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            Debug.LogError("There can only be one GameManager" + transform + " " + Instance);
            return;
        }
        Instance = this;
    }

    public void Level1Complete()
    {
        OnLevel1Complete?.Invoke();
    }

    public void Level2Complete()
    {
        OnLevel2Complete?.Invoke();
    }

}
