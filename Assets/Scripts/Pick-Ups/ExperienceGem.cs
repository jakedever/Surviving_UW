using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceGem : Pickup
{

    public int experienceGranted;

    public override void Collect()
    {
        UnityEngine.Debug.Log(hasBeenCollected);
        if (hasBeenCollected == true)
        {
            UnityEngine.Debug.Log("Exits");
            return;
        }
        else
        {
            base.Collect();
        }

        UnityEngine.Debug.Log("Adds experience");
        PlayerStats player = FindObjectOfType<PlayerStats>();
        player.IncreaseExperience(experienceGranted);
    }
}
