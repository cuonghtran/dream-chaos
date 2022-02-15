using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Frog : EnemyBase
{
    [SerializeField] private Transform _groundCheckingPoint;
    [SerializeField] private LayerMask _terrainLayers;

    [Header("Details")]
    public float jumpDistance = 4f;
    public float jumpTakeOffSpeed = 3f;
    public float jumpCooldown = 1.5f;
    float canJump = 0;

    public float attackRange = 2.7f;
    public float attackCooldown = 1.75f;
    public LayerMask attackMask;
    float canAttack = 0;
    public Transform tongueAttack;

    bool playerInRange;
    private float _rayDistance = 0.1f;
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        // init stats
        _maxHP = 37f;
        _currentHP = _maxHP;
        _currencyDrop = 30;
        _damage = 2f;
        _playerSearchRange = 11f;
        ScaleEnemiesOnLevelsAndDifficulty();

        _rigidBody = GetComponent<Rigidbody2D>();
        sRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        matDefault = sRenderer.material;
    }

    public override void Update()
    {
        base.Update();
        if (!activate)
            return;

        FindTarget();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!PauseUIManager.GamePauseMenu && !knockbacking)
        {
            if (!playerInRange)
                return;

            if (Mathf.Abs(playerDifference) > attackRange)  // Move
            {
                if (playerDifference < 0)
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    RaycastHit2D groundInfo = Physics2D.Raycast(_groundCheckingPoint.position, Vector2.down, _rayDistance, _terrainLayers);
                    if (groundInfo.collider == true)
                        Jump(false);
                }
                else
                {
                    transform.eulerAngles = new Vector3(0, -180f, 0);
                    RaycastHit2D groundInfo = Physics2D.Raycast(_groundCheckingPoint.position, Vector2.down, _rayDistance, _terrainLayers);
                    if (groundInfo.collider == true)
                        Jump(true);
                }
            }
            else  // Attack
            {
                if (playerDifference < 0)
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                }
                else
                {
                    transform.eulerAngles = new Vector3(0, -180f, 0);
                }
                RaycastHit2D groundInfo = Physics2D.Raycast(_groundCheckingPoint.position, Vector2.down, _rayDistance, _terrainLayers);
                if (groundInfo.collider == true)
                    Attack();
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

    void FindTarget()
    {

        if (Vector2.Distance(_rigidBody.position, Player.Instance.transform.position) < _playerSearchRange)
        {
            playerInRange = true;
        }
        else
            playerInRange = false;
    }

    void Jump(bool jumpingRight)
    {
        float x = jumpDistance;
        if (!jumpingRight)
            x *= -1;

        //RaycastHit2D ground = Physics2D.Raycast(_groundCheckingPoint.position, Vector2.down, 0.2f, _terrainLayers);
        //if (ground.collider != null)
        if (Time.timeSinceLevelLoad >= canJump)
        {
            canJump = Time.timeSinceLevelLoad + jumpCooldown;
            _rigidBody.velocity = new Vector2(x, jumpTakeOffSpeed);
            _animator.SetTrigger("Move");
        }
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
        Collider2D collider = Physics2D.OverlapBox(tongueAttack.position, new Vector3(1.542f, 0.317f), 0, attackMask);
        if (collider != null)
        {
            Player.Instance.TakeDamage(_damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(tongueAttack.position, new Vector3(1.542f, 0.317f));
    }
}
