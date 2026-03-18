using System.Collections;
using System.Collections.Generic;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
   public CharacterScriptableObject characterData;

   // Current Stats
   float currentHealth;
   float currentRecovery;
   float currentMoveSpeed;
   float currentMight;
   float currentProjectileSpeed;

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
        currentHealth = characterData.MaxHealth;
        currentRecovery = characterData.Recovery;
        currentMoveSpeed = characterData.MoveSpeed;
        currentMight = characterData.Might;
        currentProjectileSpeed = characterData.ProjectileSpeed; 
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
             
        }
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
    public float invicibilityDuration; 
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

    public void TakeDamage(float dmg)
    {
        // If the player is not current invincible, take damage
        if (!isInvincible)
        {
            currentHealth -= dmg;
            invincibilityTimer = invicibilityDuration;
            isInvincible = true;

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
