using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PauseUIManager : MonoBehaviour
{
    [Header("References:")]
    [SerializeField] private GameObject mainHubMenu;
    public Animator _animator;
    public GameObject shadowDrop;
    public static bool GamePauseMenu;
    public AudioMixer audioMixer;

    // Start is called before the first frame update
    void Start()
    {
        GamePauseMenu = false;
    }

    #region Paused Menu

    public void OpenPausedMenu()
    {
        AudioManager.Instance.Play("Button2");
        GamePauseMenu = true;
        Time.timeScale = 0;

        shadowDrop.SetActive(true);
        mainHubMenu.SetActive(true);
        //LeanTween.moveY(pausedMenu, Screen.height / 2, 0.2f).setDelay(0.02f);
    }

    public void ClosePausedMenu()
    {
        AudioManager.Instance.Play("Button1");
        GamePauseMenu = false;
        Time.timeScale = 1;
        shadowDrop.SetActive(false);
        _animator.SetTrigger("Close");
        //LeanTween.moveY(pausedMenu, Screen.height * 1.5f, 0.2f);
        StartCoroutine(HidePausedMenuCanvas());
    }

    IEnumerator HidePausedMenuCanvas()
    {
        yield return new WaitForSeconds(0.3f);
        mainHubMenu.SetActive(false);
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            if (!GamePauseMenu)
                OpenPausedMenu();
        }
    }

    #endregion
}
