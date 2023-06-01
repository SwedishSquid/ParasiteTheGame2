using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class Settings : MonoBehaviour
{
    public AudioMixer SettingsAudioM;
    [SerializeField] private Toggle fullScreenToggle;
    [SerializeField] private MenuScript menu;

    public void FullScreenToggle()
    {
        Screen.fullScreen = !Screen.fullScreen;
        PlayerPrefs.SetInt("screenSize", Screen.fullScreen ? 1 : 0);
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
