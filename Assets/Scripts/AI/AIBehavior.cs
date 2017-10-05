using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateStatus
{
    SUCCESS,
    FAIL,
    LOOP,
}

public interface IAIState
{
    void OnStateEnter();
    StateStatus OnStateUpdate(Transform transform);
    void OnStateExit();
}

public struct Transition
{
    public int currentState;
    public StateStatus transitionCondition;
    public int nextState;
}

public class AIBehavior : MonoBehaviour 
{
    private IAIState _currentState = null;
    private int _startingState = -1;

    private Transform _transform = null;

    private List<Transition> _allTransitions = new List<Transition>();

    private void Start()
    {
        _transform = transform;

        int RESEARCH = StatesManager.Instance().RegisterState(new ResearchState());
        int ACTION = StatesManager.Instance().RegisterState(new ActionState());

        AddTransition(ACTION, StateStatus.SUCCESS, RESEARCH);

        _startingState = ACTION;
        _currentState = StatesManager.Instance().GetState(_startingState);
        _currentState.OnStateEnter();
    }

    private void AddTransition(int currentState, StateStatus transitionCondition,int nextState)
    {
        Transition temp = new Transition();
        temp.currentState = currentState;
        temp.transitionCondition = transitionCondition;
        temp.nextState = nextState;
        _allTransitions.Add(temp);
    }

    private void CheckTransition(StateStatus status)
    {
        for(int i = 0; i < _allTransitions.Count; i++)
        {
            if(
                StatesManager.Instance().GetState(_allTransitions[i].currentState) == _currentState &&
                _allTransitions[i].transitionCondition == status
            )
            {
                _currentState.OnStateExit();
                _currentState = StatesManager.Instance().GetState(_allTransitions[i].nextState);
                _currentState.OnStateEnter();
            }
        }
    }

    private void Update()
    {
        StateStatus ret = _currentState.OnStateUpdate(_transform);
        if(ret != StateStatus.LOOP)
        {
            CheckTransition(ret);
        }
    }

}
