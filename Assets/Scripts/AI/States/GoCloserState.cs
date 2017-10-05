using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoCloserState : IAIState
{
    public void OnStateEnter()
    {
        
    }

    public StateStatus OnStateUpdate(Transform transform)
    {
        return StateStatus.LOOP;
    }

    public void OnStateExit()
    {
        
    }
}
