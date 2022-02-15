using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

public class Character : MonoBehaviour
{
    public static Character SharedInstance;

    [Header("Data:")]
    [SerializeField] private Attributes _characterAttributes;

    [Header("References")]
    public GameObject equipWeaponButton;
    public GameObject unequipWeaponButton;
    public GameObject equipPerkButton;
    public GameObject unequipPerkButton;

    [Header("Active weapon & perk slots:")]
    [SerializeField] private Transform _wpSlots;
    [SerializeField] private Transform _perkSlots;

    [Header("Weapon References:")]
    [SerializeField] private Weapons[] _weaponsData;

    [Header("UI Elements")]
    public TextMeshProUGUI toolTipText;
    private List<string> AdviceText = new List<string>()
    {
        "New weapons and perks are unlocked as progresing through the game.",
        "You can equip up to 2 weapons and 3 perks.",
        "Each weapon has a unique ability."
    };
    private int textOrder = 0;

    [Header("Slot sprites")]
    public Sprite backSprite;
    public Sprite equippedBackSprite;

    int playingProgress;

    private void Awake()
    {
        SharedInstance = this;
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void OnEnable()
    {
        AvailableWeapons();
        AvailablePerks();
        LoadWeaponsFromAttrToCharacter();
        LoadPerksFromAttrToCharacter();

        equipWeaponButton.SetActive(false);
        unequipWeaponButton.SetActive(false);
        equipPerkButton.SetActive(false);
        unequipPerkButton.SetActive(false);

        toolTipText.SetText(SetDefaultText());
    }

    string SetDefaultText()
    {
        var text = AdviceText[textOrder];
        textOrder = textOrder == 2 ? 0 : textOrder + 1;
        return text;
    }

    public void ChangeWeaponsButton_Click(bool isEquipping)
    {
        foreach (Transform slot in _wpSlots)
        {
            var item = slot.GetComponent<WeaponSlot>();
            if (item.isSelected)
            {
                item.isEquipped = isEquipping;
                item.UpdateSprite();
            }

        }

        if (isEquipping)
        {
            AudioManager.Instance.Play("Confirm_Sound");
            equipWeaponButton.SetActive(false);
            unequipWeaponButton.SetActive(true);
        }
        else
        {
            AudioManager.Instance.Play("Button1");
            equipWeaponButton.SetActive(true);
            unequipWeaponButton.SetActive(false);
        }

        StoreWeapons();
    }

    public void ChangePerksButton_Click(bool isEquipping)
    {
        foreach (Transform slot in _perkSlots)
        {
            var item = slot.GetComponent<WeaponSlot>();
            if (item.isSelected)
            {
                item.isEquipped = isEquipping;
                item.UpdateSprite();
            }
        }

        if (isEquipping)
        {
            AudioManager.Instance.Play("Confirm_Sound");
            equipPerkButton.SetActive(false);
            unequipPerkButton.SetActive(true);
        }
        else
        {
            AudioManager.Instance.Play("Button1");
            equipPerkButton.SetActive(true);
            unequipPerkButton.SetActive(false);
        }

        StorePerks();
    }

    public void LoadWeaponsFromAttrToCharacter()
    {
        if (_characterAttributes.activeWeapon1 != null || _characterAttributes.activeWeapon2 != null)
        {
            foreach (Transform slot in _wpSlots)
            {
                slot.GetComponent<Image>().sprite = backSprite;
                var item = slot.GetComponent<WeaponSlot>();
                if (_characterAttributes.activeWeapon1)
                {
                    if (_characterAttributes.activeWeapon1.weaponName.Contains(item.itemName))
                    {
                        item.isEquipped = true;
                        slot.gameObject.SetActive(true);
                        slot.GetComponent<Image>().sprite = equippedBackSprite;
                    }
                }

                if (_characterAttributes.activeWeapon2)
                {
                    if (_characterAttributes.activeWeapon2.weaponName.Contains(item.itemName))
                    {
                        item.isEquipped = true;
                        slot.gameObject.SetActive(true);
                        slot.GetComponent<Image>().sprite = equippedBackSprite;
                    }
                }
            }
        }
    }

    public void LoadPerksFromAttrToCharacter()
    {
        foreach (Transform slot in _perkSlots)
        {
            slot.GetComponent<Image>().sprite = backSprite;
            var item = slot.GetComponent<WeaponSlot>();
            foreach (string perk in _characterAttributes.Perks)
            {
                if (item.itemName == perk)
                {
                    item.isEquipped = true;
                    slot.gameObject.SetActive(true);
                    slot.GetComponent<Image>().sprite = equippedBackSprite;
                }
            }
        }
    }

    public void StoreWeapons()
    {
        int wpOrder = 0;
        foreach (Transform slot in _wpSlots)
        {
            var item = slot.GetComponent<WeaponSlot>();
            if (item.isEquipped)
            {
                wpOrder++;
                foreach (Weapons wp in _weaponsData)
                {
                    if (wp.weaponName.Contains(item.itemName))
                    {
                        if (wpOrder == 1)
                            _characterAttributes.activeWeapon1 = wp;
                        if (wpOrder == 2)
                            _characterAttributes.activeWeapon2 = wp;
                    }

                }
            }
        }

        if (wpOrder == 0)
        {
            _characterAttributes.activeWeapon1 = null;
            _characterAttributes.activeWeapon2 = null;
        }
        if (wpOrder == 1)
            _characterAttributes.activeWeapon2 = null;

    }

    public void StorePerks()
    {
        string[] perks = new string[3] { "", "", "" };
        int pOrder = 0;
        foreach (Transform slot in _perkSlots)
        {
            var item = slot.GetComponent<WeaponSlot>();
            if (item.isEquipped)
            {
                perks[pOrder] = item.itemName;
                pOrder++;
            }
        }

        _characterAttributes.Perks = perks;
    }

    public void DisplayToolTipForSelectedWeapon(Weapons wp)
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(wp.GetTooltipText());
        toolTipText.SetText(builder.ToString());
    }

