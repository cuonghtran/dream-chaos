using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Player _player;
    [SerializeField] private Weapons _miniBombWeapon;
    [SerializeField] private float throwHeight = 5f;
    private float _speed = 14.5f;
    private float _bombTiming = 1.3f;
    private bool _isFacingRight;

    // Start is called before the first frame update
    void OnEnable()
    {
        _rb = GetComponent<Rigidbody2D>();
        _player = Player.Instance;

        if(_player != null)
            _isFacingRight = _player.facingRight;

        var playerVelocity = Player.Instance.GetComponent<Rigidbody2D>().velocity;

        // flip the bomb if facing left
        if (_isFacingRight)
        {
            _rb.velocity = new Vector2(_speed, throwHeight) + (playerVelocity * 0.6f);
        } 
        else
        {
            transform.Rotate(new Vector2(0, 180));
            _rb.velocity = new Vector2(_speed * -1, throwHeight) + (playerVelocity * 0.6f);
        }
        
        StartCoroutine(BombExplodeRoutine());
    }

    IEnumerator BombExplodeRoutine()
    {
        yield return new WaitForSeconds(_bombTiming);
        FireExplosionFromPool();
        AudioManager.Instance.Play("Bomb_Explosion");
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Sent when an incoming collider makes contact with this object's
    /// collider (2D physics only).
    /// </summary>
    /// <param name="other">The Collision2D data associated with this collision.</param>
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Enemies"))
        {
            FireExplosionFromPool();
            AudioManager.Instance.Play("Bomb_Explosion");
            gameObject.SetActive(false);
        }
    }

    void FireExplosionFromPool()
    {
        GameObject explosion = ObjectPooler.SharedInstance.GetBombExplosionPooledObject();
        if (explosion != null)
        {
            explosion.transform.position = transform.position;
            // explosion.transform.rotation = transform.rotation;
            explosion.SetActive(true);
        }
    }
}
