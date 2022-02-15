using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PanCamera : MonoBehaviour
{
    Vector3 touchStart;
    
    [SerializeField] [Range(1, 6f)] float lerpTime = 4;

    [Header("Camera Borders")]
    public float topBorder = 6.4f;
    public float botBorder = -4.21f;
    public float leftBorder = -7.87f;
    public float rightBorder = 12.81f;

    CinemachineVirtualCamera cam;
    Vector3 direction = Vector3.zero;
    float dampingTime = 1.5f;

    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<CinemachineVirtualCamera>();

        // center to player when load world scene
        cam.Follow = World_Player.Instance.transform;
        Invoke("ResetCam", 0.5f);
    }

    void ResetCam()
    {
        cam.Follow = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (!CommonManager.SharedInstance.GamePause)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                direction = Vector3.zero;
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        StopCoroutine(ResetDirection());
                        touchStart = Camera.main.ScreenToWorldPoint(touch.position);
                        break;
                    case TouchPhase.Stationary:
                        //direction = Vector3.zero;
                        StartCoroutine(ResetDirection());
                        break;
                    case TouchPhase.Moved:
                        direction = touchStart - Camera.main.ScreenToWorldPoint(touch.position);
                        break;
                    case TouchPhase.Ended:
                        StartCoroutine(ResetDirection());
                        break;
                }

                Vector3 newPos = transform.position + (direction * 1.45f);
                newPos = new Vector3(Mathf.Clamp(newPos.x, leftBorder, rightBorder), Mathf.Clamp(newPos.y, botBorder, topBorder), newPos.z);
                transform.position = Vector3.Lerp(transform.position, newPos, lerpTime * Time.deltaTime);
            }
            else
            {
                StartCoroutine(ResetDirection());
            }

#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                StopCoroutine(ResetDirection());
                touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }

            if (Input.GetMouseButton(0))
            {
                direction = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 newPos = transform.position + (direction * 1.5f);
                newPos = new Vector3(Mathf.Clamp(newPos.x, leftBorder, rightBorder), Mathf.Clamp(newPos.y, botBorder, topBorder), newPos.z);
                transform.position = Vector3.Lerp(transform.position, newPos, lerpTime * Time.deltaTime);
            }
#endif
        }
    }

    IEnumerator ResetDirection()
    {
        float elapsed = 0;

        while (elapsed < dampingTime)
        {
            elapsed += Time.deltaTime;
            direction = Vector3.Lerp(direction, Vector3.zero, elapsed / dampingTime);
            yield return null;
        }
    }
}
