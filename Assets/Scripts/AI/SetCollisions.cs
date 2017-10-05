using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SetCollisions : MonoBehaviour 
{
    public string ToCheck = "";

    public UnityEvent eventEnter;
    public UnityEvent eventExit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(ToCheck))
        {
            eventEnter.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(ToCheck))
        {
            eventExit.Invoke();
        }
    }
}
