using UnityEngine;
using UnityEngine.Audio;

public class Listener
{
    private AudioSource audioSource;

    public Listener(AudioSource source)
    {
        audioSource = source;
    }

    public void PutOnClip(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
}