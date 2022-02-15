using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentsMenu : MonoBehaviour
{
    [Header("Data:")]
    [SerializeField] private Attributes _characterAttributes;

    [Header("Weapon & Perk slots")]
    [SerializeField] private Transform _wpSlots;
    [SerializeField] private Transform _perkSlots;

    [Header("Prefabs References")]
    public Transform[] weaponsPrefabs;
    public Transform[] perksPrefabs;

    float imageScale = 0.75f;

    void Start()
    {
        SetEquipmentsFromAttributes();
    }

    void SetEquipmentsFromAttributes()
    {
        AudioManager.Instance.Play("Button2");
        LoadWeapons();
        LoadPerks();
    }

    void LoadWeapons()
    {
        foreach(Transform slot in _wpSlots)
        {
            if (slot.transform.name == "ActiveWeapon1")
            {
                if (_characterAttributes.activeWeapon1 != null)
                {
                    int num = 0;
                    if (_characterAttributes.activeWeapon1.weaponName == WeaponsList.PlasmaGun)
                        num = 1;
                    else if (_characterAttributes.activeWeapon1.weaponName == WeaponsList.Shuriken)
                        num = 2;
                    else if (_characterAttributes.activeWeapon1.weaponName == WeaponsList.MiniBomb)
                        num = 3;
                    Transform newWP = Instantiate(weaponsPrefabs[num], slot.position, slot.rotation);
                    if (LevelManager.Is16x9ScreenRatio)
                        newWP.LeanScale(new Vector3(imageScale, imageScale, imageScale), 0);
                    newWP.SetParent(slot);
                    newWP.name = _characterAttributes.activeWeapon1.weaponName;
                }
            }
            if (slot.transform.name == "ActiveWeapon2")
            {
                if (_characterAttributes.activeWeapon2 != null)
                {
                    int num = 0;
                    if (_characterAttributes.activeWeapon2.weaponName == WeaponsList.PlasmaGun)
                        num = 1;
                    else if (_characterAttributes.activeWeapon2.weaponName == WeaponsList.Shuriken)
                        num = 2;
                    else if (_characterAttributes.activeWeapon2.weaponName == WeaponsList.MiniBomb)
                        num = 3;
                    Transform newWP = Instantiate(weaponsPrefabs[num], slot.position, slot.rotation);
                    if (LevelManager.Is16x9ScreenRatio)
                        newWP.LeanScale(new Vector3(imageScale, imageScale, imageScale), 0);
                    newWP.SetParent(slot);
                    newWP.name = _characterAttributes.activeWeapon2.weaponName;
                }
            }
        }
    }

    void LoadPerks()
    {
        if (_characterAttributes.Perks.Length > 0)
        {
            int slotNumber = 1;
            foreach (Transform slot in _perkSlots)
            {
                if (_characterAttributes.Perks[slotNumber - 1] != "")
                {
                    int num = 0;
                    if (_characterAttributes.Perks[slotNumber - 1] == Perks.HardenedSkin)
                        num = 1;
                    else if (_characterAttributes.Perks[slotNumber - 1] == Perks.DoubleJump)
                        num = 2;
                    else if (_characterAttributes.Perks[slotNumber - 1] == Perks.FeatherLight)
                        num = 3;
                    else if (_characterAttributes.Perks[slotNumber - 1] == Perks.Vigor)
                        num = 4;
                    else if (_characterAttributes.Perks[slotNumber - 1] == Perks.SecondWind)
                        num = 5;
                    else if (_characterAttributes.Perks[slotNumber - 1] == Perks.PowerOverwhelming)
                        num = 6;
                    else if (_characterAttributes.Perks[slotNumber - 1] == Perks.InnerRage)
                        num = 7;
                    Transform newPerk = Instantiate(perksPrefabs[num], slot.position, slot.rotation);
                    if (LevelManager.Is16x9ScreenRatio)
                        newPerk.LeanScale(new Vector3(imageScale, imageScale, imageScale), 0);
                    newPerk.SetParent(slot);
                    newPerk.name = _characterAttributes.Perks[slotNumber - 1];
                }
                slotNumber++;
            }
        }
    }
}
