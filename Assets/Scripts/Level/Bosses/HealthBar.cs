using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider _slider;
    public Gradient gradient;
    public Image fill;
    [SerializeField] float updateSpeedSeconds = 0.15f;

    private bool isDoneInit;
    private float maxHP;

    // Start is called before the first frame update
    void OnEnable()
    {
        _slider = GetComponent<Slider>();
    }

    void Update()
    {
        if (isDoneInit == false)
        {
            if (_slider != null)
            {
                _slider.normalizedValue += (Time.deltaTime);
                if (_slider.normalizedValue == 1)
                    isDoneInit = true;
            }
        }
    }

    public void SetMaxHealth(float health)
    {
        _slider.maxValue = health;
        maxHP = health;
        fill.color = gradient.Evaluate(1);
    }

    public IEnumerator SetHealth(float health)
    {
        float preChangeValue = _slider.value;
        float elapsed = 0;

        while(elapsed < updateSpeedSeconds)
        {
            elapsed += Time.deltaTime;
            _slider.value = Mathf.Lerp(preChangeValue, health, elapsed / updateSpeedSeconds);
            fill.color = gradient.Evaluate(_slider.normalizedValue);
            yield return null;
        }
        _slider.value = health;
        fill.color = gradient.Evaluate(_slider.normalizedValue);

        //_slider.value = health;
        //fill.color = gradient.Evaluate(_slider.normalizedValue);
    }
}
