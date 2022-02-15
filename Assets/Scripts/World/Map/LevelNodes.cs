using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelNodes : MonoBehaviour
{
    [Header("Data:")]
    [SerializeField] private LevelResults _levelResults;

    [Header("Level Status Sprites")]
    public Sprite inactiveLevelSprite;
    public Sprite activeLevelSprite;
    public Sprite newLevelSprite;

    private string currentLevel;
    private List<string> completedLevels;
    private List<string> levelsResults;

    // Start is called before the first frame update
    void Start()
    {
        completedLevels = _levelResults.CompletedLevels;
        levelsResults = _levelResults.LevelsResults;
        currentLevel = _levelResults.currentLevelNode;

        LoadData();
    }

    void LoadData()
    {
        foreach(Transform levelNode in transform)
        {
            LevelPortal level = levelNode.GetComponent<LevelPortal>();
            if (currentLevel != "" && currentLevel == level.levelName)
                World_Player.Instance.transform.position = level.transform.position;
            level.levelTitle = ScenesList.LevelTitles[level.levelName];

            if (completedLevels.Contains(level.levelName))
            {
                level.wasPlayed = true;
                // set stars collected
                var levelOrder = completedLevels.IndexOf(level.levelName);
                var starsResult = levelsResults[levelOrder];
                if (starsResult != null && starsResult != "")
                {
                    level.starsCollected = starsResult.Split(',');
                }

                level.DisplayLevelDetails();
                levelNode.GetComponent<SpriteRenderer>().sprite = activeLevelSprite;
            }
            else
            {
                if (level.requiredLevels.Count == 0)
                {
                    level.newLevel = true;
                    level.DisplayLevelDetails();
                    levelNode.GetComponent<SpriteRenderer>().sprite = newLevelSprite;
                }
                else
                {
                    var prevPlayedLevel = level.requiredLevels.FirstOrDefault(l => l.GetComponent<LevelPortal>().wasPlayed);
                    if (prevPlayedLevel != null)
                    {
                        level.newLevel = true;
                        level.DisplayLevelDetails();
                        levelNode.GetComponent<SpriteRenderer>().sprite = newLevelSprite;
                    }
                    else levelNode.GetComponent<SpriteRenderer>().sprite = inactiveLevelSprite;
                }
            }
        }
    }
}
