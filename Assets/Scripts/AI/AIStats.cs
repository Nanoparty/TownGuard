using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStats : Stats
{
    AIAudio audio;
    protected override void Awake()
    {
        base.Awake();
        audio = GetComponent<AIAudio>();
    }

    protected override void Start()
    {
        base.Start();
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        audio.Groan();
        GetComponent<DelayModule>().StartDelay();
    }
}
