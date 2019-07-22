using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderBuilderMenu : MonoBehaviour
{
    public const int maxIngredientsOnAnOrder = 5;
    public const int maxIngredientStack = 2;
    private const int maxIngredientsInCannon = maxIngredientsOnAnOrder * maxIngredientStack; //5 ingredients, 2 stack per.  

    [SerializeField]
    private Animator ingredientLoaderAnimator;

    //ingredients that are on pizza
    [SerializeField]
    private List<IngredientsENUM> selectedIngredients = new List<IngredientsENUM>();

    //[Header("---Ingredient Icons---")]

    [Header("---Ingredient Backgrounds---")]
    [SerializeField]
    private IngredientSlotController ingredientSlot_0;

    [SerializeField]
    private IngredientSlotController ingredientSlot_1;

    [SerializeField]
    private IngredientSlotController ingredientSlot_2;

    [SerializeField]
    private IngredientSlotController ingredientSlot_3;

    [SerializeField]
    private IngredientSlotController ingredientSlot_4;

    private readonly IngredientSlotController[] ingredientSlotArray = new IngredientSlotController[maxIngredientsOnAnOrder];
    
    [Space(5)]
    [Header("---Icon Background Colors---")]
    [SerializeField] private Color ingredientAddedColor;

    [SerializeField] private Color normalBackgroundColor;
    
    //external mono Components
    private static LevelManager levelManager;

    private void Awake()
    {
        GatherReferences();
    }

    // Start is called before the first frame update
    void Start()
    {
        InitBackgroundImagesArray();

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
    /// Add all reference to a list for easy iteration.
    /// </summary>
    private void InitBackgroundImagesArray()
    {
        ingredientSlotArray[0] = ingredientSlot_0;
        ingredientSlotArray[1] = ingredientSlot_1;
        ingredientSlotArray[2] = ingredientSlot_2;
        ingredientSlotArray[3] = ingredientSlot_3;
        ingredientSlotArray[4] = ingredientSlot_4;
    }

    /// <summary>
    /// Set to default state.
    /// </summary>
    private void ResetSelectedSlots()
    {
        foreach(var slot in ingredientSlotArray)
        {
            slot.BackgroundImage.color = normalBackgroundColor;//reset background color
            slot.QuantityTMP.enabled = false;//disabled double ingredients indicator
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
    public void SetAvailableIngredients(IngredientsENUM[] availableIngredients)
    {
        //iterate through each element in list
        //toggle on or off if icon is in list
        Debug.Log("Shelled function: SetAvailableIngredients");

    }
}

