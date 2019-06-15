[System.Serializable]
public struct OrderStruct
{
    public IngredientsENUM[] ingredients;

    public int randomWeight;

    public OrderStruct(IngredientsENUM[] ingredients, int randomWeight){
        this.ingredients = ingredients;

        this.randomWeight = randomWeight;
    }
}
