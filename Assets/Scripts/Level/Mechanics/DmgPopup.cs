using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DmgPopup : MonoBehaviour
{
    private Vector3 _randomizeIntensity = new Vector3(0.3f, 0.25f, 0);

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        //Destroy(gameObject, _destroyTimer);
        //StartCoroutine(DestroyText());
        transform.localPosition += new Vector3(UnityEngine.Random.Range(-_randomizeIntensity.x, _randomizeIntensity.x),
                                        UnityEngine.Random.Range(-_randomizeIntensity.y, _randomizeIntensity.y),
                                        UnityEngine.Random.Range(-_randomizeIntensity.z, _randomizeIntensity.z));
    }

    public void SetUp(float dmg, string targetTag="")
    {
        if (targetTag == "Player")
            transform.GetChild(0).GetComponent<TextMeshPro>().color = GetColorFromString("FF0300");
        transform.GetChild(0).GetComponent<TextMeshPro>().SetText(Mathf.RoundToInt(dmg).ToString());
    }

    public static Color GetColorFromString(string color) 
    {
        float red = Hex_to_Dec(color.Substring(0,2));
        float green = Hex_to_Dec(color.Substring(2,2));
        float blue = Hex_to_Dec(color.Substring(4,2));
        float alpha = 1f;
        if (color.Length >= 8) {
            // Color string contains alpha
            alpha = Hex_to_Dec(color.Substring(6,2));
        }
        return new Color(red, green, blue, alpha);
    }

    public static float Hex_to_Dec(string hex) 
    {
        return Convert.ToInt32(hex, 16)/255f;
    }
}
