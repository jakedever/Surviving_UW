using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PassiveItem : MonoBehaviour
{

    protected PlayerStats player;
    public PassiveItemScriptableObject passiveItemData;

    protected virtual void ApplyModifer()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerStats>();
        ApplyModifer();
    }
}
