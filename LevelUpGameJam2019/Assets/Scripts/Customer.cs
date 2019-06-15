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

    [Header("---Colliders---")]
    [SerializeField]
    private Collider pizzaHitBox;

    [SerializeField]
    private Collider customerHitBox;

    [Header("---Audio---")]
    [SerializeField]
    private SoundList hitWithPizzaSounds;

    [SerializeField]
    private SoundList badOrderSounds;

    [SerializeField]
    private SoundList customerSatisfiedSounds;


    //member component references
    private Animator animator;
    private AudioSource audioSource;

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
        //get member references
        animator = GetComponent<Animator>() as Animator;
        audioSource = GetComponent<AudioSource>() as AudioSource;

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
        if(collision.gameObject.CompareTag("PizzaProjectile"))
        {
            var pizzaProjectile = collision.gameObject.GetComponent<PizzaProjectile>() as PizzaProjectile;

            if (collision.collider == pizzaHitBox)//pizza box was hit
            { //compare ingredients
                var pizzaMatches = ComparePizzaToOrder(customerOrder, pizzaProjectile.GetIngredientsOnPizza());

                //animator.SetBool() //tell animator results of pizza

                if (pizzaMatches)
                {
                    //do happy things
                    CustomerSatisfied();
                }

                else
                {
                    //do bad things
                    RejectPizza();
                }

            }
            else if(collision.collider == customerHitBox)//customer hit with pizza
            {
                PlayRandomSound(hitWithPizzaSounds);
            }

        }
        
    }

    private void PlayRandomSound(SoundList soundClipList)
    {
        if (soundClipList.soundClipList.Length > 0)
        {
            audioSource.clip = soundClipList.GetRandomSound();
            audioSource.Play();
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

    private void CustomerSatisfied()
    {
        PlayRandomSound(customerSatisfiedSounds);
        scoreManager.OnCustomerSatisfied();

    }

    private void RejectPizza()
    {
        PlayRandomSound(badOrderSounds);
        scoreManager.OnIncorrectOrder();
    }
}
