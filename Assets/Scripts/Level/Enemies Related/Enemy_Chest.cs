using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Chest : EnemyBase
{
    public Transform tongueAttack;
    public Transform startPos;
    public Transform endPos;
    public LayerMask attackMask;
    public float _speed = 2.2f;
    float patrolSpeed = 1.5f;

    public float attackRange = 2.7f;
    public float attackCooldown = 1.7f;
    bool movingRight = false;
    float canAttack = 0;
    private Vector2 _targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        // init stats
        _maxHP = 52f;
        _currentHP = _maxHP;
        _damage = 3f;
        _currencyDrop = 35;
        _playerSearchRange = 8f;
        ScaleEnemiesOnLevelsAndDifficulty();

        sRenderer = GetComponent<SpriteRenderer>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        matDefault = sRenderer.material;
    }

    private void FixedUpdate()
    {
        if (!PauseUIManager.GamePauseMenu && !knockbacking)
        {
            if (!activate)
                return;

            if (IsPlayerInRange())  // Player in Range
            {
                if (Mathf.Abs(playerDifference) > attackRange)  // Move to Player
                {
                    if (_animator.GetBool("Patrol") == true)
                        _animator.SetBool("Patrol", false);

                    if (playerDifference < 0 && movingRight)
                    {
                        movingRight = false;
                        transform.eulerAngles = new Vector3(0, 0, 0);
                    }
                    else if (playerDifference > 0 && !movingRight)
                    {
                        movingRight = true;
                        transform.eulerAngles = new Vector3(0, -180f, 0);
                    }

                    transform.position = Vector2.MoveTowards(transform.position, _targetPosition, _speed * Time.deltaTime);
                }
                else  // Attack
                {
                    if (playerDifference < 0 && movingRight)
                    {
                        movingRight = false;
                        transform.eulerAngles = new Vector3(0, 0, 0);
                    }
                    else if (playerDifference > 0 && !movingRight)
                    {
                        movingRight = true;
                        transform.eulerAngles = new Vector3(0, -180f, 0);
                    }

                    Attack();
                }
            }
            else  // Patrol
            {
                if (_animator.GetBool("Patrol") == false)
                    _animator.SetBool("Patrol", true);

                if (movingRight)
                {
                    transform.eulerAngles = new Vector3(0, -180f, 0);
                    transform.position = Vector3.MoveTowards(transform.position, endPos.position, patrolSpeed * Time.deltaTime);
                }
                else
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    transform.position = Vector3.MoveTowards(transform.position, startPos.position, patrolSpeed * Time.deltaTime);
                }

                if (transform.position == endPos.position)
                {
                    movingRight = false;
                    transform.eulerAngles = new Vector3(0, 0, 0);
                }
                else if (transform.position == startPos.position)
                {
                    movingRight = true;
                    transform.eulerAngles = new Vector3(0, -180f, 0);
                }
            }
        }
        else if(knockbacking)
        {
            recoveryCounter += Time.deltaTime;
            if (recoveryCounter >= recoveryTime)
            {
                recoveryCounter = 0;
                knockbacking = false;
            }
        }
    }

    bool IsPlayerInRange()
    {
        if (Mathf.Abs(playerDifference) <= _playerSearchRange)
        {
            _targetPosition = new Vector2(Player.Instance.transform.position.x, transform.position.y);
            return true;
        }
        return false;
    }

    void Attack()
    {
        if (Time.timeSinceLevelLoad >= canAttack)
        {
            canAttack = Time.timeSinceLevelLoad + attackCooldown;
            _animator.SetTrigger("Attack");
            
        }
    }

    public void TongueAttack()
    {
        Collider2D collider = Physics2D.OverlapBox(tongueAttack.position, new Vector3(1.427f, 0.337f), 0, attackMask);
        if (collider != null)
        {
            Player.Instance.TakeDamage(_damage - 1);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(tongueAttack.position, new Vector3(1.427f, 0.337f));
    }
}
