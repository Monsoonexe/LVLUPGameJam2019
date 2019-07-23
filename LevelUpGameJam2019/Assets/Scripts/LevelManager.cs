using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Only one exists in each Level.
/// </summary>
[RequireComponent(typeof(Collider))]
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

    [SerializeField]
    private OrderBuilderMenu orderBuilder;

    //external mono Component references
    private GameController gameController;//should exist before this is loaded in Scene
    private ScoreManager scoreManager;

    private void Awake()
    {
        GatherReferences();
    }

    private void Start()
    {
        returnToMainMenuPrompt.SetActive(false);
    }

    private void GatherReferences()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        scoreManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit end collider.  Ending the game.");
            gameController.OnLevelsEnd();//tell GO this is so and move on to next phase
        }
    }

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

        //configure orderBuilder
        orderBuilder.SetAvailableIngredients(cannonController.AvailableIngredients);
    }

    /// <summary>
    /// Alert the GameController that we want to go to the Main Menu.
    /// </summary>
    public void ReturnToMainMenu()
    {
        gameController.ReturnToMainMenu();
    }
}
