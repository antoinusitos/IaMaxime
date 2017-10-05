using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum StateStatus
{
    SUCCESS,
    FAIL,
    LOOP,
}

public delegate bool Condition();

public interface IAIState
{
    void OnStateEnter();
    StateStatus OnStateUpdate(Infos infos);
    void OnStateExit();
}

public struct Transition
{
    public int currentState;
    public StateStatus transitionCondition;
    public Condition conditionToCheck;
    public int nextState;
}

public struct Infos
{
    public Transform _transform;
    public NavMeshAgent _navMeshAgent;
}

public class AIBehavior : MonoBehaviour 
{
    private IAIState _currentState = null;
    private int _startingState = -1;

    private Transform _transform = null;

    private List<Transition> _allTransitions = new List<Transition>();

    private float _timeForAction = 30.0f;
    private float _currentTimeForAction = 0;

    private NavMeshAgent _agent = null;

    private Infos _infos;

    private bool _arrivedToAction = false;

    private void Start()
    {
        _transform = transform;
        _agent = GetComponent<NavMeshAgent>();

        _infos = new Infos();
        _infos._transform = _transform;
        _infos._navMeshAgent = _agent;

        int RESEARCH = StatesManager.Instance().RegisterState(new ResearchState());
        int ACTION = StatesManager.Instance().RegisterState(new ActionState());
        int GOTOACTION = StatesManager.Instance().RegisterState(new GoToActionState());

        AddTransition(RESEARCH, StateStatus.SUCCESS, MustGoToAction, GOTOACTION);
        AddTransition(GOTOACTION, StateStatus.SUCCESS, CheckArrivedToAction , ACTION);
        AddTransition(ACTION, StateStatus.SUCCESS, null, RESEARCH);

        _startingState = RESEARCH;
        _currentState = StatesManager.Instance().GetState(_startingState);
        _currentState.OnStateEnter();

    }

    private void AddTransition(int currentState, StateStatus transitionCondition, Condition conditionToCheck, int nextState)
    {
        Transition temp = new Transition();
        temp.currentState = currentState;
        temp.transitionCondition = transitionCondition;
        temp.conditionToCheck = conditionToCheck;
        temp.nextState = nextState;
        _allTransitions.Add(temp);
    }

    private void CheckTransition(StateStatus status)
    {
        for(int i = 0; i < _allTransitions.Count; i++)
        {
            if(StatesManager.Instance().GetState(_allTransitions[i].currentState) == _currentState)
            {
                if(_allTransitions[i].transitionCondition == status)
                {
                    SetState(_allTransitions[i].nextState);
                    return;
                }
            }
        }

        Debug.LogError("No TRANSITION SET FOR THE STATE : " + _currentState + " for status : " + status);
    }

    private void CheckCondition()
    {
        for (int i = 0; i < _allTransitions.Count; i++)
        {
            if (StatesManager.Instance().GetState(_allTransitions[i].currentState) == _currentState)
            {
                if (_allTransitions[i].conditionToCheck != null && _allTransitions[i].conditionToCheck())
                {
                    SetState(_allTransitions[i].nextState);
                }
            }
        }
    }

    private void SetState(int nextState)
    {
        _currentState.OnStateExit();
        _currentState = StatesManager.Instance().GetState(nextState);
        _currentState.OnStateEnter();
    }

    private void Update()
    {
        StateStatus ret = _currentState.OnStateUpdate(_infos);
        CheckCondition();
        if(ret != StateStatus.LOOP)
        {
            CheckTransition(ret);
        }   
    }

    private bool ActionTriggered()
    {
        return false;
    }

    public void SetArrivedToAction(bool newState)
    {
        _arrivedToAction = newState;
    }

    private bool MustGoToAction()
    {
        _currentTimeForAction += Time.deltaTime;
        if (_currentTimeForAction >= _timeForAction)
        {
            _currentTimeForAction = 0;
            return true;
        }
        return false;
    }

    private bool CheckArrivedToAction()
    {
        return _arrivedToAction;
    }

}
