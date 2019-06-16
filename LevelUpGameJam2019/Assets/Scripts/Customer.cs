using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class Customer : MonoBehaviour
{

    private static CustomerManager customerManager;
    private static ScoreManager scoreManager;

    [SerializeField]
    private Order customerOrder;
    
    [Header("---Audio---")]
    [SerializeField]
    private SoundList hitWithPizzaSounds;

    [SerializeField]
    private SoundList badOrderSounds;

    [SerializeField]
    private SoundList customerSatisfiedSounds;

    [Header("---Colliders---")]
    [SerializeField]
    private Collider customerCollider;

    [SerializeField]
    private Collider pizzaBoxCollider;

    [Header("---Animators---")]
    [SerializeField]
    private Animator pizzaBoxAnimator;

    [SerializeField]
    private Animator customerAnimator;

    //member component references
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
            //if projectile hit pizza box
            var hitPizzaBox = false;

            collision.gameObject.SetActive(false);//expire projectile

            ContactPoint[] contactPointList = new ContactPoint[collision.contactCount];

            collision.GetContacts(contactPointList);//fill array

            foreach(var contact in contactPointList)
            {
                if (contact.thisCollider == pizzaBoxCollider)
                {
                    hitPizzaBox = true;
                    break;
                }
            }

            if (hitPizzaBox)
            {
                var pizzaProjectile = collision.gameObject.GetComponent<PizzaProjectile>() as PizzaProjectile;
                
                var pizzaMatches = ComparePizzaToOrder(customerOrder, pizzaProjectile.GetIngredientsOnPizza());

                pizzaBoxAnimator.SetBool("bDelivered", pizzaMatches); //tell box animator results of pizza
                customerAnimator.SetBool("bDelivered", pizzaMatches); //tell customer animator

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
            else//hit customer
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

        //add tallys
        scoreManager.OnCustomerSatisfied();
        scoreManager.AddScore(customerOrder.score);

        //Debug.Log("Thanks for the Pizza!!!!");

    }

    private void RejectPizza()
    {
        PlayRandomSound(badOrderSounds);
        scoreManager.OnIncorrectOrder();

        //Debug.Log("Hello, this is customer, I want to complain about a messed up order.");
    }
}
