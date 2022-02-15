using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class IntroManager : MonoBehaviour
{
    public GameObject tapText;
    bool _tapTextStatus = true;
    public GameObject policyCanvas;
    public CanvasGroup policyCanvasGroup;
    private readonly string firstPlay = "FirstPlay";

    

    // Start is called before the first frame update
    void Start()
    {
        CheckPolicyCanvas();
        Invoke("Theme", 0.9f);
        InvokeRepeating("FlickTapText", 1f, 0.6f);
    }

    void Theme()
    {
        AudioManager.Instance.PlayTheme("Theme_Song");
    }

    void Update()
    {
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            if (policyCanvas.activeSelf)
                return;
            if (PlayerPrefs.GetInt("WatchedCinematic") == 0)
                SceneController.Instance.FadeAndLoadScene(ScenesList.CinematicScene);
            else SceneController.Instance.FadeAndLoadScene(ScenesList.WorldScene);
        }

#if UNITY_EDITOR

        if (Input.GetMouseButtonDown(0))
        {
            if (policyCanvas.activeSelf)
                return;
            if (PlayerPrefs.GetInt("WatchedCinematic") == 0)
                SceneController.Instance.FadeAndLoadScene(ScenesList.CinematicScene);
            else SceneController.Instance.FadeAndLoadScene(ScenesList.WorldScene);
        }

#endif
    }

    void FlickTapText()
    {
        _tapTextStatus = !_tapTextStatus;
        tapText.SetActive(_tapTextStatus);
    }

    void CheckPolicyCanvas()
    {
        if (PlayerPrefs.GetInt(firstPlay) != 1)
        {
            policyCanvas.SetActive(true);
        }
        else return;
    }

    public void AcceptButton_Click()
    {
        AudioManager.Instance.Play("Button2");
        PlayerPrefs.SetInt(firstPlay, 1);
        policyCanvas.SetActive(false);
    }

    public void HyperLinksButton_Click(string buttonName)
    {
        Application.OpenURL(ScenesList.HyperLinks[buttonName]);
    }
}
