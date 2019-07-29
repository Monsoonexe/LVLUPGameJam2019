using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SkinnedMeshRenderer))]
[SelectionBase]
public class Customer : MonoBehaviour
{   
    [SerializeField]
    private CustomerStateENUM customerState;

    [Header("---ScriptableObject Data---")]

    [SerializeField]//set in Inspector
    [Tooltip("If this reference is null, Customer will seek a random Order.")]
    private Order customerOrder;

    public Order CustomerOrder { get { return customerOrder; } }//publicly accessable, but readonly

    /// <summary>
    /// Includes Orders the Player cannot satisfy.
    /// </summary>
    [SerializeField]
    [Tooltip("Includes Orders the Player cannot satisfy.  Will not choose impossible Orders.")]
    private OrdersScriptableList possibleOrders;

    /// <summary>
    /// Actually draw orders from this pool.
    /// </summary>
    private Order[] availableOrders;

    /// <summary>
    /// Holds visual mesh and material and sound effects.
    /// </summary>
    [SerializeField]
    private CustomerProfile customerProfile;

    /// <summary>
    /// Brain that manages 
    /// </summary>
    [SerializeField]
    private CustomerManager customerManager;

    [SerializeField]
    private ScoreData scoreData;

    [Header("---Game Events---")]
    [SerializeField]
    private GameEvent wrongOrderReceivedEvent;

    [Header("---UI---")]
    [SerializeField]
    private OrderPromptController orderPromptController;
    
    [Header("---Colliders---")]
    [SerializeField]
    private Collider customerCollider;

    [SerializeField]
    private Collider pizzaBoxCollider;

    [Header("---Animators---")]
    [SerializeField]
    private Animator customerAnimator;

    [SerializeField]
    private Animator pizzaBoxAnimator;
        
    //member component references
    private AudioSource audioSource;
    private SkinnedMeshRenderer skinnedMeshRenderer;

    //other stuff
    private bool orderHasBeenDelivered = false;
    public bool IsSatisfied { get {return  orderHasBeenDelivered; } }

    /// <summary>
    /// Customer will reject all Orders and hide Ingredients list when mad.
    /// </summary>
    private bool canDeliver = true;

    private Coroutine coroutine_rejectOrders;

    // Start is called before the first frame update
    void Awake()
    {
        GatherReferences();
    }

    private void Start()
    {
        RemoveOrdersWithUnavailableIngredients();

        if (!customerOrder)//get an order if don't have one
        {
            //cull from list orders that the Player can't satisfy
            GetNewRandomOrder();
        }

        RandomizeVisuals();//look different

        orderPromptController.LoadIcons(customerOrder.Ingredients);
        orderPromptController.ToggleVisuals(false);//start with UI hidden until in range of Player
    }
    
    //private void OnValidate()
    //{
    //    //get references on this GameObject references
    //    audioSource = GetComponent<AudioSource>() as AudioSource;
    //    skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>() as SkinnedMeshRenderer;

    //    UpdateVisuals();
    //}