    public void DisplayToolTipForSelectedPerk(string perkName)
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(Perks.GetTooltipText(perkName));
        toolTipText.SetText(builder.ToString());
    }

    void AvailableWeapons()
    {
        playingProgress = CommonManager.SharedInstance._levelResults.progress;

        int i = 0;
        foreach (Transform slot in _wpSlots)
        {
            if (i == 1 && playingProgress < 1)
                slot.GetChild(0).gameObject.SetActive(false);
            if (i == 2 && playingProgress < 3)
                slot.GetChild(0).gameObject.SetActive(false);
            if (i == 3 && playingProgress < 5)
                slot.GetChild(0).gameObject.SetActive(false);
            i++;
        }
    }

    void AvailablePerks()
    {
        int i = 0;
        foreach (Transform slot in _perkSlots)
        {
            if (i == 0 && playingProgress < 1)
                slot.GetChild(0).gameObject.SetActive(false);
            if (i == 1 && playingProgress < 2)
                slot.GetChild(0).gameObject.SetActive(false);
            if (i == 2 && playingProgress < 4)
                slot.GetChild(0).gameObject.SetActive(false);
            if (i == 3 && playingProgress < 6)
                slot.GetChild(0).gameObject.SetActive(false);
            if (i == 4 && playingProgress < 8)
                slot.GetChild(0).gameObject.SetActive(false);
            if (i == 5 && playingProgress < 10)
                slot.GetChild(0).gameObject.SetActive(false);
            if (i == 6 && playingProgress < 12)
                slot.GetChild(0).gameObject.SetActive(false);
            if (i == 7 && playingProgress < 14)
                slot.GetChild(0).gameObject.SetActive(false);
            i++;
        }
    }
}