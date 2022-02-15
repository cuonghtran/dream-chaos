using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    private Animator _playerAnim;
    private CharacterController2D _controller;
    private Rigidbody2D _rb;
    private SpriteRenderer _renderer;
    private Collider2D _playerCollider;
    private AudioSource _audioSource;

    private float fallMultiplier = 3.5f;
    private float lowJumpMultiplier = 6f;

    private float _jumpPressedRemember = 0f;
	private float _jumpPressedRememberTime = 0.1f;
	private float _groundedRemember = 0f;
	private float _groundedRememberTime = 0.1f;

    [Header("Character Attributes:")]
    [SerializeField] private float _movementSpeed = 6.4f;
    public float currentHP;
    private float maxHP;
    private float baseHP = 12;
    private bool isDead;
    private float _horizontalInput;
    private bool _jump, _crouch, _canDoubleJump, _doDoubleJump, _invulnerable;
    private bool _enableMoving = true;
    public bool facingRight;
    public float minimumYBorder = -11.5f;

    private Weapons _activeweapon1;
    private Weapons _activeweapon2;
    public Weapons _currentWeapon;

    private float _plasmaGunFireRate = 0.25f;
    private float _shurikensFireRate = 0.8f;
    private float _miniBombFireRate = 1.2f;
    private float _twinBladesAttackRate = 0.6f;
    private float _canFire = 0.15f;
    private bool _multiBomb;

    [Header("Weapon Attack Points")]
    public Transform _attackPoint;
    public Transform plasmaGunAttackPoint;
    public Transform shurikenAttackPoint;
    public Transform miniBombAttackPoint;
    public Transform jumpAttackPoint;

    [Header("Firepower References:")]
    [SerializeField] private GameObject _firestrikePrefab;
    [SerializeField] private GameObject _laserBeamObject;
    [SerializeField] private GameObject _multiBomb1Prefab;
    [SerializeField] private GameObject _multiBomb2Prefab;

    [SerializeField] private LayerMask _enemyLayers;
    public GameObject damagePopupText;
    [SerializeField] private AudioClip[] footstepsClips;

    [Header("Data:")]
    [SerializeField] private Attributes _characterAttributes;

    // perks variables
    private bool _theMoreTheBetter, _hardenedSkin, _doubleJump, _featherLight;
    private bool _vigor, _secondWind, _powerOverwhelming, _innerRage;

    private bool _secondWindProc;
    private float sizeMultiplier;
    private float potionHpRegen = 16;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        Instance = this;
        _controller = GetComponent<CharacterController2D>();
        _playerAnim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<SpriteRenderer>();
        _playerCollider = GetComponent<Collider2D>();
        _audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadWeaponsFromAttr();
        LoadAnimationsOnWeapon();
        LoadPerksFromAttributes();

        // PERK: starting health
        if (_theMoreTheBetter)
            _characterAttributes.maximumHealth = baseHP + _characterAttributes.bonusHealth + Perks.PerksValue[Perks.TheMoreTheBetter];
        else _characterAttributes.maximumHealth = baseHP + _characterAttributes.bonusHealth;
        currentHP = _characterAttributes.maximumHealth;
        maxHP = _characterAttributes.maximumHealth;
        UIManager.SharedInstance.PopulateHearts(maxHP);

        // PERK: starting movement speed
        if (_featherLight)
            _movementSpeed *= (1 + (Perks.PerksValue[Perks.FeatherLight] / 100));

        // PERK: starting firepowers size
        if(_powerOverwhelming)
        {
            sizeMultiplier = (1 + (Perks.PerksValue[Perks.PowerOverwhelming] / 100));
        }

        // PERK: increase damage & attack speed
        if(_innerRage)
        {
            float bonusValue = 1 + (Perks.PerksValue[Perks.InnerRage] / 100);
            if(_activeweapon1)
                _activeweapon1.perkBonus = bonusValue;
            if(_activeweapon2)
                _activeweapon2.perkBonus = bonusValue;

            _twinBladesAttackRate /= bonusValue;
            _plasmaGunFireRate /= bonusValue;
            _shurikensFireRate /= bonusValue;
            _miniBombFireRate /= bonusValue;
        }
        else
        {
            if(_activeweapon1) _activeweapon1.perkBonus = 1;
            if(_activeweapon2) _activeweapon2.perkBonus = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            if (transform.position.y <= minimumYBorder)
            {
                currentHP = 0;
                UIManager.SharedInstance.UpdateHearts();
                StartCoroutine(Die(true));
            }
        }
        

        // preven moving when pause menu is opened
        if (!PauseUIManager.GamePauseMenu && _enableMoving)
        {
            facingRight = _controller.FacingRight;

            ProcessInputs();
            ActivateWeapon();
            DrinkPotions();
        }
        else _horizontalInput = 0;
    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        // Movement control
        _controller.Move(_horizontalInput * _movementSpeed, _crouch, _jump, _doDoubleJump);
        _jump = false;
        _doDoubleJump = false;
    }

    #region Physical Movement

    void ProcessInputs()
    {
        //_horizontalInput = Input.GetAxis("Horizontal");
        _horizontalInput = CrossPlatformInputManager.GetAxis("Horizontal");
        _playerAnim.SetFloat("speed", Mathf.Abs(_horizontalInput));

        // Jump handler
        if (_controller.dashDirection == 0)
            JumpHandler();
    }

    public void OnLanding()
    {
        _playerAnim.SetBool("isJumping", false);
    }

    public void OnAir()
    {
        _playerAnim.SetBool("isJumping", true);  // falling animation
    }

    void JumpHandler()
    {
        _groundedRemember -= Time.deltaTime;
        if (_controller.IsGrounded)
			_groundedRemember = _groundedRememberTime;

		_jumpPressedRemember -= Time.deltaTime;
        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        //if (Input.GetButtonDown("Jump"))
            _jumpPressedRemember = _jumpPressedRememberTime;

		if ((_jumpPressedRemember > 0) && (_groundedRemember > 0))
		{
			_jumpPressedRemember = 0;
			_groundedRemember = 0;

			_playerAnim.SetBool("isJumping", true);
            _jump = true;
            _canDoubleJump = true;
		}
        else
        {
            if(_doubleJump && _canDoubleJump)
            {
                if (CrossPlatformInputManager.GetButtonDown("Jump"))
                //if (Input.GetButtonDown("Jump"))
                {
                    _playerAnim.SetBool("isJumping", true);
                    _jump = true;
                    _doDoubleJump = true;
                    _canDoubleJump = false;
                }
            }
        }
    }

    public void AllowMovement(bool moving)
    {
        _enableMoving = moving;
        if (!moving)
        {
            _horizontalInput = 0;
            _playerAnim.SetFloat("speed", 0);
        }
    }

    #endregion

    #region Weapon Handler

    void LoadWeaponsFromAttr()
    {
        _activeweapon1 = _characterAttributes.activeWeapon1;
        _activeweapon2 = _characterAttributes.activeWeapon2;
        _currentWeapon = _characterAttributes.activeWeapon1;
    }

    void LoadAnimationsOnWeapon()
    {
        int animWeaponCode = 0;
        if (_currentWeapon != null)
        {
            if (_currentWeapon.weaponName == WeaponsList.TwinBlades)
                animWeaponCode = 1;
            else if (_currentWeapon.weaponName == WeaponsList.PlasmaGun)
                animWeaponCode = 2;
            else if (_currentWeapon.weaponName == WeaponsList.Shuriken)
                animWeaponCode = 3;
            else if (_currentWeapon.weaponName == WeaponsList.MiniBomb)
                animWeaponCode = 4;
        }
        _playerAnim.SetInteger("WeaponCode", animWeaponCode);
    }

    public void ChangeWeapon()
    {
        if (_currentWeapon == _activeweapon1)
        {
            if (_activeweapon2 != null)
            {
                _canFire = Time.timeSinceLevelLoad + 0.35f;
                _currentWeapon = _activeweapon2;
            }
        }
        else if (_currentWeapon == _activeweapon2)
        {
            _canFire = Time.timeSinceLevelLoad + 0.35f;
            _currentWeapon = _activeweapon1;
        }
        _playerAnim.SetTrigger("ChangeWeapon");
        LoadAnimationsOnWeapon();
        //_playerAnim.ResetTrigger("ChangeWeapon");
    }

    void ActivateWeapon()
    {
        if (_currentWeapon != null)
        {
            if (CrossPlatformInputManager.GetButton("Fire1") || Input.GetKey(KeyCode.C)) // Change to Input to test
            {
                UseWeapon(_currentWeapon);
            }

            if (CrossPlatformInputManager.GetButton("Fire2") || Input.GetKey(KeyCode.V))
            {
                if(_currentWeapon == _activeweapon1)
                {
                    var onCD = UIManager.SharedInstance.IsWeaponSkill1OnCooldown();
                    if (onCD == false)
                    {
                        UIManager.SharedInstance.ProceedCooldown(true, _vigor);
                        UseSpecialSkill(_activeweapon1);
                    }
                }

                if (_currentWeapon == _activeweapon2)
                {
                    var onCD = UIManager.SharedInstance.IsWeaponSkill2OnCooldown();
                    if (onCD == false)
                    {
                        UIManager.SharedInstance.ProceedCooldown(false, _vigor);
                        UseSpecialSkill(_activeweapon2);
                    }
                }
            }
        }
    }

    void UseWeapon(Weapons weapon)
    {
        switch(weapon.weaponName)
        {
            case WeaponsList.TwinBlades:
                StartCoroutine(BladesCondition());
                break;

            case WeaponsList.PlasmaGun:
                FirePlasmaGun();
                break;
            
            case WeaponsList.Shuriken:
                FireShurikens();
                break;

            case WeaponsList.MiniBomb:
                FireBomb();
                break;
            
            default:
                break;
        }
    }

    void FirePlasmaGun()
    {
        if (Time.timeSinceLevelLoad >= _canFire)
        {
            _playerAnim.SetTrigger("Attack");
            _canFire = Time.timeSinceLevelLoad + _plasmaGunFireRate;
            Vector2 firePos = plasmaGunAttackPoint.position;
            if (!_controller.IsGrounded)
                firePos = jumpAttackPoint.position;

            AudioManager.Instance.Play("Plasma_Gun");
            FireBulletsFromPool("plasma", firePos, transform.rotation);
        }
    }

    void FireShurikens()
    {
        if (Time.timeSinceLevelLoad >= _canFire)
        {
            _canFire = Time.timeSinceLevelLoad + _shurikensFireRate;
            _playerAnim.SetTrigger("Attack");

            StartCoroutine(FiringShurikensRoutine());
        }
    }

    IEnumerator FiringShurikensRoutine()
    {
        Vector2 firePos;
        firePos = shurikenAttackPoint.position;
        if (!_controller.IsGrounded)
            firePos = jumpAttackPoint.position;
        FireBulletsFromPool("shuriken", firePos, transform.rotation);
        AudioManager.Instance.PlayOneShot("Shuriken");
        yield return new WaitForSeconds(0.07f);
        firePos = shurikenAttackPoint.position;
        if (!_controller.IsGrounded)
            firePos = jumpAttackPoint.position;
        FireBulletsFromPool("shuriken", firePos, transform.rotation);
        AudioManager.Instance.PlayOneShot("Shuriken");
        yield return new WaitForSeconds(0.07f);
        firePos = shurikenAttackPoint.position;
        if (!_controller.IsGrounded)
            firePos = jumpAttackPoint.position;
        FireBulletsFromPool("shuriken", firePos, transform.rotation);
        AudioManager.Instance.PlayOneShot("Shuriken");
    }

    void FireBulletsFromPool(string bulletType, Vector3 startPos, Quaternion startRotation)
    {
        GameObject bullet;
        switch(bulletType)
        {
            case "plasma":
                bullet = ObjectPooler.SharedInstance.GetBulletPooledObject();
                break;
            case "shuriken":
                bullet = ObjectPooler.SharedInstance.GetShurikenPooledObject();
                break;
            case "bomb":
                bullet = ObjectPooler.SharedInstance.GetBombPooledObject();
                break;
            default:
                bullet = null;
                break;
        }
        if (bullet != null)
        {
            bullet.transform.position = startPos;
            bullet.transform.rotation = startRotation;
            bullet.SetActive(true);
        }
    }

    void FireBomb()
    {
        if (Time.timeSinceLevelLoad >= _canFire)
        {
            _playerAnim.SetTrigger("Attack");
            AudioManager.Instance.Play("Mini_Bomb");
            _canFire = Time.timeSinceLevelLoad + _miniBombFireRate;
        }
    }

    void ThrowBomb()
    {
        Vector2 firePos = miniBombAttackPoint.position;
        FireBulletsFromPool("bomb", firePos, transform.rotation);

        if (_multiBomb)
        {
            Instantiate(_multiBomb1Prefab, firePos, transform.rotation);
            Instantiate(_multiBomb2Prefab, firePos, transform.rotation);
            _multiBomb = false;
        }

        //GameObject obj = Instantiate(_miniBombPrefab, firePos, transform.rotation);
        // PERK: increase size
        //if (_powerOverwhelming)
        //{
        //    var scaleChange = new Vector3(obj.transform.localScale.x * sizeMultiplier, obj.transform.localScale.y * sizeMultiplier, obj.transform.localScale.z * sizeMultiplier);
        //    obj.transform.localScale = scaleChange;
        //}
    }

    IEnumerator BladesCondition()
    {
        if (Time.timeSinceLevelLoad >= _canFire)
        {
            _canFire = Time.timeSinceLevelLoad + _twinBladesAttackRate;
            _playerAnim.SetTrigger("Attack");
            if (_controller.IsGrounded)
            {
                AllowMovement(false);
                yield return new WaitForSeconds(0.355f);
                AllowMovement(true);
            }
        }
    }

    public float CalculateDamage(Weapons usedWeapon)
    {
        return usedWeapon.GetFinalDamage();
    }

    public float CalculateSpecialDamage(Weapons usedWeapon)
    {
        return usedWeapon.GetSkillDamage();
    }

    public void UseSpecialSkill(Weapons weapon)
    {
        switch (weapon.weaponName)
        {
            case WeaponsList.TwinBlades:
                StartCoroutine(UseFirestrike());
                break;

            case WeaponsList.PlasmaGun:
                UseLaserBeam();
                break;

            case WeaponsList.Shuriken:
                StartCoroutine(UseDash());
                break;

            case WeaponsList.MiniBomb:
                UseMultiBombs();
                break;

            default:
                break;
        }
    }

    IEnumerator UseFirestrike()
    {
        _playerAnim.SetTrigger("Firestrike");
        AllowMovement(false);
        Vector2 firePos = plasmaGunAttackPoint.position;
        Instantiate(_firestrikePrefab, firePos, transform.rotation);
        yield return new WaitForSeconds(0.3f);
        AllowMovement(true);
    }


    void UseLaserBeam()
    {
        _controller.duringSkills = true;
        _playerAnim.SetTrigger("Attack");

        _laserBeamObject.SetActive(true);
        if (!_controller.IsGrounded)
            _laserBeamObject.transform.position = jumpAttackPoint.position;
        else _laserBeamObject.transform.position = plasmaGunAttackPoint.position;
    }

    IEnumerator UseDash()
    {
        AudioManager.Instance.Play("Dash");
        int dashDirection = facingRight ? 1 : -1;
        _playerAnim.SetInteger("DashDirection", dashDirection);
        _controller.dashDirection = dashDirection;
        yield return new WaitForSeconds(_controller.startDashTime);
        _playerAnim.SetInteger("DashDirection", 0);
    }

    void UseMultiBombs()
    {
        _multiBomb = true;
        AudioManager.Instance.Play("Mini_Bomb");
        _playerAnim.SetTrigger("Attack");

    }

    void DrinkPotions()
    {
        if (CrossPlatformInputManager.GetButton("DrinkPotion") || Input.GetKey(KeyCode.X))  // Test
        {
            bool onCD = UIManager.SharedInstance.IsPotionOnCooldown();
            if (onCD || currentHP == maxHP)
                return;

            if (LevelManager.SharedInstance.healingPotions > 0)
            {
                AudioManager.Instance.Play("Drink_Potion");

                UIManager.SharedInstance.TriggerPotionCooldown();

                currentHP = Mathf.Min(currentHP + potionHpRegen, maxHP);

                LevelManager.SharedInstance.ConsumePotion();
            }
        }
    }


    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawRay(plasmaGunAttackPointposition,
    //        new Vector3(plasmaGunAttackPoint.position.x+13f, plasmaGunAttackPoint.position.y, plasmaGunAttackPoint.position.z));
    //}

    #endregion

    #region Perks Handler

    void LoadPerksFromAttributes()
    {
        foreach(string p in _characterAttributes.Perks)
        {
            switch(p)
            {
                case Perks.TheMoreTheBetter:
                    _theMoreTheBetter = true;
                    break;
                case Perks.HardenedSkin:
                    _hardenedSkin = true;
                    break;
                case Perks.DoubleJump:
                    _doubleJump = true;
                    break;
                case Perks.FeatherLight:
                    _featherLight = true;
                    break;
                case Perks.Vigor:
                    _vigor = true;
                    break;
                case Perks.SecondWind:
                    _secondWind = true;
                    break;
                case Perks.PowerOverwhelming:
                    _powerOverwhelming = true;
                    break;
                case Perks.InnerRage:
                    _innerRage = true;
                    break;
                default: break;
            }
        }
    }

    #endregion

    #region Interactions

    public void TakeDamage(float dmg)
    {
        if (PauseUIManager.GamePauseMenu || _invulnerable || isDead)
            return;

        AudioManager.Instance.Play("Player_Hurt1");
        // CameraEffect.Instance.Shake(3, 0.1f);
        StartCoroutine(UIManager.SharedInstance.TakeDamageFlash());

        // PERK: reduce damage taken
        if (_hardenedSkin)
            dmg *= (1 - (Perks.PerksValue[Perks.HardenedSkin] / 100));
            
        currentHP -= dmg;
        if (currentHP < 0)
            currentHP = 0;
        
        UIManager.SharedInstance.UpdateHearts();

        if (currentHP > 0)
        {
            StartCoroutine(InvulAfterHurt());

            // Damage popup text
            if (damagePopupText != null)
            {
                ShowDamagePopupText(dmg);
            }
        }
        else
        {
            // PERK: regen 3 hearts when taking fatal damage
            if (_secondWind && !_secondWindProc)
            {
                _secondWindProc = true;
                currentHP += Perks.PerksValue[Perks.SecondWind];
                UIManager.SharedInstance.UpdateHearts();
            }
            else StartCoroutine(Die(false));
        }
    }

    void ShowDamagePopupText(float dmg)
    {
        // calculate dmg text position
        var topMostPos = GetComponent<SpriteRenderer>().bounds.size.y + 0.2f;
        Vector3 dmgPos = new Vector3(transform.position.x, transform.position.y + topMostPos, transform.position.z);
        var dmgText = Instantiate(damagePopupText, dmgPos, Quaternion.identity);
        dmgText.GetComponent<DmgPopup>().SetUp(dmg, transform.tag);
    }

    IEnumerator InvulAfterHurt()
    {
        Physics2D.IgnoreLayerCollision(2, 11, true);
        Physics2D.IgnoreLayerCollision(10, 11, true);
        _invulnerable = true;
        HurtFlashing();
        yield return new WaitForSeconds(1.5f);
        Physics2D.IgnoreLayerCollision(2, 11, false);
        Physics2D.IgnoreLayerCollision(10, 11, false);
        _invulnerable = false;
        RestorePlayerColorAlpha();
    }

    void HurtFlashing()
    {
        Invoke("DimPlayerColorAlpha", 0);
        Invoke("RestorePlayerColorAlpha", 0.1f);
        Invoke("DimPlayerColorAlpha", 0.2f);
        Invoke("RestorePlayerColorAlpha", 0.3f);
        Invoke("DimPlayerColorAlpha", 0.4f);
        Invoke("RestorePlayerColorAlpha", 0.5f);
        Invoke("DimPlayerColorAlpha", 0.6f);
        Invoke("RestorePlayerColorAlpha", 0.7f);
        Invoke("DimPlayerColorAlpha", 0.8f);
        Invoke("RestorePlayerColorAlpha", 0.9f);
        Invoke("DimPlayerColorAlpha", 1f);
        Invoke("RestorePlayerColorAlpha", 1.1f);
        Invoke("DimPlayerColorAlpha", 1.2f);
        Invoke("RestorePlayerColorAlpha", 1.3f);
        Invoke("DimPlayerColorAlpha", 1.4f);
    }

    void DimPlayerColorAlpha()
    {
        var c = _renderer.color;
        c.a = 0f;
        _renderer.color = c;
    }

    void RestorePlayerColorAlpha()
    {
        var c = _renderer.color;
        c.a = 1f;
        _renderer.color = c;
    }

    IEnumerator Die(bool fallDeath)
    {
        if (isDead)
            yield return null;

        isDead = true;
        
        if (!fallDeath)
        {
            _playerAnim.SetTrigger("IsDead");
            Physics2D.IgnoreLayerCollision(2, 11, false);
            Physics2D.IgnoreLayerCollision(10, 11, false);
        }
        else Invoke("StopFalling", 0.75f);

        AudioManager.Instance.Play("Player_Death");
        yield return new WaitForSeconds(0.55f);
        UIManager.SharedInstance.ShowGameOverMenu();
    }

    void StopFalling()
    {
        _rb.gravityScale = 0;
        _rb.velocity = Vector2.zero;
    }

    public void PickUpHP(int value)
    {
        if (currentHP + value >= _characterAttributes.maximumHealth)
            currentHP = _characterAttributes.maximumHealth;
        else currentHP += value;

        UIManager.SharedInstance.UpdateHearts();
    }

    AudioClip GetRandomClip()
    {
        return footstepsClips[Random.Range(0, footstepsClips.Length - 1)];
    }

    void Footsteps()
    {
        AudioClip clip = GetRandomClip();
        _audioSource.PlayOneShot(clip);
    }

    #endregion
}
