using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class ResearchState : IAIState 
{
    private Transform[] _waypoints = null;
    private Transform _transform = null;
    private Transform _body = null;
    private Vector3 _destination = Vector3.zero;
    private int _index = 0;

    private float _distanceToChange = 1.5f;
    private float _rotationSpeed = 10.0f;

    private NavMeshAgent _agent;

    public void OnStateEnter()
    {
        if (_waypoints == null)
            _waypoints = GameObject.FindGameObjectWithTag("Waypoints").GetComponent<Waypoints>().allWaypoints;
        _destination = _waypoints[Random.Range(0, _waypoints.Length)].position;
        if(_agent != null)
            _agent.SetDestination(_destination);
    }

    public StateStatus OnStateUpdate(Infos infos)
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

        Quaternion finalRot = Quaternion.LookRotation(_destination - _transform.position);
        Vector3 rot = finalRot.eulerAngles;
        rot.x = 0;
        rot.z = 0;
        finalRot = Quaternion.Euler(rot);
        _body.rotation = Quaternion.Slerp(_body.rotation, finalRot, Time.deltaTime * _rotationSpeed);

        if (Vector3.Distance(_transform.position, _destination) <= _distanceToChange)
        {
            _index ++;
            if(_index >= _waypoints.Length)
                _index = 0;
            _destination = _waypoints[Random.Range(0, _waypoints.Length)].position;

            _agent.SetDestination(_destination);
        }

        return StateStatus.LOOP;
    }

    public void OnStateExit()
    {
        _agent.SetDestination(_transform.position);
    }
}
