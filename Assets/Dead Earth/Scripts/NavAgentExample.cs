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
    public bool PathStale = false;
    [SerializeField] public AnimationCurve JumpCurve = new AnimationCurve();
    [SerializeField] public NavMeshPathStatus PathStatus = NavMeshPathStatus.PathInvalid;

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
        int nextWaypoint = (CurrentIndex + incStep >= WaypointNetwork.Waypoints.Count) ? 0 : CurrentIndex + incStep;
        Transform nextWaypointTransform = WaypointNetwork.Waypoints[nextWaypoint];

        if (nextWaypointTransform != null)
        {
            CurrentIndex = nextWaypoint;
            _navMeshAgent.destination = nextWaypointTransform.position;
            return;
        }
        CurrentIndex++;
    }

    // Update is called once per frame
    void Update()
    {
        HasPath = _navMeshAgent.hasPath;
        PathPending = _navMeshAgent.pathPending;
        PathStale = _navMeshAgent.isPathStale;
        PathStatus = _navMeshAgent.pathStatus;

        if (_navMeshAgent.isOnOffMeshLink)
        {
            StartCoroutine(Jump(1.0f));
            return;
        }

        if ((!HasPath && !PathPending) || PathStatus == NavMeshPathStatus.PathInvalid)
		{
            SetNextDestination(true);
		} else if (PathStale)
        {
            SetNextDestination(false);
        }
    }

    IEnumerator Jump(float duration)
    {
        OffMeshLinkData data = _navMeshAgent.currentOffMeshLinkData;
        Vector3 startPos = _navMeshAgent.transform.position;
        Vector3 endPos = data.endPos + (_navMeshAgent.baseOffset * Vector3.up);
        float time = 0.0f;

        while(time <= duration)
        {
            float t = time / duration;
            _navMeshAgent.transform.position = Vector3.Lerp(startPos, endPos, t) + (JumpCurve.Evaluate(t) * Vector3.up);
            time += Time.deltaTime;
            yield return null;
        }

        _navMeshAgent.CompleteOffMeshLink();
    }
}
