using UnityEngine;

[CreateAssetMenu(fileName = "Ingredient_", menuName = "ScriptableObjects/New Ingredient")]
public class IngredientSO : RichScriptableObject
{
    [Header("= Ingredient =")]
    [SerializeField]//set by Developer
    private Sprite icon;

    public Sprite Icon { get { return icon; } }//externally accessible, readonly

    /// <summary>
    /// In which order does this ingredient go on a pizza? ex first, second.
    /// </summary>
    [SerializeField]//set by Developer
    [Tooltip("In which order does this ingredient go on a pizza? ex first, second.")]
    private int orderOnPizza = 0;
    public int OrderOnPizza { get { return orderOnPizza; } }//externally accessible, readonly

    //comparison operations
    //always a comparison of ingredient value, never of Icon.

    public static bool operator > (IngredientSO a, IngredientSO b)
    {
        return a.OrderOnPizza > b.OrderOnPizza;
    }

    public static bool operator >= (IngredientSO a, IngredientSO b)
    {
        return a.OrderOnPizza >= b.OrderOnPizza;
    }

    public static bool operator < (IngredientSO a, IngredientSO b)
    {
        return a.OrderOnPizza < b.OrderOnPizza;
    }

    public static bool operator <= (IngredientSO a, IngredientSO b)
    {
        return a.OrderOnPizza <= b.OrderOnPizza;
    }

}
