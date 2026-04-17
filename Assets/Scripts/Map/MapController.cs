using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Numerics;
using UnityEngine;

public class MapController : MonoBehaviour
{

    public List<GameObject> terrainChunks;
    public GameObject player;
    public float checkerRadius;
    UnityEngine.Vector3 noTerrainPosition; 
    public LayerMask terrainMask;
    public GameObject currentChunk;
    public int currentChunkYValue;

    [Header("Optimization")]
    public List<GameObject> spawnedChunks;
    GameObject latestChunk;
    public float maxOptimizationDistance;
    float optDistance;
    float optimizerCooldown;
    public float optimizerCooldownDuration;
    
    PlayerMovement pm;

    // Start is called before the first frame update
    void Start()
    {
        pm = FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        ChunkChecker();
        ChunkOptimizer();
    }

    void ChunkChecker ()
    {
        if (!currentChunk) // Catches null
        {
            return; 
        }
        else if (currentChunk.GetComponent<ChunkGenerator>().chunkY == 1) // If the chunk is on the upper row of chunks
        { // This allows us to prevent chunks from spawning where they shouldn't i.e row 2 or -1
            // Only want to spawn a chunk if there isn't one already there
            // Checks Right
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Right").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Right").position;
                SpawnChunk(1);
            }
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Down Right").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Down Right").position;
                SpawnChunk(0);
            }
        }
        else if (currentChunk.GetComponent<ChunkGenerator>().chunkY == 0) // If the chunk is on the lower row of chunks
        { // This allows us to prevent chunks from spawning where they shouldn't i.e row 2 or -1
            // Only want to spawn a chunk if there isn't one already there
            // Checks Right
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Right").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Right").position;
                SpawnChunk(0);
            }
            // Only want to spawn a chunk if there isn't one already there
            // Checks Up Right
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Up Right").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Up Right").position;
                SpawnChunk(1);
            }
        }
        /*
        else if (pm.moveDir.x > 0 && pm.moveDir.y == 0)
        {
            // Only want to spawn a chunk if there isn't one already there
            // Checks Right
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Right").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Right").position;
                SpawnChunk();
            }
        }

        else if (pm.moveDir.x < 0 && pm.moveDir.y == 0)
        {
            // Only want to spawn a chunk if there isn't one already there
            // Checks Left
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Left").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Left").position;
                SpawnChunk();
            }
        }

        else if (pm.moveDir.x == 0 && pm.moveDir.y > 0)
        {
            // Only want to spawn a chunk if there isn't one already there
            // Checks Up
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Up").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Up").position;
                SpawnChunk();
            }
        }

        else if (pm.moveDir.x == 0 && pm.moveDir.y < 0)
        {
            // Only want to spawn a chunk if there isn't one already there
            // Checks Down
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Down").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Down").position;
                SpawnChunk();
            }
        }

        // Combination Direcitons
        if (pm.moveDir.x > 0 && pm.moveDir.y > 0)
        {
            // Only want to spawn a chunk if there isn't one already there
            // Checks Up Right
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Up Right").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Up Right").position;
                SpawnChunk();
            }
        }

        else if (pm.moveDir.x < 0 && pm.moveDir.y == 0)
        {
            // Only want to spawn a chunk if there isn't one already there
            // Checks Down Right
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Down Right").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Down Right").position;
                SpawnChunk();
            }
        }

        else if (pm.moveDir.x == 0 && pm.moveDir.y > 0)
        {
            // Only want to spawn a chunk if there isn't one already there
            // Checks Up Left
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Up Left").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Up Left").position;
                SpawnChunk();
            }
        }

        else if (pm.moveDir.x == 0 && pm.moveDir.y < 0)
        {
            // Only want to spawn a chunk if there isn't one already there
            // Checks Down Left
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Down Left").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Down Left").position;
                SpawnChunk();
            }
        }
        */
    }

    void SpawnChunk (int y)
    {
        // int rand = Random.Range(0, terrainChunks.Count);
        latestChunk = Instantiate(terrainChunks[0], noTerrainPosition, UnityEngine.Quaternion.identity);
        latestChunk.GetComponent<ChunkGenerator>().chunkY = y;
        spawnedChunks.Add(latestChunk);
    }

    void ChunkOptimizer()
    {

        optimizerCooldown -= Time.deltaTime;

        if (optimizerCooldown <= 0f)
        {
            optimizerCooldown = optimizerCooldownDuration;
        }
        else
        {
            return;
        }


        foreach (GameObject chunk in spawnedChunks)
        {
            optDistance = UnityEngine.Vector3.Distance(player.transform.position, chunk.transform.position);
            if (optDistance > maxOptimizationDistance)
            {
                chunk.SetActive(false);
            }
            else 
            {
                chunk.SetActive(true);
            }
        }        
    }
}


