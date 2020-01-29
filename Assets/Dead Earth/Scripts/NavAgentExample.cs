using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavAgentExample : MonoBehaviour
{
    public AIWaypointNetwork WaypointNetwork = null;
    public int CurrentIndex = 0;
    public bool HasPath = false;
    public bool PathPending = false;

    private NavMeshAgent _navMeshAgent = null;

    // Start is called before the first frame update
    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();

        SetNextDestination(false);
    }

    void SetNextDestination(bool increment)
	{
        if (WaypointNetwork == null) return;
        int incStep = increment ? 1 : 0;
        Transform nextWaypointTransform = null;
        while (nextWaypointTransform == null)
		{
            int nextWaypoint = (CurrentIndex + incStep >= WaypointNetwork.Waypoints.Count) ? 0 : CurrentIndex + incStep;
            nextWaypointTransform = WaypointNetwork.Waypoints[nextWaypoint];

            if (nextWaypointTransform != null)
			{
                CurrentIndex = nextWaypoint;
                _navMeshAgent.destination = nextWaypointTransform.position;
                return;
			}
        }
        CurrentIndex++;
    }

    // Update is called once per frame
    void Update()
    {
        HasPath = _navMeshAgent.hasPath;
        PathPending = _navMeshAgent.pathPending;

        if (!HasPath && !PathPending)
		{
            SetNextDestination(true);
		}
    }
}
