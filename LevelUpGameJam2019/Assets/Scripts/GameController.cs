using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    /// <summary>
    /// A static instance of a GameController.
    /// </summary>
    public static GameController gameController;

    /// <summary>
    /// LevelData for the level that is currently loaded.
    /// </summary>
    private static LevelManager currentLevelManager;
    
    [Header("---Player Data---")]
    [SerializeField]
    private PlayerStats playerStats;

    [Header("---Cannons---")]
    [SerializeField]
    private GameObject[] cannonPrefabs = new GameObject[1];

    [SerializeField]
    private int cannonPrefabIndex = 0;

    [Space(10)]
    [Header("---Ships---")]
    [SerializeField]
    private GameObject[] shipPrefabs = new GameObject[1];
    [SerializeField]
    private int shipPrefabIndex = 0;

    /// <summary>
    /// 
    /// </summary>
    [Header("---Scenes---")]
    [SerializeField]
    private string mainMenuSceneName = "MENU";
    
    private void Awake()
    {
        InitSingleton(this);

        SceneManager.sceneLoaded += InitLevel;//subscribe to event to know when a scene has changed.
    }
    
    // Update is called once per frame
    void Update()
    {
        //Handle Quit Game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }

    private void OnApplicationQuit()
    {
        Debug.Log("THANKS FOR PLAYING!");
    }

    private void InitLevel(Scene newScene, LoadSceneMode loadSceneMode)
    {
        GatherSceneReferences();

        var acceptableShipIndex = Mathf.Clamp(shipPrefabIndex, 0, shipPrefabs.Length - 1);//make sure index is w/n array bounds
        var acceptableCannonIndex = Mathf.Clamp(cannonPrefabIndex, 0, cannonPrefabs.Length - 1);//make sure index is w/n array bounds

        if(currentLevelManager)
            currentLevelManager.InitLevel(playerStats, shipPrefabs[acceptableShipIndex], cannonPrefabs[acceptableCannonIndex]);//init level
    }

    /// <summary>
    /// Ensure that only one ever exists in the scene. Sets const static.
    /// </summary>
    /// <param name="gameControllerInstance"></param>
    private static void InitSingleton(GameController gameControllerInstance)
    {
        if (!gameController)
        {
            gameController = gameControllerInstance;
            DontDestroyOnLoad(gameController.gameObject);//IMMORTALITY!
        }
        else//if one already exists
        {
            if(gameController != gameControllerInstance)
            {
                Destroy(gameControllerInstance.gameObject);//THERE CAN ONLY BE ONE!
            }
        }
    }

    /// <summary>
    /// Get handles on Monos in the Scene.
    /// </summary>
    private void GatherSceneReferences()
    {
        GameObject gameObjectQuery;//re-use init
        
        //handle level manager
        gameObjectQuery = GameObject.FindGameObjectWithTag("LevelManager");
        if (gameObjectQuery)
            currentLevelManager = gameObjectQuery.GetComponent<LevelManager>() as LevelManager;
        else
            Debug.LogError("ERROR! No LevelManager in scene!", this);
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
        //go back to the main menu after awhile.
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
