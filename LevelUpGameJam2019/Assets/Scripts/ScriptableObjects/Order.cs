using UnityEngine;

[CreateAssetMenu(fileName = "PizzaOrder_", menuName = "ScriptableObjects/New Order")]
public class Order : ScriptableObject
{
    public bool pepperoni;
    public bool sauce;
    public bool cheese;
    public bool sausage;
    public bool anchovies;

    public int randomWeight = 1;
}
