using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    InputHandler inputHandler;
    PlayerManager playerManager;

    private AudioSource audioSource;

    public AudioClip interact1;
    public AudioClip interact2;
    public AudioClip potion;
    public AudioClip pickupItem;
    public AudioClip questAccept;
    public AudioClip questComplete;
    public AudioClip defeat;
    public AudioClip victory;
    public AudioClip jump;
    public AudioClip roll;

    public AudioClip[] deaths;
    public AudioClip[] footsteps;
    public AudioClip[] swordSwings;
    public AudioClip[] swordHits;
    public AudioClip[] bodyDrop;
    public AudioClip[] groans;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        inputHandler = GetComponent<InputHandler>();
        playerManager = GetComponent<PlayerManager>();
    }

    public AudioClip randomClip(AudioClip[] clips)
    {
        int i = Random.Range(0, clips.Length);
        return clips[i];
    }

    public void Potion()
    {
        audioSource.PlayOneShot(potion, 1f);
    }

    public void Defeat()
    {
        audioSource.PlayOneShot(defeat, 1f);
    }

    public void Pickup()
    {
        audioSource.PlayOneShot(pickupItem, 1f);
    }

    public void Interact1()
    {
        audioSource.PlayOneShot(interact1, 1f);
    }
    public void Interact2()
    {
        audioSource.PlayOneShot(interact2, 1f);
    }

    public void Death()
    {
        audioSource.PlayOneShot(randomClip(deaths), 1f);
    }

    public void SwordSwing()
    {
        audioSource.PlayOneShot(randomClip(swordSwings), 1f);
    }

    public void Footstep()
    {
        if (inputHandler.moveAmount < 0.1f) return;

        if (playerManager.isJumping || playerManager.isInteracting) return;

        audioSource.PlayOneShot(randomClip(footsteps), .1f);
    }

    public void SwordHits()
    {
        audioSource.PlayOneShot(randomClip(swordHits), 1f);
    }

    public void BodyDrop()
    {
        audioSource.PlayOneShot(randomClip(bodyDrop), 1f);
    }

    public void Groan()
    {
        audioSource.PlayOneShot(randomClip(groans), 1f);
    }
}
