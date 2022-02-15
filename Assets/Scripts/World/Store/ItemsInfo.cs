using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemsInfo : MonoBehaviour
{
    public static ItemsInfo SharedInstance;

    [Header("Data:")]
    [SerializeField] private Attributes _characterAttributes;

    [Header("Information")]
    [SerializeField] string itemName;
    int amount;
    float cost;
    public enum PurchaseType { smallGems, bigGems, notGem };
    public PurchaseType purchaseType;

    [Header("References")]
    public CanvasGroup purchasePanelCG;
    public Store store;
    public TextMeshProUGUI amountText;
    public TextMeshProUGUI costText;
    public GameObject buyButton;
    CanvasGroup buyButtonCG;

    private void Awake()
    {
        SharedInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        buyButtonCG = buyButton.GetComponent<CanvasGroup>();
        LoadDetails();
    }


    void LoadDetails()
    {
        amount = StoreItems.Amount[itemName];
        cost = StoreItems.Cost[itemName];

        //amountText.SetText(amount.ToString());
        //costText.SetText(cost.ToString());

        ToggleButton();
    }

    public void BuyButton_Click()
    {
        PurchasePanel.SharedInstance.amount = amount;
        PurchasePanel.SharedInstance.itemName = itemName;
        AudioManager.Instance.Play("Button2");

        if (purchaseType != PurchaseType.notGem)
        {
            if (purchaseType == PurchaseType.smallGems)
            {
                IAPManager.instance.BuySmallAmountGems();
            }
            if (purchaseType == PurchaseType.bigGems)
            {
                IAPManager.instance.BuyBigAmountGems();
            }
        }
        else
        {
            if (cost <= _characterAttributes.totalGem)
            {
                _characterAttributes.totalGem -= (int)cost;
                if (itemName == StoreItems.OneHealingPotion || itemName == StoreItems.ThreeHealingPotions)
                    _characterAttributes.healingPotions += amount;
                else if (itemName == StoreItems.Coins || itemName == StoreItems.MoreCoins)
                    _characterAttributes.totalCurrency += amount;

                PostPurchase();
            }
        }
    }

    public void PurchaseSuccessful(int amount)
    {
        _characterAttributes.totalGem += amount;
        PostPurchase();
    }


    void PostPurchase()
    {
        store.UpdateCurrencyAndGem();
        store.UpdateAllButtonsStatus();

        PurchasePanel.SharedInstance.LoadDetailsOnPanel();
        LeanTween.alphaCanvas(purchasePanelCG, 1, 0.2f);
        purchasePanelCG.blocksRaycasts = true;
        ToggleButton();
    }

    public void ToggleButton()
    {
        if (purchaseType == PurchaseType.notGem)
        {
            if (_characterAttributes.totalGem >= cost)
                buyButtonCG.interactable = true;
            else
                buyButtonCG.interactable = false;
        }
    }
}
