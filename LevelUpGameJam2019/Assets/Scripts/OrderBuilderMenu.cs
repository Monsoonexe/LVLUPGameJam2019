using System.Collections;
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
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}

