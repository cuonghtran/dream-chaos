using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Data")]
    public BundleObject bundleObject;
    public Attributes attributes;
    public LevelResults levelResults;
    public Weapons[] weapons;

    private readonly string firstPlay = "FirstPlay";

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        InitDataOnOpeningGame();
    }

    void InitDataOnOpeningGame()
    {
        try
        {
            AudioManager.Instance.MusicSettings();
            SaveLoadSystem.LoadPlayerData(bundleObject);
        }
        catch (FileNotFoundException e)
        {
            InitWeaponsValue();
            InitAttributesValue();
            InitLevelResultsValue();

            AudioManager.Instance.FirstPlayMusicSettings();
        }
    }

    public bool CheckFirstTimePlaying()
    {
        if (PlayerPrefs.GetInt(firstPlay) == 1)
            return false;
        else return true;
    }

    void InitAttributesValue()
    {
        attributes.maximumHealth = 12;
        attributes.bonusHealth = 0;
        attributes.activeWeapon1 = weapons[0];
        attributes.activeWeapon2 = null;
        attributes.Perks = new string[0];
        attributes.healingPotions = 0;
        attributes.totalCurrency = 0;
        attributes.totalGem = 0;
        attributes.displayDmgText = true;
        attributes.displayHealthBar = true;
        attributes.difficultyMode = "Normal";
        attributes.unlockNewItem = false;
    }

    void InitLevelResultsValue()
    {
        levelResults.progress = 0;
        levelResults.currentLevelNode = "";
        levelResults.CompletedLevels = new List<string>();
        levelResults.LevelsResults = new List<string>();
    }

    void InitWeaponsValue()
    {
        int i = 0;
        while (i < weapons.Length)
        {
            switch(i)
            {
                case 0:
                    weapons[0].weaponName = "Blade";
                    weapons[0].damage = 23;
                    weapons[0].fireRate = 0.6f;
                    weapons[0].skillDamage = 32;
                    weapons[0].skillCooldown = 12;
                    weapons[0].perkBonus = 1;
                    weapons[0].weaponPower = 0;
                    weapons[0].knockBackPower = 23;
                    weapons[0].description = "Strike enemies in melee range and deal high damage.";
                    break;
                case 1:
                    weapons[1].weaponName = "Plasma Gun";
                    weapons[1].damage = 8;
                    weapons[1].fireRate = 0.25f;
                    weapons[1].skillDamage = 23;
                    weapons[1].skillCooldown = 10;
                    weapons[1].perkBonus = 1;
                    weapons[1].weaponPower = 0;
                    weapons[1].knockBackPower = 7;
                    weapons[1].description = "Fire plasma bullets rapidly that deal moderate damage.";
                    break;
                case 2:
                    weapons[2].weaponName = "Shuriken";
                    weapons[2].damage = 12;
                    weapons[2].fireRate = 0.85f;
                    weapons[2].skillDamage = 0;
                    weapons[2].skillCooldown = 6;
                    weapons[2].perkBonus = 1;
                    weapons[2].weaponPower = 0;
                    weapons[2].knockBackPower = 12;
                    weapons[2].description = "Hurl three shuriken in a burst which deal high damage.";
                    break;
                case 3:
                    weapons[3].weaponName = "Mini Bomb";
                    weapons[3].damage = 27;
                    weapons[3].fireRate = 1.2f;
                    weapons[3].skillDamage = 0;
                    weapons[3].skillCooldown = 14;
                    weapons[3].perkBonus = 1;
                    weapons[3].weaponPower = 0;
                    weapons[3].knockBackPower = 22;
                    weapons[3].description = "Throw a bomb which explodes after a short delay that deals area damage.";
                    break;
            }
            i++;
        }
    }
}
