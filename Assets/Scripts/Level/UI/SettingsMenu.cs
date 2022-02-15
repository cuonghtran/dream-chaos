using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer mainMixer;
    public Slider audioSlider;
    public TextMeshProUGUI onDmgTextText;
    public TextMeshProUGUI offDmgTextText;
    public TextMeshProUGUI onHealthBarText;
    public TextMeshProUGUI offHealthBarText;

    private float volumeValue;
    private Color selectedColor = new Color32(255, 255, 255, 255);
    private Color notSelectedColor = new Color32(130, 130, 130, 255);

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.Play("Button2");
        LoadSettingsFromAttr();
        LoadVolumeSettingsToSlider();
    }

    void LoadSettingsFromAttr()
    {
        if (LevelManager.SharedInstance.displayDmgText)
        {
            onDmgTextText.color = selectedColor;
            offDmgTextText.color = notSelectedColor;
        }
        else
        {
            offDmgTextText.color = selectedColor;
            onDmgTextText.color = notSelectedColor;
        }

        if (LevelManager.SharedInstance.displayHealthBar)
        {
            onHealthBarText.color = selectedColor;
            offHealthBarText.color = notSelectedColor;
        }
        else
        {
            offHealthBarText.color = selectedColor;
            onHealthBarText.color = notSelectedColor;
        }
    }

    void LoadVolumeSettingsToSlider()
    {
        mainMixer.GetFloat("Volume", out float gameVolume);
        audioSlider.value = Mathf.Pow(10, gameVolume / 20);
    }

    public void SetVolume(float volume)
    {
        volumeValue = volume;
        mainMixer.SetFloat("Volume", Mathf.Log10(volume) * 20);
        SaveSoundSettings();
    }

    void SaveSoundSettings()
    {
        PlayerPrefs.SetFloat(AudioManager.Instance.musicVolumePref, volumeValue);
    }

    public void OnButtonDamageText_Click()
    {
        AudioManager.Instance.Play("Button2");
        onDmgTextText.color = selectedColor;
        offDmgTextText.color = notSelectedColor;
        LevelManager.SharedInstance.characterAttr.displayDmgText = true;
        LevelManager.SharedInstance.UpdateDamageText(true);
    }

    public void OffButtonDamageText_Click()
    {
        AudioManager.Instance.Play("Button2");
        offDmgTextText.color = selectedColor;
        onDmgTextText.color = notSelectedColor;
        LevelManager.SharedInstance.characterAttr.displayDmgText = false;
        LevelManager.SharedInstance.UpdateDamageText(false);
    }

    public void OnButtonHealthBar_Click()
    {
        AudioManager.Instance.Play("Button2");
        onHealthBarText.color = selectedColor;
        offHealthBarText.color = notSelectedColor;
        LevelManager.SharedInstance.characterAttr.displayHealthBar = true;
        LevelManager.SharedInstance.UpdateHealthBar(true);
    }

    public void OffButtonHealthBar_Click()
    {
        AudioManager.Instance.Play("Button2");
        offHealthBarText.color = selectedColor;
        onHealthBarText.color = notSelectedColor;
        LevelManager.SharedInstance.characterAttr.displayHealthBar = false;
        LevelManager.SharedInstance.UpdateHealthBar(false);
    }
}
