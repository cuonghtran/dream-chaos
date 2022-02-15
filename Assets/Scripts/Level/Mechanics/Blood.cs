using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blood : MonoBehaviour
{
    private int _value = 4;
    private Rigidbody2D _rb;
    bool collected;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        _rb.velocity = new Vector2(1.5f, 3.5f);
        Destroy(this.gameObject, 10f);
    }

    /// <summary>
    /// Sent when an incoming collider makes contact with this object's
    /// collider (2D physics only).
    /// </summary>
    /// <param name="other">The Collision2D data associated with this collision.</param>
    //void OnCollisionEnter2D(Collision2D other)
    //{
    //    if (!collected)
    //    {
    //        if (other.transform.tag == "Player")
    //        {
    //            collected = true;
    //            AudioManager.Instance.Play("Coin3");
    //            Destroy(this.gameObject);
    //            Player.Instance.PickUpHP(_value);
    //        }
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collected)
        {
            if (collision.transform.tag == "Player")
            {
                Debug.Log(collision.name);
                collected = true;
                AudioManager.Instance.Play("Coin3");
                Destroy(this.gameObject);
                Player.Instance.PickUpHP(_value);
            }
        }
    }
}
