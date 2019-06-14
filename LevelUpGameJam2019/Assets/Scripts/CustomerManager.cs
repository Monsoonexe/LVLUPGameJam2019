using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    [SerializeField]
    private Order[] possibleOrders;

    [SerializeField]
    private Customer[] customersInScene;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private static int SumOrderWeights(Order[] orders)
    {
        var summedWeight = 0;

        foreach(var order in orders)
        {
            summedWeight += order.randomWeight;
        }

        return summedWeight;
    }

    public Order GetNewOrder()
    {
        var randomNumber = Random.Range(0, SumOrderWeights(possibleOrders));
        
        Order selectedOrder = null;

        foreach(var order in possibleOrders)
        {
            if(randomNumber < order.randomWeight)
            {
                selectedOrder = order;
            }

            else
            {
                randomNumber -= order.randomWeight;
            }

        }


        return selectedOrder;
        
    }
}
