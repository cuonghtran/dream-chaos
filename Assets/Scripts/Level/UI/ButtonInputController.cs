using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInputController : MonoBehaviour
{
    public RectTransform[] images;
    private float defaultSize = 160;
    private float offset = 27;

    // Start is called before the first frame update
    void Start()
    {
        if (LevelManager.Is16x9ScreenRatio)
        {
            float newSize = defaultSize + offset;
            foreach(RectTransform rt in images)
            {
                rt.sizeDelta = new Vector2(newSize, newSize);
            }
        }
    }
}
