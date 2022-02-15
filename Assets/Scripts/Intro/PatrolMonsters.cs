using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolMonsters : MonoBehaviour
{
    public Transform targetPlayer;
    public bool movingRight;
    public float _speed = 4f;
    public float rotateTime = 1.5f;
    float rotateTimer = 0;
    float playerDifference;
    float playerDetectRange = 11;
    bool activate;
    Rigidbody2D _rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        GetComponent<Animator>().SetBool("Patrol", true);

        if (!movingRight)
            transform.eulerAngles = new Vector3(0, -180f, 0);
    }

    private void FixedUpdate()
    {
        if (!CinematicManager.SharedInstance.activateMonsters)
            return;

        if (!CinematicManager.SharedInstance.fleeingMonsters)
        {
            _rigidBody.velocity = transform.right * _speed;
            rotateTimer += Time.deltaTime;
            if (rotateTimer >= rotateTime)
                Rotate();
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            _rigidBody.velocity = transform.right * _speed;
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
