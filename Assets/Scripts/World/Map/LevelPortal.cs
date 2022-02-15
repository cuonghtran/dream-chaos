using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelPortal : MonoBehaviour
{
    [Header("Level Information")]
    public string levelName;
    public string levelTitle;
    public bool wasPlayed;
    public bool newLevel;
    public string[] starsCollected = new string[3];
    public bool isMiddleNode;

    [Header("Components")]
    [SerializeField] Transform upLevel;
    [SerializeField] Transform downLevel;
    [SerializeField] Transform leftLevel;
    [SerializeField] Transform rightLevel;
    public List<Transform> requiredLevels;
    public List<Transform> previousNodes;

    [Header("Star sprites")]
    public Sprite fullStarSprite;
    public Sprite emptyStarSprite;

    [Header("Level theme sprites")]
    public Sprite hillThemeSprite;
    public Sprite beachThemeSprite;
    public Sprite deserThemeSprite;
    public Sprite forestThemeSprite;

    private Transform imageRepresent;
    private Transform star1;
    private Transform star2;
    private Transform star3;

    bool playerStay;
    float stayTime = 0.5f;
    float stayTimer = 0;

    private void Start()
    {
        ProcessDirection();
    }

    void ProcessDirection()
    {
        List<string> availableDirections = new List<string>();
        if (upLevel != null)
            availableDirections.Add("Up");

        if (downLevel != null)
            availableDirections.Add("Down");

        if (leftLevel != null)
            availableDirections.Add("Left");

        if (rightLevel != null)
            availableDirections.Add("Right");

        if (newLevel)
        {
            if (requiredLevels.Count == 0)
            {
                availableDirections.Remove("Up");
                availableDirections.Remove("Down");
                availableDirections.Remove("Left");
                availableDirections.Remove("Right");
            }

            if (previousNodes.Count > 0 && !previousNodes.Contains(upLevel))
                if (upLevel != null && !upLevel.GetComponent<LevelPortal>().newLevel)
                    availableDirections.Remove("Up");

            if (previousNodes.Count > 0 && !previousNodes.Contains(downLevel))
                if (downLevel != null && !downLevel.GetComponent<LevelPortal>().newLevel)
                    availableDirections.Remove("Down");

            if (previousNodes.Count > 0 && !previousNodes.Contains(leftLevel))
                if (leftLevel != null && !leftLevel.GetComponent<LevelPortal>().newLevel)
                    availableDirections.Remove("Left");

            if (previousNodes.Count > 0 && !previousNodes.Contains(rightLevel))
                if (rightLevel != null && !rightLevel.GetComponent<LevelPortal>().newLevel)
                    availableDirections.Remove("Right");
        }

        MovementUIManager.SharedInstance.ToggleMovementButtons(availableDirections);
    }

    public Vector3 GetNextNodePosition(string direction)
    {
        switch(direction)
        {
            case "Up": return upLevel.position;
            case "Down": return downLevel.position;
            case "Left": return leftLevel.position;
            case "Right": return rightLevel.position;
            default: return Vector2.zero;
        }
    }

    public void DisplayLevelDetails()
    {
        if (!isMiddleNode)
        {
            imageRepresent = transform.GetChild(0);
            star1 = transform.GetChild(1);
            star2 = transform.GetChild(2);
            star3 = transform.GetChild(3);
        }

        if (imageRepresent == null)
            return;

        if (wasPlayed || newLevel)
        {
            // level theme
            imageRepresent.gameObject.SetActive(true);
            switch (ScenesList.LevelTheme[levelName])
            {
                case "Hills":
                    imageRepresent.GetComponent<SpriteRenderer>().sprite = hillThemeSprite;
                    break;
                case "Beach":
                    imageRepresent.GetComponent<SpriteRenderer>().sprite = beachThemeSprite;
                    break;
                case "Desert":
                    imageRepresent.GetComponent<SpriteRenderer>().sprite = deserThemeSprite;
                    break;
                case "Forest":
                    imageRepresent.GetComponent<SpriteRenderer>().sprite = forestThemeSprite;
                    break;
            }

            if (wasPlayed)
            {
                // level stars collected
                star1.gameObject.SetActive(true);
                star2.gameObject.SetActive(true);
                star3.gameObject.SetActive(true);

                if (starsCollected.Length != 0)
                {
                    if (starsCollected[0] == "1")
                        star1.GetComponent<SpriteRenderer>().sprite = fullStarSprite;

                    if (starsCollected[1] == "1")
                        star2.GetComponent<SpriteRenderer>().sprite = fullStarSprite;

                    if (starsCollected[2] == "1")
                        star3.GetComponent<SpriteRenderer>().sprite = fullStarSprite;
                }
            }
        }
    }

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            playerStay = true;
            World_Player.Instance.currentNode = this;
            ProcessDirection();
            if (World_Player.Instance.hasMoved)
                StartCoroutine(AfterReachLevelNode());
        }
    }

    IEnumerator AfterReachLevelNode()
    {
        if(!isMiddleNode)
        {
            World_Player.Instance.hasMoved = false;
            yield return new WaitForSeconds(0.55f);
            World_Player.Instance.TogglePlayBubble(true);
        }
    }
}
