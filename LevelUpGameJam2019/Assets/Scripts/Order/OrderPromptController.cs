using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OrderPromptController : MonoBehaviour
{
    //shared external refs
    private static Transform mainCameraTransform;

    [SerializeField]
    private GameObject UIRoot;
    
    [Header("---Ingredient Icon Slots---")]
    [SerializeField]
    private Image[] ingredientSlotImages;

    [Header("---Reaction Icons---")]
    [SerializeField]
    private Image reactionImage;

    [SerializeField]
    private Sprite happySprite;

    [SerializeField]
    private Sprite madSprite;

    [Header("---Delays---")]
    [SerializeField]
    private float closePromptAfterSeconds = 3.0f;

    //Component References
    private Transform myTransform;
    
    private void Awake()
    {
        GatherReferences();
    }
        
    private void Update()
    {
        PointUITowardsCamera();
    }

    /// <summary>
    /// Get handles
    /// </summary>
    private void GatherReferences()
    {
        //get handle on static GameObjects
        if (!mainCameraTransform)
        {
            mainCameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform as Transform;
        }

        //Components on this GO
        myTransform = this.gameObject.transform as Transform;
    }
    
    /// <summary>
    /// 
    /// </summary>
    private void PointUITowardsCamera()
    {
        myTransform.LookAt(mainCameraTransform);
    }
    
    /// <summary>
    /// Iterate through and call SetActive(active) on all icons.
    /// </summary>
    private void ToggleAllIcons(bool active)
    {
        foreach (var icon in ingredientSlotImages)
        {
            icon.enabled = active;
        }
    }

    /// <summary>
    /// Wait some seconds and then disable the reaction.
    /// </summary>
    /// <param name="seconds"></param>
    /// <returns></returns>
    private IEnumerator HideIngredientsWithReactionForSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        reactionImage.enabled = false;
        ToggleAllIcons(true);
    }

    /// <summary>
    /// Disable Prompt when Order is no longer needed.
    /// </summary>
    /// <param name="seconds"></param>
    /// <returns></returns>
    private IEnumerator CloseOrderBubbleAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        ToggleVisuals(false);
    }

    /// <summary>
    /// Hide Ingredients, swap reaction sprite, and enable Component.
    /// </summary>
    /// <param name="sprite"></param>
    private void ShowReaction(Sprite sprite)
    {
        ToggleAllIcons(false);//hide ingredients
        reactionImage.sprite = sprite;//swap sprite
        reactionImage.enabled = true;//show reaction
    }

    public void ToggleVisuals(bool active)
    {
        UIRoot.SetActive(active);
    }

    /// <summary>
    /// Load Icons from Ingredients.
    /// </summary>
    public void LoadIcons(IngredientSO[] ingredients)
    {
        for (var i = 0; i < ingredientSlotImages.Length; i++)
        {
            if(i < ingredients.Length)
            {
                ingredientSlotImages[i].sprite = ingredients[i].Icon;
            }
            else
            {
                ingredientSlotImages[i].sprite = null;
            }
        }
    }

    /// <summary>
    /// Called externally.
    /// </summary>
    /// <param name="seconds"></param>
    public void CloseOrderPromptAfterSeconds(float seconds)
    {
        StartCoroutine(CloseOrderBubbleAfterSeconds(seconds));
    }

    /// <summary>
    /// 
    /// </summary>
    public void OnSuccessfulOrder()
    {
        ShowReaction(happySprite);

        StartCoroutine(CloseOrderBubbleAfterSeconds(closePromptAfterSeconds));
    }

    /// <summary>
    /// 
    /// </summary>
    public void OnFailedOrder(float reactionDelaySeconds = 1.0f)
    {
        ShowReaction(madSprite);
        StartCoroutine(HideIngredientsWithReactionForSeconds(reactionDelaySeconds));
    }

    /// <summary>
    /// 
    /// </summary>
    public void OnCustomerHit(float reactionDelaySeconds = 1.5f)
    {
        ShowReaction(madSprite);
        StartCoroutine(HideIngredientsWithReactionForSeconds(reactionDelaySeconds));
    }
}
