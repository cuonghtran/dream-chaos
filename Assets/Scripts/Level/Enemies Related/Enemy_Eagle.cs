using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy_Eagle : EnemyBase
{
    Transform target;
    public Transform enemyGPX;

    public float speed = 300f;
    public float nextWaypointDistance = 0.5f;

    Path path;
    Seeker seeker;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    // Start is called before the first frame update
    void Start()
    {
        // init stats
        _maxHP = 41f;
        _currentHP = _maxHP;
        _damage = 2f;
        _currencyDrop = 60;
        ScaleEnemiesOnLevelsAndDifficulty();

        sRenderer = enemyGPX.GetComponent<SpriteRenderer>();
        _rigidBody = GetComponent<Rigidbody2D>();
        matDefault = sRenderer.material;
        target = Player.Instance.transform;
        seeker = GetComponent<Seeker>();

        InvokeRepeating("UpdatePath", 0, 0.4f);
    }

    void UpdatePath()
    {
        if (!activate)
            return;

        if (seeker.IsDone())
            seeker.StartPath(_rigidBody.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!PauseUIManager.GamePauseMenu && !knockbacking)
        {
            if (path == null)
                return;

            if (currentWaypoint >= path.vectorPath.Count)
            {
                reachedEndOfPath = true;
                return;
            }
            else reachedEndOfPath = false;

            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - _rigidBody.position).normalized;
            Vector2 force = direction * speed * Time.deltaTime;
            _rigidBody.AddForce(force);

            float distance = Vector2.Distance(_rigidBody.position, path.vectorPath[currentWaypoint]);
            if (distance < nextWaypointDistance)
                currentWaypoint++;

            if (playerDifference > 0)
                enemyGPX.eulerAngles = new Vector3(0, -180, 0);
            else
                enemyGPX.eulerAngles = new Vector3(0, 0, 0);
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
