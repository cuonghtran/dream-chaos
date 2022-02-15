using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2Mechanics : BossBase
{
    [Header("Jump/Move Details")]
    public float jumpDistance = 5f;
    public float jumpTakeOffSpeed = 10.5f;
    public float jumpCooldown = 6f;
    float canJump = 0;
    int jumpCount = 0;

    [Header("Attack Details")]
    public float attackCooldown = 1.5f;
    public float canAttack = 0;
    bool attack2 = true;

    [Header("Information")]
    public bool movingRight;
    public Transform groundCheckingPoint;
    public Transform cannonPoint;
    public Transform cannonPoint2;
    public LayerMask terrainMask;
    public bool isGrounded;
    public GameObject missile1Prefab;
    public GameObject missile2Prefab;
    bool isEnraged;
    float playerDifference;

    // Start is called before the first frame update
    void Start()
    {
        // init stats
        _maxHP = 880f;
        _currentHP = _maxHP;
        _damage = 3f;
        _currencyDrop = 400;
        _bossName = "Cannon Tortoise";
        invulnerable = true;

        bossAnimator = GetComponent<Animator>();
        _rigidBody = GetComponent<Rigidbody2D>();
        sRenderer = GetComponent<SpriteRenderer>();
        matDefault = sRenderer.material;
    }

    protected override IEnumerator EnterBattleCoroutine()
    {
        canJump = Time.timeSinceLevelLoad + 6.5f;
        canAttack = Time.timeSinceLevelLoad + 3.5f;
        return base.EnterBattleCoroutine();
    }

    protected override void WarCry()
    {
        AudioManager.Instance.Play("Boss2_Threaten");
    }

    public void Jump(bool jumpingRight)
    {
        float x = jumpDistance;
        if (jumpingRight == false)
            x *= -1;

        if (bossAnimator.GetBool("IsEnraged"))
        {
            jumpCooldown = 5f;
            attackCooldown = 0.5f;
        }

        if (Time.timeSinceLevelLoad >= canJump)
        {
            AudioManager.Instance.Play("Boss2_Jump");
            if (movingRight)
                transform.eulerAngles = new Vector3(0, -180f, 0);
            else transform.eulerAngles = new Vector3(0, 0, 0);

            _rigidBody.velocity = new Vector2(x, jumpTakeOffSpeed);
            canJump = Time.timeSinceLevelLoad + jumpCooldown;
            jumpCount++;
            if (jumpCount > 1)
            {
                jumpCount = 0;
                movingRight = !movingRight;
            }
            bossAnimator.SetTrigger("Jump");
            canAttack += Time.deltaTime + 2.25f;
        }
    }

    public void Attack()
    {
        if (Time.timeSinceLevelLoad >= canAttack)
        {
            FindPlayer();
            canAttack = Time.timeSinceLevelLoad + attackCooldown;
            if (attack2)
            {
                bossAnimator.SetTrigger("Attack2");
                attack2 = !attack2;
            }
            else
            {
                bossAnimator.SetTrigger("Attack1");
                attack2 = !attack2;
            }
        }
    }

    public void Attack1()
    {
        Instantiate(missile1Prefab, cannonPoint2.position, transform.rotation);
    }

    public void Attack2()
    {
        Instantiate(missile2Prefab, cannonPoint.position, transform.rotation);
    }

    void FindPlayer()
    {
        playerDifference = Player.Instance.transform.position.x - transform.position.x;
        if (playerDifference < 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (playerDifference > 0)
        {
            transform.eulerAngles = new Vector3(0, -180f, 0);
        }
    }

    public bool CheckGrounded()
    {
        RaycastHit2D hit2d = Physics2D.Raycast(groundCheckingPoint.position, Vector2.down, 0.1f, terrainMask);
        if (hit2d.collider == true)
        {
            return true;
        }
        return false;
    }
}
