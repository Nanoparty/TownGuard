using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    enum stateType
    {
        Idle,
        Follow,
        Attack,
        Dead
    }

    public abstract State Run(StateManager sm);
}
