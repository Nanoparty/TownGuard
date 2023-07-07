using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAudio : MonoBehaviour
{
    Stats stats;
    AudioSource audioSource;

    public AudioClip[] footsteps;
    public AudioClip[] deaths;
    public AudioClip[] groans;
    public AudioClip[] bodyDrops;
    public AudioClip[] swordSwings;

    private void Awake()
    {
        stats = GetComponent<Stats>();
        audioSource = GetComponent<AudioSource>();
    }

    public AudioClip randomClip(AudioClip[] clips)
    {
        int i = Random.Range(0, clips.Length);
        return clips[i];
    }

    public void Footstep(AnimationEvent ae)
    {
        if (stats.isDead) return;

        if (ae.animatorClipInfo.weight > 0.5f)
        {
            audioSource.PlayOneShot(randomClip(footsteps), .1f);
        }
    }

    public void BodyDrop()
    {
        audioSource.PlayOneShot(randomClip(bodyDrops), 1f);
    }

    public void Death()
    {
        audioSource.PlayOneShot(randomClip(deaths), 1f);
    }

    public void Groan()
    {
        audioSource.PlayOneShot(randomClip(groans), 1f);
    }

    public void SwordSwing()
    {
        audioSource.PlayOneShot(randomClip(swordSwings), 1f);
    }
}
