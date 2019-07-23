using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    [SerializeField]
    private Order[] possibleOrders;

    /// <summary>
    /// List of all Customers that exist in Scene at the time of gathering.
    /// </summary>
    private Customer[] customersInScene;

    [Header("---Reaction Delays---")]
    [SerializeField]
    private float badOrderReactionTime = 1.0f;

    [SerializeField]
    private float customerHitReactionTime = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        customersInScene = GatherCustomersInScene();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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

    public void InitReactionDelays(ref float badOrderReactionTime, ref float customerHitReactionTime)
    {
        badOrderReactionTime = this.badOrderReactionTime;
        customerHitReactionTime = this.customerHitReactionTime;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public int GetNumCustomersInScene()
    {
        return customersInScene.Length;
    }

    /// <summary>
    /// Count how many Customers in Level did not receive their Order.
    /// </summary>
    /// <returns></returns>
    public int CountMissedCustomers()
    {
        var satisfiedCustomers = CountSatisfiedCustomers();

        return customersInScene.Length - satisfiedCustomers;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public int CountSatisfiedCustomers()
    {
        var satisfiedCustomers = 0;

        for(var i = 0; i < customersInScene.Length; ++i)
        {
            if (customersInScene[i].WasCustomersOrderDelivered())
            {
                ++satisfiedCustomers;
            }
        }

        return satisfiedCustomers;
    }

    /// <summary>
    /// How many Customers are in this Scene?
    /// </summary>
    /// <returns></returns>
    public int CountCustomersInScene()
    {
        return customersInScene.Length;
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
            //does the order contain an ingredient NOT in list?
            //if yes, remove order
            //++removedOrderCount;
            //if no, continue
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
