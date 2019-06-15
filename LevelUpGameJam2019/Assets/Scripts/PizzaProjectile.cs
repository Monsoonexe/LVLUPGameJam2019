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
