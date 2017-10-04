using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchState : IAIState 
{
    private Transform[] _waypoints = null;
    private Vector3 _destination = Vector3.zero;
    private Vector3 _lastPos = Vector3.zero;
    private int _index = 0;

    private float _speed = 4.0f;
    private float _offset = 0f;
    private float _distance = 0f;
    private float _startTime = 0f;

    public void OnStateEnter()
    {
        _waypoints = GameObject.FindGameObjectWithTag("Waypoints").GetComponent<Waypoints>().allWaypoints;
        _destination = _waypoints[_index].position;
        _startTime = Time.time;
    }

    public void OnStateUpdate(Transform transform)
    {
        if (_lastPos == Vector3.zero)
        {
            _lastPos = transform.position;
            _distance = Vector3.Distance(_lastPos, _destination);
        }

        float distCovered = (Time.time - _startTime) * _speed;
        _offset = distCovered / _distance;
        transform.position = Vector3.Lerp(_lastPos, _destination, _offset);

        if(Vector3.Distance(transform.position, _destination) <= 0.1f)
        {
            _index ++;
            if(_index >= _waypoints.Length)
                _index = 0;
            _destination = _waypoints[_index].position;

            _startTime = Time.time;
            _lastPos = transform.position;
            _distance = Vector3.Distance(_lastPos, _destination);
        }
    }

    public void OnStateExit()
    {

    }
}
