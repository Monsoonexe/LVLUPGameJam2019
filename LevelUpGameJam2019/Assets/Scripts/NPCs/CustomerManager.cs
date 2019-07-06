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
            summedWeight += order.randomWeight;
        }

        return summedWeight;
    }

    /// <summary>
    /// Get a handle on every Customer in scene an keep in array.
    /// </summary>
    private void GatherCustomersInScene()
    {
        var customerGameObjects = GameObject.FindGameObjectsWithTag("Customers");//gather GO

        customersInScene = new Customer[customerGameObjects.Length];//create array

        for (var i = 0; i < customersInScene.Length; ++i)//fill array
        {
            customersInScene[i] = customerGameObjects[i].GetComponent<Customer>() as Customer;
        }
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
    /// 
    /// </summary>
    /// <returns></returns>
    public Order GetNewRandomOrder()
    {
        var randomNumber = Random.Range(0, SumOrderWeights(possibleOrders));

        Order selectedOrder = null;

        foreach(var order in possibleOrders)
        {
            if(randomNumber < order.randomWeight)
            {
                selectedOrder = order;
                break;//DERP
            }

            else
            {
                randomNumber -= order.randomWeight;
            }

        }
        
        return selectedOrder;
    }

    /// <summary>
    /// Create a new Order SO (avoiding duplicates) and add it to folder.)
    /// </summary>
    public void RandomlyGenerateNewOrder()
    {
        Debug.Log("NOT YET IMPLEMENTED");
    }
}
