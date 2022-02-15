using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeAttackHit : MonoBehaviour
{
	[SerializeField] private Weapons _twinBladesWeapon;
	[SerializeField] enum AttacksWhat { EnemyBase, Boss1Mechanics, BossBase };
	[SerializeField] AttacksWhat attacksWhat;
	private int launchDirection = 1;

    private void OnEnable()
    {
		AudioManager.Instance.Play("Blade_Attack");
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.GetComponent("EnemyBase") != null || col.gameObject.GetComponent("BossBase") != null)
		{
			//if (isColliding)
			//	return;
			//isColliding = true;

			if (transform.parent.position.x < col.transform.position.x)
				launchDirection = 1;
			else
				launchDirection = -1;

			var enemy = col.gameObject;
			Vector3 tmpDirection = col.transform.position - transform.position;
			Vector3 tmpContactPoint = transform.position + tmpDirection;
			TriggerImpactEffect(tmpContactPoint);
			if (enemy.GetComponent<EnemyBase>())
				enemy.GetComponent<EnemyBase>().TakeDamage(_twinBladesWeapon.GetFinalDamage(), launchDirection * _twinBladesWeapon.knockBackPower);
			else if (enemy.GetComponent<BossBase>())
				enemy.GetComponent<BossBase>().TakeDamage(_twinBladesWeapon.GetFinalDamage());
		}

	}

	void TriggerImpactEffect(Vector3 contactPoint)
	{
		GameObject ie = ObjectPooler.SharedInstance.GetBladeImpactEffectPooledObject();
		if (ie != null)
		{
			ie.transform.position = contactPoint;
			ie.transform.rotation = Quaternion.identity;
			ie.SetActive(true);
		}
	}
}
