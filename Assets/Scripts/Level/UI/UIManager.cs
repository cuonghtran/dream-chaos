using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager SharedInstance;

    [Header("End Panels")]
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject finishMenu;

    [Header("References:")]
    [SerializeField] private GameObject _heartsContainer;
    [SerializeField] private Sprite[] _heartSprites;
    [SerializeField] private GameObject _currencyObject;
    private TextMeshProUGUI _currencyText;
    [SerializeField] private GameObject _changeWeaponButton;
    [SerializeField] private GameObject _bossPanel;
    public TextMeshProUGUI bossName;
    public CanvasGroup hurtFlashCG;
    public CanvasGroup potionFlashCG;
    private Weapons activeWp1;
    private Weapons activeWp2;
    private float maximumHP;

    [Header("Change weapon sprites")]
    public Sprite[] changeWeaponSprites;
    public Image baseSkillImage1;
    public Image cooldownSkillImage1;
    public Image baseSkillImage2;
    public Image cooldownSkillImage2;
    bool isCooldown1;
    bool isCooldown2;
    bool hasVigor;

    [Header("Special Skill Sprites")]
    public Sprite fireStrikeImage;
    public Sprite laserBeamImage;
    public Sprite dashImage;
    public Sprite tripleBombImage;

    [Header("Healing Potions")]
    public Image potionButton;
    public Image cooldownPotionImage;
    public TextMeshProUGUI potionText;
    float potionCooldownTime = 1f;
    bool isPotionCooldown;

    [Header("Data:")]
    [SerializeField] private Attributes _characterAttributes;

    public Sprite wpSprite1, wpSprite2;

    void Awake()
    {
        SharedInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _currencyText = _currencyObject.GetComponent<TextMeshProUGUI>();

        UpdateHealingPotions();

        // store character info
        maximumHP = _characterAttributes.maximumHealth;
        activeWp1 = _characterAttributes.activeWeapon1;
        activeWp2 = _characterAttributes.activeWeapon2;

        // Change weapon button
        FindWeaponSprite(activeWp1, ref wpSprite1, true);
        if (activeWp2 != null)
            FindWeaponSprite(activeWp2, ref wpSprite2, false);
        Invoke("ShowChangeWeaponButton", 0.1f);
    }

    private void Update()
    {
        SkillCooldownsHandler();
    }

    #region UI Related

    public void PopulateHearts(float maxHP)
    {
        // make the hearts scale by the grid layout
        transform.localScale = Vector3.one;

        maximumHP = Mathf.CeilToInt(maxHP);
        int numbersOfFullHeart = (int)(maximumHP / 4);

        // display full hearts
        if (numbersOfFullHeart > 0)
        {
            for (int i = 0; i < numbersOfFullHeart; i++)
            {
                PickingHeart(4);
            }
        }
    }

    public void UpdateHearts()
    {
        // Delete current hearts
        for (int i = 0; i < _heartsContainer.transform.childCount; i++)
        {
            var h = _heartsContainer.transform.GetChild(i);
            Destroy(h.gameObject);
        }

        // make the hearts scale by the grid layout
        transform.localScale = Vector3.one;

        // IDEA: 1 heart = 4 healths
        float currentHP = Mathf.CeilToInt(Player.Instance.currentHP);
        int numbersOfFullHeart = (int)(currentHP / 4);
        int remainderHeart = (int)(currentHP % 4);
        int losingHeart = 0;
        if (Player.Instance.currentHP <= 0)
            losingHeart = (int)(maximumHP / 4);
        else losingHeart = (int)((maximumHP - currentHP) / 4);

        // display full hearts
        if (numbersOfFullHeart > 0)
        {
            for (int i = 0; i < numbersOfFullHeart; i++)
            {
                PickingHeart(4);
            }
        }

        // display partial hearts
        if (remainderHeart > 0)
        {
            PickingHeart(remainderHeart);
        }

        // display empty hearts
        if (losingHeart > 0)
        {
            for (int i = 0; i < losingHeart; i++)
            {
                PickingHeart(0);
            }
        }
    }

    void PickingHeart(int num)
    {
        GameObject newHeart = new GameObject();
        Image img = newHeart.AddComponent<Image>();

        img.sprite = _heartSprites[num];
        newHeart.GetComponent<RectTransform>().SetParent(_heartsContainer.transform);
        newHeart.name = "Heart";
        newHeart.SetActive(true);
    }

    void PlayAdsAfterDie()
    {
        if (AdsManager.SharedInstance != null)
            if (UnityEngine.Random.Range(0, 100) <= 99)
                AdsManager.SharedInstance.PlayVideoAd();
    }

    public void ShowGameOverMenu()
    {
        PauseUIManager.GamePauseMenu = true;
        _gameOverPanel.SetActive(true);
        gameOverMenu.SetActive(true);
        PlayAdsAfterDie();
    }

    public void ShowFinishMenu()
    {
        AudioManager.Instance.Stop("Map_Theme");
        AudioManager.Instance.Stop("Boss_Main");
        AudioManager.Instance.Play("Win_Jingle");
        PauseUIManager.GamePauseMenu = true;
        _gameOverPanel.SetActive(true);
        finishMenu.SetActive(true);
    }

    public void ExitToWorld(bool isDead)
    {
        AudioManager.Instance.Play("Button2");
        Time.timeScale = 1f;
        PauseUIManager.GamePauseMenu = false;
        if (!isDead)
        {
            LevelManager.SharedInstance.UpdateToLevelResult();
            LevelManager.SharedInstance.UpdateToAttributes();
        }
        LevelManager.SharedInstance.LoadWorldScene();
    }

    public void RestartLevel()
    {
        AudioManager.Instance.Play("Button2");
        Time.timeScale = 1f;
        PauseUIManager.GamePauseMenu = false;
        LevelManager.SharedInstance.RestartLevel();
    }

    public void UpdateCurrency(int currencyAmount)
    {
        _currencyText.SetText(currencyAmount.ToString());
    }

    public void UpdateHealingPotions()
    {
        float defaultAlpha = 0.7f;
        if (LevelManager.SharedInstance.healingPotions > 0)
        {
            potionButton.GetComponent<CanvasGroup>().alpha = defaultAlpha;
            potionButton.GetComponent<CanvasGroup>().blocksRaycasts = true;
            cooldownPotionImage.GetComponent<CanvasGroup>().alpha = defaultAlpha;
            potionButton.GetComponent<CanvasGroup>().blocksRaycasts = true;
            potionText.text = LevelManager.SharedInstance.healingPotions.ToString();
        }
        else
        {
            potionButton.GetComponent<CanvasGroup>().alpha = 0;
            potionButton.GetComponent<CanvasGroup>().blocksRaycasts = false;
            cooldownPotionImage.GetComponent<CanvasGroup>().alpha = 0;
            potionButton.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    public void DisplayBossInformation(string bossName)
    {
        _bossPanel.SetActive(true);
        if (this.bossName != null)
            this.bossName.text = bossName;
    }

    public void ChangeWeapon()
    {
        AudioManager.Instance.Play("Change_Weapon");
        Player.Instance.ChangeWeapon();
        ShowChangeWeaponButton();
    }

    void ShowChangeWeaponButton()
    {
        if (activeWp2 != null)
        {
            if (Player.Instance._currentWeapon == activeWp1)
            {
                _changeWeaponButton.GetComponent<Image>().overrideSprite = wpSprite2;
                ToggleSpecialSkill(baseSkillImage2, cooldownSkillImage2, false);
                ToggleSpecialSkill(baseSkillImage1, cooldownSkillImage1, true);

            }
            else if (Player.Instance._currentWeapon == activeWp2)
            {
                _changeWeaponButton.GetComponent<Image>().overrideSprite = wpSprite1;
                ToggleSpecialSkill(baseSkillImage1, cooldownSkillImage1, false);
                ToggleSpecialSkill(baseSkillImage2, cooldownSkillImage2, true);
            }
            _changeWeaponButton.SetActive(true);
        }
        else _changeWeaponButton.SetActive(false);
    }

    #endregion

    #region Healing Potion Related

    public bool IsPotionOnCooldown()
    {
        return isPotionCooldown;
    }

    public void TriggerPotionCooldown()
    {
        isPotionCooldown = true;
        cooldownPotionImage.fillAmount = 1;
    }

    #endregion

    #region Special Skills Related

    public bool IsWeaponSkill1OnCooldown()
    {
        return isCooldown1;
    }
    public bool IsWeaponSkill2OnCooldown()
    {
        return isCooldown2;
    }

    public void ProceedCooldown(bool isWp1, bool hasVigorPerk)
    {
        hasVigor = hasVigorPerk;

        if (isWp1)
        {
            isCooldown1 = true;
            cooldownSkillImage1.fillAmount = 1;
        }
        else
        {
            isCooldown2 = true;
            cooldownSkillImage2.fillAmount = 1;
        }
    }

    void SkillCooldownsHandler()
    {
        if (isCooldown1)
        {
            float cd = hasVigor ? activeWp1.skillCooldown * (100 - Perks.PerksValue[Perks.Vigor]) / 100 : activeWp1.skillCooldown;

            cooldownSkillImage1.fillAmount -= 1 / cd * Time.deltaTime;
            if (cooldownSkillImage1.fillAmount <= 0)
            {
                cooldownSkillImage1.fillAmount = 0;
                isCooldown1 = false;
            }
        }

        if (isCooldown2)
        {
            float cd = hasVigor ? activeWp2.skillCooldown * (100 - Perks.PerksValue[Perks.Vigor]) / 100 : activeWp2.skillCooldown;

            cooldownSkillImage2.fillAmount -= 1 / cd * Time.deltaTime;
            if (cooldownSkillImage2.fillAmount <= 0)
            {
                cooldownSkillImage2.fillAmount = 0;
                isCooldown2 = false;
            }
        }

        if (isPotionCooldown)
        {
            cooldownPotionImage.fillAmount -= 1 / potionCooldownTime * Time.deltaTime;
            if (cooldownPotionImage.fillAmount <= 0)
            {
                cooldownPotionImage.fillAmount = 0;
                isPotionCooldown = false;
            }
        }
    }

    void ToggleSpecialSkill(Image baseImage, Image cooldownImage, bool toggle)
    {
        float defaultAlpha = 0.75f;
        if (toggle)
        {
            baseImage.GetComponent<CanvasGroup>().alpha = defaultAlpha;
            cooldownImage.GetComponent<CanvasGroup>().alpha = defaultAlpha;
            cooldownImage.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
        else
        {
            baseImage.GetComponent<CanvasGroup>().alpha = 0;
            cooldownImage.GetComponent<CanvasGroup>().alpha = 0;
            cooldownImage.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    void FindWeaponSprite(Weapons wp, ref Sprite sp, bool isWeapon1)
    {
        switch(wp.weaponName)
            {
                case WeaponsList.TwinBlades:
                    sp = changeWeaponSprites[0];
                    ChangeSpecialSkillImages(isWeapon1, fireStrikeImage);
                    break;
                case WeaponsList.PlasmaGun:
                    sp = changeWeaponSprites[1];
                    ChangeSpecialSkillImages(isWeapon1, laserBeamImage);
                    break;
                case WeaponsList.Shuriken:
                    sp = changeWeaponSprites[2];
                    ChangeSpecialSkillImages(isWeapon1, dashImage);
                    break;
                case WeaponsList.MiniBomb:
                    sp = changeWeaponSprites[3];
                    ChangeSpecialSkillImages(isWeapon1, tripleBombImage);
                    break;
            }
    }

    void ChangeSpecialSkillImages(bool isWeapon1, Sprite img)
    {
        if (isWeapon1)
        {
            baseSkillImage1.sprite = img;
            cooldownSkillImage1.sprite = img;
        }
        else
        {
            baseSkillImage2.sprite = img;
            cooldownSkillImage2.sprite = img;
        }
    }

    #endregion

    #region Miscellaneous

    public IEnumerator TakeDamageFlash()
    {
        LeanTween.alphaCanvas(hurtFlashCG, 1, 0.08f);
        yield return new WaitForSeconds(0.1f);
        LeanTween.alphaCanvas(hurtFlashCG, 0, 0.05f);
    }

    public IEnumerator DrinkPotionsFlash()
    {
        LeanTween.alphaCanvas(potionFlashCG, 1, 0.07f);
        yield return new WaitForSeconds(0.07f);
        LeanTween.alphaCanvas(potionFlashCG, 0, 0.05f);
    }

    #endregion
}
