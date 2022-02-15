using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager SharedInstance;

    [Header("Data")]
    public LevelResults levelResult;
    public Attributes characterAttr;

    [Header("Current Level Info:")]
    public int levelOrder;
    public int currencyCollectedThisSession = 0;
    public int starsCollectedThisSession = 0;
    public string[] starsCollected = new string[3];
    public bool displayDmgText;
    public bool displayHealthBar;
    public int healingPotions;
    public string difficulty;
    public static bool Is16x9ScreenRatio;

    [Header("References")]
    public GameObject star1;
    public GameObject star2;
    public GameObject star3;

    void Awake()
    {
        //Physics2D.IgnoreLayerCollision(8, 9, true);
        //Physics2D.IgnoreLayerCollision(8, 10, true);
        //Physics2D.IgnoreLayerCollision(8, 11, true);

        SharedInstance = this;

        levelOrder = levelResult.CompletedLevels.IndexOf(levelResult.currentLevelNode);
        if (levelOrder < 0)
        {
            if (levelResult.progress > 0)
                levelOrder = levelResult.progress;
            else levelOrder = 0;
        }
        if (levelOrder < levelResult.LevelsResults.Count)
        {
            var result = levelResult.LevelsResults[levelOrder];
            if (result != null && result != "")
            {
                starsCollected = result.Split(',');
                if (starsCollected[0] == "1") // already collected
                    star1.SetActive(false);
                if (starsCollected[1] == "1")
                    star2.SetActive(false);
                if (starsCollected[2] == "1")
                    star3.SetActive(false);
            }
        }
        else
        {
            starsCollected = new string[] { "0", "0", "0" };
        }
        
        displayDmgText = characterAttr.displayDmgText;
        displayHealthBar = characterAttr.displayHealthBar;
        difficulty = characterAttr.difficultyMode;
        healingPotions = characterAttr.healingPotions;

        if ((Screen.width / Screen.height) == 16 / 9)
            Is16x9ScreenRatio = true;

        StartCoroutine(PlayThemeSong());
    }

    IEnumerator PlayThemeSong()
    {
        yield return new WaitForSeconds(1f);
        AudioManager.Instance.Play("Map_Theme");
    }

    public void LoadWorldScene()
    {
        AudioManager.Instance.Stop("Map_Theme");
        AudioManager.Instance.Stop("Boss_Main");
        SceneController.Instance.FadeAndLoadScene(ScenesList.WorldScene);
    }

    public void RestartLevel()
    {
        AudioManager.Instance.Stop("Map_Theme");
        AudioManager.Instance.Stop("Boss_Main");
        SceneController.Instance.FadeAndLoadScene(levelResult.currentLevelNode);
    }

    public void CollectCurrency(int value)
    {
        currencyCollectedThisSession += value;
        UIManager.SharedInstance.UpdateCurrency(currencyCollectedThisSession);
    }

    public void CollectStar(int order)
    {
        starsCollected[order] = "1";
        starsCollectedThisSession++;
    }

    public void UpdateToLevelResult()
    {
        StringBuilder s = new StringBuilder();
        if (!levelResult.CompletedLevels.Contains(levelResult.currentLevelNode))
        {
            levelResult.CompletedLevels.Add(levelResult.currentLevelNode);
            s.Append(starsCollected[0]).Append(",");
            s.Append(starsCollected[1]).Append(",");
            s.Append(starsCollected[2]);
            levelResult.LevelsResults.Add(s.ToString());
            if (ScenesList.HeartReward[levelResult.currentLevelNode])  // reward hearts if eligible
                characterAttr.bonusHealth += 4;
            levelResult.progress++;

            // check if unlock items
            if (ScenesList.ItemsUnlockLevels.Contains(levelResult.progress))
                characterAttr.unlockNewItem = true;
        }
        else
        {
            s.Append(starsCollected[0]).Append(",");
            s.Append(starsCollected[1]).Append(",");
            s.Append(starsCollected[2]);
            levelResult.LevelsResults[levelOrder] = s.ToString();
        }
    }

    public void UpdateToAttributes()
    {
        characterAttr.totalCurrency += currencyCollectedThisSession;
        characterAttr.totalGem += (starsCollectedThisSession * 50);
    }

    public void UpdateDamageText(bool isShow)
    {
        displayDmgText = isShow;
    }

    public void UpdateHealthBar(bool isShow)
    {
        displayHealthBar = isShow;
        var hb = GameObject.FindGameObjectsWithTag("HealthBar");
        foreach (GameObject obj in hb)
        {
            obj.GetComponent<CanvasGroup>().alpha = isShow ? 1 : 0;
        }
    }

    public void ConsumePotion()
    {
        healingPotions--;
        characterAttr.healingPotions = healingPotions;
        UIManager.SharedInstance.UpdateHealingPotions();
        UIManager.SharedInstance.UpdateHearts();
        StartCoroutine(UIManager.SharedInstance.DrinkPotionsFlash());
    }
}
