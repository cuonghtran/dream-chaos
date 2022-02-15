using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Results", menuName = "Player Level Results")]
public class LevelResults : ScriptableObject
{
    // public bool firstTimePlay;
    public int progress = 0;
    public string currentLevelNode;
    public List<string> CompletedLevels;
    public List<string> LevelsResults;

    public void LoadFromData(LevelData data)
    {
        currentLevelNode = data.currentLevelNode;
        CompletedLevels = data.CompletedLevels;
        LevelsResults = new List<string>();
        LevelsResults = data.LevelsResults;
    }
}
