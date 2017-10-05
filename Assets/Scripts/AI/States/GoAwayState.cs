using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoAwayState : IAIState
{
    public void OnStateEnter()
    {
        
    }

    public StateStatus OnStateUpdate(Infos infos)
    {
        return StateStatus.LOOP;
    }

    public void OnStateExit()
    {
        
    }
}
