using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SkinnedMeshRenderer))]
public class Customer : MonoBehaviour
{
    //static object references
    private static CustomerManager customerManager;
    private static ScoreManager scoreManager;
    private static LevelManager levelManager;

    //delays
    private static float badOrderDelay = 1.0f;
    private static float customerHitDelay = 1.5f;

    [SerializeField]
    private CustomerStateENUM customerState;

    [Header("---Order Stuff---")]
    [SerializeField]
    private OrderPromptController orderPromptController;

    [SerializeField]//set in Inspector
    [Tooltip("If this reference is null, Customer will ask CustomerManager for a random Order.")]
    private Order customerOrder;

    public Order CustomerOrder { get { return customerOrder; } }//publicly accessable, but readonly

    /// <summary>
    /// Holds visual mesh and material and sound effects.
    /// </summary>
    [Header("---Profile---")]
    [SerializeField]
    private CustomerProfile customerProfile;

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

    private void OnEnable()
    {
        levelManager.LevelsEndEvent.AddListener(OnLevelsEnd);//subscribe to end level event
    }

    private void OnDisable()
    {
        levelManager.LevelsEndEvent.RemoveListener(OnLevelsEnd);//subscribe to end level event
    }

    private void OnLevelsEnd()
    {
        orderPromptController.ToggleVisuals(false);//hide visuals
        canDeliver = false;//stop taking orders
        //do something else
    }

    /// <summary>
    /// Init how long delays should be.
    /// </summary>
    private static void InitDelays()
    {
        if (customerManager)//if not, use default values
        {
            customerManager.InitReactionDelays(ref badOrderDelay, ref customerHitDelay);
        }
    }

    private void GatherReferences()
    {
        //get references on this GameObject references
        audioSource = GetComponent<AudioSource>() as AudioSource;
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>() as SkinnedMeshRenderer;

        //get references to monos in scene
        if (!customerManager)
        {
            customerManager = GameObject.FindGameObjectWithTag("CustomerManager").GetComponent<CustomerManager>() as CustomerManager;
            InitDelays();//or use defaults
        }
        
        if (!scoreManager)
        {
            scoreManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>() as ScoreManager;
        }

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

        orderPromptController.OnCustomerHit(customerHitDelay);//update visuals
        
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
        
        orderPromptController.OnFailedOrder(badOrderDelay);//update visuals

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
        yield return new WaitForSeconds(badOrderDelay);
        canDeliver = true;
    }

    /// <summary>
    /// Did this Customer receive an Order that matched the desired Order?
    /// </summary>
    /// <returns>True if proper Order was received.</returns>
    public bool WasCustomersOrderDelivered()
    {
        return orderHasBeenDelivered;
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
        customerOrder = customerManager.GetNewRandomOrder();
    }
}
