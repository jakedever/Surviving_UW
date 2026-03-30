using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StimulantsPassiveItem : PassiveItem
{
    protected override void ApplyModifer()
    {
        player.CurrentMoveSpeed *= 1 + passiveItemData.Multiplier / 100f;
    }
}
