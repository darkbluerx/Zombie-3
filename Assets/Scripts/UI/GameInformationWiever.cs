using System;
using System.Collections;
using UnityEngine;

public class GameInformationWiever : MonoBehaviour
{
    public static GameInformationWiever Instance { get; private set; } // Singleton
    public static event Action OnTimerText; // Event to update the timer text

    [Header("Drag the gameEvent to open the door")]
    [SerializeField] GameEvent openDoorEvent; // Event to open the door

    public float elapsedTime { get; private set; }  // Start is called before the first frame update

    float updateInterval = 1f;  //How often timer should update timer text

    public int zombieKillCount { get; private set; } = 0;
 
    [Header("Choose the number of zombies to kill to open the door")] 
    [SerializeField] int maxZombieKillCount = 50;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("More than one instance of GameInformationWiever found!");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(UpdateTimer());
    }

    private void OnEnable()
    {
        Enemy.OnCountZombieKillEvent += ZombieCounter; // Subscribe to the event to count zombie kill
    }

    public IEnumerator UpdateTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(updateInterval);
            elapsedTime += updateInterval;
            //Debug.Log("Time: " + elapsedTime);
            OnTimerText?.Invoke(); // Update the timer text -> UIManager
        }
    }

    public void ZombieCounter()
    {
        //if(zombieKillCount == 0) zombieKillCount = 0;
        zombieKillCount++;
        if(zombieKillCount == maxZombieKillCount) // Level 2 mission
        {
            //Debug.Log("Open Door!");
            openDoorEvent.Raise(); // Open the door
        }
    }

    public int GetZombieKillCount()
    {
        return zombieKillCount;
    }

    private void OnDisable()
    {
        Enemy.OnCountZombieKillEvent -= ZombieCounter; // Unsubscribe from the event to count zombie kill
    }
}
