using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Settings : AMenu
{
    public AudioMixer SettingsAudioM;
    [SerializeField] private Button back;

    public void FullScreenToggle()
    {
        var isFullScreen = !Screen.fullScreen;
        Screen.fullScreen = isFullScreen;
        PlayerPrefs.SetInt("screenSize", isFullScreen ? 1 : 0);
    }

    public void MusicVolume(float sliderValue)
    {
        SettingsAudioM.SetFloat("musicVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("musicVolume", sliderValue);
    }

    public void EffectsVolume(float sliderValue)
    {
        SettingsAudioM.SetFloat("effectsVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("effectsVolume", sliderValue);
    }

    public void BackPressed()
    {
        PlayerPrefs.Save();
    }

    public override void OnActive()
    {
        EventSystem.current.SetSelectedGameObject(back.gameObject);
    }
}
