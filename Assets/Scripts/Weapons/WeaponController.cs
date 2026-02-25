using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Weapon Statistic")]
    public GameObject prefab;

    public float damage;
    public float speed;
    public float cooldownDuration;
    float currentCooldown;
    public int pierce; // Max amount of time it can hit an enemy before being destroyed

    // Start is called before the first frame update
    void Start()
    {
        currentCooldown = cooldownDuration; // At start, set the current cooldown to be the cooldown duration
    }

    // Update is called once per frame
    void Update()
    {
        currentCooldown -= Time.deltaTime;
        if (currentCooldown <= 0f) // Once cooldown becomes zero, attack
        {
            Attack();
        }
    }

    void Attack()
    {
        currentCooldown = cooldownDuration; 
    }
}
