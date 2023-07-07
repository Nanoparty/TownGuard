using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeScaler : MonoBehaviour
{
    public bool music;
    public bool sound;

    AudioSource audioSource;
    float defaultVolume;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        defaultVolume = audioSource.volume;
    }

    private void Update()
    {
        if (music)
        {
            audioSource.volume = defaultVolume * Data.musicVolume;
        }
        if (sound)
        {
            audioSource.volume = defaultVolume * Data.soundVolume;
        }
    }
}
