using UnityEngine;

[CreateAssetMenu(fileName = "PizzaOrder_", menuName = "ScriptableObjects/New Order")]
public class Order : ScriptableObject
{
    public IngredientsENUM[] ingredients;

    public int randomWeight = 1;

    public int score = 100;
}
