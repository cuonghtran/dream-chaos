using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Player.Instance.AllowMovement(false);
            UIManager.SharedInstance.ShowFinishMenu();
        }
    }
}
