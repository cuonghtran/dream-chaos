using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public int progress;
    public string currentLevelNode;
    public List<string> CompletedLevels;
    public List<string> LevelsResults;

    public LevelData(LevelResults lr)
    {
        progress = lr.progress;
        currentLevelNode = lr.currentLevelNode;
        CompletedLevels = lr.CompletedLevels;
        LevelsResults = lr.LevelsResults;
    }
}
