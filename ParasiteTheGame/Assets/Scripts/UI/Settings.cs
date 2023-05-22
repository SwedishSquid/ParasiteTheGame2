using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Settings : MonoBehaviour
{
    public AudioMixer SettingsAudioM;

    private void Start()
    {
        SetStandart();
    }

    public void FullScreenToggle()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }

    public void AudioVolume(float sliderValue)
    {
        SettingsAudioM.SetFloat("settingsVolume", sliderValue);
    }

    public void SetStandart()
    {
        SettingsAudioM.SetFloat("settingsVolume", -30);
        Screen.fullScreen = true;
    }
}
