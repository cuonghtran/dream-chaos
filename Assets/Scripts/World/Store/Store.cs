using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Store : MonoBehaviour
{
    [Header("Data:")]
    [SerializeField] private Attributes _characterAttributes;
    public Transform contentPanel;

    [Header("Text References")]
    public TextMeshProUGUI currencyBar;
    public TextMeshProUGUI gemBar;

    // Start is called before the first frame update
    void OnEnable()
    {
        UpdateCurrencyAndGem();
        //UpdateAllButtonsStatus();
    }

    public void UpdateCurrencyAndGem()
    {
        currencyBar.SetText(_characterAttributes.totalCurrency.ToString());
        gemBar.SetText(_characterAttributes.totalGem.ToString());
    }

    public void UpdateAllButtonsStatus()
    {
        // update upgrades buttons
        // Upgrades.SharedInstance.UpdateAllButtonsStatus();

        foreach (Transform slot in contentPanel)
        {
            var item = slot.GetComponent<ItemsInfo>();
            item.ToggleButton();
        }
    }
}
