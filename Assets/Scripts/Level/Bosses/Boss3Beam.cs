using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3Beam : MonoBehaviour
{
    private float damage = 7f;

    void BeamSound()
    {
        AudioManager.Instance.PlayOneShot("Boss3_Beam");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Player.Instance.TakeDamage(damage);
        }
    }
}
