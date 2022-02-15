using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectHandler : MonoBehaviour
{
    private readonly string LEVEL_NODE_TAG = "LevelNode";
    private readonly string PLAYER_TAG = "Player";

    private void Update()
    {
        if (CommonManager.SharedInstance.GamePause)
            return;

        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Ended)
        {
            Vector3 touchPosWorld = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            RaycastHit2D hit = Physics2D.Raycast(touchPosWorld, Vector2.zero); // or Vector2.zero
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag(PLAYER_TAG))
                {
                    if (World_Player.Instance.currentNode != null && !World_Player.Instance.currentNode.isMiddleNode)
                        if (World_Player.Instance.movementSpeed <= 0.01f)
                            PlayPanelHandler.SharedInstance.OpenPlayPanel();
                }
            }
        }

#if UNITY_EDITOR

        if (Input.GetMouseButtonUp(0))
        {
            Vector3 touchPosWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(touchPosWorld, Vector2.zero); // or Vector2.zero
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag(PLAYER_TAG))
                {
                    if (World_Player.Instance.currentNode != null && !World_Player.Instance.currentNode.isMiddleNode)
                    {
                        if (World_Player.Instance.movementSpeed <= 0.01f)
                            PlayPanelHandler.SharedInstance.OpenPlayPanel();
                    }
                        
                }
            }
        }

#endif
    }
}
