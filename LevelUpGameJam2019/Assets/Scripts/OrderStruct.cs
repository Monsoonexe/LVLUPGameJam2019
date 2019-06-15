[System.Serializable]
public struct OrderStruct
{
    public IngredientsENUM[] ingredients;

    public OrderStruct(IngredientsENUM[] ingredients){
        this.ingredients = ingredients;

    }
}
