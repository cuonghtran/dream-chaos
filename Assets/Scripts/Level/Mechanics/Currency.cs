using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Currency : MonoBehaviour
{
    int value = 50;
    bool collected;

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Player")
        {
            if (!collected)
            {
                AudioManager.Instance.Play("Collect_Coin");
                collected = true;
                LevelManager.SharedInstance.CollectCurrency(value);
                Destroy(this.gameObject, 0.1f);
            }
        }
    }
}
