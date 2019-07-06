using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OrderPromptController : MonoBehaviour
{
    private static Transform mainCameraTransform;

    [SerializeField]
    private Customer customer;

    [SerializeField]
    private GameObject[] slots;

    [Header("---Ingredient Icons---")]
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

    [Header("---Reaction Icons---")]
    [SerializeField]
    private Image reactionImage;

    [SerializeField]
    private Sprite happySprite;

    [SerializeField]
    private Sprite madSprite;

    [Header("---Delays---")]
    [SerializeField]
    private float badOrderReactionTime = 1.0f;

    [SerializeField]
    private float customerHitReactionTime = 1.5f;

    [SerializeField]
    private float closeOrderPromptWindowTime = 3.0f;
    
    //
    private GameObject[] ingredientIcons;

    private IngredientsENUM[] ingredientsList;

    //Component References
    private Transform myTransform;
    
    void Start()
    {
        GatherReferences();

        ingredientsList = customer.GetOrderIngredients();

        ReadRecipe();

        CheckSlot();
    }
    
    private void Update()
    {
        PointUITowardsCamera();
    }

    /// <summary>
    /// 
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
    private void ReadRecipe()
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

    /// <summary>
    /// 
    /// </summary>
    private void PointUITowardsCamera()
    {
        myTransform.LookAt(mainCameraTransform);
    }

    /// <summary>
    /// 
    /// </summary>
    private void CheckSlot()
    {
        for (int i = 0; i < ingredientsList.Length; i++)
        {
            Instantiate(ingredientIcons[i], slots[i].transform, false);
        }
    }

    /// <summary>
    /// Iterate through and call SetActive(active) on all icons.
    /// </summary>
    private void ToggleAllIcons(bool active)
    {
        foreach (var icon in slots)
        {
            icon.SetActive(active);
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

        this.gameObject.SetActive(false);
    }

    private void ShowReaction(Sprite sprite)
    {
        ToggleAllIcons(false);//hide ingredients
        reactionImage.sprite = sprite;//swap sprite
        reactionImage.enabled = true;//show reaction
    }

    /// <summary>
    /// 
    /// </summary>
    public void OnSuccessfulOrder()
    {
        ShowReaction(happySprite);
    }

    /// <summary>
    /// 
    /// </summary>
    public void OnFailedOrder()
    {
        ShowReaction(madSprite);
        StartCoroutine(HideIngredientsWithReactionForSeconds(badOrderReactionTime));//show Ingredients after time
    }

    /// <summary>
    /// 
    /// </summary>
    public void OnCustomerHit()
    {
        ShowReaction(madSprite);
        StartCoroutine(HideIngredientsWithReactionForSeconds(customerHitReactionTime));//show Ingredients after time
    }
}
