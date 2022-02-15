using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeImpactEffect : MonoBehaviour
{
    public IEnumerator DestroyImpactEffect()
    {
        yield return new WaitForSeconds(0.05f);
        this.gameObject.SetActive(false);
    }
}
