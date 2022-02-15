using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PurchasePanel : MonoBehaviour
{
    public static PurchasePanel SharedInstance;

    public int amount;
    public string itemName;
    public CanvasGroup purchasePanelCG;
    public TextMeshProUGUI itemAmount;
    public TextMeshProUGUI itemImageText;

    private void Awake()
    {
        SharedInstance = this;
    }

    public void LoadDetailsOnPanel()
    {
        AudioManager.Instance.Play("Buy_Successful_Store");
        int imgOrder = 0;
        switch (itemName)
        {
            case StoreItems.PileOfGems:
                imgOrder = 1;
                break;
            case StoreItems.BigPileOfGems:
                imgOrder = 1;
                break;
            case StoreItems.OneHealingPotion:
                imgOrder = 2;
                break;
            case StoreItems.ThreeHealingPotions:
                imgOrder = 2;
                break;
            case StoreItems.Coins:
                imgOrder = 3;
                break;
            case StoreItems.MoreCoins:
                imgOrder = 3;
                break;
        }

        itemAmount.SetText(amount.ToString());
        itemImageText.SetText("<size=150%><sprite index=" + imgOrder + ">");
    }

    public void ContinueButton_Click()
    {
        AudioManager.Instance.Play("Button2");
        LeanTween.alphaCanvas(purchasePanelCG, 0, 0.2f);
        purchasePanelCG.blocksRaycasts = false;
    }
}
