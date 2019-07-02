using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class Customer : MonoBehaviour
{
    //static object references
    private static CustomerManager customerManager;
    private static ScoreManager scoreManager;

    [SerializeField]
    private OrderPromptController orderControl;

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

    //other stuff
    private bool orderHasBeenDelivered = false;

    // Start is called before the first frame update
    void Awake()
    {
        GatherReferences();

        if (!customerOrder)
        {
            customerOrder = customerManager.GetNewRandomOrder();
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

            //did it hit pizza box collider?
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
                if (!orderHasBeenDelivered)
                {
                    var pizzaProjectile = collision.gameObject.GetComponent<PizzaProjectile>() as PizzaProjectile;

                    var pizzaMatches = Order.CompareOrderToPizza(customerOrder, pizzaProjectile.GetIngredientsOnPizza());//check that order and pizza ingredients match

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
                else//order has already been delivered and accepted
                {
                    RejectPizza();
                }
            }
            else//hit customer right in the NUTS!
            {
                PlayRandomSound(hitWithPizzaSounds);
                scoreManager.OnCustomerHit();
            }
        }//end if
    }

    private void PlayRandomSound(SoundList soundClipList)
    {
        if (soundClipList.soundClipList.Length > 0)
        {
            audioSource.clip = soundClipList.GetRandomSound();
            audioSource.Play();
        }
    }

    private void CustomerSatisfied()
    {
        PlayRandomSound(customerSatisfiedSounds);

        //add tallys
        scoreManager.OnCustomerSatisfied();
        scoreManager.AddScore(customerOrder.score);

        orderControl.SuccessfulOrder();

        orderHasBeenDelivered = true;
        //Debug.Log("Thanks for the Pizza!!!!");
               

    }

    private void RejectPizza()
    {
        PlayRandomSound(badOrderSounds);
        scoreManager.OnIncorrectOrder();

        //Debug.Log("Hello, this is customer, I want to complain about a messed up order.");
    }

    /// <summary>
    /// Did this Customer receive an Order that matched the desired Order?
    /// </summary>
    /// <returns>True if proper Order was received.</returns>
    public bool WasCustomersOrderDelivered()
    {
        return orderHasBeenDelivered;
    }

    public IngredientsENUM[] GetOrderIngredients()
    {
        return customerOrder.ingredients;
    }
}
