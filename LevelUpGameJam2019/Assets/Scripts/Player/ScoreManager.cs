using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    private const string highscoreKey = "HighScore";

    [Header("---Score---")]
    [SerializeField]
    private int playerScore;

    [Header("---Tallys---")]
    [SerializeField]
    private int shotsFired;

    [SerializeField]
    private int customersSatisfied;

    [SerializeField]
    private int customersHit;

    [SerializeField]
    private int incorrectOrdersDelivered;

    [SerializeField]
    private int missedOrders;

    [SerializeField]
    private int sharksFed;

    /// <summary>
    /// Points earned for delivering a Customer's Order successfully.
    /// </summary>
    [Space (5)]
    [Header("---Scoring Factors---")]
    [SerializeField]
    [Tooltip("Points earned for delivering a Customer's Order successfully.")]
    private int customerSatisfiedPoints = 100;

    /// <summary>
    /// Points earned for each Ingredient present on Order.
    /// </summary>
    [SerializeField]
    [Tooltip("Points earned for each Ingredient present on Order.")]
    private int pointsPerIngredient = 25;

    /// <summary>
    /// Increase the point value of each additional Ingredient after the first.
    /// </summary>
    [SerializeField]
    [Tooltip("Increase the point value of each additional Ingredient after the first.")]
    private float additionalIngredientModifier = 1.0f;

    /// <summary>
    /// Deduction caused by Customer's body hit by Player's projectile.
    /// </summary>
    [Space(5)]
    [Header("---Point Deductions---")]
    [SerializeField]
    [Tooltip("Deduction caused by Customer's body hit by Player's projectile.")]
    private int customerHitDeduction = 5;

    /// <summary>
    /// Deduction caused by a wrong Order being delivered to a Customer.
    /// </summary>
    [SerializeField]
    [Tooltip("Deduction caused by a wrong Order being delivered to a Customer.")]
    private int incorrectOrderDeduction = 5;

    /// <summary>
    /// Deduction caused by a Customer not receiving a correct Order.
    /// </summary>
    [SerializeField]
    [Tooltip("Deduction caused by a Customer not receiving a correct Order.")]
    private int missedOrderDeduction = 5;

    [Space(5)]
    [Header("---UI Elements---")]
    [SerializeField]
    private TextMeshProUGUI scoreTMPro;
       
    // Start is called before the first frame update
    void Awake()
    {
      
    }

    private void Start()
    {
        //GatherReferences();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Get a handle on every class reference.
    /// </summary>
    private void GatherReferences()
    {
        //Gather external references
    }

    /// <summary>
    /// Update UI visuals with current values.  Must be called after any changes to values are made.
    /// </summary>
    private void UpdatePlayerScoreText()
    {
        scoreTMPro.text = playerScore.ToString();
    }

    /// <summary>
    /// Checks high score and saves existing score if higher.//initials are always RSO
    /// </summary>
    private void HandleHighScore()
    {
        var currentHighScore = PlayerPrefs.GetInt(highscoreKey);

        if (playerScore > currentHighScore)
        {
            Debug.Log("PLAYER BEAT THE HIGH SCORE! Old score: " + currentHighScore.ToString() + " | new high score: " + playerScore.ToString());

            //set new high score
            PlayerPrefs.SetInt(highscoreKey, playerScore);
        }
    }

    public void OnShotFired()
    {
        ++shotsFired;
    }

    public void OnCustomerSatisfied(int numberOfIngredients)
    {
        ++customersSatisfied;

        playerScore += (int)(customerSatisfiedPoints + (pointsPerIngredient + pointsPerIngredient * additionalIngredientModifier * (numberOfIngredients - 1)));

        UpdatePlayerScoreText();
    }
    
    public void OnIncorrectOrderDelivered()
    {
        ++incorrectOrdersDelivered;
        playerScore -= incorrectOrderDeduction;
        playerScore = playerScore < 0 ? 0 : playerScore;//prevent player score from falling below 0

        //update visuals
        UpdatePlayerScoreText();
    }

    public void OnSharkAtePizza()
    {
        ++sharksFed;
    }

    public void OnCustomerHit()
    {
        ++customersHit;
        playerScore -= customerHitDeduction;
        playerScore = playerScore < 0 ? 0 : playerScore;//prevent player score from falling below 0

        //update visuals
        UpdatePlayerScoreText();
    }

    /// <summary>
    /// Set all scores to 0.
    /// </summary>
    [ContextMenu("Reset Score and Tallys")]
    public void ResetScores()
    {
        playerScore = 0;
        shotsFired = 0;
        customersSatisfied = 0;
        customersHit = 0;
        missedOrders = 0;
        incorrectOrdersDelivered = 0;
        sharksFed = 0;

        UpdatePlayerScoreText();
    }

    public static void UpdateHighScore(TextMeshProUGUI tmpObject)
    {
        tmpObject.text = PlayerPrefs.GetInt(highscoreKey).ToString();
    }

}
