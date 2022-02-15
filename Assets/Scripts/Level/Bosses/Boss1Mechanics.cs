using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Mechanics : BossBase
{
    [Header("Boss info")]
    private float attackRange = 1.25f;
    public Vector3 attackOffset;
    public LayerMask attackMask;

    // Start is called before the first frame update
    void Start()
    {
        // init stats
        _maxHP = 520f;
        _currentHP = _maxHP;
        _damage = 1f;
        _currencyDrop = 200;
        _bossName = "The Bandit King";
        invulnerable = true;

        bossAnimator = GetComponent<Animator>();
        sRenderer = GetComponent<SpriteRenderer>();
        matDefault = sRenderer.material;
    }

    protected override void WarCry()
    {
        AudioManager.Instance.Play("Boss1_Threaten");
    }

    public void NormalAttack()
    {
        AudioManager.Instance.Play("Shuriken");  // attack swoosh
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Collider2D colliders = Physics2D.OverlapCircle(pos, attackRange, attackMask);
        if (colliders != null)
            Player.Instance.TakeDamage(_damage+3);
    }

    public void EnragedAttack()
    {
        AudioManager.Instance.Play("Shuriken");  // attack swoosh
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Collider2D colliders = Physics2D.OverlapCircle(pos, attackRange, attackMask);
        if (colliders != null)
            Player.Instance.TakeDamage(_damage+4);
    }

    /// <summary>
    /// Callback to draw gizmos only if the object is selected.
    /// </summary>
    void OnDrawGizmosSelected()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;
        Gizmos.DrawWireSphere(pos, attackRange);
    }
}
