using UnityEngine;

[CreateAssetMenu(fileName = "ScoreData_", menuName = "ScriptableObjects/Score Data")]
public class ScoreData : RichScriptableObject
{
    [Header("= Score Data =")]
    [SerializeField]
    private int playerScore;

    public int PlayerScore { get { return playerScore; } }//public readonly

    [SerializeField]
    private LeaderboardSO leaderboard;

    public LeaderboardSO Leaderboard { get { return leaderboard; } }//public readonly

    [Header("---Game Events---")]
    [SerializeField]
    private GameEvent levelBeginEvent;

    [SerializeField]
    private GameEvent levelEndEvent;

    [SerializeField]
    private GameEvent scoreChangedEvent;

    /// <summary>
    /// Event that fires every time the Player shoots the weapon.
    /// </summary>
    [SerializeField]
    [Tooltip("Event that fires every time the Player shoots the weapon.")]
    private GameEvent shotFiredEvent;

    [Header("---Tallys---")]
    [SerializeField]
    private int shotsFired;

    public int ShotsFired { get { return shotsFired; } }//public readonly

    [SerializeField]
    private int customersSatisfied;

    public int CustomersSatisfied { get { return customersSatisfied; } }//readonly

    [SerializeField]
    private int customersHit;

    public int CustomersHit { get { return customersHit; } }//readonly

    [SerializeField]
    private int incorrectOrders;

    public int IncorrectOrders { get { return incorrectOrders; } }//readonly

    [SerializeField]
    private int missedOrders;

    public int MissedOrders { get { return missedOrders; } }//reaodnly

    [SerializeField]
    private int sharksFed;

    public int SharksFed { get { return sharksFed; } }//readonly

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

    private void OnEnable()//when entering Play Mode
    {
        levelBeginEvent.AddListener(OnLevelBegin);
        levelEndEvent.AddListener(OnLevelsEnd);
    }

    /// <summary>
    /// How many Customers did not receive a correct order.
    /// </summary>
    /// <returns></returns>
    public int GetTallyMissedOrders()
    {//this could be improved
        var customerGameObjs = GameObject.FindGameObjectsWithTag("Customer");

        var satisfiedCount = 0;

        for (var i = 0; i < customerGameObjs.Length; ++i)
        {
            var customer = customerGameObjs[i].GetComponent<Customer>();
            if (customer.IsSatisfied)
                ++satisfiedCount;
        }

        return satisfiedCount;
    }
    
    public void OnCustomerSatisfied(int numberOfIngredients)
    {
        ++customersSatisfied;

        playerScore += (int)(customerSatisfiedPoints + (pointsPerIngredient + pointsPerIngredient * additionalIngredientModifier * (numberOfIngredients - 1)));

        scoreChangedEvent.Raise();
    }

    public void OnShotFired()
    {
        ++shotsFired;
    }
    
    public void OnIncorrectOrderDelivered()
    {
        ++incorrectOrders;

        playerScore -= incorrectOrderDeduction;
        playerScore = playerScore < 0 ? 0 : playerScore;//prevent player score from falling below 0

        scoreChangedEvent.Raise();//update visuals
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

        scoreChangedEvent.Raise();//update visuals
    }

    public void OnLevelBegin()
    {
        ResetScores();
    }

    public void OnLevelsEnd()
    {
        //ResetScores();
    }

    /// <summary>
    /// Set all scores to 0.
    /// </summary>
    [ContextMenu("Reset Score and Tallys")]
    public void ResetScores()
    {
        Debug.Log("SCORES RESET!", this);

        //reset values
        playerScore = 0;
        shotsFired = 0;
        customersSatisfied = 0;
        customersHit = 0;
        missedOrders = 0;
        incorrectOrders = 0;
        sharksFed = 0;

        scoreChangedEvent.Raise();
    }
}
