using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
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

    public void AttackAndSplit(UnityEngine.Vector3 dir, Transform location, int splitsLeft)
    {
        // Clayball that splits left
        GameObject spawnedClayball_1 = Instantiate(weaponData.Prefab);

        ClayballBehaviour cb1Behaviour = spawnedClayball_1.GetComponent<ClayballBehaviour>();
        Transform cb1Transform = spawnedClayball_1.GetComponent<Transform>();

        cb1Behaviour.currentMiscellaneous = splitsLeft; // Ensures that the spliting doesn't happen infinitely 
        
        cb1Behaviour.direction = UnityEngine.Quaternion.Euler(0, 0, 45) * dir;

        cb1Transform.position = location.position;
        cb1Transform.localScale *= 0.5f; // Cuts size of prefab in half
        cb1Transform.Rotate(0, 0, 45); // Visually rotates clayball sprite 45 degrees counter clockwise

        // Clayball that splits right
        GameObject spawnedClayball_2 = Instantiate(weaponData.Prefab);

        ClayballBehaviour cb2Behaviour = spawnedClayball_2.GetComponent<ClayballBehaviour>();
        Transform cb2Transform = spawnedClayball_2.GetComponent<Transform>();

        cb2Behaviour.currentMiscellaneous = splitsLeft; // Ensures that the spliting doesn't happen infinitely 
        
        cb2Behaviour.direction = UnityEngine.Quaternion.Euler(0, 0, -45) * dir;

        cb2Transform.position = location.position;
        cb2Transform.localScale *= 0.5f; // Cuts size of prefab in half
        cb2Transform.Rotate(0, 0, -45); // Visually rotates clayball sprite 45 degrees clockwise
    }
}
