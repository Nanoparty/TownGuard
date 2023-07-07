using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageBlink : MonoBehaviour
{
    Image image;

    public float startDelay = 0.5f;
    public float loopDelay = 0.5f;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Start()
    {
        InvokeRepeating("Blink", startDelay, loopDelay);
    }

    void Blink()
    {
        image.enabled = !image.enabled;
    }
}
