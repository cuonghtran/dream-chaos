using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactEffect : MonoBehaviour
{
    // Start is called before the first frame update
    //void OnEnable()
    //{
    //    Invoke("DestroyImpactEffect", 0.5f);
    //}

    public IEnumerator DestroyImpactEffect()
    {
        AudioManager.Instance.Play("Bullet_Impact");
        yield return new WaitForSeconds(0.05f);
        this.gameObject.SetActive(false);
    }
}
