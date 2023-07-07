using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveTrigger : MonoBehaviour
{
    SpawnManager spawnManager;
    QuestManager questManager;

    private void Awake()
    {
        spawnManager = FindObjectOfType<SpawnManager>();
        questManager = FindObjectOfType<QuestManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            spawnManager.SpawnWave();
            questManager.Gate.SetActive(true);
            AudioManager.am.StartBattle();
        }
    }
}
