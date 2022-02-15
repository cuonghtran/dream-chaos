using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicManager : MonoBehaviour
{
    public static CinematicManager SharedInstance;

    [Header("Positions")]
    public Transform point1;
    public Transform point2;
    public Transform point3;

    [Header("References")]
    public GameObject cinePlayer;
    public GameObject bladeRock;
    public Sprite emptyRock;
    public string[] dialogText;

    [Header("Dialog Bubbles")]
    public DialogBubble dialogBubble;
    //public DialogBubble dialogBubble2;
    //public DialogBubble dialogBubble3;

    [Header("Flags")]
    public float speed = 4;
    public bool activateMonsters;
    public bool fleeingMonsters;

    Rigidbody2D rb;
    Animator anim;
    bool moveToPoint1;
    bool moveToPoint2;
    bool moveToEnd;
    bool skipping;
    bool is16by9Ratio;

    private void Awake()
    {
        SharedInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if ((Screen.width / Screen.height) == 16 / 9)
        {
            is16by9Ratio = true;
            LeanTween.moveX(dialogBubble.GetComponent<RectTransform>(), -365, 0);
        }
        rb = cinePlayer.GetComponent<Rigidbody2D>();
        anim = cinePlayer.GetComponent<Animator>();

        StartCoroutine(CinematicSequences());
    }

    IEnumerator CinematicSequences()
    {
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(HomeSequences());
    }

    IEnumerator HomeSequences()
    {
        cinePlayer.transform.eulerAngles = new Vector3(0, -180, 0);
        yield return new WaitForSeconds(0.35f);
        cinePlayer.transform.eulerAngles = new Vector3(0, 0, 0);
        yield return new WaitForSeconds(0.35f);
        cinePlayer.transform.eulerAngles = new Vector3(0, -180, 0);
        yield return new WaitForSeconds(0.35f);
        cinePlayer.transform.eulerAngles = new Vector3(0, 0, 0);
        dialogBubble.Show(dialogText[0]);
        yield return new WaitForSeconds(3.5f);
        dialogBubble.Close();
        moveToPoint1 = true;
        activateMonsters = true;
    }

    IEnumerator BookSequences()
    {
        if(is16by9Ratio)
            LeanTween.moveX(dialogBubble.GetComponent<RectTransform>(), -360, 0);
        else
            LeanTween.moveX(dialogBubble.GetComponent<RectTransform>(), -295, 0);
        dialogBubble.Show(dialogText[1]);
        yield return new WaitForSeconds(3.5f);
        dialogBubble.Close();
        dialogBubble.Show(dialogText[2]);
        yield return new WaitForSeconds(2f);
        dialogBubble.Close();
        dialogBubble.Show(dialogText[3]);
        yield return new WaitForSeconds(4.5f);
        dialogBubble.Close();
        moveToPoint2 = true;
    }

    IEnumerator BladeSequences()
    {
        yield return new WaitForSeconds(1.25f);
        anim.SetTrigger("UseBlade");
        bladeRock.GetComponent<SpriteRenderer>().sprite = emptyRock;
        if (is16by9Ratio)
            LeanTween.moveX(dialogBubble.GetComponent<RectTransform>(), -245, 0);
        else
            LeanTween.moveX(dialogBubble.GetComponent<RectTransform>(), 42, 0);
        dialogBubble.Show(dialogText[4]);
        yield return new WaitForSeconds(2.5f);
        dialogBubble.Close();
        fleeingMonsters = true;
        moveToEnd = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (skipping)
            return;

        if (moveToPoint1)
        {
            rb.velocity = new Vector2(speed, 0);
            anim.SetBool("Moving", true);
        }

        if (moveToPoint2)
        {
            rb.velocity = new Vector2(speed, 0);
            anim.SetBool("Moving", true);
        }

        if (moveToEnd)
        {
            rb.velocity = new Vector2(speed, 0);
            anim.SetBool("Moving", true);
        }
    }

    private void Update()
    {
        if (Vector3.Distance(rb.position, point1.position) < 0.2f && moveToPoint1)
        {
            rb.velocity = Vector2.zero;
            moveToPoint1 = false;
            anim.SetBool("Moving", false);
            StartCoroutine(BookSequences());
        }

        if (Vector3.Distance(rb.position, point2.position) < 0.2f && moveToPoint2)
        {
            rb.velocity = Vector2.zero;
            moveToPoint2 = false;
            anim.SetBool("Moving", false);
            StartCoroutine(BladeSequences());
        }

        if (Vector3.Distance(rb.position, point3.position) < 0.2f && moveToEnd)
        {
            SwitchToWorldScene();
        }
    }

    void SwitchToWorldScene()
    {
        PlayerPrefs.SetInt("WatchedCinematic", 1);
        SceneController.Instance.FadeAndLoadScene(ScenesList.WorldScene);
    }

    public void SkipButton_Click()
    {
        skipping = true;
        SwitchToWorldScene();
    }
}
