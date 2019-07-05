﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderBuilderMenu : MonoBehaviour
{
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

    [Header("---Selected Color---")]
    [SerializeField] private Color ingredientAddedColor;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GetKeyBoardInput();

    }

    public void OnOrderFired()
    {
        selectedIngredients.Clear();
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
    }

    public void AddCheese()
    {
        selectedIngredients.Add(IngredientsENUM.Cheese);
    }

    public void AddPepperoni()
    {
        selectedIngredients.Add(IngredientsENUM.Pepperoni);
    }

    public void AddSauce()
    {
        selectedIngredients.Add(IngredientsENUM.Sauce);
    }

    public void AddSausage()
    {
        selectedIngredients.Add(IngredientsENUM.Sausage);
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

