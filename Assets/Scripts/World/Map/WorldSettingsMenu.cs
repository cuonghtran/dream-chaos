using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class WorldSettingsMenu : MonoBehaviour
{
    [Header("Data:")]
    [SerializeField] private Attributes _characterAttributes;

    [Header("Settings Menu")]
    public GameObject shadowDropSettingsMenu;
    public GameObject settingsPanel;

    [Header("Difficulty References")]
    public TextMeshProUGUI easyModeText;
    public TextMeshProUGUI normalModeText;
    public TextMeshProUGUI hardModeText;
    [Header("Audio References")]
    public AudioMixer mainMixer;
    public Slider audioSlider;
    [Header("Damage Text References")]
    public TextMeshProUGUI onDmgTextText;
    public TextMeshProUGUI offDmgTextText;
    [Header("Health Bar References")]
    public TextMeshProUGUI onHealthBarText;
    public TextMeshProUGUI offHealthBarText;

    private float volumeValue;
    private Color selectedColor = new Color32(255, 255, 255, 255);
    private Color notSelectedColor = new Color32(130, 130, 130, 255);
    private string[] DifficultyMode = { "Easy" , "Normal", "Hard" };

    // Start is called before the first frame update
    void Start()
    {
        LoadSettingsFromAttr();
        LoadVolumeSettingsToSlider();
    }

    public void OpenSettingsMenu()
    {
        AudioManager.Instance.Play("Button2");
        CommonManager.SharedInstance.GamePause = true;
        shadowDropSettingsMenu.SetActive(true);
        settingsPanel.SetActive(true);
        LeanTween.moveY(settingsPanel, Screen.height / 2, 0.2f).setDelay(0.02f);
    }

    public void CloseSettingsMenu()
    {
        AudioManager.Instance.Play("Button1");
        CommonManager.SharedInstance.GamePause = false;
        shadowDropSettingsMenu.SetActive(false);
        LeanTween.moveY(settingsPanel, Screen.height * 1.5f, 0.25f);
        StartCoroutine(HideSettingsMenu());
    }

    IEnumerator HideSettingsMenu()
    {
        yield return new WaitForSeconds(0.4f);
        settingsPanel.SetActive(false);
    }

    void LoadSettingsFromAttr()
    {
        // display dmg text
        if (_characterAttributes.displayDmgText)
        {
            onDmgTextText.color = selectedColor;
            offDmgTextText.color = notSelectedColor;
        }
        else
        {
            offDmgTextText.color = selectedColor;
            onDmgTextText.color = notSelectedColor;
        }

        // display health bar
        if (_characterAttributes.displayHealthBar)
        {
            onHealthBarText.color = selectedColor;
            offHealthBarText.color = notSelectedColor;
        }
        else
        {
            offHealthBarText.color = selectedColor;
            onHealthBarText.color = notSelectedColor;
        }

        // difficulty mode
        if (_characterAttributes.difficultyMode == "Easy")
        {
            easyModeText.color = selectedColor;
            normalModeText.color = notSelectedColor;
            hardModeText.color = notSelectedColor;
        }
        else if (_characterAttributes.difficultyMode == "Normal")
        {
            easyModeText.color = notSelectedColor;
            normalModeText.color = selectedColor;
            hardModeText.color = notSelectedColor;
        }
        else if (_characterAttributes.difficultyMode == "Hard")
        {
            easyModeText.color = notSelectedColor;
            normalModeText.color = notSelectedColor;
            hardModeText.color = selectedColor;
        }
    }

    public void SetVolume(float volume)
    {
        volumeValue = volume;
        mainMixer.SetFloat("Volume", Mathf.Log10(volume) * 20);
        SaveSoundSettings();
    }

    void LoadVolumeSettingsToSlider()
    {
        mainMixer.GetFloat("Volume", out float gameVolume);
        audioSlider.value = Mathf.Pow(10, gameVolume / 20);
    }

    void SaveSoundSettings()
    {
        PlayerPrefs.SetFloat(AudioManager.Instance.musicVolumePref, volumeValue);
    }

    // damage text
    public void OnButtonDamageText_Click()
    {
        AudioManager.Instance.Play("Button2");
        onDmgTextText.color = selectedColor;
        offDmgTextText.color = notSelectedColor;
        _characterAttributes.displayDmgText = true;
    }

    public void OffButtonDamageText_Click()
    {
        AudioManager.Instance.Play("Button2");
        offDmgTextText.color = selectedColor;
        onDmgTextText.color = notSelectedColor;
        _characterAttributes.displayDmgText = false;
    }

    // health bar
    public void OnButtonHealthBar_Click()
    {
        AudioManager.Instance.Play("Button2");
        onHealthBarText.color = selectedColor;
        offHealthBarText.color = notSelectedColor;
        _characterAttributes.displayHealthBar = true;
    }

    public void OffButtonHealthBar_Click()
    {
        AudioManager.Instance.Play("Button2");
        offHealthBarText.color = selectedColor;
        onHealthBarText.color = notSelectedColor;
        _characterAttributes.displayHealthBar = false;
    }

    // difficulty
    public void EasyModeButton_Click()
    {
        AudioManager.Instance.Play("Button2");
        easyModeText.color = selectedColor;
        normalModeText.color = notSelectedColor;
        hardModeText.color = notSelectedColor;
        _characterAttributes.difficultyMode = DifficultyMode[0];
    }

    public void NormalModeButton_Click()
    {
        AudioManager.Instance.Play("Button2");
        easyModeText.color = notSelectedColor;
        normalModeText.color = selectedColor;
        hardModeText.color = notSelectedColor;
        _characterAttributes.difficultyMode = DifficultyMode[1];
    }

    public void HardModeButton_Click()
    {
        AudioManager.Instance.Play("Button2");
        easyModeText.color = notSelectedColor;
        normalModeText.color = notSelectedColor;
        hardModeText.color = selectedColor;
        _characterAttributes.difficultyMode = DifficultyMode[2];
    }

    public void HyperLinksButton_Click(string buttonName)
    {
        Application.OpenURL(ScenesList.HyperLinks[buttonName]);
    }
}
