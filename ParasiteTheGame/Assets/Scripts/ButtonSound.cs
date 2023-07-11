using System;
using UnityEngine;

public class ButtonSound: ASoundable
{
    public static ButtonSound Instance { get; private set; }
    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
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