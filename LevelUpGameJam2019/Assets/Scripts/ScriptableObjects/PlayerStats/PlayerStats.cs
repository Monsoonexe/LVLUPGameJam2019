using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats_", menuName = "ScriptableObjects/PlayerStats")]
public class PlayerStats : RichScriptableObject
{
    [Header("= Player Stats =")]
    public string playerName = "Joe the Pizza Slinger.";

    [Header("---Upgrade Levels---")]
    public int playerLvel = 1;

    public int pizzaCannonLevel = 1;

    public int pizzaShipLevel = 1;

    [Header("---Money---")]
    public int money = 0;

    public int premiumMoney = 0;

}
