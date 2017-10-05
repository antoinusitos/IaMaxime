using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaWaypoints : MonoBehaviour
{
    public Waypoints areaWaypoints = null;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("Changement de zone");
            GameObject.FindObjectOfType<AIBehavior>().SetMustGoCLoser(true);
        }

        if (other.GetComponent<ChangeWaypoints>())
        {
            Debug.Log("SetMustGoCLoser false");
            other.GetComponent<ChangeWaypoints>().ChangeWeypointsToParent(areaWaypoints);
        }
    }
}
