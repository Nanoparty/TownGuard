using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayModule : MonoBehaviour
{
    public float delay;

    public bool active = true;

    private Coroutine coroutine;

    private IEnumerator Delay()
    {
        active = false;
        yield return new WaitForSeconds(delay);
        active = true;
    }

    public void StartDelay()
    {
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        coroutine = StartCoroutine(Delay());
    }
}
