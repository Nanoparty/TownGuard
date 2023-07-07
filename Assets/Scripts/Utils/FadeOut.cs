using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    public float fadeInSpeed = .01f;
    public float targetAlpha = 1f;
    public float delay = 1f;
    public bool delayComplete;

    public List<TMP_Text> texts;
    public List<Image> images;

    private bool begin;

    private void Start()
    {
        Disable();
        //Enable();
    }

    public void Enable()
    {
        foreach(Image i in images)
        {
            i.enabled = true;
        }
        foreach(TMP_Text t in texts)
        {
            t.enabled = true;
        }
        begin = true;
    }

    public void Disable()
    {
        Reset();
        foreach (Image i in images)
        {
            i.enabled = false;
        }
        foreach (TMP_Text t in texts)
        {
            t.enabled = false;
        }
    }

    public void Begin()
    {
        begin = true;
    }

    public void Reset()
    {
        begin = false;

        foreach(Image i in images)
        {
            if (i != null)
            {
                Color newColor = i.color;
                newColor.a = 1f;
                i.color = newColor;
            }
        }

        foreach(TMP_Text t in texts)
        {
            if (t != null)
            {
                Color newColor = t.color;
                newColor.a = 1f;
                t.color = newColor;
            }
        }
    }


    private void Update()
    {
        if (!begin) return;

        StartCoroutine(Delay(delay));

        if (!delayComplete) return;

        foreach(Image i in images)
        {
            Color curColor = i.color;
            float alphaDiff = Mathf.Abs(curColor.a - targetAlpha);
            if (alphaDiff > 0.01f)
            {
                curColor.a = Mathf.Lerp(curColor.a, targetAlpha, fadeInSpeed * Time.deltaTime);
                i.color = curColor;
            }
            else
            {
                curColor.a = targetAlpha;
                i.color = curColor;
                Disable();
            }
        }

        foreach (TMP_Text t in texts)
        {
            Color curColor = t.color;
            float alphaDiff = Mathf.Abs(curColor.a - targetAlpha);
            if (alphaDiff > 0.01f)
            {
                curColor.a = Mathf.Lerp(curColor.a, targetAlpha, fadeInSpeed * Time.deltaTime);
                t.color = curColor;
            }
            else
            {
                curColor.a = targetAlpha;
                t.color = curColor;
                Disable();
            }
        }

    }

    private IEnumerator Delay(float x)
    {
        yield return new WaitForSeconds(x);
        delayComplete = true;
    }
}
