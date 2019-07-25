using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SkinnedMeshRenderer))]
public class Customer : MonoBehaviour
{
    //static object references
    private static LevelManager levelManager;
   
    [SerializeField]
    private CustomerStateENUM customerState;

    [Header("---ScriptableObject Data---")]

    [SerializeField]//set in Inspector
    [Tooltip("If this reference is null, Customer will seek a random Order.")]
    private Order customerOrder;

    public Order CustomerOrder { get { return customerOrder; } }//publicly accessable, but readonly

    [SerializeField]
    private PossibleOrders possibleOrders;

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
    private ScoreManager scoreManager;

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
        if (!customerOrder)//get an order if don't have one
        {
            GetNewRandomOrderFromCustomerManager();
        }

        RandomizeVisuals();//look different

        orderPromptController.LoadIcons(customerOrder.Ingredients);
        orderPromptController.ToggleVisuals(false);//start with UI hidden until in range of Player
    }
    
    private void OnValidate()
    {
        //get references on this GameObject references
        audioSource = GetComponent<AudioSource>() as AudioSource;
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>() as SkinnedMeshRenderer;

        UpdateVisuals();
    }

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
    
    private void GatherReferences()
    {
        //get references on this GameObject references
        audioSource = GetComponent<AudioSource>() as AudioSource;
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>() as SkinnedMeshRenderer;

        //get references to monos in scene
        if (!levelManager)
        {
            levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>() as LevelManager;
        }
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
        scoreManager.OnCustomerHit();//score
        orderPromptController.OnCustomerHit(customerManager.CustomerHitReactionTime);//update visuals
        HandleRejectOrderCooldown();//handle cooldown coroutine
    }

    private void CustomerSatisfied()
    {
        PlayRandomSound(customerProfile.customerSatisfiedSounds); //audio
        scoreManager.OnCustomerSatisfied(customerOrder.Ingredients.Length);//tally and adjust score
        orderPromptController.OnSuccessfulOrder();//update visuals
        orderHasBeenDelivered = true;//flag to reject all future Orders
        //Debug.Log("Thanks for the Pizza!!!!");
    }

    private void RejectPizza()
    {
        PlayRandomSound(customerProfile.badOrderSounds);//audio
        scoreManager.OnIncorrectOrderDelivered();//score
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

    [ContextMenu("Get New Random Order From Customer Manager")]
    public void GetNewRandomOrderFromCustomerManager()
    {
        GatherReferences();
        customerOrder = possibleOrders.GetNewRandomOrder();
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
