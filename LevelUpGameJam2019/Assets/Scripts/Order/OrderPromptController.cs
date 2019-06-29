using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderPromptController : MonoBehaviour
{
    //static object references
    [SerializeField]
    private Customer customer;

    [SerializeField]
    private bool[] isFull;

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

    /*[SerializeField]
    private List<IngredientsENUM> selectedIngredientIcons = new List<IngredientsENUM>();*/

    private IngredientsENUM[] ingredientsList;
     

    void Awake()
    {
    }
    // Start is called before the first frame update
    void Start()
    {
        ingredientsList = customer.GetOrderIngredients();
        ReadRecipe();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReadRecipe()
    {

        ingredientIcons = new GameObject[ingredientsList.Length];
        Debug.Log(ingredientIcons.Length);
        foreach (var ingredient in ingredientsList)
        {
            Debug.Log(ingredient.ToString());
        }
        for (var i =0; i<ingredientsList.Length; i++)
        {
            switch (ingredientsList[i])
            {
                case IngredientsENUM.Sauce:
                    ingredientIcons[i] = sauceIcon;
                    Debug.Log(ingredientIcons[i].name);
                    break;
                case IngredientsENUM.Cheese:
                    ingredientIcons[i] = cheeseIcon;
                    Debug.Log(ingredientIcons[i].name);
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

            CheckSlot();

        }

    }

    public void CheckSlot()
    {
        for (int i = 0; i < ingredientsList.Length; i++)
        {
            Instantiate(ingredientIcons[i], slots[i].transform, false);
            /*if (isFull[i] == false)
            {
                // Ingrediant can be added to the first available slot
                isFull[i] = true;
                Instantiate(ingredientIcons[i], slots[i].transform, false);
                break;
            }*/
        }
    }
}
