using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    Collider damageCollider;
    AudioSource audioSource;

    public List<string> enemyTags;

    public int currentWeaponDamage = 25;

    public AudioClip[] hitSounds;

    private void Awake()
    {
        damageCollider = GetComponent<Collider>();
        audioSource = GetComponent<AudioSource>();

        damageCollider.gameObject.SetActive(true);
        damageCollider.isTrigger = true;
        damageCollider.enabled = false;

        enemyTags = new List<string>();
    }

    public void EnableDamageCollider()
    {
        damageCollider.enabled = true;
    }

    public void DisableDamageCollider()
    {
        damageCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (enemyTags.Contains(other.tag))
        {
            Stats s = other.GetComponent<Stats>();

            if (s != null)
            {
                s.TakeDamage(currentWeaponDamage);
                Hit();

                StateManager sm = other.GetComponent<StateManager>();
                if (sm != null)
                {
                    sm.hit = true;
                    sm.attacker = GetComponentInParent<Stats>().gameObject;
                }
            }
        }
    }

    public void Hit()
    {
        audioSource.PlayOneShot(randomClip(hitSounds), 1f);
    }

    public AudioClip randomClip(AudioClip[] clips)
    {
        int i = Random.Range(0, clips.Length);
        return clips[i];
    }
}
