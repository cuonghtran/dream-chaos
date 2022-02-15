using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingClouds : MonoBehaviour
{
    private float size = 24.75f;
    public float length = 24.75f;
    public float startPos;
    public float speed;
    bool didSwap;

    // Start is called before the first frame update
    void Start()
    {
        // startPos = transform.position;
        startPos = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        float temp = transform.position.x;
        if (temp > startPos + size)
        {
            foreach(Transform c in transform)
            {
                if (c.localPosition.x >= length && didSwap == false)
                {
                    c.localPosition = new Vector3(c.localPosition.x - (size * 3), transform.position.y, transform.position.z);
                    startPos += size;
                    length -= size;
                    didSwap = true;
                    Invoke("ResetFlag", 1);
                }
            }
        }
    }

    void ResetFlag()
    {
        didSwap = false;
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }
}
