using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BossRoomCollider : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BossBase _boss;
    public GameObject roomLeftBoundery;
    public GameObject roomRightBoundery;

    [Header("Cameras")]
    public CinemachineVirtualCamera playerCamera;
    public CinemachineVirtualCamera bossRoomCamera;

    private bool _isTriggered;

    private void Start()
    {
        AdjustRoomBounderies();
    }

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
            if (!_isTriggered)
            {
                roomLeftBoundery.SetActive(true);
                roomRightBoundery.SetActive(true);

                StartCoroutine(ScenePrep());
                bossRoomCamera.Priority = 2;
                _boss.InitBattle();
                _isTriggered = true;
            }
    }

    void AdjustRoomBounderies()
    {
        float offsetLeft = 2.3f;
        float offsetRight = 2.2f;

        if (LevelManager.Is16x9ScreenRatio)
        {
            transform.position = new Vector3(transform.position.x + offsetLeft, transform.position.y, transform.position.z);

            if (roomLeftBoundery != null)
            {
                roomLeftBoundery.transform.position = new Vector3(roomLeftBoundery.transform.position.x + offsetLeft, roomLeftBoundery.transform.position.y, roomLeftBoundery.transform.position.z);
            }

            if (roomRightBoundery != null)
            {
                roomRightBoundery.transform.position = new Vector3(roomRightBoundery.transform.position.x - offsetRight, roomRightBoundery.transform.position.y, roomRightBoundery.transform.position.z);
            }
        }
    }

    IEnumerator ScenePrep()
    {
        Player.Instance.AllowMovement(false);
        yield return new WaitForSeconds(2.75f);
        Player.Instance.AllowMovement(true);
    }
}
