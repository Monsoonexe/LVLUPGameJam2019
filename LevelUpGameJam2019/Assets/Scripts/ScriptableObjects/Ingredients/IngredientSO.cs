﻿using UnityEngine;

[CreateAssetMenu(fileName = "Ingredient_", menuName = "ScriptableObjects/New Ingredient")]
public class IngredientSO : ScriptableObject
{
    [SerializeField]//set by Developer
    private IngredientsENUM ingredient;

    public IngredientsENUM Ingredient { get { return ingredient; } }//externally accessible, readonly

    [SerializeField]//set by Developer
    private Sprite icon;

    public Sprite Icon { get { return icon; } }//externally accessible, readonly

    /// <summary>
    /// Prints the Ingredient this represents.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return ingredient.ToString();
    }

    //comparison operations
    //always a comparison of ingredient value, never of Icon.

    public static bool operator >(IngredientSO a, IngredientSO b)
    {
        return a.ingredient > b.ingredient;
    }

    public static bool operator >=(IngredientSO a, IngredientSO b)
    {
        return a.ingredient >= b.ingredient;
    }

    public static bool operator <(IngredientSO a, IngredientSO b)
    {
        return a.ingredient < b.ingredient;
    }

    public static bool operator <=(IngredientSO a, IngredientSO b)
    {
        return a.ingredient <= b.ingredient;
    }

}