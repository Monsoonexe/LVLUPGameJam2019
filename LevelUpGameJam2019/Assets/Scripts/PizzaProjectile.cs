﻿using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(AudioSource))]
public class PizzaProjectile : MonoBehaviour
{
    [SerializeField]
    private float floatiness = 1.0f;

    [Header("---Audio---")]
    [SerializeField]
    private AudioClip flyingSound;

    private OrderStruct ingredientsOnThisPizza;

    //components
    private Rigidbody myRigidbody;
    private Transform myTransform;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Awake()
    {
        GatherReferences();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void FixedUpdate()
    {
        myRigidbody.AddForce(myTransform.up * floatiness, ForceMode.Impulse);
    }

    private void GatherReferences()
    {
        myTransform = this.transform;
        myRigidbody = GetComponent<Rigidbody>() as Rigidbody;
        audioSource = GetComponent<AudioSource>() as AudioSource;

    }

    public OrderStruct GetIngredientsOnPizza()
    {
        return ingredientsOnThisPizza;
    }

    public void OnProjectileFired()
    {
        audioSource.clip = flyingSound;
        audioSource.Play();
    }

    /// <summary>
    /// Base.
    /// </summary>
    /// <param name="order"></param>
    public void GiveOrderIngredients(OrderStruct order)
    {
        ingredientsOnThisPizza = order;
    }

    public void GiveOrderIngredients(IngredientsENUM[] order)
    {
        GiveOrderIngredients(new OrderStruct(order));
    }

    public void GiveOrderIngredients(System.Collections.Generic.List<IngredientsENUM> order)
    {
        GiveOrderIngredients(new OrderStruct(order.ToArray()));
    }

}
