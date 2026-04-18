using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public enum GameState
    {
        Gameplay,
        LevelUp,
        Paused,
        GameOver
    }

    // Stores current state of the game
    [Header("Game States")]
    public GameState currentState;
    public GameState previousState;

    [Header("Damage Text Settings")]
    public Canvas damageTextCanvas;
    public float textFontSize = 15f;
    public TMP_FontAsset textFont;
    public Camera referenceCamera;


    [Header("Screens")]
    public GameObject pauseScreen;
    public GameObject resultsScreen;
    public GameObject levelUpScreen;

    [Header("Current Display Values")]  
    public TMPro.TMP_Text currentHealthDisplay;
    public TMPro.TMP_Text currentRecoveryDisplay;
    public TMPro.TMP_Text currentMovespeedDisplay;
    public TMPro.TMP_Text currentMightDisplay;  
    public TMPro.TMP_Text currentProjectileSpeedDisplay;
    public TMPro.TMP_Text currentMagnetDisplay;

    [Header("Results Screen Displays")]
    public Image chosenCharacterImage;
    public TMPro.TMP_Text chosenCharacterName;
    public TMPro.TMP_Text levelReachedDisplay;
    public TMPro.TMP_Text timeSurvivedDisplay;
    public List<Image> chosenWeaponsUI = new List<Image>(6);
    public List<Image> chosenItemsUI = new List<Image>(6);


    public bool isGameOver = false;
    public bool choosingUpgrades;

    [Header("Stopwatch")]
    public float timeLimit; // the time limit in seconds
    float stopwatchTime; // Current time since beginning of game
    public TMPro.TMP_Text stopwatchDisplay;
    // Reference to the player's game object
    public GameObject playerObject;
    void Awake()
    {
        if (instance == null)
        {
            // Debug.Log("GameManager should be instantiated");
            instance = this;
            // Debug.Log(instance == null);
        }
        else
        {
            Debug.LogWarning("Already an instance of GameManager, latest was deleted");
            Destroy(gameObject);
        }
        DisableScreens();
    }
    private void Update()
    {
        switch(currentState)
        {
            case GameState.Gameplay:
                CheckForPauseAndResume();
                UpdateStopwatch();
                break;

            case GameState.LevelUp:
                if(!choosingUpgrades)
                {
                    choosingUpgrades = true;
                    Time.timeScale = 0f; // Pauses the game
                    Debug.Log("Upgrades shown");
                    levelUpScreen.SetActive(true);
                }
                break;

            case GameState.Paused:
                CheckForPauseAndResume();
                break;

            case GameState.GameOver:
                if(!isGameOver)
                {
                    isGameOver = true;
                    Time.timeScale = 0f;
                    Debug.Log("GameOver State tripped");
                    DisplayResults();
                }
                break;

            default:
                Debug.LogWarning("VALUE OF currentState DOES NOT EXIST");
                break;
        }
    } 
    
    public static void GenerateFloatingText(string text, Transform target, float duration = 1f, float speed = 1f)
    {
        if (!instance.damageTextCanvas) return;

        if (!instance.referenceCamera) instance.referenceCamera = Camera.main;

        instance.StartCoroutine(instance.GenerateFloatingTextCoroutine(
            text, target, duration, speed
        )); 
    }
    
    IEnumerator GenerateFloatingTextCoroutine(string text, Transform target, float duration = 1f, float speed = 50f)
    {
        GameObject textObj = new GameObject("Damage Floating Text");
        RectTransform rect = textObj.AddComponent<RectTransform>();
        TextMeshProUGUI tmPro = textObj.AddComponent<TextMeshProUGUI>();

        tmPro.text = text;
        tmPro.horizontalAlignment = HorizontalAlignmentOptions.Center;
        tmPro.verticalAlignment = VerticalAlignmentOptions.Middle;
        tmPro.fontSize = textFontSize;
        if (textFont) tmPro.font = textFont;
        
        rect.position = referenceCamera.WorldToScreenPoint(target.position);
        textObj.transform.SetParent(instance.damageTextCanvas.transform);


        Destroy(textObj, duration);

        WaitForEndOfFrame w = new WaitForEndOfFrame();
        float t = 0f;
        float yOffset = 0f;

        while (t < duration) // Change the condition to t < duration
        {
            yield return w;
            t += Time.deltaTime;

            tmPro.color = new UnityEngine.Color(tmPro.color.r, tmPro.color.g, tmPro.color.b, 1 - t / duration);

            // For panning up
            // yOffset += speed * Time.deltaTime;
            // rect.position = referenceCamera.WorldToScreenPoint(target.position + new UnityEngine.Vector3(0, yOffset));
        }
    }
    
    public void ChangeState(GameState newState)
    {
        currentState = newState;
    }
    public void PauseGame()
    {
        if (currentState != GameState.Paused)
        {
            previousState = currentState;
            ChangeState(GameState.Paused);
            Time.timeScale = 0f; // Sets rate at which the game progresses to 0
            pauseScreen.SetActive(true);
            Debug.Log("Game has been paused");
        }
        
    }
    public void ResumeGame()
    {
        if (currentState == GameState.Paused)
        {
            ChangeState(previousState);
            Time.timeScale = 1f; // Sets rate at which the game progresses to 0
            pauseScreen.SetActive(false);
            Debug.Log("Game has been resumed");
        }       
    }
    void CheckForPauseAndResume ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape pressed");
            if(currentState == GameState.Paused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
    void DisableScreens()
    {
        pauseScreen.SetActive(false);
        resultsScreen.SetActive(false);
        levelUpScreen.SetActive(false);
    }
    public void GameOver()
    {
        timeSurvivedDisplay.text = stopwatchDisplay.text;
        ChangeState(GameState.GameOver);
    }
    void DisplayResults()
    {
        resultsScreen.SetActive(true);
        Debug.Log("Game results displayed");
    }
    public void AssignChosenCharacterUI (CharacterScriptableObject chosenCharacterData)
    {
        chosenCharacterImage.sprite = chosenCharacterData.Icon;
        chosenCharacterName.text = chosenCharacterData.name; // tutorial uses .name, Should it be .Name??
    }
    public void AssignLevelReachedUI(int levelReachedData)
    {
        levelReachedDisplay.text = levelReachedData.ToString();
    }
    public void AssignChosenWeaponsAndItemsUI (List<Image> chosenWeaponData, List<Image> chosenItemData)
    {
        if (chosenWeaponData.Count !=  chosenWeaponsUI.Count || chosenItemData.Count != chosenItemsUI.Count)
        {
            Debug.Log("Chosen weapons and item data lists have different lengths, smtg fucked up");
            return;
        }

        for (int i = 0; i < chosenWeaponsUI.Count; i++) // Iterate through Weapons, assign their sprites to the images on result screen
        {
            if (chosenWeaponData[i].sprite)
            {
                chosenWeaponsUI[i].enabled = true;
                chosenWeaponsUI[i].sprite = chosenWeaponData[i].sprite;
            }
            else
            {
                chosenWeaponsUI[i].enabled = false;
            }
        }

        for (int i = 0; i < chosenItemsUI.Count; i++) // Iterate through Items, assign their sprites to the images on result screen
        {
            if (chosenItemData[i].sprite)
            {
                chosenItemsUI[i].enabled = true;
                chosenItemsUI[i].sprite = chosenItemData[i].sprite;
            }
            else
            {
                chosenItemsUI[i].enabled = false;
            }
        }
    }
    void UpdateStopwatch()
    {
        stopwatchTime += Time.deltaTime;

        UpdateStopwatchDisplay();

        if (stopwatchTime >= timeLimit)
        {
            playerObject.SendMessage("Kill");            
        }
    }
    void UpdateStopwatchDisplay()
    {
        int minutes = Mathf.FloorToInt(stopwatchTime/60);
        int seconds = Mathf.FloorToInt(stopwatchTime%60);

        stopwatchDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);


    }
    public void StartLevelUp ()
    {
        ChangeState(GameState.LevelUp);
        playerObject.SendMessage("RemoveAndApplyUpgrades");
    }
    public void EndLevelUp ()
    {
        choosingUpgrades = false;
        Time.timeScale = 1f;
        levelUpScreen.SetActive(false);
        ChangeState(GameState.Gameplay);
    }
}