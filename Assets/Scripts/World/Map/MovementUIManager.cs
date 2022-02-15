using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementUIManager : MonoBehaviour
{
    public static MovementUIManager SharedInstance;

    [Header("Directional Button")]
    public GameObject moveUpButton;
    public GameObject moveDownButton;
    public GameObject moveLeftButton;
    public GameObject moveRightButton;

    // Start is called before the first frame update
    void Awake()
    {
        SharedInstance = this;
    }

    public void ToggleMovementButtons(List<string> directions)
    {
        if (directions.Count > 0)
        {
            if (directions.Contains("Up"))
                moveUpButton.SetActive(true);
            else moveUpButton.SetActive(false);

            if (directions.Contains("Down"))
                moveDownButton.SetActive(true);
            else moveDownButton.SetActive(false);

            if (directions.Contains("Left"))
                moveLeftButton.SetActive(true);
            else moveLeftButton.SetActive(false);

            if (directions.Contains("Right"))
                moveRightButton.SetActive(true);
            else moveRightButton.SetActive(false);
        }
        else
        {
            moveUpButton.SetActive(false);
            moveDownButton.SetActive(false);
            moveLeftButton.SetActive(false);
            moveRightButton.SetActive(false);
        }
    }
}
