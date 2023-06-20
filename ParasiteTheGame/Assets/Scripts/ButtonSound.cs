using System;
using UnityEngine;

public class ButtonSound: ASoundable
{
    public void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        
        DontDestroyOnLoad(gameObject);
    }

    public void OnSelected()
    {
        PlaySound(AudioClips[0], pitchChanged: false);
    }

    public void OnPressed()
    {
        PlaySound(AudioClips[1], pitchChanged: false);
    }
}