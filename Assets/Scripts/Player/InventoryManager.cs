using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public List<WeaponController> weaponSlots = new List<WeaponController>(6);
    public int [] weaponLevels = new int[6];
    public List<Image> weaponUISlots = new List<Image>(6);
    public List<PassiveItem> passiveItemSlots = new List<PassiveItem>(6);
    public int [] passiveItemLevels = new int[6];
    public List<Image> itemsUISlots = new List<Image>(6);

    public void AddWeapon(int slotIndex, WeaponController weapon)
    {
        weaponSlots[slotIndex] = weapon;
        weaponLevels[slotIndex] = weapon.weaponData.Level;
        weaponUISlots[slotIndex].enabled = true; // enables image component 
        weaponUISlots[slotIndex].sprite = weapon.weaponData.Icon;
    }

    public void AddPassiveItem(int slotIndex, PassiveItem item)
    {
        passiveItemSlots[slotIndex] = item;
        passiveItemLevels[slotIndex] = item.passiveItemData.Level;
        itemsUISlots[slotIndex].enabled = true; // enables image component 
        itemsUISlots[slotIndex].sprite = item.passiveItemData.Icon;
    }

    public void LevelUpWeapon (int slotIndex)
    {
        if(weaponSlots.Count > slotIndex)
        {
            WeaponController weapon = weaponSlots[slotIndex];
            if (!weapon.weaponData.NextLevelPrefab) // Check and balance for next level = null
            {
                Debug.LogError("No next level for " + weapon.name);
            }
            GameObject upgradedWeapon = Instantiate(weapon.weaponData.NextLevelPrefab, transform.position, quaternion.identity);
            upgradedWeapon.transform.SetParent(transform); // Set the weapon to be a child of the player
            AddWeapon(slotIndex, upgradedWeapon.GetComponent<WeaponController>());
            Destroy(weapon.gameObject);
            weaponLevels[slotIndex] = upgradedWeapon.GetComponent<WeaponController>().weaponData.Level;
        }
    }

    public void LevelUpPassiveItem (int slotIndex)
    {
        if(passiveItemSlots.Count > slotIndex)
        {
            PassiveItem item = passiveItemSlots[slotIndex];
            GameObject upgradedItem = Instantiate(item.passiveItemData.NextLevelPrefab, transform.position, quaternion.identity);
            upgradedItem.transform.SetParent(transform); // Set the passive item to be a child of the player
            AddPassiveItem(slotIndex, upgradedItem.GetComponent<PassiveItem>());
            Destroy(item.gameObject);
            passiveItemLevels[slotIndex] = upgradedItem.GetComponent<PassiveItem>().passiveItemData.Level;
        }
    }
}
