using UnityEngine;

[CreateAssetMenu(fileName = "PizzaOrder_", menuName = "ScriptableObjects/New Order")]
public class Order : ScriptableObject
{
    public IngredientsENUM[] ingredients;

    public int randomWeight = 1;

    public int score = 100;

    /// <summary>
    /// Determines if customer order and pizza are exactly the same.
    /// </summary>
    /// <param name="customerOrder"></param>
    /// <param name="ingredientsOnPizza"></param>
    /// <returns>Returns true if all ingredients in order are included on pizza, in proper quantities, and pizza contains no ingredient not on order.</returns>
    public static bool CompareOrderToPizza(Order customerOrder, IngredientsENUM[] ingredientsOnPizza)
    {
        return CompareOrderToPizza(customerOrder.ingredients, ingredientsOnPizza);
    }

    /// <summary>
    /// Determines if customer order and pizza are exactly the same.
    /// </summary>
    /// <param name="customerOrder"></param>
    /// <param name="ingredientsOnPizza"></param>
    /// <returns>Returns true if all ingredients in order are included on pizza, and pizza contains no ingredient not on order.</returns>
    public static bool CompareOrderToPizza(IngredientsENUM[] ingredientsOnOrder, IngredientsENUM[] ingredientsOnPizza)
    {
        //base
        //quick exit (agnostic)
        if (ingredientsOnOrder.Length != ingredientsOnPizza.Length)
        {
            return false;
        }

        else if (ingredientsOnOrder.Length == 0 && ingredientsOnPizza.Length == 0)//are both orders plain?
        {
            //plain pizza matches plain order
            return true;
        }

        else
        {
            //all ingredients on order are present on pizza, quantities match, and no extra ingredients are pizza that are not in order

            //sort lists
            ingredientsOnOrder = SortIngredientsListAscending(ingredientsOnOrder);
            ingredientsOnPizza = SortIngredientsListAscending(ingredientsOnPizza);
                       
            //check if lists are exactly the same. (fact: they are the same length)
            for(var i = 0; i < ingredientsOnOrder.Length; ++i)
            {
                if(ingredientsOnOrder[i] != ingredientsOnPizza[i])
                {
                    return false;
                }
            }

        }

        return true;

    }

    /// <summary>
    /// Sort the list in ascending order based on numeric value of enum.
    /// </summary>
    /// <param name="ingredientsList"></param>
    public static IngredientsENUM[] SortIngredientsListAscending(IngredientsENUM[] ingredientsList)
    {
        var length = ingredientsList.Length;//cache length

        if(length < 2)//don't sort small lists
        {
            return ingredientsList;
        }

        //selection sort
        for (var i = 0; i < length - 1; ++i)
        {
            var lowIndex = i;

            for(var j = length - 1; j > i; --j)
            {
                if(ingredientsList[j] < ingredientsList[i])
                {
                    lowIndex = j;
                }
            }

            //swap elements
            var temp = ingredientsList[lowIndex];//cache current occupant

            ingredientsList[lowIndex] = ingredientsList[i];//put correct element in place
            ingredientsList[i] = temp;//put temp back in for later

        }//end for

        return ingredientsList;

    }//end func

    public override string ToString()
    {
        //might override in the future
        var stringBuilder = new System.Text.StringBuilder();

        foreach (var ingredient in ingredients)
        {
            stringBuilder.Append(ingredient.ToString());
            stringBuilder.Append(" | ");
        }

        stringBuilder.Append(randomWeight.ToString());

        stringBuilder.Append(" | ");//divider

        stringBuilder.Append(score.ToString());

        return stringBuilder.ToString();
    }

    /// <summary>
    /// Sort this instance's list of ingredients
    /// </summary>
    public void SortIngredientsList()
    {
        ingredients = SortIngredientsListAscending(ingredients);
    }

}
