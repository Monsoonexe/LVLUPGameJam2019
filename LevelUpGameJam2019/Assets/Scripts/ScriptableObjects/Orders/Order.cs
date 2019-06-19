using UnityEngine;

[CreateAssetMenu(fileName = "PizzaOrder_", menuName = "ScriptableObjects/New Order")]
public class Order : ScriptableObject
{
    public IngredientsENUM[] ingredients;

    public int randomWeight = 1;

    public int score = 100;

    /// <summary>
    /// Determines if the ingredients on the pizza match the Customer's order.  Currently does not support double ingredients.
    /// </summary>
    /// <param name="customerOrder"></param>
    /// <param name="ingredientsOnPizza"></param>
    /// <returns>Returns true if all ingredients in order are included on pizza, and pizza contains no ingredient not on order.</returns>
    public static bool ComparePizzaToOrder(Order customerOrder, IngredientsENUM[] ingredientsOnPizza)
    {
        var allCustomersIngredientsOnPizza = true;
        var noExtraIngredientsOnPizza = true;

        //quick exit
        if (customerOrder.ingredients.Length != ingredientsOnPizza.Length)
        {
            //one of the below is false
            allCustomersIngredientsOnPizza = false;
            noExtraIngredientsOnPizza = false;
        }

        else
        {
            //verify that every ingredient customer wanted is present on pizza
            foreach (var custIngredient in customerOrder.ingredients)
            {
                var ingredientOnPizza = false;

                //is indeed on pizza
                foreach (var pizzaIngredi in ingredientsOnPizza)
                {

                    if (custIngredient == pizzaIngredi)
                    {
                        ingredientOnPizza = true;
                    }
                }
                //verify no extra ingredients


                if (!ingredientOnPizza)
                {
                    allCustomersIngredientsOnPizza = false;
                    break;
                }
            }


        }

        return allCustomersIngredientsOnPizza;
    }
}
