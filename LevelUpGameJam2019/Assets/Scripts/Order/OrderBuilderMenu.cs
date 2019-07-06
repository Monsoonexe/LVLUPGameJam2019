using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderBuilderMenu : MonoBehaviour
{
    public const int maxIngredientsOnAnOrder = 5;

    [SerializeField]
    private Animator ingredientLoaderAnimator;

    //ingredients that are on pizza
    [SerializeField]
    private List<IngredientsENUM> selectedIngredients = new List<IngredientsENUM>();

    //[Header("---Ingredient Icons---")]

    [Header("---Ingredient Backgrounds---")]
    [SerializeField]
    private Image sauceBackgroundImage;

    [SerializeField]
    private Image cheeseBackgroundImage;

    [SerializeField]
    private Image peppBackgroundImage;

    [SerializeField]
    private Image sausageBackgroundImage;

    [SerializeField]
    private Image anchovyBackgroundImage;

    [Space(5)]
    [Header("---Icon Background Colors---")]
    [SerializeField] private Color ingredientAddedColor;

    [SerializeField] private Color normalBackgroundColor;

    /// <summary>
    /// List of all background images.
    /// </summary>
    private readonly Image[] backgroundImages = new Image[maxIngredientsOnAnOrder];

    // Start is called before the first frame update
    void Start()
    {
        InitBackgroundImagesArray();
        ResetAllBackgrounds();
    }

    // Update is called once per frame
    void Update()
    {
        GetKeyBoardInput();

    }

    /// <summary>
    /// Add all reference to a list for easy iteration.
    /// </summary>
    private void InitBackgroundImagesArray()
    {
        backgroundImages[0] = sauceBackgroundImage;
        backgroundImages[1] = cheeseBackgroundImage;
        backgroundImages[2] = peppBackgroundImage;
        backgroundImages[3] = sausageBackgroundImage;
        backgroundImages[4] = anchovyBackgroundImage;
    }

    private void ResetAllBackgrounds()
    {
        foreach(var image in backgroundImages)
        {
            image.color = normalBackgroundColor;
        }
    }

    public void OnOrderFired()
    {
        selectedIngredients.Clear();
        ResetAllBackgrounds();
    }

    /// <summary>
    /// Get Ingredients and clear list.
    /// </summary>
    /// <returns></returns>
    public IngredientsENUM[] GetIngredients()
    {
        var ingredients = selectedIngredients.ToArray();
        selectedIngredients.Clear();//clear list after cannon shot

        return ingredients;
    }

    public void AddAnchovies()
    {
        selectedIngredients.Add(IngredientsENUM.Anchovies);
        anchovyBackgroundImage.color = ingredientAddedColor;
    }

    public void AddCheese()
    {
        selectedIngredients.Add(IngredientsENUM.Cheese);
        cheeseBackgroundImage.color = ingredientAddedColor;
    }

    public void AddPepperoni()
    {
        selectedIngredients.Add(IngredientsENUM.Pepperoni);
        peppBackgroundImage.color = ingredientAddedColor;
    }

    public void AddSauce()
    {
        selectedIngredients.Add(IngredientsENUM.Sauce);
        sauceBackgroundImage.color = ingredientAddedColor;
    }

    public void AddSausage()
    {
        selectedIngredients.Add(IngredientsENUM.Sausage);
        sausageBackgroundImage.color = ingredientAddedColor;
    }

    /// <summary>
    /// If Player has a keyboard, can use 1-0 to add ingredients.
    /// </summary>
    private void GetKeyBoardInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            AddSauce();
        }

        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            AddCheese();
        }

        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            AddPepperoni();
        }

        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            AddSausage();
        }

        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            AddAnchovies();
        }

        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            Debug.Log("No Ingredient for this key.");
        }

        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            Debug.Log("No Ingredient for this key.");
        }

        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            Debug.Log("No Ingredient for this key.");
        }

        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            Debug.Log("No Ingredient for this key.");
        }

        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Debug.Log("No Ingredient for this key.");
        }

    }
}

