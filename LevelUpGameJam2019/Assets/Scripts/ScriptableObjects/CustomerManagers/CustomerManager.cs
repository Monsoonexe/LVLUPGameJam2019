using UnityEngine;

/// <summary>
/// Holds data that is common to all Customers.
/// </summary>
[CreateAssetMenu(fileName = "CustomerManager_", menuName = "ScriptableObjects/Controllers/Customer Manager")]
public class CustomerManager : RichScriptableObject
{
    [Header("= Customer Manager =")]
    [SerializeField]
    private IngredientList ingredientsAvailableToPlayer;

    public IngredientList IngredientsAvailableToPlayer { get { return ingredientsAvailableToPlayer; } }

    /// <summary>
    /// Includes Orders the Player cannot satisfy.
    /// </summary>
    [SerializeField]
    [Tooltip("Includes Orders the Player cannot satisfy.  Will not choose impossible Orders.")]
    private OrdersScriptableList possibleOrders;

    public OrdersScriptableList PossibleOrders { get { return possibleOrders; } }

    [SerializeField]
    private float badOrderReactionTime = 1.0f;

    public float BadOrderReactionTime { get { return customerHitReactionTime; } }//publicly expose, but readonly

    [SerializeField]
    private float customerHitReactionTime = 1.5f;

    public float CustomerHitReactionTime { get { return customerHitReactionTime; } }//publicly expose, but readonly
}
