using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
public class StationaryCannon : MonoBehaviour
{
    /// <summary>
    /// Stop adjusting rotation if target within this angle.
    /// </summary>
    private const float optimalAngle = .05f;

    /// <summary>
    /// Object to which Camera will be hooked.
    /// </summary>
    [Tooltip("Object to which Camera will be hooked.  Set this to be camera point.")]
    public Transform cameraHandle;

    [Header("---Ingredients---")]
    [SerializeField]
    private IngredientSO[] availableIngredients;

    public IngredientSO[] AvailableIngredients { get { return availableIngredients; } }//publicly accessible, but only gets a copy.
    
    [Header("Projectile Stuff")]
    [Tooltip("Allows the cannon to continue tracking but disables trigger.")]
    public bool disableCannonTrigger = false;//used by Tutorial

    [SerializeField]
    private GameObject projectilePrefab;

    /// <summary>
    /// "If more than one barrel, iterates through each point."
    /// </summary>
    [Tooltip("If more than one barrel, iterates through each point.")]
    [SerializeField]
    private Transform[] projectileSpawnPoints;
    
    [SerializeField]
    private float projectileForce = 5.0f;

    [SerializeField]
    private float secondsBetweenShots = 3f;

    private float nextShootTime = 0;

    private int projectileSpawnPointIndex = 0;

    [Header("---Turret Target Tracking---")]
    [SerializeField]
    private Transform objectToTrack;

    /// <summary>
    /// Pitch on X //CAN BE THE SAME OBJECT AS BASE, IF NO SEPARATE TURRET.  WHOLE OBJECT WILL ROTATE AS EXPECTED
    /// </summary>
    [SerializeField]
    [Tooltip("Pitch on X //CAN BE THE SAME OBJECT AS BASE, IF NO SEPARATE TURRET.  WHOLE OBJECT WILL ROTATE AS EXPECTED")]
    private Transform turretTransform;

    [SerializeField]
    private int minPitch = 15;

    [SerializeField]
    private int maxPitch = 75;

    [SerializeField]
    private float swivelSpeed = 3f;

    [Header("---Audio---")]
    [SerializeField]
    private AudioClip cannonFireSound;
    
    //GO components
    private Animator myAnimator;
    private AudioSource myAudioSource;
    private Rigidbody myRigidbody;
    private Transform myBaseTransform;//swivel on Y 
    
    //object pooling stuff
    private const int projectilePoolSize = 15;
    private readonly Queue<GameObject> projectilePool = new Queue<GameObject>(projectilePoolSize);
    
    //outside monobehaviors
    /// <summary>
    /// Handles the scores and tallies.
    /// </summary>
    private ScoreManager scoreManager;

    /// <summary>
    /// Ask this guy what to put on the pizza when it's fired.
    /// </summary>
    private OrderBuilderMenu orderBuilder;

    
    void Awake()
    {
        GatherReferences();
        availableIngredients = Order.SortIngredientsListAscending(availableIngredients);//sort list
    }

    // Start is called before the first frame update
    private void Start()
    {
        InitProjectilePool();
    }

    // Update is called once per frame
    void Update()
    {
        HandleTargetTracking();

        HandleShooting();
    }

    private void OnEnable()
    {
        myAudioSource.clip = cannonFireSound;//load audio
    }

