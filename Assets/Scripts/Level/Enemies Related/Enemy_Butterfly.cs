using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Butterfly : EnemyBase
{
    public bool movingRight;
    public float _speed = 4f;
    float rotateTimer = 0;
    float rotateTime = 6.5f;

    // Start is called before the first frame update
    void Start()
    {
        // init stats
        _maxHP = 19f;
        _currentHP = _maxHP;
        _damage = 2f;
        _currencyDrop = 40;
        ScaleEnemiesOnLevelsAndDifficulty();

        sRenderer = GetComponent<SpriteRenderer>();
        _rigidBody = GetComponent<Rigidbody2D>();
        matDefault = sRenderer.material;

        if (!movingRight)
            transform.eulerAngles = new Vector3(0, -180f, 0);
    }

    private void FixedUpdate()
    {
        if (!PauseUIManager.GamePauseMenu && !knockbacking)
        {
            if (!activate)
            {
                _rigidBody.velocity = Vector2.zero;
                return;
            }

            _rigidBody.velocity = transform.right * _speed;
            rotateTimer += Time.deltaTime;
            if (rotateTimer >= rotateTime)
                Rotate();
        }
        else if (knockbacking)
        {
            recoveryCounter += Time.deltaTime;
            if (recoveryCounter >= recoveryTime)
            {
                recoveryCounter = 0;
                knockbacking = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Terrain"))
        {
            Rotate();
        }

        if (collision.transform.CompareTag("Player"))
        {
            Player.Instance.TakeDamage(_damage);
        }
    }

    void Rotate()
    {
        rotateTimer = 0;
        movingRight = !movingRight;
        if (movingRight)
            transform.eulerAngles = new Vector3(0, 0, 0);
        else
            transform.eulerAngles = new Vector3(0, -180f, 0);
    }
}
