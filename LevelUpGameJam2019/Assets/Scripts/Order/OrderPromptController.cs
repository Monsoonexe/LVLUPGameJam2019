using UnityEngine;

public class OrderPromptController : MonoBehaviour
{
    private static Transform mainCameraTransform;

    [SerializeField]
    private Customer customer;

    [SerializeField]
    private GameObject[] slots;

    private GameObject[] ingredientIcons;

    [SerializeField]
    private GameObject reactionSlot;

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

    [SerializeField]
    private GameObject happyIcon;

    [SerializeField]
    private GameObject madIcon;

    //Component References
    private Transform myTransform;


    private IngredientsENUM[] ingredientsList;
     
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
        myTransform = this.gameObject.transform;
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

    public void SuccessfulOrder()
    {
        foreach (var icon in slots)
        {
            icon.SetActive(false);
        }

        Instantiate(happyIcon, reactionSlot.transform, false);
    }

    public void FailureOrder()
    {
        foreach (var icon in slots)
        {
            icon.SetActive(false);
        }

        Instantiate(madIcon, reactionSlot.transform, false);
    }
}
