using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3LightningBolt : MonoBehaviour
{
    private Rigidbody2D _rb;
    private float damage = 6f;
    private float _speed = 11f;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.velocity = transform.right * -1 * _speed;

        var rand = Random.Range(1, 100);
        if (rand < 51) damage -= 2;
        else if (rand < 80) damage -= 1;

        AudioManager.Instance.Play("Boss3_Lightning");
        StartCoroutine(DestroyBullet());
    }

    IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(1.75f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Player.Instance.TakeDamage(damage);
            Destroy(gameObject, 0.05f);
        }
        if (collision.transform.CompareTag("Terrain"))
        {
            Destroy(gameObject);
        }
    }
}
