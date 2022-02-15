using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponSlot : MonoBehaviour, IPointerDownHandler
{
    [Header("References")]
    public GameObject equipButton;
    public GameObject unequipButton;

    [Header("Information")]
    public bool isWeapon;
    public bool isSelected;
    public bool isEquipped;
    public string itemName;
    public Transform itemSlots;
    public static WeaponSlot selectedSlot;
    public static bool weaponSelected;

    [Header("Slot sprites")]
    public Sprite backSprite;
    public Sprite selectedBackSprite;
    public Sprite equippedBackSprite;
    public Sprite selectedEquippedBackSprite;

    [Header("Weapon Representation")]
    [SerializeField] private Weapons weaponObject;

    public GameObject item {
        get {
            if (transform.childCount > 0)
                return transform.GetChild(0).gameObject;
            return null;
        }
    }

    private void OnDisable()
    {
        isSelected = false;
    }

    public void UpdateSprite()
    {
        if (isSelected)
            GetComponent<Image>().sprite = isEquipped ? selectedEquippedBackSprite : selectedBackSprite;
        else
            GetComponent<Image>().sprite = isEquipped ? equippedBackSprite : backSprite;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (transform.GetChild(0).gameObject.activeSelf == false) // check if the slot has a weapon in it
            return;

        AudioManager.Instance.Play("Button2");
        if (selectedSlot != null)
            selectedSlot.isSelected = false;

        // check if change item type
        if (isWeapon != weaponSelected && selectedSlot != null)
        {
            selectedSlot.GetComponent<Image>().sprite = selectedSlot.isEquipped ? equippedBackSprite : backSprite;
            selectedSlot.equipButton.SetActive(false);
            selectedSlot.unequipButton.SetActive(false);
        }
        weaponSelected = isWeapon;

        this.isSelected = true;
        selectedSlot = transform.GetComponent<WeaponSlot>();

        UpdateAllItemsSprite();

        if(isWeapon)
            Character.SharedInstance.DisplayToolTipForSelectedWeapon(weaponObject);
        else Character.SharedInstance.DisplayToolTipForSelectedPerk(itemName);
    }

    void UpdateAllItemsSprite()
    {
        int count = 0;
        int itemsCap = isWeapon ? 2 : 3;
        foreach(Transform slot in itemSlots)
        {
            var item = slot.GetComponent<WeaponSlot>();
            if (item.isEquipped) count++;

            if (item.isSelected)
                slot.GetComponent<Image>().sprite = item.isEquipped ? selectedEquippedBackSprite : selectedBackSprite;
            else
                slot.GetComponent<Image>().sprite = item.isEquipped ? equippedBackSprite : backSprite;
        }

        if (isEquipped)
        {
            equipButton.SetActive(false);
            if (count == 1 && isWeapon) unequipButton.SetActive(false); else unequipButton.SetActive(true);
        }  // TODO: Maybe implement the barehand combat and let players unequip both weapons
        else
        {
            if (count < itemsCap) equipButton.SetActive(true); else equipButton.SetActive(false);
            unequipButton.SetActive(false);
        }
    }
}