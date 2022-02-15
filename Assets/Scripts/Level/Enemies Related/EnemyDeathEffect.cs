using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathEffect : MonoBehaviour
{
    public IEnumerator HideEffect()
    {
        yield return new WaitForSeconds(0.07f);
        gameObject.SetActive(false);
    }
}
