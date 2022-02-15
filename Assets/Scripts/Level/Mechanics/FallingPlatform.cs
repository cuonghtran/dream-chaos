using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float speed = 5f;
    float startSpeed = 0;
    public float restTime = 0.3f;
    float restTimer = 0;
    bool isTouched;
    private GameObject carriedObject;
    private Vector3 offset;
    [Range(1f, 4f)] public float movementSmoothing = 2f;
    private float yBorder = -11.5f;

    // Start is called before the first frame update
    void Start()
    {
        carriedObject = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTouched)
            return;

        if (restTimer < restTime)
            restTimer += Time.deltaTime;
        else
        {
            if (startSpeed < speed)
                startSpeed += movementSmoothing * Time.deltaTime;
            transform.Translate(Vector2.down * startSpeed * Time.deltaTime);
        }

        if (transform.position.y <= yBorder)
            Destroy(gameObject, 0.25f);
    }

    private void LateUpdate()
    {
        if (carriedObject != null)
            carriedObject.transform.position = transform.position + offset;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
            isTouched = true;
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
