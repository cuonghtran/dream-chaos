using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Upgrades : MonoBehaviour
{
    public static Upgrades SharedInstance;

    [Header("Data:")]
    [SerializeField] private Attributes _characterAttributes;

    [Header("Text References")]
    public TextMeshProUGUI currencyBar;
    public TextMeshProUGUI gemBar;
    public GameObject descriptionText;
    public int currencyAmount;
    private int gemAmount;
    private int playingProgress;

    [Header("Weapons Panels")]
    public Transform weaponsPanel;

    private void Awake()
    {
        SharedInstance = this;
    }

    void OnEnable()
    {
        LoadCurrencyAndGem();
        AvailableWeapon();
    }

    void LoadCurrencyAndGem()
    {
        currencyAmount = _characterAttributes.totalCurrency;
        gemAmount = _characterAttributes.totalGem;

        currencyBar.SetText(currencyAmount.ToString());
        gemBar.SetText(gemAmount.ToString());
    }

    public void UpdateAllButtonsStatus()
    {
        foreach (Transform wpPanel in weaponsPanel)
        {
            var wp = wpPanel.GetComponent<WeaponsPower>();
            if (wp.gameObject.activeSelf)
                wp.ToggleButton(wp.powerLevel);
        }
    }

    void AvailableWeapon()
    {
        playingProgress = CommonManager.SharedInstance._levelResults.progress;
        if (playingProgress < 1)
            weaponsPanel.GetChild(1).gameObject.SetActive(false);

        if (playingProgress < 3)
            weaponsPanel.GetChild(2).gameObject.SetActive(false);

        if (playingProgress < 5)
            weaponsPanel.GetChild(3).gameObject.SetActive(false);
        else descriptionText.SetActive(false);
    }
}
