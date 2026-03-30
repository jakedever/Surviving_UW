using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StimulantsPassiveItem : PassiveItem
{
    protected override void ApplyModifer()
    {
        player.currentMoveSpeed *= 1 + passiveItemData.Multiplier / 100f;
    }
}
