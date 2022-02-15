using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class CommonManager : MonoBehaviour
{
    public static CommonManager SharedInstance;

    public bool Is16x9ScreenRatio;

    [Header("Data:")]
    [SerializeField] private BundleObject bundleObject;
    [SerializeField] private Attributes _characterAttributes;
    [SerializeField] public LevelResults _levelResults;

    [Header("Main Hub Menu")]
    public bool GamePause = false;
    public GameObject mainHubPanel;
    public RectTransform pagesPanelRect;
    public RectTransform tabsPanelRect;
    public RectTransform closeButtonRect;
    public GameObject shadowDrop;
    public TabGroup tabGroup;

    [Header("GUI Components")]
    public TextMeshProUGUI gUICurrencyBar;
    public TextMeshProUGUI gUIGemBar;
    public TextMeshProUGUI gUIPotions;
    public GameObject notifPanel;

    [Header("Level Status Sprites")]
    public Sprite inactiveLevelSprite;
    public Sprite activeLevelSprite;
    public Sprite newLevelSprite;

    public event Action OnCloseMainHub;

    private int playingProgress;

    private void Awake()
    {
        SharedInstance = this;

        if ((Screen.width / Screen.height) == 16 / 9)
            Is16x9ScreenRatio = true;

        StartCoroutine(PlayThemeSong());

        SaveLoadSystem.LoadPlayerData(bundleObject);
    }

    IEnumerator PlayThemeSong()
    {
        yield return new WaitForSeconds(1.2f);
        AudioManager.Instance.PlayTheme("Theme_Song");
    }

    // Start is called before the first frame update
    void Start()
    {
        OnCloseMainHub += CloseMainPanel;
        AdjustPagePanelHeightByScreenSize();
        CheckIfUnlockNewItems();
        LoadAttributesData();
    }

    private void OnDestroy()
    {
        OnCloseMainHub -= CloseMainPanel;
    }

    void AdjustPagePanelHeightByScreenSize()
    {
        if (Is16x9ScreenRatio)
        {
            pagesPanelRect.sizeDelta = new Vector2(pagesPanelRect.sizeDelta.x, pagesPanelRect.sizeDelta.y - 220);
            tabsPanelRect.anchoredPosition = new Vector2(tabsPanelRect.anchoredPosition.x, -4);
            closeButtonRect.anchoredPosition = new Vector2(closeButtonRect.anchoredPosition.x, -192);
        }
    }

    void CheckIfUnlockNewItems()
    {
        playingProgress = _levelResults.progress;

        // Items unlock levels
        if (_characterAttributes.unlockNewItem && ScenesList.ItemsUnlockLevels.Contains(playingProgress))
        {
            StartCoroutine(PlayNotifSound());
            _characterAttributes.unlockNewItem = false;
            notifPanel.SetActive(true);
        }
        else notifPanel.SetActive(false);
    }

    IEnumerator PlayNotifSound()
    {
        yield return new WaitForSeconds(1);
        AudioManager.Instance.Play("New_Item_Notif");
    }

    void LoadAttributesData()
    {
        gUICurrencyBar.SetText(_characterAttributes.totalCurrency.ToString());
        gUIGemBar.SetText(_characterAttributes.totalGem.ToString());
        gUIPotions.SetText(_characterAttributes.healingPotions.ToString());
    }

    // Main Hub Menu

    public void OpenMainHubPanel(int tabIndex)
    {
        AudioManager.Instance.Play("Button2");
        GamePause = true;
        shadowDrop.SetActive(true);
        mainHubPanel.SetActive(true);
        notifPanel.SetActive(false);
        _characterAttributes.unlockNewItem = false; // turn off the notif panel

        StartCoroutine(OpenPanel(tabIndex));
        ToggleGUIBars(false);
        LeanTween.moveY(mainHubPanel, Screen.height / 2, 0.25f).setDelay(0.02f);
    }

    IEnumerator OpenPanel(int tabIndex)
    {
        TabButton selectedTab = tabGroup.GetTabButtonByIndex(tabIndex);
        yield return new WaitForSeconds(0.05f);
        if (selectedTab != null)
            tabGroup.OnTabSelected(selectedTab);
    }

    public void CloseMainHubPanel()
    {
        AudioManager.Instance.Play("Button1");
        GamePause = false;
        shadowDrop.SetActive(false);
        ToggleGUIBars(true);
        LeanTween.moveY(mainHubPanel, Screen.height * 1.5f, 0.25f);
        StartCoroutine(HideMainHubCanvas());
        OnCloseMainHub.Invoke();
    }

    void ToggleGUIBars(bool active)
    {
        gUICurrencyBar.transform.parent.gameObject.SetActive(active);
        gUIGemBar.transform.parent.gameObject.SetActive(active);
        gUIPotions.transform.parent.gameObject.SetActive(active);
    }

    IEnumerator HideMainHubCanvas()
    {
        yield return new WaitForSeconds(0.4f);
        mainHubPanel.SetActive(false);
    }

    void CloseMainPanel()
    {
        LoadAttributesData();
        SaveLoadSystem.SavePlayerData(bundleObject);
    }
}
