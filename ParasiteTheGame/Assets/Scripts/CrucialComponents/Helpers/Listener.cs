using UnityEngine;
using UnityEngine.Audio;

public class Listener
{
    private AudioSource audioSource;
    private AudioSource backEffectsSource;
    private bool isBattle;

    public Listener(AudioSource source, AudioSource backEffcts)
    {
        audioSource = source;
        backEffectsSource = backEffcts;
        audioSource.Play();
        backEffectsSource.Play();
    }

    public void ChangeAudioSource(AudioSource source)
    {
        audioSource.Pause();
        audioSource = source;
        audioSource.Play();
        
        isBattle = !isBattle;
        
        if (isBattle)
            backEffectsSource.Pause();
        else
            backEffectsSource.Play();
    }
}