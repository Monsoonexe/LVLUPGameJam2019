using UnityEngine;

[CreateAssetMenu(fileName = "CustomerManager_", menuName = "ScriptableObjects/Controllers/Customer Manager")]
public class CustomerManager : ScriptableObject
{
    [SerializeField]
    private Order[] possibleOrders;
    
    [Header("---Reaction Delays---")]
    [SerializeField]
    private float badOrderReactionTime = 1.0f;

    public float BadOrderReactionTime { get { return customerHitReactionTime; } }//publicly expose, but readonly

    [SerializeField]
    private float customerHitReactionTime = 1.5f;

    public float CustomerHitReactionTime { get { return customerHitReactionTime; } }//publicly expose, but readonly
    
    /// <summary>
    /// Add up all the weights in this list.
    /// </summary>
    /// <param name="orders"></param>
    /// <returns></returns>
    private static int SumOrderWeights(Order[] orders)
    {
        var summedWeight = 0;

        foreach(var order in orders)
        {
            summedWeight += order.RandomWeight;
        }

        return summedWeight;
    }

    /// <summary>
    /// Get a handle on every Customer in scene an keep in array.
    /// </summary>
    private static Customer[] GatherCustomersInScene()
    {
        var customerGameObjects = GameObject.FindGameObjectsWithTag("Customer");//gather GO

        var customersInScene = new Customer[customerGameObjects.Length];//create array

        for (var i = 0; i < customersInScene.Length; ++i)//fill array
        {
            customersInScene[i] = customerGameObjects[i].GetComponent<Customer>() as Customer;
        }

        return customersInScene;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Order GetNewRandomOrder()
    {
        var randomNumber = Random.Range(0, SumOrderWeights(possibleOrders));

        Order selectedOrder = null;

        foreach(var order in possibleOrders)
        {
            if(randomNumber < order.RandomWeight)
            {
                selectedOrder = order;
                break;//DERP
            }

            else
            {
                randomNumber -= order.RandomWeight;
            }

        }
        
        return selectedOrder;
    }

    /// <summary>
    /// Orders should only be given to customers that have ingredients that are available to the Player.  Remove Orders that have Ingredients not available to Player.
    /// </summary>
    public void RemoveOrdersWithUnavailableIngredients(IngredientSO[] availableIngredients)
    {
        var removedOrderCount = 0;//accumulator

        for(var i = 0; i < possibleOrders.Length; ++i)//for each order,
        {
            for(var j = 0; j < possibleOrders[i].Ingredients.Length; ++j)//for each ingredient on each order
            {
                var ingredientIsInList = false;

                foreach(var availIngredient in availableIngredients)//is that ingredient in this list?
                {
                    if(possibleOrders[i].Ingredients[j] == availIngredient)
                    {
                        ingredientIsInList = true;
                    }
                }

                if (!ingredientIsInList)//
                {
                    possibleOrders[i] = null;//remove Order from list
                    ++removedOrderCount;
                    break;
                }
            }
        }

        var newOrderArray = new Order[possibleOrders.Length - removedOrderCount];

        //fill array
        var newOrderIndex = 0;

        foreach(var order in possibleOrders)
        {
            if(order != null)
            {
                newOrderArray[newOrderIndex] = order;
                ++newOrderIndex;
            }
        }

        possibleOrders = newOrderArray;//assign to new, smaller array
    }
}
