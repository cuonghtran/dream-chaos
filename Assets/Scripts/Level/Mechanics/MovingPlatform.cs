using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Vector3 pointA;
    public Vector3 pointB;
    public float speed = 3f;
    public float restTime = 2f;
    bool resting;
    float restTimer = 0;
    bool movingPlatform;
    Vector3 targetPosition;

    private GameObject carriedObject;
    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        carriedObject = null;

        if (pointA != Vector3.zero && pointB != Vector3.zero)
            movingPlatform = true;
        targetPosition = pointB;
    }

    // Update is called once per frame
    void Update()
    {
        if (!movingPlatform)
            return;

        if (!resting)
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        else
        {
            restTimer += Time.deltaTime;
            if (restTimer >= restTime)
                resting = false;
        }

        if (targetPosition == pointB)
        {
            if (Vector3.Distance(transform.position, pointB) < 0.001f)
            {
                targetPosition = pointA;
                resting = true;
                restTimer = 0;
            }
        }

        if (targetPosition == pointA)
        {
            if (Vector3.Distance(transform.position, pointA) < 0.001f)
            {
                targetPosition = pointB;
                resting = true;
                restTimer = 0;
            }
        }
            
    }

    private void LateUpdate()
    {
        if (carriedObject != null)
            carriedObject.transform.position = transform.position + offset;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        carriedObject = collision.gameObject;
        offset = carriedObject.transform.position - transform.position;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        carriedObject = null;
    }


}
