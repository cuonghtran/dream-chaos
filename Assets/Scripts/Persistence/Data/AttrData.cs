using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttrData
{
    public float maximumHealth;
    public float bonusHealth;

    public WeaponData activeWeapon1;
    public WeaponData activeWeapon2;

    public string[] Perks = new string[3];

    public int healingPotions;

    public int totalCurrency;
    public int totalGem;

    public bool displayDmgText;
    public bool displayHealthBar;
    public string difficultyMode;

    public bool unlockNewItem;

    public AttrData(Attributes attr)
    {
        maximumHealth = attr.maximumHealth;
        bonusHealth = attr.bonusHealth;
        activeWeapon1 = new WeaponData(attr.activeWeapon1);
        activeWeapon2 = new WeaponData(attr.activeWeapon2);
        Perks = attr.Perks;
        healingPotions = attr.healingPotions;
        totalCurrency = attr.totalCurrency;
        totalGem = attr.totalGem;
        displayDmgText = attr.displayDmgText;
        displayHealthBar = attr.displayHealthBar;
        difficultyMode = attr.difficultyMode;
        unlockNewItem = attr.unlockNewItem;
    }
}
