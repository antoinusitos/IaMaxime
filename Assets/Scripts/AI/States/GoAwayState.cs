using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoAwayState : IAIState
{
    public void OnStateEnter(Infos infos)
    {
        
    }

    public StateStatus OnStateUpdate()
    {
        return StateStatus.LOOP;
    }

    public void OnStateExit()
    {
        
    }
}
