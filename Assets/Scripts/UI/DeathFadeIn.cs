using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeathFadeIn : MonoBehaviour
{
    PlayerManager playerManager;

    public float fadeInSpeed = .01f;
    public float targetAlpha = 1f;

    public TMP_Text text;
    public Image image;
    public GameObject respawn;

    public bool begin;

    private void Awake()
    {
        playerManager = FindObjectOfType<PlayerManager>();
    }
    private void Start()
    {
        //Reset();
    }

    public void Begin()
    {
        Reset();
        begin = true;
        Debug.Log("Begin Death Fade");
    }

    public void Reset()
    {
        Debug.Log("Reset");
        begin = false;

        if (image != null)
        {
            Color newColor = image.color;
            newColor.a = 0f;
            image.color = newColor;
        }

        if (text != null)
        {
            Color newColor = text.color;
            newColor.a = 0f;
            text.color = newColor;
        }

        respawn.SetActive(false);
    }


    private void Update()
    {
        Debug.Log("Lolipop");

        if (!begin) return;

        Debug.Log("Death Update");

        if (image != null)
        {
            Debug.Log("Image update");
            Color curColor = image.color;
            float alphaDiff = Mathf.Abs(curColor.a - targetAlpha);
            if (alphaDiff > 0.01f)
            {
                curColor.a = Mathf.Lerp(curColor.a, targetAlpha, fadeInSpeed * Time.deltaTime);
                image.color = curColor;
            }
            else
            {
                curColor.a = targetAlpha;
                image.color = curColor;
                respawn.SetActive(true);
                playerManager.canRespawn = true;
                Debug.Log("Finish death Fade");
            }
        }

        if(text != null)
        {
            Color curColor = image.color;
            float alphaDiff = Mathf.Abs(curColor.a - targetAlpha);
            if (alphaDiff > 0.01f)
            {
                curColor.a = Mathf.Lerp(curColor.a, targetAlpha, fadeInSpeed * Time.deltaTime);
                text.color = curColor;
            }
            else
            {
                curColor.a = targetAlpha;
                text.color = curColor;
                respawn.SetActive(true);
                playerManager.canRespawn = true;
            }
        }
    }
}
