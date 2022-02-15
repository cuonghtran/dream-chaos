using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogBubble : MonoBehaviour
{
    public TextMeshProUGUI dialogText;
    private string currentText;
    string alphaCode = "<color=#00000000>";
    const float maxTextTime = 0.1f;
    public static int textSpeed = 2;

    CanvasGroup canvasGroup;

    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
    }

    public void Show(string text)
    {
        canvasGroup.alpha = 1;
        currentText = text;
        StartCoroutine(DisplayText());
    }

    public void Close()
    {
        StopAllCoroutines();
        canvasGroup.alpha = 0;
    }

    IEnumerator DisplayText()
    {
        dialogText.text = "";

        foreach(char c in currentText.ToCharArray())
        {
            if (c.ToString() == "/")
                dialogText.text += "\n";
            else dialogText.text += c;
            yield return new WaitForSeconds(0.03f);
        }

        yield return null;
    }
}
