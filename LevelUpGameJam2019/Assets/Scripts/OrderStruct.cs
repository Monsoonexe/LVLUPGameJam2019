[System.Serializable]
public struct OrderStruct
{
    public bool pepperoni;
    public bool sauce;
    public bool cheese;
    public bool sausage;
    public bool anchovies;
    public int randomWeight;

    public OrderStruct(bool sauce, bool cheese, bool pepperoni, bool sausage, bool anchovies, int randomWeight){
        this.sauce = sauce;
        this.cheese = cheese;
        this.pepperoni = pepperoni;
        this.sausage = sausage;
        this.anchovies = anchovies;
        this.randomWeight = randomWeight;
    }
}
