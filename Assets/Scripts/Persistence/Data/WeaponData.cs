using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponData
{
    public string weaponName;
    public float damage;
    public float fireRate;
    public float skillDamage;
    public float skillCooldown;
    public float perkBonus = 0;
    public string description;
    public int weaponPower = 0;
    public float knockBackPower;

    public WeaponData(Weapons wp)
    {
        if (wp != null)
        {
            weaponName = wp.weaponName;
            damage = wp.damage;
            fireRate = wp.fireRate;
            skillDamage = wp.skillDamage;
            skillCooldown = wp.skillCooldown;
            perkBonus = wp.perkBonus;
            description = wp.description;
            weaponPower = wp.weaponPower;
            knockBackPower = wp.knockBackPower;
        }
    }
}