    /// <summary>
    /// Gathers references that this behavior depends on.
    /// </summary>
    private void GatherReferences()
    {
        //get Components on this GO
        myBaseTransform = this.transform;
        myAudioSource = GetComponent<AudioSource>();
        myRigidbody = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Create GOs from Prefab and add to queue.
    /// </summary>
    private void InitProjectilePool()
    {
        for(var i = 0; i < projectilePoolSize; ++i)
        {
            var newProjectile = Instantiate(projectilePrefab);
            
            newProjectile.SetActive(false);
            projectilePool.Enqueue(newProjectile);
        }
    }

    /// <summary>
    /// Get an expired projectile from the pool or create a new one.
    /// </summary>
    /// <returns></returns>
    private GameObject GetNewProjectile()
    {
        var queuedProjectile = projectilePool.Dequeue();

        queuedProjectile.SetActive(false);//reset projectile
        queuedProjectile.SetActive(true);//turn on

        projectilePool.Enqueue(queuedProjectile);//re-queue for re-use

        //var pizzaProjectile = queuedProjectile.GetComponent<PizzaProjectile>() as PizzaProjectile;//is this really necessary?//avoid reflection where possible

        return queuedProjectile;
    }

    /// <summary>
    /// Get the point at which the projectile should spawn.
    /// </summary>
    /// <returns></returns>
    private Transform GetProjectileSpawnPoint()
    {
        //loop index
        if (++projectileSpawnPointIndex >= projectileSpawnPoints.Length)
        {
            projectileSpawnPointIndex = 0;
        }

        return projectileSpawnPoints[projectileSpawnPointIndex];
    }

    /// <summary>
    /// Do all the things.
    /// </summary>
    private void FireProjectile()
    {
        //pew pew
        if (projectilePrefab)
        {
            //Debug.Log("Firing a Projectile from Spawn Point No: " + projectileSpawnPointIndex + " / " + projectileSpawnPoints.Length, this.gameObject );
            scoreManager.OnShotFired();//increment counter

            //Get projectile from pool
            var newProjectile = GetNewProjectile();

            //pick a point to spawn at
            var projectileSpawnPoint = GetProjectileSpawnPoint();

            newProjectile.transform.position = projectileSpawnPoint.position;
            newProjectile.transform.rotation = projectileSpawnPoint.rotation;

            //give proper order
            var pizzaProjectile = newProjectile.GetComponent<PizzaProjectile>() as PizzaProjectile;

            //handle physics
            pizzaProjectile.GiveProjectileForce(turretTransform.forward * projectileForce);

            if (orderBuilder)
            {
                pizzaProjectile.GiveOrderIngredients(orderBuilder.GetIngredients());
                orderBuilder.OnOrderFired();
            }
            else
            {
                Debug.Log("No Order Builder connected to Cannon. Blank pizza.", this);
            }
            
            //audio
            myAudioSource.Play();
        }

        else
        {
            Debug.Log(this.gameObject.name + ":  No Projectile Prefab.  BANG!", this.gameObject);
        }
    }

    /// <summary>
    /// Handle input and cooldown of Shooting.
    /// </summary>
    private void HandleShooting()
    {
        if (Input.GetButtonDown("Fire1") && !disableCannonTrigger)//check for input and not disabled
        {
            if (Time.time > nextShootTime)//check for cooldown
            {
                //Debug.Log("Time To ZAPPPPPPPPPPPPPPPP!");
                FireProjectile();
                nextShootTime = Time.time + secondsBetweenShots;//apply cooldown
            }
        }
    }
    
    /// <summary>
    /// Rotate the Base and Turret to point at the Target.
    /// </summary>
    private void HandleTargetTracking()
    {
        //get relative position of target 
        var relative = myBaseTransform.InverseTransformPoint(objectToTrack.position);

        //get the angle between base forward and target on horizontal plane
        var horAngle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;

        //get relative position vertical
        relative = turretTransform.InverseTransformPoint(objectToTrack.position);

        //get angle between turrets and target on vertical slice plane
        var vertAngle = Mathf.Atan2(relative.y, relative.z) * Mathf.Rad2Deg;

        //Debug.Log("HorAngle: " + horAngle + " | VertAngle: " + vertAngle);

        //handle base swivel
        if (Mathf.Abs(horAngle) > optimalAngle) myBaseTransform.Rotate(0, Time.deltaTime * horAngle * swivelSpeed, 0);

        //handle turret pitch
        if (Mathf.Abs(vertAngle) > optimalAngle) turretTransform.Rotate(Time.deltaTime * -vertAngle * swivelSpeed, 0, 0);

        //CLAMP TURRET SO DOESN'T SHOOT OWN SPACE STATION!!!
        var localRot = turretTransform.localEulerAngles.x;

        if (localRot < 180)
        {
            if (localRot > minPitch)
            {
                localRot = minPitch;
            }
        }
        else if (localRot < 360 - maxPitch)
        {
            localRot = 360 - maxPitch;
        }

        turretTransform.localEulerAngles = new Vector3(localRot, 0, 0);
    }

    /// <summary>
    /// Set hook and initialize Order Builder.
    /// </summary>
    /// <param name="orderBuilder"></param>
    public void SetOrderBuilder(OrderBuilderMenu orderBuilder)
    {
        this.orderBuilder = orderBuilder;
    }

    /// <summary>
    /// Set Score Manager externally.
    /// </summary>
    /// <param name="scoreManager"></param>
    public void SetScoreManager(ScoreManager scoreManager)
    {
        this.scoreManager = scoreManager;
    }

    /// <summary>
    /// Procedure to follow when the level is at an end.
    /// </summary>
    public void OnLevelsEnd()
    {
        this.enabled = false;//disable controls, but not visuals
    }
}
