using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IngredientList_", menuName = "ScriptableObjects/Orders/Ingredient List")]
public class IngredientList : RichScriptableObject
{
    public List<IngredientSO> ingredients = new List<IngredientSO>();

}
