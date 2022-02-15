using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Bat : EnemyBase
{
    public bool movingRight;
    public float _speed = 3.1f;
    bool canFire = true;
    float fireRate = 2f;
    float cooldownCounter;
    public GameObject fireballObject;
    public Vector3 offset;
    float rotateTimer = 0;
    float rotateTime = 9;

    // Start is called before the first frame update
    void Start()
    {
        // init stats
        _maxHP = 45f;
        _currentHP = _maxHP;
        _damage = 2f;
        _currencyDrop = 50;
        ScaleEnemiesOnLevelsAndDifficulty();

        sRenderer = GetComponent<SpriteRenderer>();
        _rigidBody = GetComponent<Rigidbody2D>();
        matDefault = sRenderer.material;

        if (!movingRight)
            transform.eulerAngles = new Vector3(0, -180f, 0);
    }

    void LateUpdate()
    {
        if (!activate)
        {
            _rigidBody.velocity = Vector2.zero;
            return;
        }

        if (Mathf.Abs(playerDifference) <= 0.5f)
        {
            if (canFire)
            {
                canFire = false;
                Instantiate(fireballObject, transform.position + offset, Quaternion.identity);
                cooldownCounter = 0;
            }
            else
            {
                cooldownCounter += Time.deltaTime;
                if (cooldownCounter >= fireRate)
                {
                    cooldownCounter = 0;
                    canFire = true;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (!PauseUIManager.GamePauseMenu && !knockbacking)
        {
            if (!activate)
                return;

            _rigidBody.velocity = transform.right * _speed;
            rotateTimer += Time.deltaTime;
            if (rotateTimer >= rotateTime)
                Rotate();

            if (playerDifference >= -0.5f && playerDifference <= 0.5f)
            {
                if (canFire)
                {
                    canFire = false;
                    Instantiate(fireballObject, transform.position + offset, Quaternion.identity);
                    cooldownCounter = 0;
                }
                else
                {
                    cooldownCounter += Time.deltaTime;
                    if (cooldownCounter >= fireRate)
                    {
                        cooldownCounter = 0;
                        canFire = true;
                    }
                }
            }
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
