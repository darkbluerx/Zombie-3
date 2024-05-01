using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ZombieSpawner : MonoBehaviour
{
    public static ZombieSpawner Instance { get; private set; } // Singleton

    public static event Action OnZombieSpawned; // Event to update spawned zombies count

    [Header("Zombie prefabs, place zombies")]
    [SerializeField] List<GameObject> zombiePrefabs;

    [Header("Spawn points, place empty Gameobject")]
    [SerializeField] List<Transform> spawnPoints;

    [Header("Spawm timer")]
    [SerializeField] float minSpawnTime = 20f;
    [SerializeField] float maxSpawnTime = 40f;

    [Header("Determines how many zombies spawn throughout the game")]
    [SerializeField] int maxZombieCount = 10;

    int zombieCount = 0; //current zombie count
    int zombiesToSpawn = 1; //how many zombies spawn at a time

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("More than one instance of ZombieSpawner found!");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(SpawnZombies());
    }

    private float GetSpawnTime()
    {
        return UnityEngine.Random.Range(minSpawnTime, maxSpawnTime);
    }

    private IEnumerator SpawnZombies()
    {
        while (zombieCount < maxZombieCount)
        {
            for (int i = 0; i < zombiesToSpawn; i++)
            {
                yield return new WaitForSeconds(1f); // Wait for one second before instantiating the next zombie

                SpawnZombie();

                zombieCount++;
            }

            zombiesToSpawn++; // Increment the number of zombies to spawn on the next iteration

            yield return new WaitForSeconds(GetSpawnTime()); // Wait for a random time before spawning the next batch of zombies
        }
    }

    private void SpawnZombie()
    {
        if (zombiePrefabs.Count == 0 || spawnPoints.Count == 0)
        {
            Debug.LogWarning("Zombie prefabs or spawn points not set!");
            return;
        }

        int randomIndex = UnityEngine.Random.Range(0, spawnPoints.Count);
        Transform spawnPoint = spawnPoints[randomIndex];

        randomIndex = UnityEngine.Random.Range(0, zombiePrefabs.Count);
        GameObject zombiePrefab = zombiePrefabs[randomIndex];

        Instantiate(zombiePrefab, spawnPoint.position, Quaternion.identity);
    }

    public int GetZombieCount() //returns the zombie count
    {
        return zombieCount;
    }

    public void UpdateZombieCount()
    {
        OnZombieSpawned?.Invoke();
    }
}
