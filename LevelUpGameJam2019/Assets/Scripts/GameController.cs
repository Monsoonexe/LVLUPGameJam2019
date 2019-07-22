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
    private GameObject[] cannonPrefabs;

    [SerializeField]
    private int cannonPrefabIndex = 0;

    [Space(10)]
    [Header("---Ships---")]
    [SerializeField]
    private GameObject[] shipPrefabs;
    [SerializeField]
    private int shipPrefabIndex = 0;

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
        InitSingleton(this);

        SceneManager.sceneLoaded += InitLevel;//subscribe to event to know when a scene has changed.
    }

    // Start is called before the first frame update
    void Start()
    {
        GatherSceneReferences();

        currentLevelManager.LevelsEndEvent.AddListener(OnLevelsEnd);//subscribe to event to know when level has ended.

        var acceptableShipIndex = Mathf.Clamp(shipPrefabIndex, 0, shipPrefabs.Length - 1);//make sure index is w/n array bounds
        var acceptableCannonIndex = Mathf.Clamp(cannonPrefabIndex, 0, cannonPrefabs.Length - 1);//make sure index is w/n array bounds

        currentLevelManager.InitLevel(playerStats, shipPrefabs[acceptableShipIndex], cannonPrefabs[acceptableCannonIndex]);//init level

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

    private void InitLevel(Scene newScene, LoadSceneMode loadSceneMode)
    {
        Debug.Log("Init Level shelled...", this);
        Start();

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

        //handle score manager
        gameObjectQuery = GameObject.FindGameObjectWithTag("ScoreManager");
        if (gameObjectQuery)
            scoreManager = gameObjectQuery.GetComponent<ScoreManager>() as ScoreManager;
        else
            Debug.LogError("ERROR! No Score Manager in Scene!");

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
