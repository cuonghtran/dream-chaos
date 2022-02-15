using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapons : ScriptableObject
{
    public string weaponName;
    public float damage;
    public float fireRate;
    public float skillDamage;
    public float skillCooldown;
    public float perkBonus = 0;
    public string description;
    public int weaponPower = 0;
    private float dmgIncreasedPerPower = 0.19f;
    public float knockBackPower;

    // Tooltip related
    // public Color textColor;

    public string ColoredName()
    {
        // string hexColor = ColorUtility.ToHtmlStringRGB(textColor);
        string hexColor = "FF6A00";
        return $"<color=#{hexColor}>{weaponName}</color>";
    }

    public string GetTooltipText()
    {
        StringBuilder sb = new StringBuilder();

        sb.Append("<size=44>").Append(ColoredName()).Append("</size>").AppendLine();
        sb.Append(description);
        return sb.ToString();
    }

    public float GetFinalDamage()
    {
        return damage * (powerModifier() + perkBonus) * Random.Range(0.8f, 1.2f);
    }

    public float GetSkillDamage()
    {
        return skillDamage * (powerModifier() + perkBonus) * Random.Range(0.8f, 1.2f);
    }

    float powerModifier()
    {
        return weaponPower * dmgIncreasedPerPower;
    }

    public void LoadFromData(WeaponData data)
    {
        weaponName = data.weaponName;
        damage = data.damage;
        fireRate = data.fireRate;
        skillDamage = data.skillDamage;
        skillCooldown = data.skillCooldown;
        perkBonus = data.perkBonus;
        description = data.description;
        weaponPower = data.weaponPower;
        knockBackPower = data.knockBackPower;
    }
}