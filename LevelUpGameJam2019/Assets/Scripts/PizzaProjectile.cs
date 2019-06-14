using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PizzaProjectile : MonoBehaviour
{
    private OrderStruct ingredientsOnThisPizza;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public OrderStruct GetIngredientsOnPizza(){
        return ingredientsOnThisPizza;
    }

}
