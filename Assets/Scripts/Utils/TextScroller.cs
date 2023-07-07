using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextScroller : MonoBehaviour
{
    public string text;
    public float delay;

    public TMP_Text textComponent;
    public GameObject nextIcon;

    public bool begin;
    public bool done;
    public float timer;
    public int curLength;

    private void Awake()
    {
        textComponent = GetComponentInChildren<TMP_Text>();
    }

    void Start()
    {
        curLength = 0;
        textComponent.text = "";
        nextIcon.SetActive(false);
    }

    void Update()
    {
        if (begin)
        {
            timer += Time.deltaTime;

            if (timer > delay)
            {
                timer = 0;
                if (curLength < text.Length)
                {
                    curLength += 1;
                    nextIcon.SetActive(false);
                }
                else
                {
                    begin = false;
                    done = true;
                    nextIcon.SetActive(true);
                }
                
                string partialText = text.Substring(0, curLength);
                textComponent.text = partialText;
            }
        }
    }
}
