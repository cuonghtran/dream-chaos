using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float cameraBoundLeft;
    public float cameraBoundRight;
    public Transform target;
    public float smoothSpeed;
    public Vector3 offset;
    public bool isFlipping;
    private float standardSmoothSpeed = 4.6f;
    private float flipCameraCounter;
    private float flipCameraTime = 1.75f;

    private void Start()
    {
        if (LevelManager.Is16x9ScreenRatio) // 16:9 ratio
        {
            cameraBoundLeft -= 2.2f;
            cameraBoundRight += 2.2f;
        }
        else
        {
            cameraBoundLeft = -30;
        }

        smoothSpeed = standardSmoothSpeed;
    }

    private void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        if (desiredPosition.x <= cameraBoundLeft)
            desiredPosition.x = cameraBoundLeft;
        if (desiredPosition.x >= cameraBoundRight)
            desiredPosition.x = cameraBoundRight;
        desiredPosition.y *= 0.8f;

        Vector3 smoothedPostion = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPostion;

        if (isFlipping)
        {
            smoothSpeed = 1f;
            flipCameraCounter += Time.deltaTime;
            if (flipCameraCounter >= flipCameraTime)
            {
                flipCameraCounter = 0;
                isFlipping = false;
            }
        }
        else
        {
            smoothSpeed = Mathf.MoveTowards(smoothSpeed, standardSmoothSpeed, flipCameraTime * Time.deltaTime);
        }
    }
}
