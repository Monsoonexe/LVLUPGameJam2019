using System.Collections;
using UnityEngine;

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

    [Header("---Audio---")]
    [SerializeField]
    private AudioClip sharkJumpSound;

    [SerializeField]
    private AudioClip sharkEatPizzaSound;

    [SerializeField]
    private AudioClip sharkDeliciousSound;

    private float nextSharkAttackTime = 0;

    /// <summary>
    /// This is the thing I want to eat.
    /// </summary>
    private Transform attackTarget;

    //shark state
    private bool sharkIsAttacking = false;

    //member component references
    private Transform myTransform;
    private Rigidbody myRigidbody;
    private AudioSource myAudioSource;

    private void Awake()
    {

        GatherReferences();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //when shark reaches peak, angle nose down towards water
        if (sharkIsAttacking)
        {
            myTransform.LookAt(attackTarget);
        }
    }

    private void FixedUpdate()
    {
        //course correction cuz the pizza moves so fast
    }

    /// <summary>
    /// Called when collider hits sphere of influence.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PizzaProjectile"))//noms detected
        {
            if (Time.time > nextSharkAttackTime)//it's time to shark!
            {
                attackTarget = other.transform;

                StartSharkAttack();

            }

            else
            {
                //too tired to shark.
            }

        }
        else
        {
            //I'm a shark, but even I won't touch whatever this was.  I'm only a pizza shark.
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

    private void StartSharkAttack()
    {
        myTransform.LookAt(attackTarget);
        myRigidbody.AddForce(myTransform.forward * sharkJumpForce, ForceMode.Impulse);

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
}
