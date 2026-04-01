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
    protected PlayerStats playerStats;

    protected virtual void Awake()
    {
        pm = FindObjectOfType<PlayerMovement>();
        playerStats = FindFirstObjectByType<PlayerStats>();
    }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        currentCooldown = weaponData.CooldownDuration; // At start, set the current cooldown to be the cooldown duration
    }

    // Update is called once per frame
    protected virtual void Update()
    {       
        currentCooldown -= Time.deltaTime;
        // UnityEngine.Debug.Log("CooldownDuration = "+ weaponData.CooldownDuration + " || " + currentCooldown + " = currentCooldown. weaponData name is Energy Drink: " + (weaponData.Name == "Energy Drink"));
        UnityEngine.Debug.Log(currentCooldown + " = currentCooldown. ReducedCooldown = " + weaponData.CooldownDuration / playerStats.CurrentAttackSpeed);
        
        if (weaponData.Name != "Energy Drink" && currentCooldown <= 0f /*+ weaponData.CooldownDuration / playerStats.CurrentAttackSpeed*/ )
        {
            // UnityEngine.Debug.Log("Not Energydrink update");
            Attack();
        }
        else if (currentCooldown <= 0f /*+ (weaponData.CooldownDuration / playerStats.CurrentAttackSpeed)*/) // Once cooldown becomes zero, attack
        {
            // UnityEngine.Debug.Log(currentCooldown);
            Attack();
        }
    }

    protected virtual void Attack()
    {
        currentCooldown = weaponData.CooldownDuration; 
    }
}
