using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class PizzaProjectile : MonoBehaviour
{
    [SerializeField]
    private float floatiness = 1.0f;

    private OrderStruct ingredientsOnThisPizza;

    //components
    private Rigidbody myRigidbody;
    private Transform myTransform;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>() as Rigidbody;
        myTransform = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        myRigidbody.AddForce(myTransform.up * floatiness, ForceMode.Impulse);
    }

    public OrderStruct GetIngredientsOnPizza(){
        return ingredientsOnThisPizza;
    }

}
