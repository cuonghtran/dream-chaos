using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPanel : MonoBehaviour
{
    private void OnEnable()
    {
        LeanTween.moveY(transform.gameObject, Screen.height * 1.5f, 0);
    }
}
