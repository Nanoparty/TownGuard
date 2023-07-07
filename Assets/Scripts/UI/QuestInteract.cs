using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestInteract : Interactable
{
    QuestManager questManager;
    PlayerStats playerStats;
    PlayerAudio playerAudio;
    PlayerInventory playerInventory;

    public Transform WeaponSpawn;
    public GameObject QuestAccept;
    public GameObject QuestComplete;

    public GameObject SwordPickup;
    public GameObject HeavySwordPickup;
    public GameObject ManaSwordPickup;

    private void Awake()
    {
        questManager = FindObjectOfType<QuestManager>();
        playerStats = FindObjectOfType<PlayerStats>();
        playerAudio = FindObjectOfType<PlayerAudio>();
        playerInventory = FindObjectOfType<PlayerInventory>();
    }
    public override void Interact(PlayerManager playerManager)
    {
        base.Interact(playerManager);

        DisplayQuestDialog(playerManager);
    }

    private void DisplayQuestDialog(PlayerManager playerManager)
    {
        AudioManager.am.Approval();

        playerManager.dialogPopUp.SetActive(true);

        TextScroller ts = playerManager.dialogPopUp.GetComponentInChildren<TextScroller>();
        ts.text = questManager.currentText;
        ts.curLength = 0;
        ts.done = false;
        ts.begin = true;

        playerInventory.SetPotionCount(3);
    }
}
