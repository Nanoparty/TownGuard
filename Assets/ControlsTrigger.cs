using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsTrigger : MonoBehaviour
{
    PlayerManager playerManager;

    private void Awake()
    {
        playerManager = FindObjectOfType<PlayerManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerManager.ControlsListener();
            Destroy(this.gameObject);
        }
    }
}
