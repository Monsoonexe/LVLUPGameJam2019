using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    [SerializeField]
    private GameObject playerObject;

    /// <summary>
    /// 
    /// </summary>
    [Header("---Scenes---")]
    [SerializeField]
    private string mainMenuSceneName = "MainMenu_Scene";
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }

    /// <summary>
    /// [ALPHA] Nothing happens yet.
    /// </summary>
    private void SaveData()
    {
        //nada
    }

    public void QuitGame()
    {
        SaveData();
        Debug.Log("THANKS FOR PLAYING!");

        Application.Quit();//that's all, folks
    }


    /// <summary>
    /// When the Player reaches the end of the level, this handles showing info to the player and then loading the next scene.
    /// </summary>
    public void OnLevelsEnd()
    {
        //migrate Player input 
        //-disable cannon input
        //-enable menu controls (mouse, keyboard)
        //Show player stats
        //wait for Player to move on to next level
        ReturnToMainMenu();
    }

    ///<summary>
    ///Return to the main menu.
    ///</summary>
    public void ReturnToMainMenu()
    {
        SaveData();

        //load next scene
        SceneManager.LoadSceneAsync(mainMenuSceneName);
    }


}
