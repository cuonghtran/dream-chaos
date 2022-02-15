using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Cactus : EnemyBase
{
    public float _speed = 1.7f;
    private float _rayDistance = 1f;
    private bool _movingRight = true;
    [SerializeField] private Transform _groundCheckingPoint;
    [SerializeField] private LayerMask _terrainLayers;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        // init stats
        _maxHP = 49f;
        _currentHP = _maxHP;
        _currencyDrop = 40;
        _damage = 3f;
        ScaleEnemiesOnLevelsAndDifficulty();

        _rigidBody = GetComponent<Rigidbody2D>();
        sRenderer = GetComponent<SpriteRenderer>();
        matDefault = sRenderer.material;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!activate)
            return;

        if (!PauseUIManager.GamePauseMenu && !knockbacking)
        {
            transform.Translate(Vector2.right * _speed * Time.deltaTime);

            RaycastHit2D groundInfo = Physics2D.Raycast(_groundCheckingPoint.position, Vector2.down, _rayDistance, _terrainLayers);
            if (groundInfo.collider == false)
            {
                if (_movingRight)
                {
                    transform.eulerAngles = new Vector3(0, -180f, 0);
                    _movingRight = false;
                }
                else
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    _movingRight = true;
                }
            }

            RaycastHit2D wallInfo = Physics2D.Raycast(new Vector2(_groundCheckingPoint.position.x, _groundCheckingPoint.position.y + 0.5f), Vector2.right, _rayDistance - 0.9f, _terrainLayers);
            if (wallInfo.collider != null && wallInfo.collider.transform != transform)
            {
                if (_movingRight)
                {
                    transform.eulerAngles = new Vector3(0, -180f, 0);
                    _movingRight = false;
                }
                else
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    _movingRight = true;
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
}
