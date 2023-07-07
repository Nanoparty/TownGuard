using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    SpawnManager spawnManager;

    public List<string> startQuest1;
    public List<string> endQuest1;

    public List<string> startQuest2;
    public List<string> endQuest2;

    public List<string> startQuest3;
    public List<string> endQuest3;

    public List<string> currentDialogs;
    public string currentText;
    public int dialogIndex = 0;

    public int CurrentQuest = 1;
    public string QuestStatus = "Not Started";

    public GameObject NewQuest;
    public GameObject FinishedQuest;
    public GameObject QuestAccept;
    public GameObject QuestComplete;

    public Transform WeaponSpawn;

    public GameObject SwordPickup;
    public GameObject HeavySwordPickup;
    public GameObject ManaSwordPickup;

    public GameObject Gate;

    public bool firstTime = true;

    

    private void Awake()
    {
        spawnManager = FindObjectOfType<SpawnManager>();
    }

    void Start()
    {
        SetQuest(CurrentQuest);
    }

    public void SetQuest(int i)
    {
        CurrentQuest = i;

        SetQuestStatus("Not Started");
    }

    public void Next()
    {
        if (dialogIndex < currentDialogs.Count - 1)
        {
            dialogIndex += 1;
            currentText = currentDialogs[dialogIndex];
        }
        else if (dialogIndex == currentDialogs.Count - 1)
        {
            if (QuestStatus == "Not Started")
            {
                
                NewQuest.GetComponent<Image>().enabled = false;
                AudioManager.am.QuestAccept();
                //spawn sword
                if (CurrentQuest == 1 && firstTime) Instantiate(SwordPickup, WeaponSpawn);
                firstTime = false;
                //Quest Accept Popup
                QuestAccept.GetComponent<FadeOut>().Enable();
                SetQuestStatus("In Progress");
            }
            else if (QuestStatus == "Finished")
            {
                if (CurrentQuest == 1) Instantiate(HeavySwordPickup, WeaponSpawn);
                if (CurrentQuest == 2) Instantiate(ManaSwordPickup, WeaponSpawn);
                //Quest Complete Popup
                FinishedQuest.GetComponent<Image>().enabled = false;
                SetQuest(CurrentQuest + 1);
                AudioManager.am.QuestComplete();
                QuestComplete.GetComponent<FadeOut>().Enable();
                
            }
        }
    }

    public bool hasNext()
    {
        return dialogIndex < currentDialogs.Count - 1;
    }

    public void SetQuestStatus(string s)
    {
        QuestStatus = s;
        if (CurrentQuest > 3) return;

        if (QuestStatus == "Not Started")
        {
            NewQuest.GetComponent<Image>().enabled = true;
            FinishedQuest.GetComponent<Image>().enabled = false;

            Gate.SetActive(false);

            switch (CurrentQuest)
            {
                case 1:
                    currentDialogs = startQuest1;
                    break;
                case 2:
                    currentDialogs = startQuest2;
                    break;
                case 3:
                    currentDialogs = startQuest3;
                    break;
            }
            
        }

        if (QuestStatus == "In Progress")
        {
            NewQuest.GetComponent<Image>().enabled = false;

            spawnManager.WaveTrigger.SetActive(true);
        }

        if (QuestStatus == "Finished")
        {
            FinishedQuest.GetComponent<Image>().enabled = true;

            Gate.SetActive(false);

            switch (CurrentQuest)
            {
                case 1:
                    currentDialogs = endQuest1;
                    break;
                case 2:
                    currentDialogs = endQuest2;
                    break;
                case 3:
                    currentDialogs = endQuest3;
                    break;
            }
        }

        dialogIndex = 0;
        currentText = currentDialogs[0];
    }

    public void Reset()
    {
        SetQuestStatus("Not Started");
    }
}