    private void OnCollisionEnter(Collision collision)
    {
        //what did we get hit by?
        if (collision.gameObject.CompareTag("PizzaProjectile"))
        {
            //if projectile hit pizza box
            var hitPizzaBox = false;

            collision.gameObject.SetActive(false);//expire projectile

            var contactPointList = new ContactPoint[collision.contactCount];

            collision.GetContacts(contactPointList);//fill array

            //did it hit pizza box collider?
            foreach (var contact in contactPointList)
            {
                if (contact.thisCollider == pizzaBoxCollider)
                {
                    hitPizzaBox = true;
                    break;
                }
            }

            if (hitPizzaBox)
            {
                if (!orderHasBeenDelivered && canDeliver)
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
                OnCustomerHit();
            }
        }//end if
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))//if we collided with Player
        {
            orderPromptController.ToggleVisuals(true);//show
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))//if we collided with Player
        {
            orderPromptController.ToggleVisuals(false);//hide
        }
    }

    /// <summary>
    /// Get references on this GameObject.
    /// </summary>
    private void GatherReferences()
    {
        audioSource = GetComponent<AudioSource>();
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
    }

    [ContextMenu("Update Visuals")]
    private void UpdateVisuals()
    {
        skinnedMeshRenderer.sharedMesh = customerProfile.characterVisualMesh;
    }

    private void PlayRandomSound(SoundList soundClipList)
    {
        if (soundClipList.soundClipList.Length > 0)
        {
            audioSource.clip = soundClipList.GetRandomSound();
            audioSource.Play();
        }
    }
    
    /// <summary>
    /// The Customer was hit with a PizzaProjectile. REACT! roll a d6.
    /// </summary>
    private void OnCustomerHit()
    {
        PlayRandomSound(customerProfile.hitWithPizzaSounds);//audio
        scoreData.OnCustomerHit();//score
        orderPromptController.OnCustomerHit(customerManager.CustomerHitReactionTime);//update visuals
        HandleRejectOrderCooldown();//handle cooldown coroutine
    }

    private void CustomerSatisfied()
    {
        PlayRandomSound(customerProfile.customerSatisfiedSounds); //audio
        scoreData.OnCustomerSatisfied(customerOrder.Ingredients.Length);//tally and adjust score
        orderPromptController.OnSuccessfulOrder();//update visuals
        orderHasBeenDelivered = true;//flag to reject all future Orders
        //Debug.Log("Thanks for the Pizza!!!!");
    }

    private void RejectPizza()
    {
        PlayRandomSound(customerProfile.badOrderSounds);//audio
        wrongOrderReceivedEvent.Raise();//score
        orderPromptController.OnFailedOrder(customerManager.CustomerHitReactionTime);//update visuals
        HandleRejectOrderCooldown();//handle cooldown
        //Debug.Log("Hello, this is customer, I want to complain about a messed up order.");
    }

    /// <summary>
    /// Start new, restart if already existing to avoid overlap.
    /// </summary>
    private void HandleRejectOrderCooldown()
    {
        //handle cooldown coroutine
        if (coroutine_rejectOrders != null)//stop existing
        {
            StopCoroutine(coroutine_rejectOrders);
        }
        coroutine_rejectOrders = StartCoroutine(RejectOrdersWhileMad());//start new
    }

    /// <summary>
    /// Add up all the weights in this list.
    /// </summary>
    /// <param name="orders"></param>
    /// <returns></returns>
    private static int SumOrderWeights(Order[] orders)
    {
        var summedWeight = 0;

        foreach (var order in orders)
        {
            summedWeight += order.RandomWeight;
        }

        return summedWeight;
    }
    
    /// <summary>
    /// Orders should only be given to customers that have ingredients that are available to the Player.  Remove Orders that have Ingredients not available to Player.
    /// </summary>
    [ContextMenu("Remove Orders With Unavailable Ingredients")]
    public void RemoveOrdersWithUnavailableIngredients()
    {
        var workingOrderCollection = new Order[possibleOrders.list.Count];
        possibleOrders.list.CopyTo(workingOrderCollection, 0);

        var removedOrderCount = 0;//accumulator

        for (var i = 0; i < workingOrderCollection.Length; ++i)//for each order,
        {
            for (var j = 0; j < workingOrderCollection[i].Ingredients.Length; ++j)//for each ingredient on each order
            {
                var ingredientIsInList = false;

                foreach (var availIngredient in customerManager.IngredientsAvailableToPlayer.ingredients)//is that ingredient in this list?
                {
                    if (workingOrderCollection[i].Ingredients[j] == availIngredient)
                    {
                        ingredientIsInList = true;
                    }
                }

                if (!ingredientIsInList)//
                {
                    workingOrderCollection[i] = null;//remove Order from list
                    ++removedOrderCount;
                    break;
                }
            }
        }

        var newOrderArray = new Order[workingOrderCollection.Length - removedOrderCount];

        //fill array
        var newOrderIndex = 0;

        foreach (var order in workingOrderCollection)
        {
            if (order != null)
            {
                newOrderArray[newOrderIndex] = order;
                ++newOrderIndex;
            }
        }

        availableOrders = newOrderArray;//assign to new, smaller array
    }

    /// <summary>
    /// Reject Orders while mad.
    /// </summary>
    /// <returns></returns>
    private IEnumerator RejectOrdersWhileMad()
    {
        canDeliver = false;
        yield return new WaitForSeconds(customerManager.BadOrderReactionTime);
        canDeliver = true;
    }
    
    [ContextMenu("Randomize Visuals")]
    public void RandomizeVisuals()
    {
        GatherReferences();
        skinnedMeshRenderer.material = customerProfile.GetRandomMaterial();
    }

    [ContextMenu("Get New Random Order")]
    public void GetNewRandomOrder()
    {
        var randomNumber = Random.Range(0, SumOrderWeights(availableOrders));

        Order selectedOrder = null;

        foreach (var order in availableOrders)
        {
            if (randomNumber < order.RandomWeight)
            {
                selectedOrder = order;
                break;//DERP
            }
            else
            {
                randomNumber -= order.RandomWeight;
            }
        }
        customerOrder = selectedOrder;
    }

    /// <summary>
    /// Procedure to follow at the end of the level.
    /// </summary>
    public void OnLevelsEnd()
    {
        orderPromptController.ToggleVisuals(false);//hide visuals
        canDeliver = false;//stop taking orders
        //do something else
        //tally order received or not.
    }
}
