using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Settings : MonoBehaviour
{
    public AudioMixer SettingsAudioM;

    public void FullScreenToggle()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }

    public void AudioVolume(float sliderValue)
    {
        SettingsAudioM.SetFloat("settingsVolume", sliderValue);
    }
}
