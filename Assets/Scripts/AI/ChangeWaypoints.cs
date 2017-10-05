using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeWaypoints : MonoBehaviour 
{
    public void ChangeWeypointsToParent(Waypoints newWaypoints)
    {
        GetComponentInParent<AIBehavior>().SetWaypoints(newWaypoints);
        GetComponentInParent<AIBehavior>().SetMustGoCLoser(false);
    }
	
}
