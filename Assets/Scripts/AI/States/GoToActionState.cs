using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class GoToActionState : IAIState
{
    private float _distanceToChange = 1.5f;
    private float _rotationSpeed = 10.0f;

    private Transform _body = null;
    private Transform _transform = null;
    private Vector3 _destination = Vector3.zero;

    private NavMeshAgent _agent;

    public void OnStateEnter(Infos infos)
    {
        if (_transform == null)
        {
            _transform = infos._transform;
            _agent = infos._navMeshAgent;
            _agent.SetDestination(_destination);
        }
        if (_body == null)
        {
            _body = _transform.GetChild(0);
        }

        _destination = GameObject.FindGameObjectWithTag("Action").transform.position;
        _destination.y = 0;
        if(_agent != null)
            _agent.SetDestination(_destination);
    }

    public StateStatus OnStateUpdate()
    {
        _body.rotation = Quaternion.Slerp(_body.rotation, Quaternion.Euler(0,0,0), Time.deltaTime * _rotationSpeed);

        if (Vector3.Distance(_transform.position, _destination) <= _distanceToChange)
        {
            return StateStatus.SUCCESS;
        }

        return StateStatus.LOOP;
    }

    public void OnStateExit()
    {
        _agent.SetDestination(_transform.position);
    }

}
