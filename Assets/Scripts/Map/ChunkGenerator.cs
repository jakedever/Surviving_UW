using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class ChunkGenerator : MonoBehaviour
{
    public bool flipped;
    public int chunkY;
    private Quaternion tilemapRotation = new Quaternion();
    public BoxCollider2D barrier;

    public List<GameObject> dirtTilemaps;
    public List<GameObject> grassTilemaps;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        barrier = transform.Find("Barrier").GetComponent<BoxCollider2D>();
        GenerateChunk();

    }

    void GenerateChunk ()
    {   
        UnityEngine.Debug.Log("Spawned chunk at " + gameObject.transform.localPosition);
        GameObject dirtTilemap = Instantiate(dirtTilemaps[Random.Range(0, dirtTilemaps.Count)], transform);
        dirtTilemap.transform.position += new UnityEngine.Vector3 (5, 1, 0);

        GameObject grassTilemap = Instantiate(grassTilemaps[Random.Range(0, grassTilemaps.Count)], transform);
        grassTilemap.transform.position += new UnityEngine.Vector3 (5, 1, 0);
        
        if (chunkY == 0)
        {
            flipped = true;
            dirtTilemap.transform.localScale = new Vector3(-1, -1, 1);
            dirtTilemap.transform.localPosition += new Vector3(-10, -2, 0); // Adjust based on your tilemap size

            grassTilemap.transform.localScale = new Vector3(-1, -1, 1);
            grassTilemap.transform.localPosition += new Vector3(-10, -2, 0); // Adjust based on your tilemap size

            barrier.transform.localPosition += new Vector3(0, -9.7f, 0);
        }
    }
}
