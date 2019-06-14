using UnityEngine;

[SelectionBase]
public class StationaryCannon : MonoBehaviour
{
    /// <summary>
    /// Stop adjusting rotation if target within this angle.
    /// </summary>
    private static readonly float optimalAngle = .05f;

    [Header("Projectile Stuff")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform[] projectileSpawnPoints;
    private int projectileSpawnPointIndex = 0;

    [Header("Turret Target Tracking")]
    public Transform objectToTrack;
    [SerializeField] private int minPitch = 15;
    [SerializeField] private int maxPitch = 75;

    [SerializeField] private float swivelSpeed = 3f;

    [SerializeField] private float secondsBetweenShots = 3f;

    private Transform baseTransform;//swivel on Y //
    private Transform turretTransform;//pitch on X //CAN BE THE SAME OBJECT AS BASE, IF NO SEPARATE TURRET.  WHOLE OBJECT WILL ROTATE AS EXPECTED

    private float nextShootTime = 0;

    //object pooling stuff

    private void Awake()
    {
        //if (!target)
        //{
        //    target = GameObject.FindGameObjectWithTag("Player").transform as Transform;
        //}
    }

    // Start is called before the first frame update
    void Start()
    {
        baseTransform = this.transform;
        turretTransform = baseTransform.GetChild(0).transform as Transform;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            if (objectToTrack)
            {
                HandleTargetTracking();
                HandleShooting();

            }
            
        }
        
        
    }

    private void FireProjectile()
    {
        //pew pew
        if (projectilePrefab)
        {
            //Debug.Log("Firing a Projectile from Spawn Point No: " + projectileSpawnPointIndex + " / " + projectileSpawnPoints.Length, this.gameObject );

            //pick a point to spawn at
            var spawnPoint = projectileSpawnPoints[projectileSpawnPointIndex].position;

            //loop index
            if (++projectileSpawnPointIndex >= projectileSpawnPoints.Length)
            {
                projectileSpawnPointIndex = 0;
            }

            //create new object
            var newProjectile = Instantiate(projectilePrefab, spawnPoint, Quaternion.identity);

            //point at target
            newProjectile.transform.LookAt(objectToTrack);
        }

        else
        {
            Debug.Log(this.gameObject.name + ": BANG!", this.gameObject);

        }
    }

    private void HandleShooting()
    {
        RaycastHit raycastHitInfo;
        
        //check line of sight
        if (Physics.Raycast(turretTransform.position, turretTransform.forward, out raycastHitInfo, 1000, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            //can I see the target?
            if (raycastHitInfo.collider.gameObject.transform == objectToTrack)
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

            else
            {
                //Debug.Log("Did not hit player.... hit: " + raycastHitInfo.collider.name, this.gameObject);
            }
        }
        else
        {
            //Debug.Log("HIT NOTHING!", this.gameObject);
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
