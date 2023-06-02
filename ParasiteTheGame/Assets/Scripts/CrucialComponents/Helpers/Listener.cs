using UnityEngine;
using UnityEngine.Audio;

public class Listener
{
    private AudioSource audioSource;

    public Listener(AudioSource source)
    {
        audioSource = source;
        audioSource.Play();
    }

    public void ChangeAudioSource(AudioSource source)
    {
        audioSource.Pause();
        audioSource = source;
        audioSource.Play();
    }
}