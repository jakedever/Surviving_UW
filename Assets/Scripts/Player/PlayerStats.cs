using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Jobs.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public CharacterScriptableObject characterData;

    // Current Stats
    // [HideInInspector]
    [Header("Current Stats")]
    float currentHealth;
    float currentRecovery;
    float currentMoveSpeed;
    float currentMight;
    public float currentAttackSpeed = 1f;
    float currentProjectileSpeed;
    float currentMagnet;
    Animator animator;

    #region Current Stats Properties
    public float CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            if(currentHealth != value)
            {
                currentHealth = value;
                // Update realtime value of the stat
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentHealthDisplay.text = "Health: " + currentHealth;
                }
            }
        }
    }

    public float CurrentRecovery
    {
        get { return currentRecovery; }
        set
        {
            if(currentRecovery != value)
            {
                currentRecovery = value;
                // Update realtime value of the stat
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentRecoveryDisplay.text = "Recovery: " + currentRecovery;
                }
            }
        }
    }

    public float CurrentMoveSpeed
    {
        get { return currentMoveSpeed; }
        set
        {
            if(currentMoveSpeed != value)
            {
                currentMoveSpeed = value;
                // Update realtime value of the stat
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentMovespeedDisplay.text = "Move Speed: " + currentMoveSpeed;
                }
            }
        }
    }

    public float CurrentMight
    {
        get { return currentMight; }
        set
        {
            if(currentMight != value)
            {
                currentMight = value;
                // Update realtime value of the stat
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentMightDisplay.text = "Might: " + currentMight;
                }
            }
        }
    }

    public float CurrentAttackSpeed
    {
        get { return currentAttackSpeed; }
        set
        {
            if(currentAttackSpeed != value)
            {
                currentAttackSpeed = value;
                // Update realtime value of the stat
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentMightDisplay.text = "Attack Speed: " + currentAttackSpeed;
                }
            }
        }
    }

    public float CurrentProjectileSpeed
    {
        get { return currentProjectileSpeed; }
        set
        {
            if(currentProjectileSpeed != value)
            {
                currentProjectileSpeed = value;
                // Update realtime value of the stat
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentProjectileSpeedDisplay.text = "Projectile Speed: " + currentProjectileSpeed;
                }
            }
        }
    }

    public float CurrentMagnet
    {
        get { return currentMagnet; }
        set
        {
            if(currentMagnet != value)
            {
                currentMagnet = value;
                // Update realtime value of the stat
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentMagnetDisplay.text = "Magnet: " + currentMagnet;
                }
            }
        }
    }

    #endregion

    // Experience Management
    [Header("Experience/Level")]
    public int experience = 0;
    public int level = 1;
    public int experienceCap;

    [System.Serializable] // Lets Unity access this and have its information pesist/be used
    public class LevelRange
    {
        public int startLevel;
        public int endLevel;
        public int experienceCapIncrease;   
    }

    public List<LevelRange> levelRanges;
    InventoryManager inventory; 
    public int weaponIndex;
    public int passiveItemsIndex;
    
    [Header("UI")]
    public Image healthBar;
    public Image expBar;
    public Text levelText;

    public GameObject secondWeapon;
    public GameObject firstPassiveItem, secondPassiveItem;

    void Awake()
    {
        characterData = CharacterSelector.GetData();
        CharacterSelector.instance.DestroySingleton();

        inventory = GetComponent<InventoryManager>();
        animator = characterData.animation;

        CurrentHealth = characterData.MaxHealth;
        CurrentRecovery = characterData.Recovery;
        CurrentMoveSpeed = characterData.MoveSpeed;
        CurrentMight = characterData.Might;
        CurrentProjectileSpeed = characterData.ProjectileSpeed; 
        CurrentMagnet = characterData.Magnet;

    }

    void Start()
    {
        // Debug.Log(GameManager.instance == null);
        GameManager.instance.currentHealthDisplay.text = "Health: " + currentHealth;
        GameManager.instance.currentRecoveryDisplay.text = "Recovery: " + currentRecovery;
        GameManager.instance.currentMovespeedDisplay.text = "Movespeed: " + currentMoveSpeed;
        GameManager.instance.currentMightDisplay.text = "Might: " + currentMight;
        GameManager.instance.currentProjectileSpeedDisplay.text = "Projectile Speed: " + currentProjectileSpeed;
        GameManager.instance.currentMagnetDisplay.text = "Magnet: " + currentMagnet;

        GameManager.instance.AssignChosenCharacterUI(characterData);

        // Pass starting weapon
        SpawnWeapon(characterData.StartingWeapon);
        // SpawnWeapon(secondWeapon); // Allows for manual attachment

        // SpawnPassiveItem(firstPassiveItem);
        // SpawnPassiveItem(secondPassiveItem);
        // Initialize the experience require for level ups to the first value in the list 
        experienceCap = levelRanges[0].experienceCapIncrease;

        UpdateHealthBar();
        UpdateExpBar();
        UpdateLevelText();
    }

    private void Update()
    {
        if (invincibilityTimer > 0)
        {
            invincibilityTimer -= Time.deltaTime;
        }
        else if (isInvincible)
        {
             isInvincible = false;
        }

        Recover();

    }
    public void IncreaseExperience(int amount)
    {
        experience += amount;

        LevelUpChecker();

        UpdateExpBar();
    }

    void LevelUpChecker ()
    {
        if (experience >= experienceCap)
        {
            level++;
            experience -= experienceCap;

            int experienceCapIncrease = 0;
            foreach (LevelRange range in levelRanges)
            {
                if (level >= range.startLevel && level <= range.endLevel)
                {
                    experienceCapIncrease = range.experienceCapIncrease;
                    break; 
                }
            }
            experienceCap += experienceCapIncrease;

            UpdateLevelText();

            GameManager.instance.StartLevelUp();
        }
    }
    // I-frames
    [Header("I-Frames")]
    public float invicibilityDuration = 1; 
    float invincibilityTimer;
    bool isInvincible;
    public void RestoreHealth(int amount)
    {
        if (CurrentHealth < characterData.MaxHealth)
        {
            CurrentHealth += amount;

            if (CurrentHealth > characterData.MaxHealth)
            {
                CurrentHealth = characterData.MaxHealth; 
            }
        }
    }
    void Recover ()
    {
        if (CurrentHealth < characterData.MaxHealth) {
            CurrentHealth += CurrentRecovery * Time.deltaTime; 

            if (CurrentHealth > characterData.MaxHealth)
            {
                CurrentHealth = characterData.MaxHealth;
            }
        }
    }
    public void SpawnWeapon(GameObject weapon)
    {
        if(weaponIndex >= inventory.weaponSlots.Count - 1)
        {
            Debug.LogError("Inventory slot already full");
            return; 
        }
        
        //Spawn starting weapon
        GameObject spawnedWeapon = Instantiate(weapon, transform.position, Quaternion.identity);
        spawnedWeapon.transform.SetParent(transform);
        inventory.AddWeapon(weaponIndex, spawnedWeapon.GetComponent<WeaponController>()); // Add the weapon to it's appropriate iventory slot
        Debug.Log("Added " + spawnedWeapon.name + " to character");

        weaponIndex++;
    }
    public void SpawnPassiveItem(GameObject item)
    {
        if(passiveItemsIndex >= inventory.passiveItemSlots.Count - 1)
        {
            Debug.LogError("Passive item slot already full");
            return; 
        }
        
        //Spawn starting weapon
        GameObject spawnedItem = Instantiate(item, transform.position, Quaternion.identity);
        spawnedItem.transform.SetParent(transform);
        inventory.AddPassiveItem(passiveItemsIndex, spawnedItem.GetComponent<PassiveItem>()); // Add the weapon to it's appropriate iventory slot
        Debug.Log("Added " + spawnedItem.name + " to character");

        passiveItemsIndex++;
    }

    void UpdateHealthBar()
    {
        // Update the health bar 
        healthBar.fillAmount = currentHealth / characterData.MaxHealth;
    }

    void UpdateExpBar()
    {
        expBar.fillAmount = (float)experience / experienceCap;
    }

    void UpdateLevelText()
    {
        levelText.text = "LV " + level.ToString();
    }
    public void TakeDamage(float dmg)
    {
        // If the player is not current invincible, take damage
        if (!isInvincible)
        {
            isInvincible = true;
            CurrentHealth -= dmg;
            invincibilityTimer = invicibilityDuration;
            

            Debug.Log("Player took " + dmg + " damage");
        }
        
        if (CurrentHealth <= 0)
        {
            Kill();
        }

        UpdateHealthBar();
    }
    public void Kill()
    {
        if(!GameManager.instance.isGameOver)
        {
            GameManager.instance.AssignLevelReachedUI(level);
            GameManager.instance.AssignChosenWeaponsAndItemsUI(inventory.weaponUISlots, inventory.itemsUISlots);
            GameManager.instance.GameOver();
        }
        // Destroy(gameObject);
    }
}
