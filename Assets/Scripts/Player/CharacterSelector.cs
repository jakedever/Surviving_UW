using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    // Start is called before the first frame update
    public static CharacterSelector instance;
    public CharacterScriptableObject characterData;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);    
        }
        else
        {
            Debug.LogWarning("Already an instance of CharacterSelector. Newest one deleted");
            Destroy(gameObject);
        }   
    } 

    public static CharacterScriptableObject GetData()
    {
        return instance.characterData;
    }

    public void SelectCharacter(CharacterScriptableObject character)
    {
        characterData = character; 
    }

    public void DestroySingleton()
    {
        instance = null;
        // Destroy(gameObject);
    }
}
