using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExplosion : MonoBehaviour
{
    private Player _player;
    [SerializeField] private Weapons _miniBombWeapon;
    private int launchDirection = 1;
    private float timing = 0.05f;

    // Start is called before the first frame update
    void OnEnable()
    {
        _player = Player.Instance;
        //StartCoroutine(DestroyExplosion());
    }

    public IEnumerator DestroyExplosion()
    {
        yield return new WaitForSeconds(timing);
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

            if (collision.transform.GetComponent<EnemyBase>() != null)
                collision.transform.GetComponent<EnemyBase>().TakeDamage(_player.CalculateDamage(_miniBombWeapon), launchDirection * _miniBombWeapon.knockBackPower);
            else if (collision.transform.GetComponent<BossBase>() != null)
                collision.transform.GetComponent<BossBase>().TakeDamage(_player.CalculateDamage(_miniBombWeapon));
        }
    }
}
