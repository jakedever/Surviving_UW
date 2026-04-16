using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class ChunkGenerator : MonoBehaviour
{
    public bool flipped;
    private Quaternion tilemapRotation = new Quaternion();

    public List<GameObject> dirtTilemaps;
    public List<GameObject> grassTilemaps;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnChunk();
    }

    void SpawnChunk ()
    {   
        GameObject dirtTilemap = Instantiate(dirtTilemaps[Random.Range(0, dirtTilemaps.Count)], gameObject.transform.position, Quaternion.identity, gameObject.transform);
        GameObject grassTilemap = Instantiate(grassTilemaps[Random.Range(0, grassTilemaps.Count)], gameObject.transform.position, Quaternion.identity, gameObject.transform);
    
        if (flipped)
        {
            dirtTilemap.transform.localScale = new Vector3(-1, -1, 1);
            dirtTilemap.transform.localPosition += new Vector3(-10, -2, 0); // Adjust based on your tilemap size

            grassTilemap.transform.localScale = new Vector3(-1, -1, 1);
            grassTilemap.transform.localPosition += new Vector3(-10, -2, 0); // Adjust based on your tilemap size
        }
    }
}
