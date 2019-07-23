using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Only one exists in each Level.  Handles initializing the level.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class LevelManager : MonoBehaviour
{
    /// <summary>
    /// Event that gets called when the level is over.
    /// </summary>
    public readonly UnityEvent LevelsEndEvent = new UnityEvent();

    /// <summary>
    /// Player starts attached to this Transform.
    /// </summary>
    [SerializeField]
    [Tooltip("Player starts attached to this Transform.")]
    private Transform playerStartHandle;
    
    [Header("---UI---")]
    [SerializeField]
    private GameObject returnToMainMenuPrompt;

    [Header("---Audio---")]
    [SerializeField]
    private AudioClip levelMusic;

    //member Component refs
    private AudioSource myAudioSource;
    
    //external mono Component references
    private GameController gameController;//should exist before this is loaded in Scene
    private ScoreManager scoreManager;//should exist before this is loaded in Scene
    private CustomerManager customerManager;//should exist before this is loaded in Scene
    private OrderBuilderMenu orderBuilder;//should exist before this is loaded in Scene
    private Transform mainCameraTransform;//should exist before this is loaded in Scene

    private void Awake()
    {
        GatherReferences();
    }

    private void Start()
    {
        returnToMainMenuPrompt.SetActive(false);

        myAudioSource.clip = levelMusic;//load level music
        myAudioSource.Play();
    }

    /// <summary>
    /// Gather references to dependencies.
    /// </summary>
    private void GatherReferences()
    {
        //get member refs
        myAudioSource = GetComponent<AudioSource>();

        //get external references
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        scoreManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();
        customerManager = GameObject.FindGameObjectWithTag("CustomerManager").GetComponent<CustomerManager>();
        orderBuilder = GameObject.FindGameObjectWithTag("OrderBuilder").GetComponent<OrderBuilderMenu>();
        mainCameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    /// <summary>
    /// Procedure to follow when the level has come to an end.
    /// </summary>
    public void OnLevelsEnd()
    {
        returnToMainMenuPrompt.SetActive(true);//show button to return to main menu
        LevelsEndEvent.Invoke();
    }

    /// <summary>
    /// Initialization should be done here to guarantee execution order.
    /// </summary>
    /// <param name="playerData"></param>
    /// <param name="shipPrefab"></param>
    /// <param name="cannonPrefab"></param>
    public void InitLevel(PlayerStats playerData, GameObject shipPrefab, GameObject cannonPrefab)
    {
        var shipGO = Instantiate(shipPrefab, playerStartHandle);//spawn pizza ship
        var shipController = shipGO.GetComponent<PizzaShipController>();//get handle on controller

        var cannonGO = Instantiate(cannonPrefab, shipController.CannonSpawnPoint);//spawn cannon
        var cannonController = cannonGO.GetComponent<StationaryCannon>();//get handle on controller

        //configure cannon
        cannonController.SetOrderBuilder(orderBuilder);
        cannonController.SetScoreManager(scoreManager);

        //configure camera handle
        cannonController.cameraHandle.SetParent(shipGO.transform);//move cannon up a level in hierarchy so doesn't spin around cannon
        mainCameraTransform.SetParent(cannonController.cameraHandle);//hook camera to handle
        mainCameraTransform.position = Vector3.zero;//center to handle

        //configure orderBuilder
        orderBuilder.SetAvailableIngredients(cannonController.AvailableIngredients);

        //configure customer manager
        customerManager.RemoveOrdersWithUnavailableIngredients(cannonController.AvailableIngredients);

        //maybe call the garbage collector here?
    }

    /// <summary>
    /// Alert the GameController that we want to go to the Main Menu.
    /// </summary>
    public void ReturnToMainMenu()
    {
        gameController.ReturnToMainMenu();
    }
}
