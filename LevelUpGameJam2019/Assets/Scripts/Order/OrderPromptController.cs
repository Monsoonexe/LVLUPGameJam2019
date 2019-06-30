using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderPromptController : MonoBehaviour
{
    
    [SerializeField]
    private Customer customer;

    [SerializeField]
    private GameObject[] slots;

    private GameObject[] ingredientIcons;

    [SerializeField]
    private GameObject sauceIcon;

    [SerializeField]
    private GameObject cheeseIcon;

    [SerializeField]
    private GameObject peppIcon;

    [SerializeField]
    private GameObject sausageIcon;

    [SerializeField]
    private GameObject anchovyIcon;

    private IngredientsENUM[] ingredientsList;
     
    void Start()
    {
        ingredientsList = customer.GetOrderIngredients();
        ReadRecipe();
        CheckSlot();
    }

    public void ReadRecipe()
    {
        ingredientIcons = new GameObject[ingredientsList.Length];

        for (var i = 0; i < ingredientsList.Length; i++)
        {
            switch (ingredientsList[i])                
            {
                case IngredientsENUM.Sauce:
                    ingredientIcons[i] = sauceIcon;
                    break;
                case IngredientsENUM.Cheese:
                    ingredientIcons[i] = cheeseIcon;
                    break;
                case IngredientsENUM.Pepperoni:
                    ingredientIcons[i] = peppIcon;
                    break;
                case IngredientsENUM.Sausage:
                    ingredientIcons[i] = sausageIcon;
                    break;
                case IngredientsENUM.Anchovies:
                    ingredientIcons[i] = anchovyIcon;
                    break;
            }
        }
    }

    public void CheckSlot()
    {
        for (int i = 0; i < ingredientsList.Length; i++)
        {
            Instantiate(ingredientIcons[i], slots[i].transform, false);
        }
    }
}
