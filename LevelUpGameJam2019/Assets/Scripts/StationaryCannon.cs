using UnityEngine;

[SelectionBase]
[RequireComponent(typeof(AudioSource))]
public class StationaryCannon : MonoBehaviour
{
    /// <summary>
    /// Stop adjusting rotation if target within this angle.
    /// </summary>
    private const float optimalAngle = .05f;

    //outside monobehaviors
    private static ScoreManager scoreManager;

    [Header("Projectile Stuff")]
    [SerializeField]
    private GameObject projectilePrefab;
    
    [Tooltip("If more than one, iterates through each point.")]
    [SerializeField]
    private Transform[] projectileSpawnPoints;

    /// <summary>
    /// Ask this guy what to put on the pizza when it's fired.
    /// </summary>
    [SerializeField]
    private OrderBuilderMenu orderBuilder;

    [SerializeField]
    private float projectileForce = 5.0f;

    private int projectileSpawnPointIndex = 0;

    [Header("Turret Target Tracking")]

    [SerializeField]
    private Transform objectToTrack;

    [SerializeField]
    private Transform turretTransform;//pitch on X //CAN BE THE SAME OBJECT AS BASE, IF NO SEPARATE TURRET.  WHOLE OBJECT WILL ROTATE AS EXPECTED

    [SerializeField]
    private int minPitch = 15;

    [SerializeField]
    private int maxPitch = 75;

    [SerializeField]
    private float swivelSpeed = 3f;

    [SerializeField]
    private float secondsBetweenShots = 3f;

    [Header("---Audio---")]
    [SerializeField]
    private AudioClip cannonFireSound;

    //GO components
    private AudioSource audioSource;
    private Transform baseTransform;//swivel on Y //
    private Animator anim;

    private float nextShootTime = 0;

    //object pooling stuff
    private readonly GameObject[] projectilePool = new GameObject[10];

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        GatherReferences();

    }

    // Update is called once per frame
    void Update()
    {
        
        HandleTargetTracking();

        if(Input.GetButtonDown("Fire1"))
        {
            
            HandleShooting();
            
        }
        
    }

    private void GatherReferences()
    {

        baseTransform = this.transform;
        audioSource = GetComponent<AudioSource>() as AudioSource;

        scoreManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>() as ScoreManager;
    }

    private void FireProjectile()
    {
        //pew pew
        if (projectilePrefab)
        {
            //Debug.Log("Firing a Projectile from Spawn Point No: " + projectileSpawnPointIndex + " / " + projectileSpawnPoints.Length, this.gameObject );
            scoreManager.OnShotFired();//increment counter

            //pick a point to spawn at
            var spawnPoint = projectileSpawnPoints[projectileSpawnPointIndex].position;

            //loop index
            if (++projectileSpawnPointIndex >= projectileSpawnPoints.Length)
            {
                projectileSpawnPointIndex = 0;
            }

            //create new object and cache references
            var newProjectile = Instantiate(projectilePrefab, spawnPoint, Quaternion.identity) as GameObject;

            //handle physics of projectile
            var attachedRB = newProjectile.GetComponent<Rigidbody>() as Rigidbody;

            attachedRB.AddForce(turretTransform.forward * projectileForce, ForceMode.Impulse);

            //give proper order
            var pizzaProjectile = newProjectile.GetComponent<PizzaProjectile>() as PizzaProjectile;
            if(orderBuilder) pizzaProjectile.GiveOrderIngredients(orderBuilder.GetIngredients());
            pizzaProjectile.OnProjectileFired();

            //play cannon fire sound
            audioSource.clip = cannonFireSound;
            audioSource.Play();
        }

        else
        {
            Debug.Log(this.gameObject.name + ": BANG!", this.gameObject);

        }
    }

    private void HandleShooting()
    {

        //Debug.Log("HIT PLAYER! ZAP ZAP ZAP!!!!");
        //check shoot delay
        if (Time.time > nextShootTime)
        {
            //Debug.Log("Time To ZAPPPPPPPPPPPPPPPP!");
            FireProjectile();
            nextShootTime = Time.time + secondsBetweenShots;

        }

    }
    
    private void HandleTargetTracking()
    {
        //get relative position of target 
        var relative = baseTransform.InverseTransformPoint(objectToTrack.position);

        //get the angle between base forward and target on horizontal plane
        var horAngle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;

        //get relative position vertical
        relative = turretTransform.InverseTransformPoint(objectToTrack.position);

        //get angle between turrets and target on vertical slice plane
        var vertAngle = Mathf.Atan2(relative.y, relative.z) * Mathf.Rad2Deg;

        //Debug.Log("HorAngle: " + horAngle + " | VertAngle: " + vertAngle);

        //handle base swivel
        if (Mathf.Abs(horAngle) > optimalAngle) baseTransform.Rotate(0, Time.deltaTime * horAngle * swivelSpeed, 0);

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
}
