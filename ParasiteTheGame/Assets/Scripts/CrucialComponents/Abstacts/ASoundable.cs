using System;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class ASoundable: MonoBehaviour
{
    public AudioClip[] AudioClips;
    protected AudioSource audioSource;

    public void PlaySound(AudioClip audioClip, float volume = 1f, bool destroyed = false, bool pitchChanged = true,
        float p1 = 0.85f, float p2 = 1.2f)
    {
        audioSource.pitch = pitchChanged ? Random.Range(p1, p2) : 1f;
        audioSource.PlayOneShot(audioClip);
    }
}