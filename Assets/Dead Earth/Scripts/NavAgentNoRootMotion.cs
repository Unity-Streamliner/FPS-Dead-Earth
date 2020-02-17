using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavAgentNoRootMotion : MonoBehaviour
{
    public AIWaypointNetwork WaypointNetwork = null;
    public int CurrentIndex = 0;
    public bool HasPath = false;
    public bool PathPending = false;
    public bool PathStale = false;
    [SerializeField] public AnimationCurve JumpCurve = new AnimationCurve();
    [SerializeField] public NavMeshPathStatus PathStatus = NavMeshPathStatus.PathInvalid;

    private NavMeshAgent _navMeshAgent = null;
    private Animator _animator = null;
    private float originalMaxSpeed = 0;

    // Start is called before the first frame update
    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        originalMaxSpeed = _navMeshAgent.speed;

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
        int turnOnSpot;
        HasPath = _navMeshAgent.hasPath;
        PathPending = _navMeshAgent.pathPending;
        PathStale = _navMeshAgent.isPathStale;
        PathStatus = _navMeshAgent.pathStatus;

        Vector3 cross = Vector3.Cross(transform.forward, _navMeshAgent.desiredVelocity.normalized);
        float horizontal = cross.y < 0 ? -cross.magnitude : cross.magnitude;

        Debug.Log(_navMeshAgent.desiredVelocity.magnitude);
        if (_navMeshAgent.desiredVelocity.magnitude < 1.0f && (Vector3.Angle(transform.forward, _navMeshAgent.desiredVelocity) > 50.0f))
        {
            Debug.Log("turnOnSpot");
            _navMeshAgent.speed = 0.1f;
            turnOnSpot = (int)Mathf.Sign(horizontal);
        } else
        {
            _navMeshAgent.speed = originalMaxSpeed;
            turnOnSpot = 0;
        }
        
        horizontal = Mathf.Clamp(horizontal * 4.32f, -2.32f, 2.32f);
        _animator.SetFloat("Horizontal", horizontal, 0.1f, Time.deltaTime);
        _animator.SetFloat("Vertical", _navMeshAgent.desiredVelocity.magnitude, 0.1f, Time.deltaTime);
        _animator.SetInteger("TurnOnSpot", turnOnSpot);

        /* if (_navMeshAgent.isOnOffMeshLink)
        {
            StartCoroutine(Jump(1.0f));
            return;
        } */

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
