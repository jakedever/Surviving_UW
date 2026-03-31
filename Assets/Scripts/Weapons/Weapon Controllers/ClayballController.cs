using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClayballController : WeaponController
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }
    // Update is called once per frame
    protected override void Attack()
    {
        base.Attack();
        GameObject spawnedClayball = Instantiate(weaponData.Prefab);
        spawnedClayball.transform.position = transform.position; // Assigns position of spawned knife to be the same as the empty attached to the player object
        spawnedClayball.GetComponent<ClayballBehaviour>().DirectionChecker(pm.lastMovedVector);
    }
}
