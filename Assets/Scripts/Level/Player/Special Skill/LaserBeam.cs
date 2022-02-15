using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LaserBeam : MonoBehaviour
{
    [SerializeField] private GameObject _laserBeamDustPrefab;
    [SerializeField] private Weapons _plasmaGunWeapon;
    private CharacterController2D _controller;
    private Player _player;
    private int launchDirection = 1;

    public Light2D pointLight;
    public Light2D beamLight;
    float lightIntensity = 0.8f;

    private void Start()
    {
        _player = Player.Instance;
        _controller = _player.GetComponent<CharacterController2D>();
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        Invoke("PlaySoundFX", 0.2f);
        StartCoroutine(DisableObject());
    }

    void PlaySoundFX()
    {
        AudioManager.Instance.Play("Laser_Beam");
    }

    IEnumerator DisableObject()
    {
        yield return new WaitForSeconds(0.7f);
        _controller.duringSkills = false;
        yield return new WaitForSeconds(1f);
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Enemies"))
        {
            if (transform.position.x < collision.transform.position.x)
                launchDirection = 1;
            else
                launchDirection = -1;

            if (collision.GetComponent<EnemyBase>() != null)
                collision.GetComponent<EnemyBase>().TakeDamage(_player.CalculateSpecialDamage(_plasmaGunWeapon), launchDirection * _plasmaGunWeapon.knockBackPower * 3);
            else if (collision.GetComponent<BossBase>() != null)
                collision.GetComponent<BossBase>().TakeDamage(_player.CalculateSpecialDamage(_plasmaGunWeapon));
        }
    }

    public void FireLight()
    {
        pointLight.intensity = lightIntensity;
    }

    public void BeamLight()
    {
        beamLight.intensity = lightIntensity;
    }

    public void EndLights()
    {
        Instantiate(_laserBeamDustPrefab, transform.position, transform.rotation);
        pointLight.intensity = 0;
        beamLight.intensity = 0;
    }
}
