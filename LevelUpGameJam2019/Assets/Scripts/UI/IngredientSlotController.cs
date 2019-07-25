using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IngredientSlotController : MonoBehaviour
{
    /// <summary>
    /// Max number of duplicate Ingredients allowed.
    /// </summary>
    public const int maxIngredientStack = 2;

    /// <summary>
    /// Shared class reference.
    /// </summary>
    private static OrderBuilderMenu parentOrderBuilder;

    /// <summary>
    /// Which Ingredient this slot represents.
    /// </summary>
    [Tooltip("Which Ingredient this slot represents.")]
    [SerializeField]//set by Developer
    private IngredientSO ingredient;//used internally

    public IngredientSO Ingredient { get { return ingredient; } }//visible externally, readonly

    /// <summary>
    /// Background image of this slot.  Yellow if selected.
    /// </summary>
    [Header("---Component References---")]
    [Tooltip("Background image of this slot.  Yellow if selected.")]
    [SerializeField]//set by Developer
    private Image backgroundImage;//used internally

    public Image BackgroundImage { get { return backgroundImage; } }//visible externally, readonly

    /// <summary>
    /// Picture of ingredient.
    /// </summary>
    [Tooltip("Picture of ingredient.")]
    [SerializeField]//set by Developer
    private Image ingredientIcon;//used internally

    public Image IngredientIcon { get { return ingredientIcon; } }//visible externally, readonly

    /// <summary>
    /// Which key Player presses to get this ingredient. Display only.
    /// </summary>
    [Tooltip("Which key Player presses to get this ingredient. Display only.")]
    [SerializeField]//set by Developer
    private TextMeshProUGUI keystrokeTMP;//used internally

    public TextMeshProUGUI KeystrokeTMP { get { return keystrokeTMP; } }//visible externally, readonly

    /// <summary>
    /// Quantity of ingredient.  ie double pepp.
    /// </summary>
    [Tooltip("Quantity of ingredient.  ie double pepp.")]
    [SerializeField]//set by Developer
    private TextMeshProUGUI quantityTMP;//used internally

    public TextMeshProUGUI QuantityTMP { get { return quantityTMP; } }//visible externally, readonly

    /// <summary>
    /// Button to add this Ingredient onto Order.
    /// </summary>
    [Tooltip("Button to add this Ingredient onto Order.")]
    [SerializeField]//set by Developer
    private Button iconButton;//used internally

    public Button IconButton { get { return iconButton; } }//visible externally, readonly

    private int ingredientQuantity = 0;

    /// <summary>
    /// Used to init references.
    /// </summary>
    private void Awake()
    {
        if(!parentOrderBuilder)
            parentOrderBuilder = GetComponentInParent<OrderBuilderMenu>();
    }

    private void OnEnable()
    {
        iconButton.onClick.AddListener(AddIngredientToOrder);//handle event
    }

    private void OnDisable()
    {
        iconButton.onClick.RemoveListener(AddIngredientToOrder);//handle event
    }

    /// <summary>
    /// Called by Button in Scene. Add this Ingredient to Order.
    /// </summary>
    public void AddIngredientToOrder()
    {
        if (++ingredientQuantity > maxIngredientStack) return;//only have 0, 1, or 2 quantity

        backgroundImage.color = parentOrderBuilder.AddedBackgroundColor;//change background color
        parentOrderBuilder.AddIngredient(ingredient);//add to Ingredients on Order list
        quantityTMP.enabled = ingredientQuantity > 1;//enable tmp if x2 quantity
    }

    public void SetKeystrokeValue(int keystroke)
    {
        keystrokeTMP.text = keystroke.ToString();
    }

    /// <summary>
    /// Init this Slot externally.
    /// </summary>
    /// <param name="ingredient"></param>
    /// <param name="sprite"></param>
    /// <param name="keystroke"></param>
    public void InitSlot(IngredientSO ingredient, Sprite sprite, int keystroke)
    {
        this.ingredient = ingredient;
        this.ingredientIcon.sprite = sprite;
        this.keystrokeTMP.text = keystroke.ToString();
    }

    /// <summary>
    /// Inform this Slot that it will not be used this round.
    /// </summary>
    public void ToggleVisuals(bool active)
    {
        gameObject.SetActive(active);
    }

    /// <summary>
    /// Hide visuals, reset counter...
    /// </summary>
    public void ResetSlot()
    {
        ingredientQuantity = 0;
        backgroundImage.color = parentOrderBuilder.NormalBackgroundColor;//reset background color
        quantityTMP.enabled = false;//disabled double ingredients indicator (don't show 0 or 1)
    }

    /// <summary>
    /// Externally configure Slot.
    /// </summary>
    /// <param name="keystroke">What to show to the Player.</param>
    /// <param name="ingredient">Which Ingredient this Slot adds.</param>
    /// <param name="ingredientIcon">The graphic to display.</param>
    public void ConfigureSlot(int keystroke, IngredientSO ingredient, Sprite ingredientIcon)
    {//base
        this.keystrokeTMP.text = keystroke.ToString();
        this.ingredient = ingredient;
        this.ingredientIcon.sprite = ingredientIcon;
    }

    /// <summary>
    /// Externally configure Slot.
    /// </summary>
    /// <param name="keystroke">What to show to the Player.</param>
    /// <param name="ingredient">Which Ingredient this Slot adds.</param>
    public void ConfigureSlot(IngredientSO ingredient, int keystroke)
    {
        ConfigureSlot(keystroke, ingredient, ingredient.Icon);
    }

    /// <summary>
    /// Externally configure Slot.
    /// </summary>
    /// <param name="keystroke">What to show to the Player.</param>
    /// <param name="ingredient">Which Ingredient this Slot adds.</param>
    /// <param name="ingredientIcon">The graphic to display.</param>
    public void ConfigureSlot(string keystroke, IngredientSO ingredient, Sprite ingredientIcon)
    {
        this.keystrokeTMP.text = keystroke;
        this.ingredient = ingredient;
        this.ingredientIcon.sprite = ingredientIcon;
    }
}
