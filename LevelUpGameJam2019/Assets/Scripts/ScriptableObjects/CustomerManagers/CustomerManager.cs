using UnityEngine;

/// <summary>
/// Holds data that is common to all Customers.
/// </summary>
[CreateAssetMenu(fileName = "CustomerManager_", menuName = "ScriptableObjects/Controllers/Customer Manager")]
public class CustomerManager : ScriptableObject
{
    [Header("---Reaction Delays---")]
    [SerializeField]
    private float badOrderReactionTime = 1.0f;

    public float BadOrderReactionTime { get { return customerHitReactionTime; } }//publicly expose, but readonly

    [SerializeField]
    private float customerHitReactionTime = 1.5f;

    public float CustomerHitReactionTime { get { return customerHitReactionTime; } }//publicly expose, but readonly
}
