using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TooltipHandler : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI toolTipText;
    private static GameObject _selectedSlot;

    [Header("Slot sprites")]
    public Sprite normalSlotSprite;
    public Sprite selectedSlotSprite;

    // Commented out because we don't use tooltip here anymore

    //public void OnPointerDown(PointerEventData eventData)
    //{
    //    var childObject = gameObject.transform.GetChild(0);
    //    if (childObject)
    //    {
    //        if (gameObject.transform.parent.name == "Weapons_Panel")
    //        {
    //            toolTipText.SetText(WeaponsList.WeaponTooltip[childObject.name]);
    //        }
    //        else if (gameObject.transform.parent.name == "Perks_Panel")
    //        {
    //            toolTipText.SetText(Perks.GetTooltipText(childObject.name));
    //        }
    //    }

    //    if (_selectedSlot != null)
    //        _selectedSlot.GetComponent<Image>().sprite = normalSlotSprite;
        
    //    gameObject.GetComponent<Image>().sprite = selectedSlotSprite;
    //    _selectedSlot = gameObject;
    //}
}
