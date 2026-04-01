using UnityEngine;

public class EnergyDrinkBehaviour : MeleeWeaponBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        UnityEngine.Debug.Log("Energy Drink Start()");
        ChangeAttackSpeed(currentMiscellaneous);
    }

    private void OnDestroy() {
        UnityEngine.Debug.Log("Energy Drink OnDestroy()");
        ChangeAttackSpeed(-currentMiscellaneous);
    }

    protected void ChangeAttackSpeed(float amount)
    {
        UnityEngine.Debug.Log("Changed attack speed by amount " + amount);
        if (amount > 0)
        {
            playerStats.CurrentAttackSpeed *= amount;
        }
        else if (amount < 0)
        {
            playerStats.CurrentAttackSpeed /= amount * -1;
        }
        
    }
}
