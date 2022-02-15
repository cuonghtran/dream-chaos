using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [Header("Data:")]
    public Attributes _characterAttributes;
    public static ObjectPooler SharedInstance;

    [Header("References:")]
    public Transform bulletParent;
    public List<GameObject> bulletPooledObject;
    public List<GameObject> shurikenPooledObject;
    public List<GameObject> impactPooledObject;
    public List<GameObject> bladeImpactPooledObject;
    public List<GameObject> bombPooledObject;
    public List<GameObject> bombExplosionPooledObject;
    public List<GameObject> enemyDeathFXPooledObject;
    public int amountToPool = 10;

    [Header("Prefabs:")]
    public GameObject bulletPrefab;
    public GameObject shurikenPrefab;
    public GameObject impactEffectPrefab;
    public GameObject bladeImpactPrefab;
    public GameObject bombPrefab;
    public GameObject bombExplosionPrefab;
    public GameObject enemyDeathFXPrefab;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        SharedInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // PERK: increase size
        bool poPerk = false;
        float multiplier = 0;
        foreach(string perk in _characterAttributes.Perks)
        {
            if (perk == Perks.PowerOverwhelming)
            {
                poPerk = true;
                multiplier = (1 + (Perks.PerksValue[Perks.PowerOverwhelming] / 100));
            }
        }

        // pooling the plasma bullet
        if (_characterAttributes.activeWeapon1.weaponName == WeaponsList.PlasmaGun || (_characterAttributes.activeWeapon2 != null && _characterAttributes.activeWeapon2.weaponName == WeaponsList.PlasmaGun))
        {
            bulletPooledObject = new List<GameObject>();

            for(int i = 0; i < amountToPool + 3; i++)
            {
                GameObject obj = Instantiate(bulletPrefab);
                if(poPerk)
                {
                    var scaleChange = new Vector3(obj.transform.localScale.x * multiplier, obj.transform.localScale.y * multiplier, obj.transform.localScale.z * multiplier);
                    obj.transform.localScale = scaleChange;
                }
                obj.SetActive(false);
                bulletPooledObject.Add(obj);
                obj.transform.SetParent(bulletParent);
            }
        }
        
        // pooling the shurikens
        if(_characterAttributes.activeWeapon1.weaponName == WeaponsList.Shuriken || (_characterAttributes.activeWeapon2 != null && _characterAttributes.activeWeapon2.weaponName == WeaponsList.Shuriken))
        {
            shurikenPooledObject = new List<GameObject>();

            for(int i = 0; i < amountToPool; i++)
            {
                GameObject obj = Instantiate(shurikenPrefab);
                if(poPerk)
                {
                    var scaleChange = new Vector3(obj.transform.localScale.x * multiplier, obj.transform.localScale.y * multiplier, obj.transform.localScale.z * multiplier);
                    obj.transform.localScale = scaleChange;
                }
                obj.SetActive(false);
                shurikenPooledObject.Add(obj);
                obj.transform.SetParent(bulletParent);
            }
        }

        // pooling the bombs
        if (_characterAttributes.activeWeapon1.weaponName == WeaponsList.MiniBomb || (_characterAttributes.activeWeapon2 != null && _characterAttributes.activeWeapon2.weaponName == WeaponsList.MiniBomb))
        {
            bombPooledObject = new List<GameObject>();

            for (int i = 0; i < amountToPool - 3; i++)
            {
                GameObject obj = Instantiate(bombPrefab);
                if (poPerk)
                {
                    var scaleChange = new Vector3(obj.transform.localScale.x * multiplier, obj.transform.localScale.y * multiplier, obj.transform.localScale.z * multiplier);
                    obj.transform.localScale = scaleChange;
                }
                obj.SetActive(false);
                bombPooledObject.Add(obj);
                obj.transform.SetParent(bulletParent);
            }
        }

        // pooling the bomb explosions
        bombExplosionPooledObject = new List<GameObject>();

        for (int i = 0; i < amountToPool + 2; i++)
        {
            GameObject obj = Instantiate(bombExplosionPrefab);
            obj.SetActive(false);
            bombExplosionPooledObject.Add(obj);
            obj.transform.SetParent(bulletParent);
        }

        // pooling impact effect
        impactPooledObject = new List<GameObject>();
        for (int i=0; i<amountToPool+5; i++)
        {
            GameObject obj = Instantiate(impactEffectPrefab);
            obj.SetActive(false);
            impactPooledObject.Add(obj);
            obj.transform.SetParent(bulletParent);
        }

        // pooling the blade impact effect
        if (_characterAttributes.activeWeapon1.weaponName == WeaponsList.TwinBlades || (_characterAttributes.activeWeapon2 != null && _characterAttributes.activeWeapon2.weaponName == WeaponsList.TwinBlades))
        {
            bladeImpactPooledObject = new List<GameObject>();
            for (int i = 0; i < amountToPool - 4; i++)
            {
                GameObject obj = Instantiate(bladeImpactPrefab);
                obj.SetActive(false);
                bladeImpactPooledObject.Add(obj);
                obj.transform.SetParent(bulletParent);
            }
        }

        // pooling enemy death effect
        enemyDeathFXPooledObject = new List<GameObject>();
        for (int i=0; i<amountToPool; i++)
        {
            GameObject obj = Instantiate(enemyDeathFXPrefab);
            obj.SetActive(false);
            enemyDeathFXPooledObject.Add(obj);
            obj.transform.SetParent(bulletParent);
        }
    }

    public GameObject GetBulletPooledObject()
    {
        for (int i = 0; i < bulletPooledObject.Count; i++)
        {
            if (!bulletPooledObject[i].activeInHierarchy)
                return bulletPooledObject[i];
        }
        return null;
    }

    public GameObject GetShurikenPooledObject()
    {
        for (int i = 0; i < shurikenPooledObject.Count; i++)
        {
            if (!shurikenPooledObject[i].activeInHierarchy)
                return shurikenPooledObject[i];
        }
        return null;
    }

    public GameObject GetImpactEffectPooledObject()
    {
        for (int i = 0; i < impactPooledObject.Count; i++)
        {
            if (!impactPooledObject[i].activeInHierarchy)
                return impactPooledObject[i];
        }
        return null;
    }

    public GameObject GetBladeImpactEffectPooledObject()
    {
        for (int i = 0; i < bladeImpactPooledObject.Count; i++)
        {
            if (!bladeImpactPooledObject[i].activeInHierarchy)
            {
                return bladeImpactPooledObject[i];
            }
        }
        return null;
    }

    public GameObject GetBombPooledObject()
    {
        for (int i = 0; i < bombPooledObject.Count; i++)
        {
            if (!bombPooledObject[i].activeInHierarchy)
                return bombPooledObject[i];
        }
        return null;
    }

    public GameObject GetBombExplosionPooledObject()
    {
        for (int i = 0; i < bombExplosionPooledObject.Count; i++)
        {
            if (!bombExplosionPooledObject[i].activeInHierarchy)
                return bombExplosionPooledObject[i];
        }
        return null;
    }

    public GameObject GetEnemyDeathFXPooledObject()
    {
        for (int i = 0; i < enemyDeathFXPooledObject.Count; i++)
        {
            if (!enemyDeathFXPooledObject[i].activeInHierarchy)
                return enemyDeathFXPooledObject[i];
        }
        return null;
    }
}
