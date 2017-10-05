using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowState : IAIState
{
    private float _rotationSpeed = 10.0f;

    private Transform _body = null;
    private Transform _transform = null;
    private Transform _playerToFollow = null;

    private NavMeshAgent _agent;

    public void OnStateEnter(Infos infos)
    {
        if (_transform == null)
        {
            _transform = infos._transform;
            _agent = infos._navMeshAgent;
            _playerToFollow = GameObject.FindGameObjectWithTag("Player").transform;
            _agent.SetDestination(_playerToFollow.position);
        }
        if (_body == null)
        {
            _body = _transform.GetChild(0);
        }
    }

    public StateStatus OnStateUpdate()
    {
        _agent.SetDestination(_playerToFollow.position);

        Quaternion finalRot = Quaternion.LookRotation(_playerToFollow.position - _transform.position);
        Vector3 rot = finalRot.eulerAngles;
        rot.x = 0;
        rot.z = 0;
        finalRot = Quaternion.Euler(rot);

        _body.rotation = Quaternion.Slerp(_body.rotation, finalRot, Time.deltaTime * _rotationSpeed);

        return StateStatus.LOOP;
    }

    public void OnStateExit()
    {

    }
}
