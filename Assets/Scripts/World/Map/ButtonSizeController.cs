using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSizeController : MonoBehaviour
{
    public RectTransform[] set1;
    public RectTransform[] set2;

    private float scaler1 = 1.1f;
    private float scaler2 = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        if (CommonManager.SharedInstance.Is16x9ScreenRatio)
        {
            foreach (RectTransform rt in set1)
            {
                rt.LeanScale(new Vector3(scaler1, scaler1), 0);
            }

            LeanTween.moveX(set1[0], -478, 0);
            LeanTween.moveX(set1[1], -885, 0);

            foreach (RectTransform rt in set2)
            {
                rt.LeanScale(new Vector3(scaler2, scaler2), 0);
            }
            LeanTween.moveY(set2[0], -360, 0);
            LeanTween.moveY(set2[1], -660, 0);
            LeanTween.moveY(set2[2], -970, 0);
        }
    }
}
