using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionState : IAIState
{
    private float _timeForAction = 5.0f;
    private float _currentTime = 0f;

    public void OnStateEnter()
    {
        _currentTime = 0;
    }

    public StateStatus OnStateUpdate(Transform transform)
    {
        _currentTime += Time.deltaTime;
        if(_currentTime >= _timeForAction)
        {
            return StateStatus.SUCCESS;
        }
        
        return StateStatus.LOOP;
    }

    public void OnStateExit()
    {

    }
}
