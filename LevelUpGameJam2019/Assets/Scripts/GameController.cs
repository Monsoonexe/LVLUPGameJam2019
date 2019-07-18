﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private StationaryCannon pizzaCannon;

    [SerializeField]
    private GameObject ingredientBuilderCanvas;

    [SerializeField]
    private GameObject returnToMainMenuPrompt;

    /// <summary>
    /// 
    /// </summary>
    [Header("---Scenes---")]
    [SerializeField]
    private string mainMenuSceneName = "MainMenu_Scene";

    //external Component references
    private ScoreManager scoreManager;

    private void Awake()
    {
        GatherReferences();
    }

    // Start is called before the first frame update
    void Start()
    {
        returnToMainMenuPrompt.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }

    private void GatherReferences()
    {
        if (!scoreManager)
        {
            scoreManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>() as ScoreManager;
        }

        if (!pizzaCannon)
        {
            pizzaCannon = GameObject.FindGameObjectWithTag("Player").GetComponent<StationaryCannon>() as StationaryCannon;
        }
    }

    public void ReloadLevel()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
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
        pizzaCannon.gameObject.SetActive(false);
        ingredientBuilderCanvas.SetActive(false);
        //Show player stats
        //wait for Player to move on to next level
        scoreManager.OnLevelsEnd();
        returnToMainMenuPrompt.SetActive(true);
    }

    ///<summary>
    ///Return to the main menu.
    ///</summary>
    public void ReturnToMainMenu()
    {
        //load next scene
        SceneManager.LoadSceneAsync(mainMenuSceneName);
    }

}
