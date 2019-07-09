using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    private const string highscoreKey = "HighScore";

    [Header("---Score---")]
    [SerializeField]
    private int playerScore;

    [SerializeField]
    private LeaderboardSO leaderboard;

    [Header("---Windows---")]
    [SerializeField]
    private LevelEndReadoutController levelEndReadoutController;

    [SerializeField]
    private NewHighScoreWindowManager newHighScoreWindowManager;

    [Header("---Tallys---")]
    [SerializeField]
    private int shotsFired;

    [SerializeField]
    private int customersSatisfied;

    [SerializeField]
    private int customersHit;

    [SerializeField]
    private int incorrectOrders;

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

    [SerializeField]
    private TextMeshProUGUI highScoreTMPro;

    //external Mono References
    private CustomerManager customerManager;

    private void Start()
    {
        GatherReferences();

        UpdatePlayerScoreText();
        UpdateHighScoreText();
    }

    private void GatherReferences()
    {
        if (!customerManager)
        {
            customerManager = GameObject.FindGameObjectWithTag("CustomerManager").GetComponent<CustomerManager>() as CustomerManager;
        }
    }

    /// <summary>
    /// Update UI visuals with current values.  Must be called after any changes to values are made.
    /// </summary>
    private void UpdatePlayerScoreText()
    {
        scoreTMPro.text = playerScore.ToString();
    }

    private void UpdateHighScoreText()
    {
        highScoreTMPro.text = leaderboard.highScore.ToString();
    }

    #region Get Tally Methods

    public int GetTallyCustomersSatisfied()
    {
        return customersSatisfied;
    }

    /// <summary>
    /// Of all the Customers in the Scene, how many of them got an incorrect Order?
    /// </summary>
    /// <returns></returns>
    public int GetTallyMissedOrders()
    {
        return customerManager.CountMissedCustomers();
    }

    public int GetTallyCustomersHit()
    {
        return customersHit;
    }

    public int GetTallyIncorrectOrders()
    {
        return incorrectOrders;
    }

    public int GetTallySharksFed()
    {
        return sharksFed;
    }

    public int GetTallyShotsFired()
    {
        return shotsFired;
    }

    #endregion

    public int GetPlayerScore()
    {
        return playerScore;
    }

    /// <summary>
    /// Get a copy of the entry with given index.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public LeaderboardEntry GetEntry(int index)
    {
        return leaderboard.GetEntry(index);
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
        ++incorrectOrders;

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
        //reset values
        playerScore = 0;
        shotsFired = 0;
        customersSatisfied = 0;
        customersHit = 0;
        missedOrders = 0;
        incorrectOrders = 0;
        sharksFed = 0;

        //update UI
        UpdatePlayerScoreText();
        UpdateHighScoreText();
    }

    /// <summary>
    /// Things to do when the level has ended.
    /// </summary>
    public void OnLevelsEnd()
    {
        //check if Player got a new high score
        
        if (leaderboard.IsScoreOnLeaderboard(playerScore))
        {
            newHighScoreWindowManager.gameObject.SetActive(true);
        }

        else
        {
            levelEndReadoutController.gameObject.SetActive(true);
        }

    }

    /// <summary>
    /// Called by Okay Button.
    /// </summary>
    /// <param name="playerName"></param>
    public void ConfirmNewHighScoreName(string playerName)
    {
        leaderboard.SubmitNewScore(new LeaderboardEntry(playerName, playerScore));
        levelEndReadoutController.gameObject.SetActive(true);
    }

}
