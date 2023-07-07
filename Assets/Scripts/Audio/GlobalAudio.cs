using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalAudio : MonoBehaviour
{
    public static GlobalAudio pa;
    private AudioSource audioSource;

    public AudioClip closeInteraction;
    public AudioClip death;

    public AudioClip[] footsteps;
    public AudioClip[] swordSwings;
    public AudioClip[] swordHits;

    private void Awake()
    {
        if (pa == null)
        {
            pa = this;
        }
        else if (pa != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
    }

    public AudioClip randomClip(AudioClip[] clips)
    {
        int i = Random.Range(0, clips.Length);
        return clips[i];
    }

    public void SwordSwing()
    {
        audioSource.PlayOneShot(randomClip(swordSwings), 1f);
    }
}
