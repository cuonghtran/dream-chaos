using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenesList
{
    public static string OpeningScene = "Intro Scene";
    public static string CinematicScene = "Cinematic Scene";
    public static string WorldScene = "World";
    public static string Level1 = "Level1";
    public static string Level2 = "Level2";
    public static string Level3 = "Level3";
    public static string Level4 = "Level4";
    public static string Level5 = "Level5";
    public static string Level6 = "Level6";
    public static string Level7 = "Level7";
    public static string Level8 = "Level8";
    public static string Level9 = "Level9";
    public static string Level10 = "Level10";
    public static string Level11 = "Level11";
    public static string Level12 = "Level12";
    public static string Level13 = "Level13";
    public static string Level14 = "Level14";
    public static string Level15 = "Level15";
    public static string Level16 = "Level16";
    public static string Level17 = "Level17";
    public static string Level18 = "Level18";
    public static string Level19 = "Level19";
    public static string Level20 = "Level20";
    public static string Level21 = "Level21";
    public static string Level22 = "Level22";
    public static string Level23 = "Level23";
    public static string Level24 = "Level24";


    public static Dictionary<string, string> LevelTitles = new Dictionary<string, string>()
    {
        { "", "" },
        { Level1, "Level 1" },
        { Level2, "Level 2" },
        { Level3, "Level 3" },
        { Level4, "Level 4" },
        { Level5, "Level 5" },
        { Level6, "Level 6" },
        { Level7, "Level 7" },
        { Level8, "Level 8" },
        { Level9, "Level 9" },
        { Level10, "Level 10" },
        { Level11, "Level 11" },
        { Level12, "Level 12" },
        { Level13, "Level 13" },
        { Level14, "Level 14" },
        { Level15, "Level 15" },
        { Level16, "Level 16" },
        { Level17, "Level 17" },
        { Level18, "Level 18" },
        { Level19, "Level 19" },
        { Level20, "Level 20" },
        { Level21, "Level 21" },
        { Level22, "Level 22" },
        { Level23, "Level 23" },
        { Level24, "Level 24" },
    };

    public static Dictionary<string, string> LevelTheme = new Dictionary<string, string>()
    {
        { Level1, "Hills" },
        { Level2, "Hills" },
        { Level3, "Beach" }, // beachhill
        { Level4, "Beach" }, // beachhill
        { Level5, "Desert" },
        { Level6, "Desert" },
        { Level7, "Beach" }, // beachrock
        { Level8, "Hills" },
        { Level9, "Hills" }, 
        { Level10, "Desert" },
        { Level11, "Beach" }, // beachhill
        { Level12, "Beach" }, // beachrock
        { Level13, "Forest" },
        { Level14, "Forest" },
        { Level15, "Forest" },
        { Level16, "Forest" }, 
        { Level17, "Desert" },
        { Level18, "Desert" },
        { Level19, "Beach" }, // beachrock
        { Level20, "Hills" },
        { Level21, "Beach" }, // beachhill
        { Level22, "Desert" },
        { Level23, "Forest" },
        { Level24, "Beach" },  // beachrock
    };

    public static Dictionary<string, bool> HeartReward = new Dictionary<string, bool>()
    {
        { Level1, false },
        { Level2, false },
        { Level3, true },
        { Level4, false },
        { Level5, false },
        { Level6, true },
        { Level7, false },
        { Level8, true },
        { Level9, false },
        { Level10, true },
        { Level11, false },
        { Level12, false },
        { Level13, true },
        { Level14, false },
        { Level15, true },
        { Level16, false },
        { Level17, false },
        { Level18, true },
        { Level19, false },
        { Level20, true },
        { Level21, false },
        { Level22, true },
        { Level23, false },
        { Level24, false },
    };

    public static List<int> ItemsUnlockLevels = new List<int>()
    { 1, 2, 3, 4, 5, 6, 8, 10, 12, 14};

    public static readonly Dictionary<string, string> HyperLinks = new Dictionary<string, string>()
    {
        { "Privacy Policy", "https://www.websitepolicies.com/policies/view/T8sGExXM" },
        { "Terms and Conditions", "https://www.websitepolicies.com/policies/view/HjAWhk2D" },
    };
}
