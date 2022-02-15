using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBase : MonoBehaviour
{
    protected float _maxHP = 10f;
    protected float _damage = 1f;
    protected float _currentHP;
    protected int _currencyDrop = 200;
    protected string _bossName;
    private bool _isFlipped;
    public bool invulnerable;
    protected bool isDead;

    [Header("References")]
    public HealthBar hpBar;
    public GameObject damagePopupText;

    [Header("Material References")]
    public Material matWhite;
    public Material matDefault;

    protected Rigidbody2D _rigidBody;
    protected Animator bossAnimator;
    protected SpriteRenderer sRenderer;

    public void InitBattle()
    {
        StartCoroutine(EnterBattleCoroutine());
    }

    protected virtual IEnumerator EnterBattleCoroutine()
    {
        StartCoroutine(PlayBossTheme());
        yield return new WaitForSeconds(1.5f);
        UIManager.SharedInstance.DisplayBossInformation(_bossName);
        hpBar.SetMaxHealth(_currentHP);
        bossAnimator.SetTrigger("Taunt");
        WarCry();
        yield return new WaitForSeconds(0.75f);
        invulnerable = false;
        bossAnimator.SetTrigger("EnterBattle");
    }

    protected virtual void WarCry()
    {

    }

    protected IEnumerator PlayBossTheme()
    {
        AudioManager.Instance.Stop("Map_Theme");
        AudioManager.Instance.Play("Boss_Intro");
        yield return new WaitForSeconds(2);
        AudioManager.Instance.Play("Boss_Main");
    }

    public virtual void TakeDamage(float dmg)
    {
        if (invulnerable || isDead)
            return;

        _currentHP -= dmg;
        StartCoroutine(hpBar.SetHealth(_currentHP));

        // Damage popup text
        if(damagePopupText != null)
        {
            ShowDamagePopupText(dmg);
        }

        // Hurt animation
        sRenderer.material = matWhite;
        if (_currentHP >= 0)
            Invoke("ResetMaterial", 0.12f);

        if (_currentHP <= (_maxHP * 45 / 100)) // enrage if hp less than 35% max hp
            bossAnimator.SetBool("IsEnraged", true);

        if (_currentHP <= 0)
            StartCoroutine(Die());
    }

    protected void ResetMaterial()
    {
        sRenderer.material = matDefault;
    }

    protected void ShowDamagePopupText(float dmg)
    {
        // calculate dmg text position
        var topMostPos = sRenderer.bounds.size.y / 2;
        Vector3 damagePos = new Vector3(transform.position.x, transform.position.y + topMostPos, transform.position.z);
        var dmgText = Instantiate(damagePopupText, damagePos, Quaternion.identity);
        dmgText.GetComponent<DmgPopup>().SetUp(dmg);
    }

    protected IEnumerator Die()
    {
        bossAnimator.SetTrigger("Death");
        Physics2D.IgnoreLayerCollision(2, 11, true);
        Physics2D.IgnoreLayerCollision(10, 11, true);
        isDead = true;

        // drop currency
        LevelManager.SharedInstance.CollectCurrency(_currencyDrop);

        yield return new WaitForSeconds(2);
        Physics2D.IgnoreLayerCollision(2, 11, false);
        Physics2D.IgnoreLayerCollision(10, 11, false);
        UIManager.SharedInstance.ShowFinishMenu();
    }

    /// <summary>
    /// Sent when an incoming collider makes contact with this object's
    /// collider (2D physics only).
    /// </summary>
    /// <param name="other">The Collision2D data associated with this collision.</param>
    void OnCollisionEnter2D(Collision2D other)
    {
        if (isDead)
            return;

        if (other.transform.CompareTag("Player"))
        {
            Player.Instance.TakeDamage(_damage+1);
        }
    }

    public void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if (transform.position.x > Player.Instance.transform.position.x && _isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0, 180, 0);
            _isFlipped = false;
        }
        else if (transform.position.x < Player.Instance.transform.position.x && !_isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0, 180, 0);
            _isFlipped = true;
        }
    }
}
