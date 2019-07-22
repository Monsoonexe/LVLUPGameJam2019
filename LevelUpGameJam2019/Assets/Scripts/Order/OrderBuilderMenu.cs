using System.Collections.Generic;
using UnityEngine;

public class OrderBuilderMenu : MonoBehaviour
{
    #region static consts
    /// <summary>
    /// Number of different Ingredients that can exist in a Level.
    /// </summary>
    public const int uniqueIngredients = 5;
    
    #endregion

    [SerializeField]
    private Animator ingredientLoaderAnimator;

    //ingredients that are on pizza
    [SerializeField]//visualization only! no touchy
    private List<IngredientsENUM> selectedIngredients = new List<IngredientsENUM>();
    
    [Header("---Ingredient Slots---")]
    [SerializeField]//set by Developer
    private IngredientSlotController[] ingredientSlots = new IngredientSlotController[uniqueIngredients];
    
    [Header("---Icon Background Colors---")]
    [SerializeField]//set by Developer
    private Color addedBackgroundColor;

    public Color AddedBackgroundColor { get { return addedBackgroundColor; } }// externally visible, readonly

    [SerializeField]//set by Developer
    private Color normalBackgroundColor;

    public Color NormalBackgroundColor { get { return normalBackgroundColor; } }// externally visible, readonly

    //external mono Components
    private static LevelManager levelManager;

    private void Awake()
    {
        GatherReferences();
    }

    // Start is called before the first frame update
    void Start()
    {
        ResetSelectedSlots();
    }

    // Update is called once per frame
    void Update()
    {
        GetKeyBoardInput();
    }

    private void OnEnable()
    {
        levelManager.LevelsEndEvent.AddListener(OnLevelsEnd);//subscribe to end level event
    }

    private void OnDisable()
    {
        levelManager.LevelsEndEvent.RemoveListener(OnLevelsEnd);//subscribe to end level event
    }

    private void GatherReferences()
    {
        if (!levelManager)
        {
            levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        }        
    }
    
    /// <summary>
    /// Set to default state.
    /// </summary>
    private void ResetSelectedSlots()
    {
        foreach(var slot in ingredientSlots)
        {
            slot.ResetSlot();//start over
        }
    }

    public void OnOrderFired()
    {
        selectedIngredients.Clear();

        ResetSelectedSlots();
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

    /// <summary>
    /// Add an Ingredient to the list.
    /// </summary>
    /// <param name="ingredient">Int to cast to an ENUM</param>
    public void AddIngredient(int ingredient)
    {
        AddIngredient((IngredientsENUM)ingredient);
    }
    
    /// <summary>
    /// Add an Ingredient to the list.
    /// </summary>
    /// <param name="ingredient"></param>
    public void AddIngredient(IngredientsENUM ingredientToAdd)
    {
        selectedIngredients.Add(ingredientToAdd);
    }

    /// <summary>
    /// If Player has a keyboard, can use 1-0 to add ingredients.
    /// </summary>
    private void GetKeyBoardInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            AddIngredient(0);// 1 - 1
            ingredientSlots[0].AddIngredientToOrder();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            AddIngredient(1);// 2 - 1
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            AddIngredient(2);// 3 - 1
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            AddIngredient(3);// 4 - 1
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            AddIngredient(4);// 5 - 1
        }

        //guaranteed 5 ingredients max
        //if (Input.GetKeyDown(KeyCode.Alpha6))
        //{
        //    Debug.Log("No Ingredient for this key.");
        //}

        //if (Input.GetKeyDown(KeyCode.Alpha7))
        //{
        //    Debug.Log("No Ingredient for this key.");
        //}

        //if (Input.GetKeyDown(KeyCode.Alpha8))
        //{
        //    Debug.Log("No Ingredient for this key.");
        //}

        //if (Input.GetKeyDown(KeyCode.Alpha9))
        //{
        //    Debug.Log("No Ingredient for this key.");
        //}

        //if (Input.GetKeyDown(KeyCode.Alpha0))
        //{
        //    Debug.Log("No Ingredient for this key.");
        //}

    }

    /// <summary>
    /// Procedure to follow at the end of the level.
    /// </summary>
    public void OnLevelsEnd()
    {
        this.gameObject.SetActive(false);//disable this whole object.  no taking orders after time is up!
    }

    /// <summary>
    /// Remove Ingredients as options that are not in this list.
    /// </summary>
    /// <param name="availableIngredients"></param>
    public void SetAvailableIngredients(IngredientsENUM[] ingredientsAvailableOnCannon)
    {
        //iterate through each element in list
        //toggle on or off if icon is in list
        Debug.Log("Shelled function: SetAvailableIngredients");

        foreach(var slot in ingredientSlots)//disable ALL slots.
        {
            slot.DisableSlot();
        }

        for(var i = 0; i < ingredientSlots.Length; ++i)//only add the ones you need.
        {
            if (ingredientSlots[i].Ingredient == ingredientsAvailableOnCannon[i])
            {

            }

        }

    }
}

