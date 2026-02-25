using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class MapController : MonoBehaviour
{

    public List<GameObject> terrainChunks;
    public GameObject player;
    public float checkerRadius;
    Vector3 noTerrainPosition; 
    public LayerMask terrainMask;
    public GameObject currentChunk;
    
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
    }

    void ChunkChecker ()
    {
        if (!currentChunk)
        {
            return; 
        }
        else if (pm.moveDir.x > 0 && pm.moveDir.y == 0)
        {
            // Only want to spawn a chunk if there isn't one already there
            // Checks Right
            if (!Physics2D.OverlapCircle(player.transform.position + new Vector3 (20, 0, 0), checkerRadius))
            {
                noTerrainPosition = player.transform.position + new Vector3 (20, 0, 0);
                SpawnChunk();
            }
        }

        else if (pm.moveDir.x < 0 && pm.moveDir.y == 0)
        {
            // Only want to spawn a chunk if there isn't one already there
            // Checks Left
            if (!Physics2D.OverlapCircle(player.transform.position + new Vector3 (-20, 0, 0), checkerRadius))
            {
                noTerrainPosition = player.transform.position + new Vector3 (-20, 0, 0);
                SpawnChunk();
            }
        }

        else if (pm.moveDir.x == 0 && pm.moveDir.y > 0)
        {
            // Only want to spawn a chunk if there isn't one already there
            // Checks Up
            if (!Physics2D.OverlapCircle(player.transform.position + new Vector3 (0, 20, 0), checkerRadius))
            {
                noTerrainPosition = player.transform.position + new Vector3 (0, 20, 0);
                SpawnChunk();
            }
        }

        else if (pm.moveDir.x == 0 && pm.moveDir.y < 0)
        {
            // Only want to spawn a chunk if there isn't one already there
            // Checks Down
            if (!Physics2D.OverlapCircle(player.transform.position + new Vector3 (0, -20, 0), checkerRadius))
            {
                noTerrainPosition = player.transform.position + new Vector3 (0, -20, 0);
                SpawnChunk();
            }
        }

        // Combination Direcitons
        if (pm.moveDir.x > 0 && pm.moveDir.y > 0)
        {
            // Only want to spawn a chunk if there isn't one already there
            // Checks Up Right
            if (!Physics2D.OverlapCircle(player.transform.position + new Vector3 (20, 20, 0), checkerRadius))
            {
                noTerrainPosition = player.transform.position + new Vector3 (20, 20, 0);
                SpawnChunk();
            }
        }

        else if (pm.moveDir.x < 0 && pm.moveDir.y == 0)
        {
            // Only want to spawn a chunk if there isn't one already there
            // Checks Down Right
            if (!Physics2D.OverlapCircle(player.transform.position + new Vector3 (20, -20, 0), checkerRadius))
            {
                noTerrainPosition = player.transform.position + new Vector3 (20, -20, 0);
                SpawnChunk();
            }
        }

        else if (pm.moveDir.x == 0 && pm.moveDir.y > 0)
        {
            // Only want to spawn a chunk if there isn't one already there
            // Checks Up Left
            if (!Physics2D.OverlapCircle(player.transform.position + new Vector3 (-20, 20, 0), checkerRadius))
            {
                noTerrainPosition = player.transform.position + new Vector3 (-20, 20, 0);
                SpawnChunk();
            }
        }

        else if (pm.moveDir.x == 0 && pm.moveDir.y < 0)
        {
            // Only want to spawn a chunk if there isn't one already there
            // Checks Down Left
            if (!Physics2D.OverlapCircle(player.transform.position + new Vector3 (-20, -20, 0), checkerRadius))
            {
                noTerrainPosition = player.transform.position + new Vector3 (-20, -20, 0);
                SpawnChunk();
            }
        }


    }

    void SpawnChunk ()
    {
        int rand = Random.Range(0, terrainChunks.Count);
        Instantiate(terrainChunks[rand], noTerrainPosition, Quaternion.identity);
    }
}


