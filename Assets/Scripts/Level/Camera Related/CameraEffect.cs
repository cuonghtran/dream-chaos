using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraEffect : MonoBehaviour
{
	public static CameraEffect Instance;

    private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin multiChannelPerlin;

    public float cameraBoundLeft;
    public float cameraBoundRight;
    public Transform target;
    public float smoothSpeed;
    public Vector3 offset;
    public bool isFlipping;
    private float standardSmoothSpeed = 4.6f;
    private float flipCameraCounter;
    private float flipCameraTime = 1.75f;

    private float shakeTimer;

    private void Awake()
    {
        Instance = this;
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        multiChannelPerlin =
            virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Start()
    {
        if (LevelManager.Is16x9ScreenRatio) // 16:9 ratio
        {
            cameraBoundLeft -= 2.2f;
            cameraBoundRight += 2.2f;
            target.position = new Vector3(target.position.x, target.position.y + 0.4f, target.position.z);
        }

        smoothSpeed = standardSmoothSpeed;
    }

    public void Shake(float intensity, float time)
    {
        multiChannelPerlin.m_FrequencyGain = intensity;
        shakeTimer = time;
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0)
            {
                multiChannelPerlin.m_FrequencyGain = 0f;
            }
        }
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
