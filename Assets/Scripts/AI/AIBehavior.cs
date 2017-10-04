using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAIState
{
    void OnStateEnter();
    void OnStateUpdate(Transform transform);
    void OnStateExit();
}

public class AIBehavior : MonoBehaviour 
{
    private IAIState _currentState = null;
    private int _startingState = -1;

    private Transform _transform = null;

    private void Start()
    {
        _transform = transform;

        _startingState = StatesManager.Instance().RegisterState(new ResearchState());
        print("_startingState:"+_startingState);
        _currentState = StatesManager.Instance().GetState(_startingState);
        print("_currentState:" + _currentState);
        _currentState.OnStateEnter();
    }

    private void Update()
    {
        _currentState.OnStateUpdate(_transform);
    }

}
