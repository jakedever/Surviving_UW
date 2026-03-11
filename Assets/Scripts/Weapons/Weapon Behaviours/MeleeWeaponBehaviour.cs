using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base Script for all Melee Weapons

public class MeleeWeaponBehaviour : MonoBehaviour
{
    public float destroyAfterSeconds;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Destroy(gameObject, destroyAfterSeconds);
    }
}
