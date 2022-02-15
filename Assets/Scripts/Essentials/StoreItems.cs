using System.Collections;
using System.Collections.Generic;

public class StoreItems
{
    public const string PileOfGems = "Pile of Gems";
    public const string BigPileOfGems = "Big Pile of Gems";
    public const string OneHealingPotion = "Healing Potion";
    public const string ThreeHealingPotions = "Healing Potions";
    public const string Coins = "Coins";
    public const string MoreCoins = "More Coins!";

    public static Dictionary<string, int> Amount = new Dictionary<string, int>()
    {
        { PileOfGems, 300 },
        { BigPileOfGems, 700 },
        { OneHealingPotion, 1 },
        { ThreeHealingPotions, 3 },
        { Coins, 600 },
        { MoreCoins, 1800 },
    };

    public static Dictionary<string, float> Cost = new Dictionary<string, float>()
    {
        { PileOfGems, 0.99f },
        { BigPileOfGems, 1.99f },
        { OneHealingPotion, 150 },
        { ThreeHealingPotions, 450 },
        { Coins, 200 },
        { MoreCoins, 500 },
    };
}
