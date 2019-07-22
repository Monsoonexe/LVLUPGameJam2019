using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IngredientSlotController : MonoBehaviour
{
    private static OrderBuilderMenu parentOrderBuilder;

    /// <summary>
    /// Which Ingredient this slot represents.
    /// </summary>
    [Tooltip("Which Ingredient this slot represents.")]
    [SerializeField]//set by Developer
    private IngredientsENUM ingredient;//used internally

    public IngredientsENUM Ingredient { get { return ingredient; } }//visibly external, readonly

    /// <summary>
    /// Background image of this slot.  Yellow if selected.
    /// </summary>
    [Header("---Component References---")]
    [Tooltip("Background image of this slot.  Yellow if selected.")]
    [SerializeField]//set by Developer
    private Image backgroundImage;//used internally

    public Image BackgroundImage { get { return backgroundImage; } }//visibly external, readonly

    /// <summary>
    /// Picture of ingredient.
    /// </summary>
    [Tooltip("Picture of ingredient.")]
    [SerializeField]//set by Developer
    private Image ingredientIcon;//used internally

    public Image IngredientIcon { get { return ingredientIcon; } }//visibly external, readonly

    /// <summary>
    /// Which key one presses to get this ingredient.
    /// </summary>
    [Tooltip("Which key one presses to get this ingredient.")]
    [SerializeField]//set by Developer
    private TextMeshProUGUI keystrokeTMP;//used internally

    public TextMeshProUGUI KeystrokeTMP { get { return keystrokeTMP; } }//visibly external, readonly

    /// <summary>
    /// Quantity of ingredient.  ie double pepp.
    /// </summary>
    [Tooltip("Quantity of ingredient.  ie double pepp.")]
    [SerializeField]//set by Developer
    private TextMeshProUGUI quantityTMP;//used internally

    public TextMeshProUGUI QuantityTMP { get { return quantityTMP; } }//visibly external, readonly

    /// <summary>
    /// Button to add this Ingredient onto Order.
    /// </summary>
    [Tooltip("Button to add this Ingredient onto Order.")]
    [SerializeField]//set by Developer
    private Button iconButton;//used internally

    public Button IconButton { get { return iconButton; } }//visibly external, readonly

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
        parentOrderBuilder.AddIngredient(ingredient);
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
    public void InitSlot(IngredientsENUM ingredient, Sprite sprite, int keystroke)
    {
        this.ingredient = ingredient;
        this.ingredientIcon.sprite = sprite;
        this.keystrokeTMP.text = keystroke.ToString();
    }

    /// <summary>
    /// Inform this Slot that it will not be used this round.
    /// </summary>
    public void DisableSlot()
    {
        this.gameObject.SetActive(false);
    }

}
