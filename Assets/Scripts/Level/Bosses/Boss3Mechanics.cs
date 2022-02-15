using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3Mechanics : BossBase
{
    [Header("References")]
    public GameObject lightningBoltPrefab;
    public Transform firePoint;
    public GameObject beamObject;

    [Header("Positions")]
    public Transform leftBottomPoint;
    public Transform rightBottomPoint;
    public Transform leftTopPoint;
    public Transform rightTopPoint;

    [Header("Fight Details")]
    public float teleportCooldown = 8;
    public float inviTime = 1.5f;
    public float attackCooldown = 0.8f;
    float beamCooldown = 1.5f;

    float canTeleport = 0;
    float canAttack = 0;
    float inviTimer = 0;
    bool isInvisible;
    bool lookingRight;
    bool triggerEnrage;
    bool patrolling;
    Transform attackPos;
    Transform targetPos;
    float speed = 3.5f;
    int count = 0;
    bool enragePhase;

    Collider2D bCollider;

    // Start is called before the first frame update
    void Start()
    {
        // init stats
        _maxHP = 1040f;
        _currentHP = _maxHP;
        _damage = 4f;
        _currencyDrop = 700;
        _bossName = "Dream Portal";
        invulnerable = true;

        bossAnimator = GetComponent<Animator>();
        _rigidBody = GetComponent<Rigidbody2D>();
        sRenderer = GetComponent<SpriteRenderer>();
        bCollider = GetComponent<Collider2D>();
        matDefault = sRenderer.material;
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseUIManager.GamePauseMenu || isDead)
            return;

        if (bossAnimator.GetBool("Phase1") && bossAnimator.GetBool("IsEnraged") == false)  // PHASE 1
        {
            // CHECK IF CAN TELEPORT
            if (Time.timeSinceLevelLoad >= canTeleport)
            {
                ToggleSpriteRenderer(false);
                isInvisible = true;
                canTeleport = Time.timeSinceLevelLoad + teleportCooldown;
            }

            if (inviTimer >= inviTime)
            {
                inviTimer = 0;
                isInvisible = false;
                lookingRight = !lookingRight;
                if (lookingRight)
                {
                    transform.eulerAngles = new Vector3(0, 180f, 0);
                    attackPos = leftBottomPoint;
                }
                else
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    attackPos = rightBottomPoint;
                }

                transform.position = attackPos.position;
                ToggleSpriteRenderer(true);
                AudioManager.Instance.Play("Boss3_Teleport");
                canAttack += Time.deltaTime + 2f;
            }

            if (isInvisible)
                inviTimer += Time.deltaTime;
            else  // PHASE 1 ATTACK
            {
                Phase1Attack();
            }
        }

        if (bossAnimator.GetBool("IsEnraged") && bossAnimator.GetBool("Phase1") == false)  // ENRAGE PHASE
        {
            if (triggerEnrage == false)
            {
                StartCoroutine(SwitchPhase());
            }

            if (inviTimer >= inviTime)
            {
                invulnerable = false;
                inviTimer = 0;
                lookingRight = !lookingRight;
                if (lookingRight)
                {
                    transform.eulerAngles = new Vector3(0, 180f, 90);
                    attackPos = leftTopPoint;
                    targetPos = rightTopPoint;
                }
                else
                {
                    transform.eulerAngles = new Vector3(0, 0, 90);
                    attackPos = rightTopPoint;
                    targetPos = leftTopPoint;
                }

                transform.position = attackPos.position;
                ToggleSpriteRenderer(true);
                AudioManager.Instance.Play("Boss3_Teleport");
                isInvisible = false;
                canAttack = Time.timeSinceLevelLoad + beamCooldown;
                count++;
            }

            if (isInvisible)
                inviTimer += Time.deltaTime;
            else  // PHASE 2 ATTACK
            {
                if (invulnerable == false)
                {
                    if (targetPos != null)
                    {
                        if (transform.position == targetPos.position)
                        {
                            patrolling = false;
                            StartCoroutine(StopBeaming());
                        }
                        else Phase2Attack();
                    }
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (bossAnimator.GetBool("IsEnraged"))
        {
            if (targetPos != null && patrolling)
                transform.position = Vector2.MoveTowards(transform.position, targetPos.position, speed * Time.deltaTime);
        }
    }

    void Phase1Attack()
    {
        if (Time.timeSinceLevelLoad >= canAttack)
        {
            canAttack = Time.timeSinceLevelLoad + attackCooldown;
            bossAnimator.SetTrigger("Attack");
        }
    }

    public void FireLightningBolt()
    {
        Instantiate(lightningBoltPrefab, firePoint.position, transform.rotation);
    }

    void Phase2Attack()
    {
        if (Time.timeSinceLevelLoad >= canAttack)
        {
            canAttack = Time.timeSinceLevelLoad + 6;
            if (patrolling == false)
            {
                bossAnimator.SetTrigger("Attack");
                patrolling = true;
            }
        }
    }

    public void BeamingAttack()
    {
        bossAnimator.SetBool("Beaming", true);
        beamObject.SetActive(true);
        bossAnimator.ResetTrigger("Attack");
    }

    IEnumerator StopBeaming()
    {
        if (isInvisible == false)
        {
            bossAnimator.SetBool("Beaming", false);
            beamObject.SetActive(false);
            yield return new WaitForSeconds(1.25f);
            ToggleSpriteRenderer(false);

            // RESET FIGHT
            if (count > 2)
            {
                bossAnimator.SetBool("Phase1", true);
                bossAnimator.SetBool("IsEnraged", false);
                triggerEnrage = false;
                canTeleport = Time.timeSinceLevelLoad + teleportCooldown;
                canAttack = 2.5f;
            }

            isInvisible = true;
        }
    }

    IEnumerator SwitchPhase()
    {
        invulnerable = true;
        bossAnimator.SetBool("Phase1", false);
        triggerEnrage = true;
        yield return new WaitForSeconds(1.5f);
        ToggleSpriteRenderer(false);
        isInvisible = true;
    }

    protected override IEnumerator EnterBattleCoroutine()
    {
        StartCoroutine(PlayBossTheme());
        yield return new WaitForSeconds(1.5f);
        UIManager.SharedInstance.DisplayBossInformation(_bossName);
        hpBar.SetMaxHealth(_currentHP);
        bossAnimator.SetTrigger("Taunt");
        AudioManager.Instance.Play("Boss3_Threaten");
        yield return new WaitForSeconds(0.75f);
        invulnerable = false;
        bossAnimator.SetBool("Phase1", true);
        canTeleport = Time.timeSinceLevelLoad + 4;
        canAttack = Time.timeSinceLevelLoad + 1;
    }

    public override void TakeDamage(float dmg)
    {
        if (invulnerable || isDead)
            return;

        _currentHP -= dmg;
        StartCoroutine(hpBar.SetHealth(_currentHP));

        // Damage popup text
        if (damagePopupText != null)
        {
            ShowDamagePopupText(dmg);
        }

        // Hur animation
        sRenderer.material = matWhite;
        if (_currentHP > 0)
            Invoke("ResetMaterial", 0.11f);

        if (_currentHP <= (_maxHP * 45 / 100) && enragePhase == false) // enrage if hp less than 45% max hp
        {
            enragePhase = true;
            bossAnimator.SetBool("IsEnraged", true);
            bossAnimator.SetBool("Phase1", false);
        }

        if (_currentHP <= 0)
            StartCoroutine(Die());
    }

    void ToggleSpriteRenderer(bool toggle)
    {
        sRenderer.enabled = toggle;
        bCollider.enabled = toggle;
    }
}
