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
    [SerializeField]
    private IngredientsENUM ingredient;

    public IngredientsENUM Ingredient { get { return ingredient; } }//readonly

    /// <summary>
    /// Background image of this slot.  Yellow if selected.
    /// </summary>
    [Header("---Component References---")]
    [Tooltip("Background image of this slot.  Yellow if selected.")]
    [SerializeField]
    private Image backgroundImage;
    
    public Image BackgroundImage { get { return backgroundImage; } }//readonly

    /// <summary>
    /// Picture of ingredient.
    /// </summary>
    [Tooltip("Picture of ingredient.")]
    [SerializeField]
    private Image ingredientIcon;

    public Image IngredientIcon { get { return ingredientIcon; } }//readonly

    /// <summary>
    /// Which key one presses to get this ingredient.
    /// </summary>
    [Tooltip("Which key one presses to get this ingredient.")]
    [SerializeField]
    private TextMeshProUGUI keystrokeTMP;

    public TextMeshProUGUI KeystrokeTMP { get { return keystrokeTMP; } }//readonly

    /// <summary>
    /// Quantity of ingredient.  ie double pepp.
    /// </summary>
    [Tooltip("Quantity of ingredient.  ie double pepp.")]
    [SerializeField]
    private TextMeshProUGUI quantityTMP;

    public TextMeshProUGUI QuantityTMP { get { return quantityTMP; } }//readonly

    /// <summary>
    /// Button to add this Ingredient onto Order.
    /// </summary>
    [Tooltip("Button to add this Ingredient onto Order.")]
    [SerializeField]
    private Button iconButton;

    public Button IconButton { get { return iconButton; } }//readonly

    /// <summary>
    /// Used to init references.
    /// </summary>
    private void Awake()
    {
        if(!parentOrderBuilder)
            parentOrderBuilder = GetComponentInParent<OrderBuilderMenu>();
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

}
