using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class Settings : MonoBehaviour
{
    public AudioMixer SettingsAudioM;
    [SerializeField] private MenuScript menu;

    public void FullScreenToggle()
    {
        var isFullScreen = !Screen.fullScreen;
        Screen.fullScreen = isFullScreen;
        PlayerPrefs.SetInt("screenSize", isFullScreen ? 1 : 0);
    }

    public void AudioVolume(float sliderValue)
    {
        SettingsAudioM.SetFloat("settingsVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("settingsVolume", sliderValue);
    }

    public void BackPressed()
    {
        PlayerPrefs.Save();
        menu.UpdateContinueButton();
    }
}
