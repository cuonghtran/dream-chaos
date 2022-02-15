using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayPanelHandler : MonoBehaviour
{
    public static PlayPanelHandler SharedInstance;

    [Header("Level Data")]
    public LevelResults levelResults;

    [Header("Components")]
    public GameObject shadowPanel;
    public GameObject playPanel;
    public TextMeshProUGUI levelTitle;
    private CanvasGroup panelCG;

    [Header("Stars References")]
    public Transform starsPanel;
    public Sprite wholeStarSprite;
    public Sprite emptyStarSprite;
    private string[] starsCollected = new string[3];

    string selectedLevel;

    private void Awake()
    {
        SharedInstance = this;
    }

    private void Start()
    {
        panelCG = shadowPanel.GetComponent<CanvasGroup>();
    }

    void LoadLevelData()
    {
        var lvlIndex = levelResults.CompletedLevels.IndexOf(World_Player.Instance.currentNode.levelName);
        if (lvlIndex < 0)  // have not played this level
        {
            foreach(Transform star in starsPanel)
            {
                star.GetComponent<Image>().sprite = emptyStarSprite;
            }
        }
        else
        {
            var result = levelResults.LevelsResults[lvlIndex];
            if (result != null && result != "")
            {
                starsCollected = result.Split(',');
                int i = 0;
                foreach (Transform star in starsPanel)
                {
                    if (starsCollected[i] == "1")
                        star.GetComponent<Image>().sprite = wholeStarSprite;
                    else star.GetComponent<Image>().sprite = emptyStarSprite;
                    i++;
                }
            }
        }
    }

    public void PlayButton_Click()
    {
        if (selectedLevel != "")
        {
            AudioManager.Instance.Stop("Theme_Song");
            AudioManager.Instance.Play("Warp_Jingle");
            SceneController.Instance.FadeAndLoadScene(selectedLevel);
        }
    }

    public void OpenPlayPanel()
    {
        AudioManager.Instance.Play("Click_Level_Node");
        CommonManager.SharedInstance.GamePause = true;
        LoadLevelData();
        shadowPanel.SetActive(true);
        panelCG.blocksRaycasts = true;
        LeanTween.alphaCanvas(panelCG, 1, 0.2f);

        if (World_Player.Instance.currentNode != null)
        {
            selectedLevel = World_Player.Instance.currentNode.levelName;
            levelResults.currentLevelNode = selectedLevel;
            levelTitle.SetText(selectedLevel);
        }
    }

    public void ClosePlayPanel()
    {
        AudioManager.Instance.Play("Button1");
        panelCG.blocksRaycasts = false;
        Invoke("TurnOffGamePause", 0.3f);
        LeanTween.alphaCanvas(panelCG, 0, 0.2f);
    }

    void TurnOffGamePause()
    {
        CommonManager.SharedInstance.GamePause = false;
    }
}
