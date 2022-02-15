using UnityEngine;
using System.Collections;
using UnityEngine;

public class UtilitiesClass
{
    public static IEnumerator FadeOut(CanvasGroup canvasGrp, float fadeTime, YieldInstruction fadeInstruction)
    {
        float elapsedTime = 0.0f;
        canvasGrp.blocksRaycasts = false;
        while (elapsedTime < fadeTime)
        {
            yield return fadeInstruction;
            elapsedTime += Time.deltaTime;
            canvasGrp.alpha = 1.0f - Mathf.Clamp01(elapsedTime / fadeTime);
        }
    }

    public static IEnumerator FadeIn(CanvasGroup canvasGrp, float fadeTime, YieldInstruction fadeInstruction)
    {
        float elapsedTime = 0.0f;
        canvasGrp.blocksRaycasts = true;
        while (elapsedTime < fadeTime)
        {
            yield return fadeInstruction;
            elapsedTime += Time.deltaTime;
            canvasGrp.alpha = Mathf.Clamp01(elapsedTime / fadeTime);
        }
    }

    //Ray ray = Camera.main.ScreenPointToRay(Input.touches[0].position); // check raycast on object
    //RaycastHit hit;

    //if (Physics.Raycast(ray, out hit))
    //{
    //    if (hit.collider != null)
    //    {
    //        Debug.Log(hit);
    //    }
    //}

    //foreach (Touch touch in Input.touches)  // check touch on UI
    //{
    //    int id = touch.fingerId;
    //    if (EventSystem.current.IsPointerOverGameObject(id))
    //    {
    //        // ui touched
    //    }
    //}

    // movement process on 2D map
    //movementDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    //movementSpeed = Mathf.Clamp(movementDirection.sqrMagnitude, 0.0f, 1.0f);
    //movementDirection.Normalize();
    // On FixedUpdate
    //_rigidBody.velocity = movementDirection * movementSpeed * _speed;
}
