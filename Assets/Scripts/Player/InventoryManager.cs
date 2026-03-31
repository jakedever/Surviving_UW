using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class InventoryManager : MonoBehaviour
{
    public List<WeaponController> weaponSlots = new List<WeaponController>(6);
    public int [] weaponLevels = new int[6];
    public List<Image> weaponUISlots = new List<Image>(6);
    public List<PassiveItem> passiveItemSlots = new List<PassiveItem>(6);
    public int [] passiveItemLevels = new int[6];
    public List<Image> itemsUISlots = new List<Image>(6);

    [System.Serializable]
    public class WeaponUpgrade
    {
        public GameObject initialWeapon; // stores prefab of weapon's first level
        public WeaponScriptableObject weaponData;

    }

    [System.Serializable]   
    public class ItemUpgrade
    {
        public GameObject initialItem; // stores prefab of weapon's first level
        public PassiveItemScriptableObject itemData;
    }

    [System.Serializable]
    public class UpgradeUI
    {
        public Text upgradeNameDisplay;
        public Text upgradeDescriptionDisplay;
        public Image upgradeIcon;
        public Button upgradeButton;
    }

    public List<WeaponUpgrade> weaponUpgradesOptions = new List<WeaponUpgrade>();
    public List<ItemUpgrade> itemUpgradesOptions = new List<ItemUpgrade>();
    public List<UpgradeUI> upgradeUIOptions = new List<UpgradeUI>();
    PlayerStats player;

    void Start()
    {
        player = GetComponent<PlayerStats>();
    }

    public void AddWeapon(int slotIndex, WeaponController weapon)
    {
        weaponSlots[slotIndex] = weapon;
        weaponLevels[slotIndex] = weapon.weaponData.Level;
        weaponUISlots[slotIndex].enabled = true; // enables image component 
        weaponUISlots[slotIndex].sprite = weapon.weaponData.Icon;

        if (GameManager.instance != null && GameManager.instance.choosingUpgrades)
        {
            GameManager.instance.EndLevelUp();
        }
    }
    public void AddPassiveItem(int slotIndex, PassiveItem item)
    {
        passiveItemSlots[slotIndex] = item;
        passiveItemLevels[slotIndex] = item.passiveItemData.Level;
        itemsUISlots[slotIndex].enabled = true; // enables image component 
        itemsUISlots[slotIndex].sprite = item.passiveItemData.Icon;

        if (GameManager.instance != null && GameManager.instance.choosingUpgrades)
        {
            GameManager.instance.EndLevelUp();
        }
    }
    public void LevelUpWeapon (int slotIndex)
    {
        if(weaponSlots.Count > slotIndex) // Makes sure index is within bounds of List
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

            if (GameManager.instance != null && GameManager.instance.choosingUpgrades)
            {
                GameManager.instance.EndLevelUp();
            }
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

            if (GameManager.instance != null && GameManager.instance.choosingUpgrades)
            {
                GameManager.instance.EndLevelUp();
            }
        }
    }
    public void ApplyUpgradeOptions()
    {
        foreach (var upgradeOptions in upgradeUIOptions) // For each upgrade option available in the UI
        {
            int upgradeType = Random.Range(1, 3); // Randomly assign either 1 (Weapon) or 2 (PassiveItem)
            if (upgradeType == 1)
            {
                // If it randomly picks upgradeType == 1 (a weapon), get a random weapon
                WeaponUpgrade choosenWeaponUpgrade = weaponUpgradesOptions[Random.Range(0, weaponUpgradesOptions.Count)];

                if (choosenWeaponUpgrade != null) 
                {
                    bool newWeapon = false;
                    for (int i = 0; i < weaponSlots.Count; i++)
                    {
                        Debug.Log("Tried to add " + choosenWeaponUpgrade.weaponData.Name + " to weapon slot " + i);
                        if (weaponSlots[i] != null && weaponSlots[i].weaponData == choosenWeaponUpgrade.weaponData)
                        {
                            Debug.Log("Found Item already in Player inventory, serving an upgraded one");
                            newWeapon = false;
                            if (!newWeapon)
                            {
                                upgradeOptions.upgradeButton.onClick.AddListener(() => LevelUpWeapon(i)); // Dynamically does events??
                                upgradeOptions.upgradeDescriptionDisplay.text = choosenWeaponUpgrade.weaponData.NextLevelPrefab.GetComponent<WeaponController>().weaponData.Description;
                                upgradeOptions.upgradeNameDisplay.text = choosenWeaponUpgrade.weaponData.NextLevelPrefab.GetComponent<WeaponController>().weaponData.Name;
                            }
                            break;
                        }
                        else
                        {
                            newWeapon = true;
                        }   
                    }
                    if(newWeapon) // Spawn the new weapon
                    {
                        upgradeOptions.upgradeButton.onClick.AddListener(() => player.SpawnWeapon(choosenWeaponUpgrade.initialWeapon));
                        upgradeOptions.upgradeDescriptionDisplay.text = choosenWeaponUpgrade.weaponData.Description;
                        upgradeOptions.upgradeNameDisplay.text = choosenWeaponUpgrade.weaponData.Name;
                    }

                    upgradeOptions.upgradeIcon.sprite = choosenWeaponUpgrade.weaponData.Icon;
                }
            }

            else if (upgradeType == 2)
            {
                ItemUpgrade choosenItemUpgrade = itemUpgradesOptions[Random.Range(0, itemUpgradesOptions.Count)];

                if (choosenItemUpgrade != null)
                {
                    bool newItem = false;
                    for (int i = 0; i < passiveItemSlots.Count; i++)
                    {
                        if (passiveItemSlots[i] != null && passiveItemSlots[i].passiveItemData == choosenItemUpgrade.itemData)
                        {
                            newItem = false;
                            if (!newItem)
                            {
                                upgradeOptions.upgradeButton.onClick.AddListener(() => LevelUpPassiveItem(i)); // Dynamically does events??
                                upgradeOptions.upgradeDescriptionDisplay.text = choosenItemUpgrade.itemData.NextLevelPrefab.GetComponent<PassiveItem>().passiveItemData.Description;
                                upgradeOptions.upgradeNameDisplay.text = choosenItemUpgrade.itemData.NextLevelPrefab.GetComponent<PassiveItem>().passiveItemData.Name;
                            }
                            break;
                        }
                        else
                        {
                            newItem = true;
                        }   
                    }
                    if(newItem) // Spawn the new item
                    {
                        upgradeOptions.upgradeButton.onClick.AddListener(() => player.SpawnPassiveItem(choosenItemUpgrade.initialItem));
                        upgradeOptions.upgradeDescriptionDisplay.text = choosenItemUpgrade.itemData.Description;
                        upgradeOptions.upgradeNameDisplay.text = choosenItemUpgrade.itemData.Name;
                    }

                    upgradeOptions.upgradeIcon.sprite = choosenItemUpgrade.itemData.Icon;
                }
            }
        }
    }
    void RemoveUpgradeOptions ()
    {
        foreach (var upgradeOptions in upgradeUIOptions)
        {
            upgradeOptions.upgradeButton.onClick.RemoveAllListeners();
        }
    }

    void RemoveAndApplyUpgrades()
    {
        RemoveUpgradeOptions();
        ApplyUpgradeOptions();
    }
}
