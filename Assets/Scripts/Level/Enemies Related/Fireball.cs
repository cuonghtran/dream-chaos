using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    private Rigidbody2D _rb;
    private float damage = 3f;
    private float _speed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        damage = Random.Range(1, 100) < 31 ? damage : damage+1;
        //_rb.velocity = new Vector3(0, _speed * -1, 0);

        AudioManager.Instance.Play("Bat_Fireball");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            TriggerImpactEffect();
            Player.Instance.TakeDamage(damage);
            Destroy(gameObject, 0.1f);
        }
        if (collision.transform.CompareTag("Terrain"))
        {
            TriggerImpactEffect();
            Destroy(gameObject, 0.01f);
        }
    }

    void TriggerImpactEffect()
    {
        GameObject ie = ObjectPooler.SharedInstance.GetImpactEffectPooledObject();
        if (ie != null)
        {
            ie.transform.position = transform.position;
            ie.transform.rotation = transform.rotation;
            ie.SetActive(true);
        }
    }
}
