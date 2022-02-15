using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioMixerGroup mainMixerGroup;
    public Sound[] sounds;

    private readonly string firstPlay = "FirstPlay";
    public readonly string musicVolumePref = "MusicVolumePref";
    private int firstPlayInt;
    private float volumeValue;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            GameObject obj = new GameObject();
            obj.transform.parent = transform;
            obj.name = s.name;
            s.source = obj.AddComponent<AudioSource>();
            s.source.outputAudioMixerGroup = mainMixerGroup;
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        MusicSettingsHandler();
    }

    public void PlayTheme(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        if (!s.source.isPlaying)
            s.source.Play();
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Play();
    }

    public void PlayOneShot(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.PlayOneShot(s.clip);
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Stop();
    }

    void MusicSettingsHandler()
    {
        firstPlayInt = PlayerPrefs.GetInt(firstPlay);

        if (firstPlayInt == 0)
        {
            volumeValue = 0.5f;
            mainMixerGroup.audioMixer.SetFloat("Volume", Mathf.Log10(volumeValue) * 20);
            PlayerPrefs.SetFloat(musicVolumePref, volumeValue);
            PlayerPrefs.SetInt(firstPlay, -1);
        }
        else
        {
            volumeValue = PlayerPrefs.GetFloat(musicVolumePref);
            mainMixerGroup.audioMixer.SetFloat("Volume", Mathf.Log10(volumeValue) * 20);
        }
    }

    public void FirstPlayMusicSettings()
    {
        volumeValue = 0.5f;
        mainMixerGroup.audioMixer.SetFloat("Volume", Mathf.Log10(volumeValue) * 20);
        PlayerPrefs.SetFloat(musicVolumePref, volumeValue);
    }

    public void MusicSettings()
    {
        volumeValue = PlayerPrefs.GetFloat(musicVolumePref);
        if (volumeValue != 0)
            mainMixerGroup.audioMixer.SetFloat("Volume", Mathf.Log10(volumeValue) * 20);
    }
}
