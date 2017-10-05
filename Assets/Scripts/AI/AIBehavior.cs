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
public delegate void AfterTransition();

public interface IAIState
{
    void OnStateEnter(Infos infos);
    StateStatus OnStateUpdate();
    void OnStateExit();
}

public struct Transition
{
    public int currentState;
    public StateStatus transitionCondition;
    public Condition conditionToCheck;
    public int nextState;
    public AfterTransition afterTransition;
}

public struct Infos
{
    public Transform _transform;
    public NavMeshAgent _navMeshAgent;
    public Transform _playerToFollow;
    public Waypoints _waypoints;
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
    private bool _seePlayer = false;
    private bool _mustGoCloser = false;

    public Waypoints _waypoints = null;
    

    private void Start()
    {
        _transform = transform;
        _agent = GetComponent<NavMeshAgent>();

        _infos = new Infos();
        _infos._transform = _transform;
        _infos._navMeshAgent = _agent;
        _infos._waypoints = _waypoints;

        int RESEARCH = StatesManager.Instance().RegisterState(new ResearchState());
        int ACTION = StatesManager.Instance().RegisterState(new ActionState());
        int GOTOACTION = StatesManager.Instance().RegisterState(new GoToActionState());
        int FOLLOW = StatesManager.Instance().RegisterState(new FollowState());
        int GoCloser = StatesManager.Instance().RegisterState(new GoCloserState());

        AddTransition(RESEARCH, StateStatus.SUCCESS, MustGoToAction, GOTOACTION, null);
        AddTransition(RESEARCH, StateStatus.SUCCESS, CheckSeePlayer, FOLLOW, null);
        AddTransition(RESEARCH, StateStatus.SUCCESS, CheckMustGoCloser, GoCloser, null);
        
        AddTransition(GOTOACTION, StateStatus.SUCCESS, CheckArrivedToAction , ACTION, null);
        AddTransition(GOTOACTION, StateStatus.SUCCESS, CheckSeePlayer, FOLLOW, null);

        AddTransition(ACTION, StateStatus.SUCCESS, null, RESEARCH, null);
        AddTransition(ACTION, StateStatus.SUCCESS, CheckSeePlayer, FOLLOW, null);
        AddTransition(ACTION, StateStatus.SUCCESS, CheckMustGoCloser, GoCloser, null);

        AddTransition(FOLLOW, StateStatus.SUCCESS, CheckNotSeePlayer, RESEARCH, null);

        AddTransition(GoCloser, StateStatus.SUCCESS, CheckMustNOTGoCloser, RESEARCH, null);

        _startingState = RESEARCH;
        _currentState = StatesManager.Instance().GetState(_startingState);
        _currentState.OnStateEnter(_infos);

    }

    private void AddTransition(int currentState, StateStatus transitionCondition, Condition conditionToCheck, int nextState, AfterTransition afterTransition)
    {
        Transition temp = new Transition();
        temp.currentState = currentState;
        temp.transitionCondition = transitionCondition;
        temp.conditionToCheck = conditionToCheck;
        temp.afterTransition = afterTransition;
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
                    if (_allTransitions[i].afterTransition != null)
                    {
                        _allTransitions[i].afterTransition();
                    }
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
                    if (_allTransitions[i].afterTransition != null)
                    {
                        _allTransitions[i].afterTransition();
                    }
                    return;
                }
            }
        }
    }

    private void SetState(int nextState)
    {
        _currentState.OnStateExit();
        _currentState = StatesManager.Instance().GetState(nextState);
        _currentState.OnStateEnter(_infos);
    }

    private void Update()
    {
        StateStatus ret = _currentState.OnStateUpdate();
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

    public void SetArrivedToAction(bool newState)
    {
        _arrivedToAction = newState;
    }

    private bool CheckArrivedToAction()
    {
        return _arrivedToAction;
    }

    public void SetSeePlayer(bool newState)
    {
        _seePlayer = newState;
    }

    private bool CheckSeePlayer()
    {
        return _seePlayer;
    }

    private bool CheckNotSeePlayer()
    {
        return !_seePlayer;
    }

    public void SetMustGoCLoser(bool newState)
    {
        _mustGoCloser = newState;
    }
    
    private bool CheckMustGoCloser()
    {
        return _mustGoCloser;
    }

    private bool CheckMustNOTGoCloser()
    {
        return !_mustGoCloser;
    }

    private void GoingCloser()
    {
        _mustGoCloser = false;
    }

    public void SetWaypoints(Waypoints newWaypoints)
    {
        _waypoints = newWaypoints;
        _infos._waypoints = newWaypoints;
    }
}
