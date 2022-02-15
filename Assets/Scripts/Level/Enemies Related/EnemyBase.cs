using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    [Header("References")]
    public Material matWhite;
    public Material matDefault;
    public GameObject damagePopupText;
    [SerializeField] private GameObject _bloodDropPrefab;

    protected float _maxHP = 10f;
    protected float _damage = 1f;
    protected float _currentHP;
    protected int _currencyDrop = 10;
    protected int _chanceToDropHP = 25;
    protected bool activate;
    protected float recoveryCounter;

    [Header("Information")]
    public float recoveryTime = 0.25f;
    public float playerDifference;
    protected float playerDetectRange = 20f; // on screen distance
    protected float _playerSearchRange = 10; // default searching range for player
    private bool _isFlipped = false;

    protected SpriteRenderer sRenderer;
    protected Rigidbody2D _rigidBody;
    protected Animator _animator;
    protected bool knockbacking;
    protected int levelPlayOrder;

    float minimumYBorder = -10.5f;
    string difficulty;

    public event Action<float> OnHealthPctChanged = delegate { };

    public virtual void Update()
    {
        playerDifference = Player.Instance.transform.position.x - transform.position.x;
        if (Mathf.Abs(playerDifference) <= playerDetectRange)
            activate = true;
        else activate = false;

        if (transform.position.y <= minimumYBorder)
        {
            _currentHP = 0;
            StartCoroutine(Die(false));
        }
    }

    public void TakeDamage(float dmg, float knockBackEffect)
    {
        if (!activate)
            return;

        _playerSearchRange = playerDetectRange;  // after take damage, make search range equal to detect range

        _currentHP -= dmg;

        // health bar
        float currentHPPct = (float)_currentHP / (float)_maxHP;
        OnHealthPctChanged(currentHPPct);

        // Damage popup text
        if(damagePopupText != null && LevelManager.SharedInstance.displayDmgText)
            ShowDamagePopupText(dmg);

        sRenderer.material = matWhite;
        if (_currentHP > 0)
            Invoke("ResetMaterial", 0.12f);

        Knockback(knockBackEffect);

        if (_currentHP <= 0)
            StartCoroutine(Die(true));
    }

    void ResetMaterial()
    {
        sRenderer.material = matDefault;
    }

    void Knockback(float knockBackEffect)
    {
        _rigidBody.AddForce(Vector2.right * knockBackEffect, ForceMode2D.Impulse);
        if (knockBackEffect >= 18)
        {
            recoveryCounter = 0;
            knockbacking = true;
        }
    }

    void ShowDamagePopupText(float dmg)
    {
        var topMostPos = sRenderer.bounds.size.y / 2;
        Vector3 dmgPos = new Vector3(transform.position.x, transform.position.y + topMostPos, transform.position.z);

        var dmgText = Instantiate(damagePopupText, dmgPos, Quaternion.identity);
        dmgText.GetComponent<DmgPopup>().SetUp(dmg);
    }

    IEnumerator Die(bool isKilled)
    {
        if (isKilled)
        {
            // drop currency
            LevelManager.SharedInstance.CollectCurrency(_currencyDrop);

            // X% chance to drop blood when die
            if (UnityEngine.Random.Range(0, 100) <= _chanceToDropHP)
                DropBlood();
        }

        AudioManager.Instance.Play("Monsters_Death");
        // destroy monster
        TriggerDeathEffect();
        this.GetComponent<Collider2D>().enabled = false;
        gameObject.SetActive(false);
        yield return new WaitForSeconds(0.05f);
        Destroy(this.gameObject, 0.2f);
    }

    void DropBlood()
    {
        Debug.Log("Drop");
        GameObject blood = Instantiate(_bloodDropPrefab, transform.position, transform.rotation);
    }

    void TriggerDeathEffect()
    {
        GameObject ie = ObjectPooler.SharedInstance.GetEnemyDeathFXPooledObject();
        if (ie != null)
        {
            ie.transform.position = transform.position;
            ie.transform.rotation = transform.rotation;
            ie.SetActive(true);
        }
    }

    /// <summary>
    /// Sent when an incoming collider makes contact with this object's
    /// collider (2D physics only).
    /// </summary>
    /// <param name="other">The Collision2D data associated with this collision.</param>
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            Player.Instance.TakeDamage(_damage);
        }
    }

    public void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if (transform.position.x > Player.Instance.transform.position.x && !_isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0, 180, 0);
            _isFlipped = true;
        }
        else if (transform.position.x < Player.Instance.transform.position.x && _isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0, 180, 0);
            _isFlipped = false;
        }
    }

    protected void ScaleEnemiesOnLevelsAndDifficulty()
    {
        levelPlayOrder = LevelManager.SharedInstance.levelOrder;
        difficulty = LevelManager.SharedInstance.difficulty;

        int increasedHP = 15;
        int increasedDamage = 1;
        int increasedCurrency = 10;

        // progression scale
        if (levelPlayOrder > 6)
        {
            _maxHP += increasedHP;
            _currentHP += increasedHP;
            _damage += increasedDamage;
            _currencyDrop += increasedCurrency;
        }

        if (levelPlayOrder > 12)
        {
            _maxHP += increasedHP;
            _currentHP += increasedHP;
            _damage += increasedDamage;
            _currencyDrop += increasedCurrency;
        }

        if (levelPlayOrder > 18)
        {
            _maxHP += increasedHP;
            _currentHP += increasedHP;
            _damage += increasedDamage;
            _currencyDrop += increasedCurrency;
        }

        // difficulty scale
        float easyScale = 0.75f;
        float hardScale = 1.3f;

        if (difficulty == "Easy")
        {
            _maxHP = Mathf.FloorToInt(_maxHP * easyScale);
            _currentHP = Mathf.FloorToInt(_currentHP * easyScale);
            _damage -= 1;
            _chanceToDropHP += 10;
        }
        else if (difficulty == "Hard")
        {
            _maxHP = Mathf.CeilToInt(_maxHP * hardScale);
            _currentHP = Mathf.CeilToInt(_currentHP * hardScale);
            _damage += 1;
            _chanceToDropHP -= 10;
        }
    }
}
