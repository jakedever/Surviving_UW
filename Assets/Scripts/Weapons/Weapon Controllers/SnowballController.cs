using UnityEngine;

public class SnowballController : WeaponController
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();
        GameObject spawnedSnowball = Instantiate(weaponData.Prefab);
        spawnedSnowball.transform.position = transform.position; // Assigns position of spawned knife to be the same as the empty attached to the player object
        spawnedSnowball.GetComponent<SnowballBehaviour>().DirectionChecker(pm.lastMovedVector);
    }

    public void SpawnAOE(UnityEngine.Vector3 location)
    {
        GameObject spawnedAOE = Instantiate(weaponData.AdditionalObjects[0]);
        Destroy(spawnedAOE, 1.5f);
        spawnedAOE.transform.position = location;
    }
}
