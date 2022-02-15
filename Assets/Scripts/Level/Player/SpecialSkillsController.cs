using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class SpecialSkillsController : MonoBehaviour
{
    public Image skillImage1;
    public Image skillImage2;
    public Image cooldownImage1;
    public Image cooldownImage2;

    Weapons currentWp;
    Weapons wp1;
    Weapons wp2;

    bool isCooldown1;
    bool isCooldown2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //wp1 = Player.Instance._activeweapon1;
        //wp2 = Player.Instance._activeweapon2;
        //if (CrossPlatformInputManager.GetButton("Fire2") || Input.GetKey(KeyCode.V))
        //{
        //    currentWp = Player.Instance._currentWeapon;
        //    if (currentWp == wp1 && isCooldown1 == false)
        //    {
        //        isCooldown1 = true;
        //        cooldownImage1.fillAmount = 1;

        //        Player.Instance.UseSpecialSkill(wp1);
        //    }

        //    if (currentWp == wp2 && isCooldown2 == false)
        //    {
        //        isCooldown2 = true;
        //        cooldownImage2.fillAmount = 1;

        //        Player.Instance.UseSpecialSkill(wp2);
        //    }
        //}

        //if (isCooldown1)
        //{
        //    cooldownImage1.fillAmount -= 1 / wp1.skillCooldown * Time.deltaTime;
        //    if(cooldownImage1.fillAmount <= 0)
        //    {
        //        cooldownImage1.fillAmount = 0;
        //        isCooldown1 = false;
        //    }
        //}

        //if (isCooldown2)
        //{
        //    cooldownImage2.fillAmount -= 1 / wp2.skillCooldown * Time.deltaTime;
        //    if (cooldownImage2.fillAmount <= 0)
        //    {
        //        cooldownImage2.fillAmount = 0;
        //        isCooldown1 = false;
        //    }
        //}
    }
}
