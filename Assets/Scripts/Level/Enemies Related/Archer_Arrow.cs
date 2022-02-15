using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer_Arrow : MonoBehaviour
{
    private Rigidbody2D _rb;
    private float damage = 2f;
    private float _speed = 12f;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.velocity = transform.right * _speed;
        damage = Random.Range(1, 100) < 41 ? damage : damage + 1;

        AudioManager.Instance.Play("Archer_Arrow");
        StartCoroutine(DestroyBullet());
    }

    IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(1.25f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.CompareTag("Player"))
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
