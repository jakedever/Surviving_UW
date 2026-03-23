using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Weapon Statistic")]
    public WeaponScriptableObject weaponData;
    float currentCooldown;

    protected PlayerMovement pm;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        pm = FindObjectOfType<PlayerMovement>();
        currentCooldown = weaponData.CooldownDuration; // At start, set the current cooldown to be the cooldown duration
    }

    // Update is called once per frame
    protected void Update()
    {
        currentCooldown -= Time.deltaTime;
        if (currentCooldown <= 0f) // Once cooldown becomes zero, attack
        {
            Attack();
        }
    }

    protected virtual void Attack()
    {
        currentCooldown = weaponData.CooldownDuration; 
    }
}
