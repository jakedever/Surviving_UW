using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plusCPassiveItem : PassiveItem
{
    protected override void ApplyModifer()
    {
        player.CurrentMight += passiveItemData.Multiplier;
    }
}
