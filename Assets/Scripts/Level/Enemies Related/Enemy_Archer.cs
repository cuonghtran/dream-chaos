using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Archer : EnemyBase
{
    public Transform startPos;
    public Transform endPos;
    bool movingRight = true;

    public float _speed = 2f;
    bool canFire = true;
    float fireRate = 2f;
    float cooldownCounter;
    public GameObject arrowObject;
    public Transform firePoint;

    // Start is called before the first frame update
    void Start()
    {
        // init stats
        _maxHP = 37f;
        _currentHP = _maxHP;
        _damage = 3f;
        _currencyDrop = 40;
        _playerSearchRange = 12f;
        ScaleEnemiesOnLevelsAndDifficulty();

        sRenderer = GetComponent<SpriteRenderer>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        matDefault = sRenderer.material;

        startPos.position = new Vector2(startPos.position.x, transform.position.y);
        endPos.position = new Vector2(endPos.position.x, transform.position.y);
        _animator.SetBool("Patrol", true);
    }

    private void FixedUpdate()
    {
        if (!activate)
            return;

        if (!PauseUIManager.GamePauseMenu && !knockbacking)
        {
            if (Mathf.Abs(playerDifference) <= _playerSearchRange)
            {
                if (_animator.GetBool("Patrol") == true)
                    _animator.SetBool("Patrol", false);

                if (playerDifference < 0 && movingRight)
                {
                    movingRight = false;
                    transform.eulerAngles = new Vector3(0, -180f, 0);
                }
                else if (playerDifference > 0 && !movingRight)
                {
                    movingRight = true;
                    transform.eulerAngles = new Vector3(0, 0, 0);
                }

                if (canFire)
                {
                    canFire = false;
                    _animator.SetTrigger("Attack");
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
            else
            {
                if (_animator.GetBool("Patrol") == false)
                    _animator.SetBool("Patrol", true);

                if (movingRight)
                    transform.position = Vector3.MoveTowards(transform.position, endPos.position, _speed * Time.deltaTime);
                else transform.position = Vector3.MoveTowards(transform.position, startPos.position, _speed * Time.deltaTime);

                if (transform.position == endPos.position)
                {
                    movingRight = false;
                    transform.eulerAngles = new Vector3(0, -180f, 0);
                }
                else if (transform.position == startPos.position)
                {
                    movingRight = true;
                    transform.eulerAngles = new Vector3(0, 0, 0);
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

    public void FireArrow()
    {
        _animator.ResetTrigger("Attack");
        Instantiate(arrowObject, firePoint.position, transform.rotation);
        cooldownCounter = 0;
    }
}
