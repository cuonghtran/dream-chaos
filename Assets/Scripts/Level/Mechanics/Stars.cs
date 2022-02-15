using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stars : MonoBehaviour
{
    public int order;
    private SpriteRenderer rend;
    private Color color;
    private float maxIntensity = 0.65f;
    private float minIntensity = -0.25f;
    private float acceleration;
    private bool incre = true;
    private bool collected;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        color = rend.material.GetColor("_EmissionColor");
        acceleration = (maxIntensity - minIntensity) / 1.75f;
    }

    private void Update()
    {
        if (incre)
        {
            color.a += acceleration * Time.deltaTime;
            rend.material.SetColor("_EmissionColor", color);
            if (color.a >= maxIntensity)
            {
                color.a = maxIntensity;
                incre = false;
            }
        }
        else
        {
            color.a -= acceleration * Time.deltaTime;
            rend.material.SetColor("_EmissionColor", color);
            if (color.a <= minIntensity)
            {
                color.a = minIntensity;
                incre = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            if (!collected)
            {
                AudioManager.Instance.Play("Collect_Star");
                LevelManager.SharedInstance.CollectStar(order);
                gameObject.SetActive(false);
                collected = true;
            }
        }
    }
}
