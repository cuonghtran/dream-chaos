using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public AttrData attributes;
    public LevelData levelResults;
    public WeaponData twinBladesWeapon;
    public WeaponData plasmaGunWeapon;
    public WeaponData shurikenWeapon;
    public WeaponData miniBombWeapon;

    public PlayerData(Attributes attr, LevelResults lvlResults, Weapons[] weapons)
    {
        attributes = new AttrData(attr);
        levelResults = new LevelData(lvlResults);
        twinBladesWeapon = new WeaponData(weapons[0]);
        plasmaGunWeapon = new WeaponData(weapons[1]);
        shurikenWeapon = new WeaponData(weapons[2]);
        miniBombWeapon = new WeaponData(weapons[3]);
    }
}
