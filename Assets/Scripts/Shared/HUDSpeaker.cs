using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDSpeaker : Singleton<HUDSpeaker>
{
    private AudioSource _audio;

    void Start()
    {
        _audio = GetRequiredComponent<AudioSource>();
    }

    public static void PlaySound(AudioClip clip)
    {
        Instance._audio.clip = clip;
        Instance._audio.Play();
    }

    public static void StopSound()
    {
        Instance._audio.Stop();
    }
}
