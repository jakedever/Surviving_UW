using UnityEngine;

public class EnergyDrinkController : WeaponController
{

    UnityEngine.Vector3 verticalShift = new UnityEngine.Vector3(0, 1, 0);
     // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();
        GameObject spawnedEnergyDrink = Instantiate(weaponData.Prefab);
        spawnedEnergyDrink.transform.position = transform.position + verticalShift;
        spawnedEnergyDrink.transform.parent = transform;
    }
}
