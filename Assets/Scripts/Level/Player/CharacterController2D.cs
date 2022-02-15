using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityStandardAssets.CrossPlatformInput;

public class CharacterController2D : MonoBehaviour
{
	[Header("Character Attributes:")]
	[SerializeField] private float m_JumpForce = 14.5f;							// Amount of force added when the player jumps.
	[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;			// Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private bool m_AirControl = true;							// Whether or not a player can steer while jumping;
	
	[Header("References:")]
	[SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheckCenter;
	[SerializeField] private Transform m_GroundCheckLeft;                           // A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_GroundCheckRight;
	[SerializeField] private Transform m_CeilingCheck;							// A position marking where to check for ceilings
	//[SerializeField] private Collider2D m_CrouchDisableCollider;				// A collider that will be disabled when crouching
	[SerializeField] private ParticleSystem _dustPS;
	[SerializeField] private ParticleSystem dashPS;

	private float fallMultiplier = 3f;
    private float lowJumpMultiplier = 5.5f;

	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded

    public bool IsGrounded { get; private set; }
    private bool m_Collided;           // Whether or not the player is collided with environments
	const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D m_Rigidbody2D;

    public bool FacingRight { get; private set; } = true;
    public bool duringSkills;
	private Vector3 m_Velocity = Vector3.zero;

	private float dashTime;
	public float dashSpeed;
	public float startDashTime;
	public float dashDirection;

	private float moveTime = 0;
	private float maxMoveTime = 0.81f;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;
	public UnityEvent OnAirEvent;

	// [System.Serializable]
	// public class BoolEvent : UnityEvent<bool> { }

	// public BoolEvent OnCrouchEvent;
	// public bool m_wasCrouching = false;

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		// if (OnCrouchEvent == null)
		// 	OnCrouchEvent = new BoolEvent();
	}

    private void Start()
    {
		dashTime = startDashTime;
    }

    private void LateUpdate()
	{
		bool wasGrounded = IsGrounded;
		IsGrounded = false;

		var colliders0 = Physics2D.Raycast(m_GroundCheckCenter.position, Vector2.down, 0.4f, m_WhatIsGround);
		var colliders1 = Physics2D.Raycast(m_GroundCheckLeft.position, Vector2.down, 0.4f, m_WhatIsGround);
		var colliders2 = Physics2D.Raycast(m_GroundCheckRight.position, Vector2.down, 0.4f, m_WhatIsGround);
		if (colliders0.collider != null || colliders1.collider != null || colliders2.collider != null)
		{
			if (Mathf.Abs(m_Rigidbody2D.velocity.y) < 0.01f)
            {
				IsGrounded = true;
				if (!wasGrounded)
				{
					OnLandEvent.Invoke();
				}
			}
		}
		else OnAirEvent.Invoke();

		// smooth the jump action
		SmoothJump();
	}

	void SmoothJump()
    {
        // apply more gravity while falling
        if (m_Rigidbody2D.velocity.y < 0)
            m_Rigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        // jump shorter if not hold the button
        else if (m_Rigidbody2D.velocity.y > 0 && !CrossPlatformInputManager.GetButton("Jump") /* !Input.GetButton("Jump") */)
            m_Rigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
    }

	public void Move(float move, bool crouch, bool jump, bool doDoubleJump)
	{
		/*
		// If crouching, check to see if the character can stand up
		if (!crouch && !jump)
		{
			// If the character has a ceiling preventing them from standing up, keep them crouching
			if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
			{
				crouch = true;
			}
		}
		*/

		// store time spent on moving
		if (move != 0)
		{
			if (moveTime <= maxMoveTime)
				moveTime += Time.deltaTime;
		}
		else moveTime = 0;
		

		if (dashDirection != 0)
		{
			if (dashTime <= 0)
			{
				dashDirection = 0;
				dashTime = startDashTime;
				m_Rigidbody2D.velocity = Vector2.zero;
			}
			else
			{
				CreateDashEffect();
				dashTime -= Time.deltaTime;
				if (dashDirection == 1)
					m_Rigidbody2D.velocity = Vector2.right * dashSpeed;
				else if (dashDirection == -1)
					m_Rigidbody2D.velocity = Vector2.left * dashSpeed;
			}
		}

		//only control the player if grounded or airControl is turned on
		if ((IsGrounded || m_AirControl) && dashDirection == 0)
		{
			/*
			// If crouching
			if (crouch)
			{
				if (!m_wasCrouching)
				{
					m_wasCrouching = true;
					OnCrouchEvent.Invoke(true);
				}

				// Reduce the speed by the crouchSpeed multiplier
				move *= m_CrouchSpeed;

				// Disable one of the colliders when crouching
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = false;
			} else
			{
				// Enable the collider when not crouching
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = true;

				if (m_wasCrouching)
				{
					m_wasCrouching = false;
					OnCrouchEvent.Invoke(false);
				}
			}
			*/

			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

			if(!duringSkills)
            {
				// If the input is moving the player right and the player is facing left...
				if (move > 0 && !FacingRight)
				{
					// ... flip the player.
					Flip();
				}
				// Otherwise if the input is moving the player left and the player is facing right...
				else if (move < 0 && FacingRight)
				{
					// ... flip the player.
					Flip();
				}
			}
			
		}

		// If the player should jump...
		if (jump && dashDirection == 0)
		{
			AudioManager.Instance.Play("Jump1");
			// Add a vertical force to the player.
			IsGrounded = false;

			float moveBoost = (moveTime / maxMoveTime) * 1.8f;
			float jumpBoost = ((moveTime / maxMoveTime) * 0.18f) * m_JumpForce;
			float jumpForce = m_JumpForce + jumpBoost;
			move *= moveBoost;

			if (!doDoubleJump)
				m_Rigidbody2D.velocity = new Vector2(move, jumpForce);
			else
				m_Rigidbody2D.velocity = new Vector2(move, jumpForce * 0.74f);
			CreateDustEffect();
		}
	}


	private void Flip()
	{
		CreateDustEffect();
		// Switch the way the player is labelled as facing.
		FacingRight = !FacingRight;

		transform.Rotate(0f, 180f, 0);

		// flip the camera
		CameraEffect.Instance.isFlipping = true;
	}

	void CreateDustEffect()
    {
        _dustPS.Play();
    }

	void CreateDashEffect()
    {
		dashPS.gameObject.SetActive(true);
		dashPS.Play();
	}
}