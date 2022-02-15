using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Red_Slime : EnemyBase
{
    private Vector2 _targetPosition;
    public float _speed = 3f;
    private float _rayDistance = 0.7f;
    [SerializeField] private Transform _groundCheckingPoint;
    [SerializeField] private LayerMask _terrainLayers;
    private float jumpTakeOffSpeed = 8.8f;
    public float jumpCooldown = 0.4f;
    float canJump = 0;

    // Start is called before the first frame update
    void Start()
    {
        // init stats
        _maxHP = 26f;
        _currentHP = _maxHP;
        _damage = 2f;
        _currencyDrop = 30;
        _playerSearchRange = 14;
        ScaleEnemiesOnLevelsAndDifficulty();

        _rigidBody = GetComponent<Rigidbody2D>();
        sRenderer = GetComponent<SpriteRenderer>();
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
            if (_targetPosition == Vector2.zero)
                return;

            LookAtPlayer();
            transform.position = Vector2.MoveTowards(transform.position, _targetPosition, _speed * Time.deltaTime);

            // to make it more organic
            //Vector2 direction = (_targetPosition - _rigidBody.position).normalized;
            //Vector2 force = direction * (_speed * 100) * Time.deltaTime;
            //_rigidBody.AddForce(force);

            RaycastHit2D groundInfo = Physics2D.Raycast(_groundCheckingPoint.position, Vector2.down, _rayDistance, _terrainLayers);
            if (groundInfo.collider == false)
                Jump();

            RaycastHit2D wallInfo = Physics2D.Raycast(new Vector2(_groundCheckingPoint.position.x, _groundCheckingPoint.position.y + 0.15f), Vector2.right, _rayDistance, _terrainLayers);
            if (wallInfo.collider != null && wallInfo.collider.transform != transform)
                Jump();

            wallInfo = Physics2D.Raycast(new Vector2(_groundCheckingPoint.position.x, _groundCheckingPoint.position.y + 0.15f), Vector2.left, _rayDistance, _terrainLayers);
            if (wallInfo.collider != null && wallInfo.collider.transform != transform)
                Jump();
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
            _targetPosition = new Vector2(Player.Instance.transform.position.x, transform.position.y);
        }
        else
            _targetPosition = Vector2.zero;
    }

    void Jump()
    {
        if (Time.timeSinceLevelLoad >= canJump)
        {
            canJump = Time.timeSinceLevelLoad + jumpCooldown;
            _rigidBody.velocity = new Vector2(0, jumpTakeOffSpeed);
        }
    }
}
