using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogInteraction : Interactable
{
    public string dialogText;

    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    
    public override void Interact(PlayerManager playerManager)
    {
        base.Interact(playerManager);

        SpeakWithCharacter(playerManager);
    }

    private void SpeakWithCharacter(PlayerManager playerManager)
    {
        playerManager.dialogPopUp.SetActive(true);
        TextScroller ts = playerManager.dialogPopUp.GetComponentInChildren<TextScroller>();
        ts.text = dialogText;
        ts.begin = true;
    }

}
