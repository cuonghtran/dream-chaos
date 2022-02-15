using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attributes", menuName = "Character Attributes")]
public class Attributes : ScriptableObject
{
    public float maximumHealth = 12;
    public float bonusHealth = 0;

    public Weapons activeWeapon1;
    public Weapons activeWeapon2;

    public string[] Perks = new string[3];

    public int healingPotions;

    public int totalCurrency = 0;
    public int totalGem = 0;

    public bool displayDmgText = true;
    public bool displayHealthBar = true;
    public string difficultyMode;

    public bool unlockNewItem;

    public void LoadFromData(AttrData data)
    {
        maximumHealth = data.maximumHealth;
        bonusHealth = data.bonusHealth;
        activeWeapon1.LoadFromData(data.activeWeapon1);
        if (activeWeapon2 != null)
            activeWeapon2.LoadFromData(data.activeWeapon2);
        Perks = data.Perks;
        healingPotions = data.healingPotions;
        totalCurrency = data.totalCurrency;
        totalGem = data.totalGem;
        displayDmgText = data.displayDmgText;
        displayHealthBar = data.displayHealthBar;
        difficultyMode = data.difficultyMode;
        unlockNewItem = data.unlockNewItem;
    }
}
