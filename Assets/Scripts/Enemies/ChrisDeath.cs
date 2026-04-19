using UnityEngine;

public class ChrisDeath : MonoBehaviour
{
    public SoundEffectManager soundEffectManager;
    public PlayerStats playerStats;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        soundEffectManager = FindObjectOfType<SoundEffectManager>();    
        playerStats = FindObjectOfType<PlayerStats>();

        if (playerStats.characterData.Name == "Omar")
        {
            soundEffectManager.PlaySound("Omar Chris");
        }
        else if (playerStats.characterData.Name == "Mateo")
        {
            soundEffectManager.PlaySound("Mateo Chris");
        }
        else if (playerStats.characterData.Name == "Cyn")
        {
            soundEffectManager.PlaySound("Cyn Chris");
        }
    }

    private void OnDestroy()
    {
        GameManager.instance.GameOver();
    }
}
