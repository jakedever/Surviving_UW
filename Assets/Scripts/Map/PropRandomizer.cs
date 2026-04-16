using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PropRandomizer : MonoBehaviour
{

    public List<GameObject> propSpawnPatterns; // Holds all random location variations of props available
    public List<Transform> propSpawnPoints; // Is assigned a propSpawnPattern and then used to spawn props
    public List<GameObject> propPrefabs; // List of propPrefabs to Instantiate
    private GameObject spawnPattern; // The chosen propSpawnPattern

    // Start is called before the first frame update
    void Start()
    {
        DetermineSpawnPattern();
        SpawnProps();
    }

    void DetermineSpawnPattern()
    {
        spawnPattern = propSpawnPatterns[Random.Range(0, propSpawnPatterns.Count)];
        UnityEngine.Debug.Log(spawnPattern);

        propSpawnPoints = new List<Transform>();
        for (int i = 0; i < spawnPattern.transform.childCount; i++)
        {
            Transform child = spawnPattern.transform.GetChild(i);
            propSpawnPoints.Add(child);
            UnityEngine.Debug.Log("Child " + i + ": " + child.name + " at " + child.position);
        }
    }
    void SpawnProps()
    {
        foreach (Transform sp in propSpawnPoints)
        {
            UnityEngine.Debug.Log(sp.position);
            int rand = Random.Range(0, propPrefabs.Count);
            GameObject prop = Instantiate(propPrefabs[rand], sp.position, Quaternion.identity, sp);
            UnityEngine.Debug.Log("Spawned Prop");
        }
    }
}
