using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Collider))]
public class Customer : MonoBehaviour
{

    private static CustomerManager customerManager;
    private static ScoreManager scoreManager;

    [SerializeField]
    private Order customerOrder;

    private Animator animator;

    // Start is called before the first frame update
    void Awake()
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

        if (!scoreManager)
        {
            scoreManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>() as ScoreManager;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        //what did we get hit by?
        if(collision.gameObject.CompareTag("PizzaProjectile")){
            var pizzaProjectile = collision.gameObject.GetComponent<PizzaProjectile>() as PizzaProjectile;

            //compare ingredients
            var pizzaMatches = ComparePizzaToOrder(customerOrder, pizzaProjectile.GetIngredientsOnPizza());

            //animator.SetBool() //tell animator results of pizza

            if(pizzaMatches)
            {
                //do happy things
                CustomerSatisfied();
                scoreManager.OnCustomerSatisfied();
            }

            else
            {
                //do bad things
                RejectPizza();
                scoreManager.OnIncorrectOrder();
            }
        }
        
    }

    private static bool ComparePizzaToOrder(Order customerOrder, OrderStruct pizzaIngredients)
    {
        var ingredientsAllMatch = true;
        
        //quick exit
        if(customerOrder.ingredients.Length != pizzaIngredients.ingredients.Length)
        {
            ingredientsAllMatch = false;
        }

        else
        {
            //verify each ingredient customer wanted
            foreach(var custIngredient in customerOrder.ingredients)
            {
                var ingredientOnPizza = false;

                //is indeed on pizza
                foreach (var pizzaIngredi in pizzaIngredients.ingredients)
                {

                    if(custIngredient == pizzaIngredi)
                    {
                        ingredientOnPizza = true;
                    }
                }

                if (!ingredientOnPizza)
                {
                    ingredientsAllMatch = false;
                    break;
                }
            }
        }

        return ingredientsAllMatch;
    }

    private void CustomerSatisfied(){

    }

    private void RejectPizza(){

    }
}
