using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bundle Object", menuName = "Bundle Object")]
public class BundleObject : ScriptableObject
{
    public Attributes attributes;
    public LevelResults levelResults;
    public Weapons twinBladesWeapon;
    public Weapons plasmaGunWeapon;
    public Weapons shurikenWeapon;
    public Weapons miniBombWeapon;

    public BundleObject(Attributes attr, LevelResults lvlResults, Weapons[] weapons)
    {
        attributes = attr;
        levelResults = lvlResults;
        twinBladesWeapon = weapons[0];
        plasmaGunWeapon = weapons[1];
        shurikenWeapon = weapons[2];
        miniBombWeapon = weapons[3];
    }

    public void LoadFromData(PlayerData data)
    {
        attributes.LoadFromData(data.attributes);
        levelResults.LoadFromData(data.levelResults);
        twinBladesWeapon.LoadFromData(data.twinBladesWeapon);
        plasmaGunWeapon.LoadFromData(data.plasmaGunWeapon);
        shurikenWeapon.LoadFromData(data.shurikenWeapon);
        miniBombWeapon.LoadFromData(data.miniBombWeapon);
    }
}
