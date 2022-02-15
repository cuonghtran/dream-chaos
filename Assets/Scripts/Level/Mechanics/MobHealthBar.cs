using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MobHealthBar : MonoBehaviour
{
    [SerializeField] Image foregroundImage;
    [SerializeField] float updateSpeedSeconds = 0.15f;
    [SerializeField] float positionOffset;
    CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        GetComponentInParent<EnemyBase>().OnHealthPctChanged += HandleHealthChanged;
    }

    void HandleHealthChanged(float pct)
    {
        if (LevelManager.SharedInstance.displayHealthBar)
            StartCoroutine(ChangeToPct(pct));
    }

    IEnumerator ChangeToPct(float pct)
    {
        canvasGroup.alpha = 1;

        float preChangePct = foregroundImage.fillAmount;
        float elapsed = 0;

        while(elapsed < updateSpeedSeconds)
        {
            elapsed += Time.deltaTime;
            foregroundImage.fillAmount = Mathf.Lerp(preChangePct, pct, elapsed / updateSpeedSeconds);
            yield return null;
        }
        foregroundImage.fillAmount = pct;
    }

    private void LateUpdate()
    {
        transform.eulerAngles = new Vector3(0, 0, 0);
    }

    //private void OnDisable()
    //{
    //    GetComponentInParent<EnemyBase>().OnHealthPctChanged -= HandleHealthChanged;
    //}

    //private void OnDestroy()
    //{
    //    GetComponentInParent<EnemyBase>().OnHealthPctChanged -= HandleHealthChanged;
    //}
}
