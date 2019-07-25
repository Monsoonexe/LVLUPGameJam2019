﻿using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(AudioSource))]
public class PizzaProjectile : MonoBehaviour
{
    private const int lifeTime = 8;//seconds

    [SerializeField]
    private float floatiness = 1.0f;

    [Header("---Visuals---")]
    [Tooltip("The visuals of the projectile.")]
    [SerializeField]
    private GameObject pizzaPieModel;//Toggle them off so whole object doesn't need to be affected.  sound and effects will exist after pie disappears

    [Tooltip("The visuals of the trail effect.")]
    [SerializeField]
    private GameObject trailEffect;//Toggle them off so whole object doesn't need to be affected.  sound and effects will exist after pie disappears

    [Header("---Audio---")]
    [SerializeField]
    private AudioClip flyingSound;
    
    //order
    private IngredientSO[] ingredientsOnThisPizza;

    //components
    private Rigidbody myRigidbody;
    private Transform myTransform;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Awake()
    {
        GatherReferences();
    }

    private void OnEnable()
    {
        StartCoroutine(ExpireAfterTime());

        //play sound
        audioSource.clip = flyingSound;
        audioSource.Play();
    }

    private void OnDisable()
    {
        ResetProjectile();
    }
    
    private void FixedUpdate()
    {
        myRigidbody.AddForce(myTransform.up * floatiness, ForceMode.Impulse);//give frisbee-like physics
    }

    private void OnTriggerEnter(Collider other)
    {
        //expire projectile if hits water
        if (other.CompareTag("Water"))
        {
            this.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// End-of-the-level procedure.
    /// </summary>
    private void OnLevelsEnd()
    {
        this.gameObject.SetActive(false);//no points allowed after buzzer
    }

    /// <summary>
    /// Object is part of a pool, so reset instead of Destroying.
    /// </summary>
    private void ResetProjectile()
    {
        //reset physics
        myRigidbody.velocity = Vector3.zero;
        myRigidbody.angularVelocity = Vector3.zero;

        //reset rotation
        myTransform.rotation = Quaternion.identity;
    }

    /// <summary>
    /// Expire this object after established lifetime.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ExpireAfterTime()
    {
        yield return new WaitForSeconds(lifeTime);

        this.gameObject.SetActive(false);//expire
    }

    /// <summary>
    /// Get all needed member components.
    /// </summary>
    private void GatherReferences()
    {
        //gather component references
        myTransform = this.transform;
        myRigidbody = GetComponent<Rigidbody>() as Rigidbody;
        audioSource = GetComponent<AudioSource>() as AudioSource;
    }

    public IngredientSO[] GetIngredientsOnPizza()
    {
        return ingredientsOnThisPizza;
    }

    /// <summary>
    /// Base.
    /// </summary>
    /// <param name="order"></param>
    public void GiveOrderIngredients(IngredientSO[] order)
    {
        ingredientsOnThisPizza = order;
    }

    /// <summary>
    /// Put these Ingredients on the order.
    /// </summary>
    /// <param name="order"></param>
    public void GiveOrderIngredients(System.Collections.Generic.List<IngredientSO> order)
    {
        //call base function
        GiveOrderIngredients(order.ToArray());
    }

    /// <summary>
    /// Apply launch force, such as when fired out of a cannon.
    /// </summary>
    /// <param name="force"></param>
    public void GiveProjectileForce(Vector3 force)
    {
        myRigidbody.AddForce(force, ForceMode.Impulse);
    }
}
