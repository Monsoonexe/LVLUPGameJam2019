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

    private void PointUITowardsCamera()
    {
        myTransform.LookAt(mainCameraTransform);
    }

    private void CheckSlot()
    {
        for (int i = 0; i < ingredientsList.Length; i++)
        {
            Instantiate(ingredientIcons[i], slots[i].transform, false);
        }
    }

    /// <summary>
    /// SetActive(false) on all icons.
    /// </summary>
    private void DisableAllIcons()
    {
        foreach (var icon in slots)
        {
            icon.SetActive(false);
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
    }

    /// <summary>
    /// 
    /// </summary>
    public void OnSuccessfulOrder()
    {
        DisableAllIcons();
        reactionImage.sprite = happySprite;
        reactionImage.enabled = true;
        //Instantiate(happyIcon, reactionSlot.transform, false);
    }

    public void OnFailedOrder()
    {
        DisableAllIcons();

        //Instantiate(madIcon, reactionSlot.transform, false);
        reactionImage.sprite = madSprite;
        reactionImage.enabled = true;
        StartCoroutine(HideIngredientsWithReactionForSeconds(badOrderReactionTime));
    }

    public void OnCustomerHit()
    {
        reactionImage.sprite = madSprite;
        reactionImage.enabled = true;
        StartCoroutine(HideIngredientsWithReactionForSeconds(customerHitReactionTime));
    }
}
