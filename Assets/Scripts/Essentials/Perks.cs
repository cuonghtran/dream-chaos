using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Perks
{
    public const string TheMoreTheBetter = "The More The Better";
    public const string HardenedSkin = "Hardened Skin";
    public const string DoubleJump = "Double Jump";
    public const string FeatherLight = "Feather Light";
    public const string Vigor = "Vigor";
    public const string SecondWind = "Second Wind";
    public const string PowerOverwhelming = "Power Overwhelming";
    public const string InnerRage = "Inner Rage";


    // tooltip related
    public static string ColoredName(string perkName)
    {
        return $"<color=#007CFF>{perkName}</color>";
    }

    public static string GetTooltipText(string perkName)
    {
        StringBuilder sb = new StringBuilder();

        sb.Append("<size=44>").Append(ColoredName(perkName)).Append("</size>").AppendLine();
        sb.Append(PerksTooltip[perkName]);
        return sb.ToString();
    }

    public static Dictionary<string, string> PerksTooltip = new Dictionary<string, string>()
    {
        { TheMoreTheBetter, "Gains 2 additional hearts." },
        { HardenedSkin, "Reduces all damage taken." },
        { DoubleJump, "Gains an ability to double jump." },
        { FeatherLight, "Increases movement speed." },
        { Vigor, "Reduce the cooldown of all special skills." },
        { SecondWind, "Gains a second life. Can only be used 1 time per level." },
        { PowerOverwhelming, "Increases the size of all bullets and projectiles." },
        { InnerRage, "Increases damage and attack speed of all weapons." },
    };

    public static Dictionary<string, float> PerksValue = new Dictionary<string, float>()
    {
        { TheMoreTheBetter, 2*4 },
        { HardenedSkin, 25 },
        { DoubleJump, 2 },
        { FeatherLight, 20 },
        { Vigor, 20 },
        { SecondWind, 3*4 },
        { PowerOverwhelming, 30 },
        { InnerRage, 10 },
    };
}
