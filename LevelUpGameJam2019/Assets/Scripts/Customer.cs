using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Collider))]
public class Customer : MonoBehaviour
{

    private static CustomerManager customerManager;

    [SerializeField]
    private Order customerOrder;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        GatherReferences();

        if (!customerOrder)
        {
            customerOrder = customerManager.GetNewOrder();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GatherReferences()
    {
        animator = GetComponent<Animator>() as Animator;

        //get static reference
        if (!customerManager)
        {
            customerManager = GameObject.FindGameObjectWithTag("CustomerManager").GetComponent<CustomerManager>() as CustomerManager;
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        //what did we get hit by?
        if(col.CompareTag("PizzaProjectile")){
            var pizzaProjectile = col.gameObject.GetComponent<PizzaProjectile>() as PizzaProjectile;

            //compare ingredients
            var pizzaMatches = ComparePizzaToOrder(customerOrder, pizzaProjectile.GetIngredientsOnPizza());

            //animator.SetBool() //tell animator results of pizza

            if(pizzaMatches){//do happy things
                CustomerSatisfied();
            }

            else{//do bad things
                RejectPizza();
            }
        }
        

    }

    private static bool ComparePizzaToOrder(Order customerOrder, OrderStruct pizzaIngredients)
    {
        var ingredientsAllMatch = true;
        if(customerOrder.pepperoni != pizzaIngredients.pepperoni){
            ingredientsAllMatch = false;
        }
        else if(customerOrder.anchovies != pizzaIngredients.anchovies){
            
            ingredientsAllMatch = false;
        }
        else if(customerOrder.cheese != pizzaIngredients.cheese){
            
            ingredientsAllMatch = false;
        }
        else if(customerOrder.sausage != pizzaIngredients.sausage){
            ingredientsAllMatch = false;
        }
        else if(customerOrder.sauce != pizzaIngredients.sauce){
            ingredientsAllMatch = false;
        }
        return ingredientsAllMatch;
    }

    private void CustomerSatisfied(){

    }

    private void RejectPizza(){

    }
}
