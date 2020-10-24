using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuController : MonoBehaviour
{
    private const string _GameFirstSceneName = "Level1";

    public static int _gameMode = 1;


    public void ClickToStartGameWithOnePlayer()
    {
        Debug.Log("Start Button With 1 Player Was Clicked.");
        _gameMode = 1;
        SceneManager.LoadScene(_GameFirstSceneName);
    }

    public void ClickToStartGameWithTwoPlayers()
    {
        Debug.Log("Start Button With 2 Players Was Clicked.");
        _gameMode = 2;
        SceneManager.LoadScene(_GameFirstSceneName);
    }

    public void ClickToLoadGame()
    {
        Debug.Log("Load Button Clicked.");
        SceneManager.LoadScene(_GameFirstSceneName);
    }

    public void ClickToExitGame()
    {
        Debug.Log("Exit Button Clicked.");
        Application.Quit();
    }

}
