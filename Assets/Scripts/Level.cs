using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    int totalScenes;

    public void Start()
    {
        totalScenes = SceneManager.sceneCountInBuildSettings;
    }
    public void LoadGameOver()
    {
        StartCoroutine(GameOver());        
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(totalScenes - 1); //we keep game over as last sceen
    }

    public void LoadStartMenu()
    {
        GameSession[] currentGameSessions = FindObjectsOfType<GameSession>();
        foreach( GameSession gameSession in currentGameSessions)
        {
            Destroy(gameSession);
        }
        SceneManager.LoadScene(0);
    }
    public void LoadNextScene()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene + 1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

}
