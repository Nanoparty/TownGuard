using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager am;
    public AudioClip battleMusic;
    public AudioClip victoryMusic;
    public AudioClip introMusic;
    public AudioClip menuMusic;
    public AudioClip whoosh;
    public AudioClip hover;
    public AudioClip click;
    public AudioClip waveStart;
    public AudioClip waveEnd;
    public AudioClip questAccept;
    public AudioClip questComplete;

    public AudioClip[] approve;

    private AudioSource audioSource;
    private AudioSource sfx;

    private void Awake()
    {
        if (am == null)
        {
            am = this;
        }
        else if (am != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
        sfx = transform.GetChild(0).GetComponent<AudioSource>();
    }

    public AudioClip randomClip(AudioClip[] clips)
    {
        int i = Random.Range(0, clips.Length);
        return clips[i];
    }

    public void StartBattle()
    {
        audioSource.clip = battleMusic;
        audioSource.Play();
    }

    public void EndBattle()
    {
        audioSource.Stop();
    }

    public void StartIntro()
    {
        audioSource.clip = introMusic;
        audioSource.Play();
    }

    public void EndIntro()
    {
        audioSource.Stop();
    }

    public void StartMenu()
    {
        audioSource.clip = menuMusic;
        audioSource.Play();
    }

    public void EndMenu()
    {
        audioSource.Stop();
    }

    public void Victory()
    {
        audioSource.PlayOneShot(victoryMusic, .5f);
    }

    public void Whoosh()
    {
        sfx.PlayOneShot(whoosh, 1f);
    }

    public void Hover()
    {
        sfx.PlayOneShot(hover, 1f);
    }

    public void QuestComplete()
    {
        sfx.PlayOneShot(questComplete, 1f);
    }

    public void QuestAccept()
    {
        sfx.PlayOneShot(questAccept, 1f);
    }

    public void WaveStart()
    {
        sfx.PlayOneShot(waveStart, 1f);
    }

    public void WaveEnd()
    {
        sfx.PlayOneShot(waveEnd, 1f);
    }

    public void Click()
    {
        sfx.PlayOneShot(click, 1f);
    }

    public void Approval()
    {
        sfx.PlayOneShot(randomClip(approve), 1f);
    }
}
