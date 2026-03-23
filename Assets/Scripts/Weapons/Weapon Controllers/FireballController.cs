using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballController : WeaponController
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();
        // Instantiate a fireball (make a copy of the prefab)
        // assign position (transform information)
        // 

        GameObject spawnedFireball = Instantiate(weaponData.Prefab);

        spawnedFireball.transform.position = transform.position; // Assigns position of spawned knife to be the same as the empty attached to the player object
        spawnedFireball.GetComponent<FireballBehaviour>().DirectionChecker(pm.lastMovedVector);
    }
}
