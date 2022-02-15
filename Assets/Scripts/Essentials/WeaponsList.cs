using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsList
{
    public const string TwinBlades = "Blade";
    public const string PlasmaGun = "Plasma Gun";
    public const string Shuriken = "Shuriken";
    public const string MiniBomb = "Mini Bomb";

    public static Dictionary<string, string> WeaponTooltip = new Dictionary<string, string>()
    {
        //{ "", "" },
        { TwinBlades, "<size=44><color=#6AEC00>" + TwinBlades + "</color></size><color=#EBEC00><i> -Weapon-</i></color>\nStrike enemies in short range and deal high damage." },
        { PlasmaGun, "<size=44><color=#6AEC00>" + PlasmaGun + "</color></size><color=#EBEC00><i> -Weapon-</i></color>\nFire plasma bullets rapidly that deal moderate damage." },
        { Shuriken, "<size=44><color=#6AEC00>" + Shuriken + "</color></size><color=#EBEC00><i> -Weapon-</i></color>\nHurl three shuriken in a burst which deal high damage." },
        { MiniBomb, "<size=44><color=#6AEC00>" + MiniBomb + "</color></size><color=#EBEC00><i> -Weapon-</i></color>\nThrow a bomb which explodes after a short delay that deals area damage." },
    };

    public static Dictionary<int, int> UpgradesCost = new Dictionary<int, int>()
    {
        { 0, 1000 },
        { 1, 1500 },
        { 2, 3000 },
        { 3, 4000 },
        { 4, 5000 },
        { 5, 6000 },
    };
}
