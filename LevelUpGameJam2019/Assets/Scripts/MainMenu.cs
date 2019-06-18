using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
 
    public void PlayGame()
    {
        Debug.Log("Playgame");
        SceneManager.LoadScene("MAIN");
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("MENU");
    }

    public void ExitGame()
    {
        Debug.Log("Exit");
        Application.Quit();
    }
}
