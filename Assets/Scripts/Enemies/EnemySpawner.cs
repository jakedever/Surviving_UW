using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using UnityEngine;
using UnityEngine.AI;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public List<EnemyGroup> enemyGroups; 
        public int waveQuota; // Total number of enemies to spawn in this wave
        public float spawnInterval; // Interval at which to spawn them
        public float spawnCount; // Number of enemy spawned so far
    }

    [System.Serializable]
    public class EnemyGroup
    {
        public string enemyName; // Name of enemy type spawned in this wave
        public int enemyCount; // Number of enemy type spawned in this wave
        public int spawnCount; // Number of enemies already spawned in this wave
        public GameObject enemyPrefab; // Enemy Prefab
    }

    public GameObject chris;
    public List<Wave> waves; 
    public int currentWaveCount; // index of the current wave (starts at 0)

    [Header("Spawner Attribute")]
    float spawnTimer; // Timer to determine next enemy
    public int enemiesAlive;
    public int maxEnemiesAllowed;
    public bool maxEnemiesReached;
    public float waveInterval; //  The interval between each wave

    [Header("Spawn Positions")]
    public List<Transform> relativeSpawnPoints;

    
    Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerStats>().transform;
        CalculateWaveQuota();
        Invoke("SpawnChris", 60f);
    }

    // Update is called once per frame
    void Update()
    {
        if(currentWaveCount < waves.Count && waves[currentWaveCount].spawnCount == waves[currentWaveCount].waveQuota) // Check if wave has ended and next wace should start
        {
            StartCoroutine(BeginNextWave());
        }

        spawnTimer += Time.deltaTime;

        // Checks if it is time to spawn the next enemy 
        if(spawnTimer >= waves[currentWaveCount].spawnInterval)
        {
            spawnTimer = 0f;
            SpawnEnemies();
        }
        
    }

    void SpawnChris()
    {
        Instantiate(chris);
    }

    IEnumerator BeginNextWave()
    {
        // Wait for waveInterval seconds before starting next wave
        yield return new WaitForSeconds(waveInterval);

        // If more waves to start after current wave, move on to next wave
        if (currentWaveCount < waves.Count - 1)
        {
            currentWaveCount++;
            CalculateWaveQuota();
        }
    }

    void CalculateWaveQuota()
    {
        int currentWaveQuota = 0; 
        foreach (var enemyGroup in waves[currentWaveCount].enemyGroups)
        {
            currentWaveQuota += enemyGroup.enemyCount;
        }

        waves[currentWaveCount].waveQuota = currentWaveQuota;
    }

    void SpawnEnemies ()
    {
        if(waves[currentWaveCount].spawnCount < waves[currentWaveCount].waveQuota && !maxEnemiesReached) // If isn't at max enemy count for the wave yet, repeats
        {
            // Spawn each enemy type until all enemy type quotas are filled 
            foreach (var enemyGroup in waves[currentWaveCount].enemyGroups)
            {
                // Check if minimum number of enemies of this type have been spawned 
                if(enemyGroup.spawnCount < enemyGroup.enemyCount)
                {
                    // limits number of enemies at a time
                    if (enemiesAlive >= maxEnemiesAllowed)
                    {
                        maxEnemiesReached = true;
                        return;
                    }
                    Instantiate(enemyGroup.enemyPrefab, player.position + relativeSpawnPoints[Random.Range(0, relativeSpawnPoints.Count)].position, Quaternion.identity);

                    enemyGroup.spawnCount++;
                    waves[currentWaveCount].spawnCount++;
                    enemiesAlive++;
                }
            }
        }

        if (enemiesAlive < maxEnemiesAllowed)
        {
            maxEnemiesReached = false;
        }
    }

    public void OnEnemyKilled()
    {
        enemiesAlive--; 
    }
}
