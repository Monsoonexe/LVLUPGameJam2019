using UnityEngine;

[CreateAssetMenu(fileName = "PizzaOrder_", menuName = "ScriptableObjects/New Order")]
public class Order : ScriptableObject
{
    [SerializeField]//set in Inspector
    private IngredientSO[] ingredientSOs = new IngredientSO[0];//

    public IngredientSO[] Ingredients { get { return ingredientSOs; } }//publicly accessible, but gets a copy, not actual.

    [SerializeField]//set in Inspector
    private int randomWeight = 1;

    public int RandomWeight { get { return randomWeight; } }//publicly accessible, but readonly
    
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>VS does not recognize this as a valid Unity Callback (Message) but it totally works.</remarks>
    private void OnValidate()
    {
        ingredientSOs = SortIngredientsListAscending(ingredientSOs);//sort list
    }

    /// <summary>
    /// Determines if customer order and pizza are exactly the same.
    /// </summary>
    /// <param name="customerOrder"></param>
    /// <param name="ingredientsOnPizza"></param>
    /// <returns>Returns true if all ingredients in order are included on pizza, in proper quantities, and pizza contains no ingredient not on order.</returns>
    public static bool CompareOrderToPizza(Order customerOrder, IngredientsENUM[] ingredientsOnPizza)
    {
        return CompareOrderToPizza(customerOrder.Ingredients, ingredientsOnPizza);
    }

    /// <summary>
    /// Determines if customer order and pizza are exactly the same.
    /// </summary>
    /// <param name="customerOrder"></param>
    /// <param name="ingredientsOnPizza"></param>
    /// <returns>Returns true if all ingredients in order are included on pizza, and pizza contains no ingredient not on order.</returns>
    /// <remarks>Assumes both lists are sorted in ascending numeric order.</remarks>
    public static bool CompareOrderToPizza(IngredientSO[] ingredientsOnOrder, IngredientsENUM[] ingredientsOnPizza)
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
            //all ingredients in order are present on pizza, quantities match, and no extra ingredients exist on pizza that are not in order
            //sort list
            //ingredientsOnOrder = SortIngredientsListAscending(ingredientsOnOrder); //call this at loadtime to avoid redundant sorts
            ingredientsOnPizza = SortIngredientsListAscending(ingredientsOnPizza);//sort this only when needed.

            //check if lists are exactly the same. (fact: they are the same length)
            for (var i = 0; i < ingredientsOnOrder.Length; ++i)
            {
                if (ingredientsOnOrder[i].Ingredient != ingredientsOnPizza[i])
                {
                    return false;
                }
            }
        }
        return true;
    }

    /// <summary>
    /// Determines if customer order and pizza are exactly the same.
    /// </summary>
    /// <param name="customerOrder"></param>
    /// <param name="ingredientsOnPizza"></param>
    /// <returns>Returns true if all ingredients in order are included on pizza, and pizza contains no ingredient not on order.</returns>
    /// <remarks>Assumes both lists are sorted in ascending numeric order.</remarks>
    [System.Obsolete("RSO.  Deprecated because it will probably never be used.  Use different overloaded version instead.")]
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
            //all ingredients in order are present on pizza, quantities match, and no extra ingredients exist on pizza that are not in order
            //sort list
            //ingredientsOnOrder = SortIngredientsListAscending(ingredientsOnOrder); //call this at loadtime to avoid redundant sorts
            ingredientsOnPizza = SortIngredientsListAscending(ingredientsOnPizza);//sort this only when needed.
                       
            //check if lists are exactly the same. (fact: they are the same length)
            for (var i = 0; i < ingredientsOnOrder.Length; ++i)
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
    /// Sort the list in ascending order based on numeric value of enum using Selection Sort.
    /// </summary>
    /// <param name="ingredientsList"></param>
    public static IngredientSO[] SortIngredientsListAscending(IngredientSO[] ingredientsList)
    {
        var length = ingredientsList.Length;//cache length

        if (length < 2)//don't sort small lists
        {
            return ingredientsList;
        }

        //selection sort
        for (var i = 0; i < length - 1; ++i)
        {
            var lowIndex = i;

            for(var j = length - 1; j > i; --j)
            {
                if(ingredientsList[j] < ingredientsList[lowIndex])
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

    /// <summary>
    /// Sort the list in ascending order based on numeric value of enum using Selection Sort.
    /// </summary>
    /// <param name="ingredientsList"></param>
    public static IngredientsENUM[] SortIngredientsListAscending(IngredientsENUM[] ingredientsList)
    {
        var length = ingredientsList.Length;//cache length

        if (length < 2)//don't sort small lists
        {
            return ingredientsList;
        }

        //selection sort
        for (var i = 0; i < length - 1; ++i)
        {
            var lowIndex = i;

            for (var j = length - 1; j > i; --j)
            {
                if (ingredientsList[j] < ingredientsList[lowIndex])
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

    /// <summary>
    /// Sort this instance's list of ingredients.
    /// </summary>
    public void SortIngredientsList()
    {
        ingredientSOs = SortIngredientsListAscending(ingredientSOs);
    }

    /// <summary>
    /// String consists of each ingredient. Supports multiples of ingredient.
    /// </summary>
    /// <returns>e.g. "Sauce | Cheese | Anchovies | "</returns>
    public override string ToString()
    {
        //might override in the future
        var stringBuilder = new System.Text.StringBuilder();

        foreach (var ingredient in ingredientSOs)
        {
            stringBuilder.Append(ingredient.ToString());
            stringBuilder.Append(" | ");
        }

        stringBuilder.Append(randomWeight.ToString());
        
        return stringBuilder.ToString();
    }

}
