using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void SceneChanger(string name)
    {
        if (name == "Game")
        {
            StartCoroutine(DelayedSceneLoad(name));
        }
        else
        {
            SceneManager.LoadScene(name);
        }
        Time.timeScale = 1;
    }

    IEnumerator DelayedSceneLoad(string name)
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(name);
    }

    public void doExitGame()
    {
        Application.Quit();
    }
}
