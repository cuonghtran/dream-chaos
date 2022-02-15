using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FinishMenu : MonoBehaviour
{
    [Header("Stars References")]
    public Transform starsPanel;
    public Sprite wholeStarSprite;
    public Sprite emptyStarSprite;

    [Header("Currency References")]
    public TextMeshProUGUI currencyDisplay;
    public TextMeshProUGUI currencyGUI;

    [Header("Gems References")]
    public TextMeshProUGUI gemsDisplay;

    string[] totalStarsCollected;
    int starsCollectedThisSession;
    int currencyCollected;
    float guiCurrencyValue;
    float initValue = 0;
    float acceleration;
    int gemsPerStar = 50;

    // Start is called before the first frame update
    void Start()
    {
        totalStarsCollected = LevelManager.SharedInstance.starsCollected;
        starsCollectedThisSession = LevelManager.SharedInstance.starsCollectedThisSession;
        currencyCollected = LevelManager.SharedInstance.currencyCollectedThisSession;

        guiCurrencyValue = currencyCollected;
        acceleration = currencyCollected / 1.25f;

        LoadCollectedStars();
        LoadGemsReward();
    }

    void Update()
    {
        LoadCollectedCurrency(acceleration);
    }

    void LoadCollectedStars()
    {
        int i = 0;
        foreach(Transform star in starsPanel)
        {
            if (totalStarsCollected[i] == "1")
                star.GetComponent<Image>().sprite = wholeStarSprite;
            else star.GetComponent<Image>().sprite = emptyStarSprite;
            i++;
        }
    }

    void LoadGemsReward()
    {
        if (starsCollectedThisSession > 0)
        {
            gemsDisplay.text = (gemsPerStar * starsCollectedThisSession).ToString();
        }
    }

    void LoadCollectedCurrency(float acceleration)
    {
        if (initValue < currencyCollected)
            initValue += acceleration * Time.deltaTime;
        else
            initValue = currencyCollected;

        if (guiCurrencyValue > 0)
            guiCurrencyValue -= acceleration * Time.deltaTime;
        else guiCurrencyValue = 0;

        currencyDisplay.SetText(Mathf.RoundToInt(initValue).ToString());
        currencyGUI.SetText(Mathf.RoundToInt(guiCurrencyValue).ToString());
    }
}
