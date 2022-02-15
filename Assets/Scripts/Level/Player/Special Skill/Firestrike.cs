using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firestrike : MonoBehaviour
{
    private Player _player;
    private Rigidbody2D _rb;
    [SerializeField] private Weapons _bladesWeapon;
    private float _speed = 17.5f;
    private int launchDirection = 1;
    private bool _isFacingRight;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _player = Player.Instance;

        if (_player != null)
            _isFacingRight = _player.facingRight;

        // flip the bomb if facing left
        if (_isFacingRight)
        {
            _rb.velocity = new Vector2(_speed, 0);
        }
        else
        {
            // transform.Rotate(new Vector2(0, 180));
            _rb.velocity = new Vector2(_speed * -1, 0);
        }
        AudioManager.Instance.Play("Blade_Firestrike");
        StartCoroutine(DestroyBullet());
    }

    IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(1.55f);
        Destroy(this.gameObject);
    }

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("Enemies"))
        {
            if (transform.position.x < other.transform.position.x)
                launchDirection = 1;
            else
                launchDirection = -1;

            if (other.GetComponent<EnemyBase>() != null)
                other.GetComponent<EnemyBase>().TakeDamage(_player.CalculateSpecialDamage(_bladesWeapon), launchDirection * _bladesWeapon.knockBackPower);
            else if (other.GetComponent<BossBase>() != null)
                other.GetComponent<BossBase>().TakeDamage(_player.CalculateSpecialDamage(_bladesWeapon));
        }

    }
}
