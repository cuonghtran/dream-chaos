using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuriken_Bullet : MonoBehaviour
{
    private Player _player;
    private Rigidbody2D _rb;
    [SerializeField] private Weapons _shurikenWeapon;
    private float _speed = 17f;
    private int launchDirection = 1;

    // Start is called before the first frame update
    void OnEnable()
    {
        _rb = GetComponent<Rigidbody2D>();
        _player = Player.Instance;

        _rb.velocity = transform.right * _speed;
        
        StartCoroutine(DestroyShuriken());
    }

    IEnumerator DestroyShuriken()
    {
        yield return new WaitForSeconds(1.1f);
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Enemies")
        {
            TriggerImpactEffect();

            if (transform.position.x < other.transform.position.x)
                launchDirection = 1;
            else
                launchDirection = -1;

            if (other.GetComponent<EnemyBase>())
                other.GetComponent<EnemyBase>().TakeDamage(_player.CalculateDamage(_shurikenWeapon), launchDirection * _shurikenWeapon.knockBackPower);
            else if (other.GetComponent<BossBase>())
                other.GetComponent<BossBase>().TakeDamage(_player.CalculateDamage(_shurikenWeapon));
            this.gameObject.SetActive(false);
        }

        if (other.transform.tag == "Terrain")
        {
            TriggerImpactEffect();
            this.gameObject.SetActive(false);
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
