using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
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
        public int weaponUpgradeIndex;
        public GameObject initialWeapon; // stores prefab of weapon's first level
        public WeaponScriptableObject weaponData;

    }

    [System.Serializable]   
    public class ItemUpgrade
    {
        public int itemUpgradeIndex;
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
    public void LevelUpWeapon (int slotIndex, int upgradeIndex)
    {
        if(weaponSlots.Count > slotIndex) // Makes sure index is within bounds of List
        {
            WeaponController weapon = weaponSlots[slotIndex];
            if (!weapon.weaponData.NextLevelPrefab) // Check and balance for next level = null
            {
                
            }
            GameObject upgradedWeapon = Instantiate(weapon.weaponData.NextLevelPrefab, transform.position, quaternion.identity);
            upgradedWeapon.transform.SetParent(transform); // Set the weapon to be a child of the player
            AddWeapon(slotIndex, upgradedWeapon.GetComponent<WeaponController>());
            Destroy(weapon.gameObject);
            weaponLevels[slotIndex] = upgradedWeapon.GetComponent<WeaponController>().weaponData.Level;

            weaponUpgradesOptions[slotIndex].weaponData = upgradedWeapon.GetComponent<WeaponController>().weaponData;

            if (GameManager.instance != null && GameManager.instance.choosingUpgrades)
            {
                GameManager.instance.EndLevelUp();
            }
        }
    }
    public void LevelUpPassiveItem (int slotIndex,  int upgradeIndex)
    {
        if(passiveItemSlots.Count > slotIndex)
        {
            PassiveItem item = passiveItemSlots[slotIndex];
            GameObject upgradedItem = Instantiate(item.passiveItemData.NextLevelPrefab, transform.position, quaternion.identity);
            upgradedItem.transform.SetParent(transform); // Set the passive item to be a child of the player
            AddPassiveItem(slotIndex, upgradedItem.GetComponent<PassiveItem>());
            Destroy(item.gameObject);
            passiveItemLevels[slotIndex] = upgradedItem.GetComponent<PassiveItem>().passiveItemData.Level;

            itemUpgradesOptions[slotIndex].itemData = upgradedItem.GetComponent<PassiveItem>().passiveItemData;

            if (GameManager.instance != null && GameManager.instance.choosingUpgrades)
            {
                GameManager.instance.EndLevelUp();
            }
        }
    }
    public void ApplyUpgradeOptions()
    {

        List<WeaponUpgrade> availableWeaponUpgrades = new List<WeaponUpgrade>(weaponUpgradesOptions);
        List<ItemUpgrade> availableItemUpgrades = new List<ItemUpgrade>(itemUpgradesOptions);

        foreach (var upgradeOptions in upgradeUIOptions) // For each upgrade option available in the UI
        {


            if(availableWeaponUpgrades.Count == 0 && availableItemUpgrades.Count == 0)
            {
                return;
            }

            int upgradeType;

            if(availableWeaponUpgrades.Count == 0)
            {
                upgradeType = 1;
            }
            else if (availableItemUpgrades.Count == 0)
            {
                upgradeType = 2;
            }
            else
            {
                upgradeType = Random.Range(1, 3); // Randomly choose between weapons and items
            }
            
            
            if (upgradeType == 1)
            {
                // If it randomly picks upgradeType == 1 (a weapon), get a random weapon
                WeaponUpgrade choosenWeaponUpgrade = availableWeaponUpgrades[Random.Range(0, availableWeaponUpgrades.Count)];

                availableWeaponUpgrades.Remove(choosenWeaponUpgrade);

                if (choosenWeaponUpgrade != null) 
                {

                    EnableUpgradeUI(upgradeOptions);
                    bool newWeapon = false;
                    for (int i = 0; i < weaponSlots.Count; i++)
                    {
                        if (weaponSlots[i] != null && weaponSlots[i].weaponData == choosenWeaponUpgrade.weaponData)
                        {
                            // Debug.Log("Found Item already in Player inventory, serving an upgraded one");
                            newWeapon = false;
                            if (!newWeapon)
                            {
                                if (!choosenWeaponUpgrade.weaponData.NextLevelPrefab)
                                {
                                    DisableUpgradeUI(upgradeOptions);
                                    break;
                                }


                                upgradeOptions.upgradeButton.onClick.AddListener(() => LevelUpWeapon(i, choosenWeaponUpgrade.weaponUpgradeIndex)); // Dynamically does events??
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
                ItemUpgrade choosenItemUpgrade = availableItemUpgrades[Random.Range(0, availableItemUpgrades.Count)];

                availableItemUpgrades.Remove(choosenItemUpgrade);

                if (choosenItemUpgrade != null)
                {
                    EnableUpgradeUI(upgradeOptions);
                    bool newItem = false;
                    for (int i = 0; i < passiveItemSlots.Count; i++)
                    {
                        if (passiveItemSlots[i] != null && passiveItemSlots[i].passiveItemData == choosenItemUpgrade.itemData)
                        {
                            newItem = false;
                            if (!newItem)
                            {

                                if (!choosenItemUpgrade.itemData.NextLevelPrefab)
                                {
                                    DisableUpgradeUI(upgradeOptions);
                                    break;
                                }

                                upgradeOptions.upgradeButton.onClick.AddListener(() => LevelUpPassiveItem(i, choosenItemUpgrade.itemUpgradeIndex)); // Dynamically does events??
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
            DisableUpgradeUI(upgradeOptions);
        }
    }

    void RemoveAndApplyUpgrades()
    {
        RemoveUpgradeOptions();
        ApplyUpgradeOptions();
    }

    void DisableUpgradeUI(UpgradeUI ui)
    {
        ui.upgradeNameDisplay.transform.parent.gameObject.SetActive(false);
    }

    void EnableUpgradeUI(UpgradeUI ui)
    {
        ui.upgradeNameDisplay.transform.parent.gameObject.SetActive(true);
    }
}
