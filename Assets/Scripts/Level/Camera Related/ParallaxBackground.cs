using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private float length;
    // private float startPos;
    private Vector3 startPos;
    public GameObject mainCam;
    public float parallaxEffect;
    [SerializeField] [Range(0, 5f)] float lerpTime = 3.4f;
    bool loopImages = false;

    // Start is called before the first frame update
    void Start()
    {
        // startPos = transform.position;
        startPos = transform.position;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        float temp = mainCam.transform.position.x * (1 - parallaxEffect);
        // float dist = mainCam.transform.position.x * parallaxEffect;
        Vector3 distance = new Vector3(mainCam.transform.position.x * parallaxEffect, mainCam.transform.position.y * Mathf.Min(parallaxEffect + 0.1f, 1), transform.position.z);

        transform.position = new Vector3(startPos.x + distance.x, startPos.y + distance.y, transform.position.z);

        if (temp > startPos.x + length)
            startPos.x += length;
        if (temp < startPos.x - length)
            startPos.x -= length;
    }
}
