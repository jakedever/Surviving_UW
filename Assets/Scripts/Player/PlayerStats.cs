using System.Collections;
using System.Collections.Generic;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerStats : MonoBehaviour
{
    public CharacterScriptableObject characterData;

    // Current Stats
    // [HideInInspector]
    public float currentHealth;
    [HideInInspector]
    public float currentRecovery;
    [HideInInspector]
    public float currentMoveSpeed;
    [HideInInspector]
    public float currentMight;
    [HideInInspector]
    public float currentProjectileSpeed;
    [HideInInspector]
    public float currentMagnet;

    public List<GameObject> spawnedWeapons;

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

    void Awake()
    {
        characterData = CharacterSelector.GetData();
        CharacterSelector.instance.DestroySingleton();

        currentHealth = characterData.MaxHealth;
        currentRecovery = characterData.Recovery;
        currentMoveSpeed = characterData.MoveSpeed;
        currentMight = characterData.Might;
        currentProjectileSpeed = characterData.ProjectileSpeed; 
        currentMagnet = characterData.Magnet;

        // Pass starting weapon
        SpawnWeapon(characterData.StartingWeapon);
    }

    void Start()
    {
        // Initialize the experience require for level ups to the first value in the list 
        experienceCap = levelRanges[0].experienceCapIncrease;
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
        }
    }

    // I-frames
    [Header("I-Frames")]
    public float invicibilityDuration = 1; 
    float invincibilityTimer;
    bool isInvincible;

    public void RestoreHealth(int amount)
    {
        if (currentHealth < characterData.MaxHealth)
        {
            currentHealth += amount;

            if (currentHealth > characterData.MaxHealth)
            {
                currentHealth = characterData.MaxHealth; 
            }
        }
    }

    void Recover ()
    {
        if (currentHealth < characterData.MaxHealth) {
            currentHealth += currentRecovery * Time.deltaTime; 

            if (currentHealth > characterData.MaxHealth)
            {
                currentHealth = characterData.MaxHealth;
            }
        }
    }

    public void SpawnWeapon(GameObject weapon)
    {
        //Spawn starting weapon
        GameObject spawnedWeapon = Instantiate(weapon, transform.position, Quaternion.identity);
        spawnedWeapon.transform.SetParent(transform);
        spawnedWeapons.Add(spawnedWeapon);
    }

    public void TakeDamage(float dmg)
    {
        // If the player is not current invincible, take damage
        if (!isInvincible)
        {
            isInvincible = true;
            currentHealth -= dmg;
            invincibilityTimer = invicibilityDuration;
            

            Debug.Log("Player took " + dmg + " damage");
        }
        
        if (currentHealth <= 0)
        {
            Kill();
        }
    }

    public void Kill()
    {
        Debug.Log("*steve harvey voice* KILL");
        Destroy(gameObject);
    }
}
