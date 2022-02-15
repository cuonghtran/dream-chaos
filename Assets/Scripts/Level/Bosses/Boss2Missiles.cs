using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2Missiles : MonoBehaviour
{
    public bool isMissileHorizontal;

    private Rigidbody2D _rb;
    private float _speed = 10f;
    int damage = 4;

    void OnEnable()
    {
        _rb = GetComponent<Rigidbody2D>();
        if (isMissileHorizontal)
            _rb.velocity = -1 * transform.right * _speed;
        else _rb.velocity = new Vector2(-1 * transform.right.x, 0.45f) * (_speed-0.9f);

        AudioManager.Instance.Play("Boss2_Missile");
        //AudioManager.Instance.Play("Boss2_Fire");
        StartCoroutine(DestroyBullet());
    }

    IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.CompareTag("Player"))
        {
            TriggerImpactEffect();

            Player.Instance.TakeDamage(damage);
            Destroy(gameObject, 0.05f);
        }

        if (collision.transform.CompareTag("Terrain"))
        {
            TriggerImpactEffect();
            this.gameObject.SetActive(false);
        }
    }

    void TriggerImpactEffect()
    {
        AudioManager.Instance.Play("Bomb_Explosion");
        GameObject ie = ObjectPooler.SharedInstance.GetBombExplosionPooledObject();
        if (ie != null)
        {
            ie.transform.position = transform.position;
            ie.transform.rotation = transform.rotation;
            ie.SetActive(true);
        }
    }
}
