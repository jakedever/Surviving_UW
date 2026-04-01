using UnityEngine;

public class WrenchController : WeaponController
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();
        GameObject spawnedWrench = Instantiate(weaponData.Prefab);
        spawnedWrench.transform.position = transform.position; // Assigns position of spawned knife to be the same as the empty attached to the player object
        spawnedWrench.GetComponent<WrenchBehaviour>().DirectionChecker(pm.lastMovedVector);
    }
}
