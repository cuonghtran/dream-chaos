using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeTiles : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
{
        transform.GetComponent<Renderer>().sortingLayerName = "Far Background";
        transform.GetComponent<Renderer>().sortingOrder = -10;

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        transform.GetComponent<Renderer>().sortingLayerName = "Foreground";
        transform.GetComponent<Renderer>().sortingOrder = 1;
    }
}
