using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    QuestManager questManager;

    public Transform[] SpawnPositions;
    public Transform[] GuardPositions;
    public GameObject WaveTrigger;

    public GameObject WaveStart;
    public GameObject WaveEnd;

    public GameObject enemyParent;
    public GameObject guardParent;

    public int numberOfWaves;
    public int numberOfEnemies;

    public int currentWave = 1;
    public bool waveActive;

    public GameObject enemySoldier;
    public GameObject skeleton;
    public GameObject fastSkeleton;
    public GameObject goblin;
    public GameObject skeletonKnight;

    public GameObject guard;

    public List<GameObject> enemies;
    public List<GameObject> guards;

    private void Awake()
    {
        questManager = FindObjectOfType<QuestManager>();
    }
    private void Start()
    {
        WaveTrigger.SetActive(false);

        SpawnGuard(2, GuardPositions[1], guard);
        SpawnGuard(2, GuardPositions[2], guard);
        SpawnGuard(2, GuardPositions[3], guard);
        SpawnGuard(2, GuardPositions[0], guard);
        //SpawnWave();
    }

    private void Update()
    {
        if (waveActive)
        {
            if (AllEnemiesDead())
            {
                Debug.Log("Wave Complete");
                waveActive = false;
                questManager.SetQuestStatus("Finished");
                currentWave++;
                AudioManager.am.EndBattle();
                AudioManager.am.WaveEnd();
                WaveEnd.GetComponent<FadeOut>().Enable();
            }
        }
    }

    public void Reset()
    {
        waveActive = false;

        foreach(GameObject e in enemies)
        {
            Destroy(e);
        }
        enemies.Clear();

        foreach(GameObject g in guards)
        {
            Destroy(g);
        }
        guards.Clear();

        SpawnGuard(2, GuardPositions[1], guard);
        SpawnGuard(2, GuardPositions[2], guard);
        SpawnGuard(2, GuardPositions[3], guard);
        SpawnGuard(2, GuardPositions[0], guard);
    }

    public void SpawnWave()
    {
        AudioManager.am.WaveStart();
        WaveTrigger.SetActive(false);
        WaveStart.GetComponent<FadeOut>().Enable();

        if(currentWave == 1)
        {
            Spawn(2, SpawnPositions[0], goblin);
            Spawn(2, SpawnPositions[1], goblin);
            Spawn(2, SpawnPositions[2], goblin);
            Spawn(2, SpawnPositions[3], goblin);
            Spawn(2, SpawnPositions[4], goblin);

            //SpawnGuard(1, GuardPositions[0], guard);

            waveActive = true;
        }

        if (currentWave == 2)
        {
            Spawn(3, SpawnPositions[1], goblin);
            Spawn(3, SpawnPositions[2], skeleton);
            Spawn(3, SpawnPositions[2], goblin);
            Spawn(3, SpawnPositions[3], skeleton);
            Spawn(3, SpawnPositions[4], goblin);

            SpawnGuard(1, GuardPositions[0], guard);
            SpawnGuard(1, GuardPositions[1], guard);
            SpawnGuard(1, GuardPositions[2], guard);
            SpawnGuard(1, GuardPositions[3], guard);

            waveActive = true;
        }

        if (currentWave == 3)
        {
            Spawn(4, SpawnPositions[1], goblin);
            Spawn(4, SpawnPositions[2], skeleton);
            Spawn(3, SpawnPositions[2], skeletonKnight);
            Spawn(4, SpawnPositions[3], skeleton);
            Spawn(4, SpawnPositions[4], goblin);

            SpawnGuard(2, GuardPositions[0], guard);
            SpawnGuard(2, GuardPositions[1], guard);
            SpawnGuard(2, GuardPositions[2], guard);
            SpawnGuard(2, GuardPositions[3], guard);

            waveActive = true;
        }


    }

    private void Spawn(int num, Transform pos, GameObject enemy)
    {
        for (int i = 0; i < num; i++)
        {
            GameObject e = Instantiate(enemy, pos);
            enemies.Add(e);
            e.transform.parent = enemyParent.transform;
        }
    }

    private void SpawnGuard(int num, Transform pos, GameObject enemy)
    {
        for (int i = 0; i < num; i++)
        {
            GameObject e = Instantiate(enemy, pos);
            e.transform.parent = guardParent.transform;
            guards.Add(e);
        }
    }

    private bool AllEnemiesDead()
    {
        foreach(GameObject e in enemies)
        {
            Stats s = e.GetComponent<Stats>();
            if (!s.isDead)
                return false;
        }

        return true;
    }

    


}
