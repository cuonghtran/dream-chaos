using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponsPower : MonoBehaviour
{
    [Header("Data:")]
    [SerializeField] private Attributes _characterAttributes;
    [SerializeField] private Weapons targetWeapon;

    [Header("References")]
    public Upgrades upgradesPanel;
    public int powerLevel;
    public Image powerBar;
    public TextMeshProUGUI costText;
    public GameObject deceaseButton;
    public GameObject increaseButton;
    CanvasGroup decreaseButtonCG;
    CanvasGroup increaseButtonCG;

    [Header("Weapon Power Bars Sprites")]
    public Sprite[] powerBarsSprites;

    // Start is called before the first frame update
    void OnEnable()
    {
        decreaseButtonCG = deceaseButton.GetComponent<CanvasGroup>();
        increaseButtonCG = increaseButton.GetComponent<CanvasGroup>();

        LoadDataFromAttributes();
    }

    void LoadDataFromAttributes()
    {
        powerLevel = targetWeapon.weaponPower;
        powerBar.sprite = powerBarsSprites[powerLevel];
        if (powerLevel < 6)
            costText.SetText(GetCost(powerLevel).ToString());
        else costText.SetText("MAX");
        ToggleButton(powerLevel);
    }

    public void IncreaseButton()
    {
        AudioManager.Instance.Play("Confirm_Sound");
        if (powerLevel >= 6)
            return;

        int nextLevelCost = int.Parse(costText.text);
        if (nextLevelCost <= _characterAttributes.totalCurrency)
        {
            powerLevel++;
            _characterAttributes.totalCurrency -= nextLevelCost;
            ButtonsHandler(powerLevel, _characterAttributes.totalCurrency);
        }
    }

    public void DecreaseButton()
    {
        AudioManager.Instance.Play("Button1");
        if (powerLevel <= 0)
            return;

        powerLevel--;
        int refundCost = GetCost(powerLevel);
        _characterAttributes.totalCurrency += refundCost;

        ButtonsHandler(powerLevel, _characterAttributes.totalCurrency);
    }

    void ButtonsHandler(int pLvl, int amount)
    {
        powerBar.sprite = powerBarsSprites[powerLevel];
        upgradesPanel.currencyBar.SetText(amount.ToString());
        if (powerLevel < 6)
            costText.SetText(GetCost(powerLevel).ToString());
        else costText.SetText("MAX");

        targetWeapon.weaponPower = powerLevel;
        _characterAttributes.totalCurrency = amount;

        upgradesPanel.UpdateAllButtonsStatus();
    }

    public void ToggleButton(int pLvl)
    {
        if (pLvl == 6)
        {
            increaseButtonCG.interactable = false;
            decreaseButtonCG.interactable = true;
            costText.color = Color.black;
            costText.fontStyle = FontStyles.Bold;
        }
        else if (pLvl < 6)
        {
            costText.fontStyle = FontStyles.Normal;
            int requiredAmt = GetCost(powerLevel);
            if (requiredAmt <= _characterAttributes.totalCurrency)
            {
                increaseButtonCG.interactable = true;
                costText.color = Color.white;
            }
            else
            {
                increaseButtonCG.interactable = false;
                costText.color = Color.red;
            }

            if (pLvl > 0)
                decreaseButtonCG.interactable = true;
            else decreaseButtonCG.interactable = false;
        }
    }

    int GetCost(int pLvl)
    {
        int nextLevelCost = WeaponsList.UpgradesCost[pLvl];
        return nextLevelCost;
    }
}
