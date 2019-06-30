using System.Collections.Generic;
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

    /// <summary>
    /// "If more than one barrel, iterates through each point."
    /// </summary>
    [Tooltip("If more than one barrel, iterates through each point.")]
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

    /// <summary>
    /// pitch on X //CAN BE THE SAME OBJECT AS BASE, IF NO SEPARATE TURRET.  WHOLE OBJECT WILL ROTATE AS EXPECTED
    /// </summary>
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
    private readonly List<GameObject> projectilePool = new List<GameObject>();
    private const int maxProjectilesInScene = 30;

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

        HandleShooting();

        
    }

    /// <summary>
    /// Gathers references that this behavior depends on.
    /// </summary>
    private void GatherReferences()
    {

        baseTransform = this.transform;
        audioSource = GetComponent<AudioSource>() as AudioSource;

        scoreManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>() as ScoreManager;
    }

    /// <summary>
    /// Get an expired projectile from the pool or create a new one.
    /// </summary>
    /// <returns></returns>
    private GameObject GetNewProjectile()
    {
        //first try to use an expired projectile
        foreach(var projectile in projectilePool)
        {
            if (!projectile.activeSelf)//this projectile has expired and can be reused
            {
                projectile.SetActive(true);

                return projectile;
            }
        }
        //no expired projectile; create a new one if there is space
        if (projectilePool.Count < maxProjectilesInScene)
        {
            var newProjectile = Instantiate(projectilePrefab);

            projectilePool.Add(newProjectile);

            return newProjectile;
        }

        else//create a new object not controlled by pool
        {
            Debug.LogError("Object Pooling ERROR! Cannon requesting more projectiles than allowed.  Change fire rate or pool quantity.", this.gameObject);
            return Instantiate(projectilePrefab); 
        }
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
            var newProjectile = GetNewProjectile() as GameObject;

            //pick a point to spawn at
            var projectileSpawnPoint = GetProjectileSpawnPoint();

            newProjectile.transform.position = projectileSpawnPoint.position;
            newProjectile.transform.rotation = projectileSpawnPoint.rotation;

            //handle physics of projectile
            var attachedRB = newProjectile.GetComponent<Rigidbody>() as Rigidbody;

            attachedRB.AddForce(turretTransform.forward * projectileForce, ForceMode.Impulse);

            //give proper order
            var pizzaProjectile = newProjectile.GetComponent<PizzaProjectile>() as PizzaProjectile;

            if (orderBuilder)
            {
                pizzaProjectile.GiveOrderIngredients(orderBuilder.GetIngredients());
                orderBuilder.OnOrderFired();
            }
            else
            {
                Debug.Log("No orders given to Cannon. Blank pizza.", this);
            }
            
            //play cannon fire sound
            audioSource.clip = cannonFireSound;
            audioSource.Play();
        }

        else
        {
            Debug.Log(this.gameObject.name + ": BANG!", this.gameObject);

        }
    }

    /// <summary>
    /// Handle input and cooldown of Shooting.
    /// </summary>
    private void HandleShooting()
    {
        if (Input.GetButtonDown("Fire1"))//check for input
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
