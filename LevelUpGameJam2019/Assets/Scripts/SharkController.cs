﻿using System.Collections;
using UnityEngine;

public enum SharkControllerState
{
    patrolling,
    jumping
}

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class SharkController : MonoBehaviour
{
    private static ScoreManager scoreManager;

    [Header("---SharkAttack---")]
    [SerializeField]
    private int secondsBetweenSharkAttacks = 10;

    /// <summary>
    /// Seconds added to shark attack timer if Pizza had anchovies on it.
    /// </summary>
    [SerializeField]
    [Tooltip("Seconds added to shark attack timer if Pizza had anchovies on it.")]
    private int anchovieCoolDown = 15;

    [SerializeField]
    private int sharkJumpForce = 15;

    [Header("---Colliders---")]
    [SerializeField]
    private Collider sphereOfInfluenceCollider;
    
    [SerializeField]
    private Collider mouthCollider;

    [Header("---Patrol Stuff---")]
    [SerializeField]
    private Transform[] patrolWaypoints;

    [SerializeField]
    private float sharkSwimSpeed = 1.0f;

    private int patrolWaypointsIndex = 0;

    private const float closeEnoughDistance = 0.1f;

    [Header("---Audio---")]
    [SerializeField]
    private AudioClip sharkJumpSound;

    [SerializeField]
    private AudioClip sharkEatPizzaSound;

    [SerializeField]
    private AudioClip sharkDeliciousSound;

    [SerializeField]
    private AudioClip splashSound;

    private float nextSharkAttackTime = 0;

    /// <summary>
    /// The state of this instance of a shark.
    /// </summary>
    private SharkControllerState sharkControllerState;
    /// <summary>
    /// This is the thing I want to eat.
    /// </summary>
    private Transform attackTarget;
    
    //member component references
    private Transform myTransform;
    private Rigidbody myRigidbody;
    private AudioSource myAudioSource;

    private Vector3 TESTSTARTPOSITION;

    private void Awake()
    {

        GatherReferences();
    }

    // Start is called before the first frame update
    void Start()
    {
        TESTSTARTPOSITION = myTransform.position;
        sharkControllerState = SharkControllerState.patrolling;
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void FixedUpdate()
    {
        switch (sharkControllerState)
        {
            case SharkControllerState.patrolling:
                SharkPatrol();
                break;
            case SharkControllerState.jumping:
                //roar
                break;
        }

    }

    /// <summary>
    /// Called when collider hits sphere of influence.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PizzaProjectile"))//noms detected
        {
            Debug.Log("Pizza Detected.");
            if (Time.time > nextSharkAttackTime && sharkControllerState == SharkControllerState.patrolling)//it's time to shark!
            {
                attackTarget = other.transform;

                StartSharkJump();

            }

            else
            {
                if (Time.time < nextSharkAttackTime)
                {
                    //too tired to shark.
                    Debug.Log("Still in cooldown.  Seconds Remaining: " + (nextSharkAttackTime - Time.time));
                }
            }
        

        }

        else if (other.gameObject.CompareTag("Water"))//if shark lands back in water
        {
            if(sharkControllerState == SharkControllerState.jumping)
            {
                OnSharkBackInWater();

            }
            else
            {

            }

            //lower shark so he isn't floating on top of the water
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.transform == attackTarget)//if our target has elluded us,
        {
            attackTarget = null;

            //point nose towards water
        }

    }

    /// <summary>
    /// Called if Shark actually gets a hold of Pizza.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PizzaProjectile"))//got the noms
        {
            var pizzaProjectile = collision.gameObject.GetComponent<PizzaProjectile>() as PizzaProjectile;//

            collision.gameObject.SetActive(false);//chomp pizza

            scoreManager.OnSharkAtePizza();//tally

            Debug.Log("CHOMP! Shark ate a pizza.", this);
            myRigidbody.angularVelocity = Vector3.zero;

            //play sound, if one is set
            if (sharkEatPizzaSound)
            {
                myAudioSource.clip = sharkEatPizzaSound;
                myAudioSource.Play();
            }

            //check for anchvoies on pizza
            foreach (var ingredient in pizzaProjectile.GetIngredientsOnPizza().ingredients)
            {
                if(ingredient == IngredientsENUM.Anchovies)
                {
                    //Sharks love anchovies
                    OnSharkAteAnchovies();

                }
            }
            //check if pizza had anchovies
        }
    }

    /// <summary>
    /// Move the Shark between waypoints.
    /// </summary>
    private void SharkPatrol()
    {
        var targetWaypoint = patrolWaypoints[patrolWaypointsIndex].position;

        //Debug.Log("Moving....." + patrolWaypointsIndex);
        if (Vector3.Distance(targetWaypoint, myTransform.position) > closeEnoughDistance)
        {
            //handle rotatition
            myTransform.LookAt(targetWaypoint);

            //move towards
            myRigidbody.MovePosition(myTransform.position + myTransform.forward * Time.deltaTime * sharkSwimSpeed);
        }
        else
        {
            //set next waypoint
            patrolWaypointsIndex = ++patrolWaypointsIndex >= patrolWaypoints.Length ? 0 : patrolWaypointsIndex;//increment / reset index
        }
    }
    
    private void OnSharkBackInWater()
    {
        Debug.Log("Back in water....");

        myRigidbody.useGravity = false;
        myRigidbody.velocity = Vector3.zero;

        sphereOfInfluenceCollider.enabled = true;//stop using this collider for now.

        //reset rotations
        Vector3 defaultRotations = new Vector3(0, myTransform.localEulerAngles.y, 0);
        myTransform.localEulerAngles = defaultRotations;

        //resume patrol
        sharkControllerState = SharkControllerState.patrolling;

        //audio
        if (splashSound)
        {
            myAudioSource.clip = splashSound;
            myAudioSource.Play();
        }
    }

    private void OnSharkAteAnchovies()
    {
        StartCoroutine(PlayDeliciousSound());//make yummy sound

        nextSharkAttackTime = Time.time + anchovieCoolDown;//increase delay
    }

    /// <summary>
    /// Wait until current sound is done, then cue next sound.
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayDeliciousSound()
    {
        float delayBetweenSounds = 0.25f;

        //wait for current clip to stop playing
        yield return new WaitForSeconds(myAudioSource.clip.length + delayBetweenSounds);

        if (sharkDeliciousSound)
        {
            myAudioSource.clip = sharkDeliciousSound;
            myAudioSource.Play();
        }
    }

    private IEnumerator EnableColliders()
    {
        yield return new WaitForSeconds(1.0f);
        sphereOfInfluenceCollider.enabled = false;//stop using this collider for now.

    }
    
    private void StartSharkJump()
    {
        sharkControllerState = SharkControllerState.jumping;

        //where does the shark need to aim in order to be at the same point in space at the same time as the pizza?
        var interceptPoint = FirstOrderIntercept(myTransform.position,
        myRigidbody.velocity,
        sharkJumpForce,
        attackTarget.position,
        attackTarget.GetComponent<Rigidbody>().velocity
        );

        myTransform.LookAt(interceptPoint);

        myRigidbody.AddForce(myTransform.forward * sharkJumpForce, ForceMode.Impulse);

        myRigidbody.useGravity = true;//turn on gravity

        StartCoroutine(EnableColliders());

        //set cooldown
        nextSharkAttackTime = Time.time + secondsBetweenSharkAttacks;

        //play jump sound
        if (sharkJumpSound)
        {
            myAudioSource.clip = sharkJumpSound;
            myAudioSource.Play();
        }

    }

    /// <summary>
    /// Gather needed references.
    /// </summary>
    private void GatherReferences()
    {
        //static stuff
        if (!scoreManager)
        {
            scoreManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>() as ScoreManager;
        }

        //member stuff
        myTransform = this.transform;
        myRigidbody = GetComponent<Rigidbody>() as Rigidbody;
        myAudioSource = GetComponent<AudioSource>() as AudioSource;
    }

    //CREDIT: Daniel Brauer http://wiki.unity3d.com/index.php?title=Calculating_Lead_For_Projectiles
    //first-order intercept using absolute target position
    private static Vector3 FirstOrderIntercept
    (
        Vector3 shooterPosition,
        Vector3 shooterVelocity,
        float shotSpeed,
        Vector3 targetPosition,
        Vector3 targetVelocity
    )
    {
        var targetRelativePosition = targetPosition - shooterPosition;
        var targetRelativeVelocity = targetVelocity - shooterVelocity;
        var t = FirstOrderInterceptTime
        (
            shotSpeed,
            targetRelativePosition,
            targetRelativeVelocity
        );
        return targetPosition + t * (targetRelativeVelocity);
    }

    //first-order intercept using relative target position
    private static float FirstOrderInterceptTime(float shotSpeed, Vector3 targetRelativePosition, Vector3 targetRelativeVelocity)
    {
        var velocitySquared = targetRelativeVelocity.sqrMagnitude;

        if (velocitySquared < 0.001f)
            return 0f;

        var a = velocitySquared - shotSpeed * shotSpeed;

        //handle similar velocities
        if (Mathf.Abs(a) < 0.001f)
        {
            var t = -targetRelativePosition.sqrMagnitude /
            (
                2f * Vector3.Dot
                (
                    targetRelativeVelocity,
                    targetRelativePosition
                )
            );
            return Mathf.Max(t, 0f); //don't shoot back in time
        }

        var b = 2f * Vector3.Dot(targetRelativeVelocity, targetRelativePosition);
        var c = targetRelativePosition.sqrMagnitude;
        var determinant = b * b - 4f * a * c;

        if (determinant > 0f)
        { //determinant > 0; two intercept paths (most common)
            float t1 = (-b + Mathf.Sqrt(determinant)) / (2f * a),
                    t2 = (-b - Mathf.Sqrt(determinant)) / (2f * a);
            if (t1 > 0f)
            {
                if (t2 > 0f)
                    return Mathf.Min(t1, t2); //both are positive
                else
                    return t1; //only t1 is positive
            }
            else
                return Mathf.Max(t2, 0f); //don't shoot back in time
        }
        else if (determinant < 0f) //determinant < 0; no intercept path
            return 0f;
        else //determinant = 0; one intercept path, pretty much never happens
            return Mathf.Max(-b / (2f * a), 0f); //don't shoot back in time
    }

    [ContextMenu("Reset Position")]
    public void ResetSharkPositionTest()
    {
        myTransform.position = TESTSTARTPOSITION;
    }
}
