using UnityEngine;

/// <summary>
/// Possible orders that can be given during a level or time.  prebuilt by Developer.
/// </summary>
[CreateAssetMenu(fileName = "PossibleOrders_", menuName = "ScriptableObjects/Possible Orders")]
public class PossibleOrders : RichScriptableObject
{
    [Header("= Possible Orders =")]
    [SerializeField]
    [Tooltip("Pre-built by Developer.")]
    private Order[] possibleOrders = new Order[0];

    public Order[] Orders { get { return possibleOrders; } }

    private void OnEnable()
    {
        ValidateArray();
    }

    private void ValidateArray()
    {
        var nullCount = 0;

        foreach(var order in possibleOrders)
        {
            if(order == null)
            {
                ++nullCount;
            }
        }

        if(nullCount > 0)
        {
            Debug.Log("Removing null refs from this array.", this);
            var newArray = new Order[possibleOrders.Length - nullCount];
            var newArrayIndex = 0;

            for(var i = 0; i < possibleOrders.Length; ++i)
            {
                if(possibleOrders[i] != null)
                {
                    newArray[newArrayIndex] = possibleOrders[i];
                    ++newArrayIndex;
                }
            }

            possibleOrders = newArray;
        }
    }
}
