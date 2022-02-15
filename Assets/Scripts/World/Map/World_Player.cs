using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityStandardAssets.CrossPlatformInput;
using Cinemachine;

public class World_Player : MonoBehaviour
{
    public static World_Player Instance;

    private Rigidbody2D _rigidBody;
    private Animator _anim;
    public CinemachineVirtualCamera mainCam;
    public float camSpeed = 2;
    [SerializeField] private Transform _canvas;
    [SerializeField] AnimationClip idleAnimation;

    private float _speed = 4.25f;
    private Vector2 movementDirection;
    public float movementSpeed;

    [Header("Movement References")]
    public LevelPortal currentNode;
    public bool hasMoved;
    public GameObject playBubble;
    private float lastMove;
    private Vector3 targetNode;
    private float lastSqrMag = Mathf.Infinity;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        Instance = this;
        _rigidBody = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CommonManager.SharedInstance.GamePause == false)
        {
            ProcessInputs();
            Animate();
        }
    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        _rigidBody.velocity = movementDirection;
    }

    void ProcessInputs()
    {
        movementSpeed = Mathf.Clamp(movementDirection.sqrMagnitude, 0.0f, 1.0f);
        float sqrMag = (targetNode - transform.position).sqrMagnitude;
        if (sqrMag > lastSqrMag)
        {
            movementDirection = Vector2.zero;
            transform.position = currentNode.transform.position;
            //Invoke("OpenPlayPanel", 0.25f);
        }
        lastSqrMag = sqrMag;
    }

    void Animate()
    {
        // play idle animation after idle for x seconds
        lastMove += Time.deltaTime;
        if (movementSpeed > 0.01f)
            lastMove = 0f;

        if( movementDirection != Vector2.zero)
        {
            _anim.SetFloat("Horizontal", movementDirection.x);
            _anim.SetFloat("Vertical", movementDirection.y);
        }
        _anim.SetFloat("Speed", movementSpeed);

        if (lastMove >= 5f)
        {
            _anim.Play("WPlayer_Facing_Down");
            
        }
    }

    //void OpenPlayPanel()
    //{
    //    PlayPanelHandler.SharedInstance.OpenPlayPanel();
    //}

    void CalculateDirectionalVector(Vector3 targetNode)
    {
        movementDirection = (targetNode - transform.position).normalized * _speed;
        lastSqrMag = Mathf.Infinity;
    }

    #region Movement Handler

    public void MoveUpButton_Click()
    {
        StartCoroutine(MovementProcesses());
        targetNode = currentNode.GetNextNodePosition("Up");
        CalculateDirectionalVector(targetNode);
    }

    public void MoveDownButton_Click()
    {
        StartCoroutine(MovementProcesses());
        targetNode = currentNode.GetNextNodePosition("Down");
        CalculateDirectionalVector(targetNode);
    }

    public void MoveLeftButton_Click()
    {
        StartCoroutine(MovementProcesses());
        targetNode = currentNode.GetNextNodePosition("Left");
        CalculateDirectionalVector(targetNode);
    }

    public void MoveRightButton_Click()
    {
        StartCoroutine(MovementProcesses());
        targetNode = currentNode.GetNextNodePosition("Right");
        CalculateDirectionalVector(targetNode);
    }

    IEnumerator MovementProcesses()
    {
        AudioManager.Instance.Play("Click_Move_On_Map");
        playBubble.SetActive(false);
        hasMoved = true;
        MovementUIManager.SharedInstance.ToggleMovementButtons(new List<string>());
        Vector3 playerPoint = Camera.main.WorldToViewportPoint(transform.position);
        if (playerPoint.x < 0 || playerPoint.y < 0)
        {
            mainCam.Follow = transform;
            yield return new WaitForSeconds(0.55f);
            mainCam.Follow = null;
        }
    }

    public void TogglePlayBubble(bool active)
    {
        if (active && currentNode != null && !currentNode.isMiddleNode && movementSpeed <= 0.01f)
            playBubble.SetActive(active);
    }

    #endregion
}
