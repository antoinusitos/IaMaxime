using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour 
{

    private float _speed = 5.0f;

    private Rigidbody _rigidBody = null;

    private Vector3 _dir = Vector3.zero;

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    private void Update() 
    {
        _dir = Vector3.zero;

		if(Input.GetKey(KeyCode.Z))
        {
            _dir += transform.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            _dir -= transform.forward;
        }
        if (Input.GetKey(KeyCode.D))
        {
            _dir += transform.right;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            _dir -= transform.right;
        }

        _dir.Normalize();
        _rigidBody.MovePosition(_rigidBody.position + _dir * Time.deltaTime * _speed);
	}
}
