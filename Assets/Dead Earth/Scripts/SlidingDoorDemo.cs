using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorState
{
    Open, Animating, Closed
}

public class SlidingDoorDemo : MonoBehaviour
{

    public float Slidingdistance = 4.0f;
    public float Duration = 1.5f;
    public AnimationCurve JumpCurve = new AnimationCurve();

    private Vector3 openPos = Vector3.zero;
    private Vector3 closedPos = Vector3.zero;
    private DoorState doorState = DoorState.Closed;

    // Start is called before the first frame update
    void Start()
    {
        closedPos = transform.position;
        openPos = closedPos - Vector3.right * Slidingdistance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && doorState != DoorState.Animating)
        {
            StartCoroutine(AnimateDoor(doorState == DoorState.Open ? DoorState.Closed : DoorState.Open));
            /*if (doorState == DoorState.Closed)
            {
                doorState = DoorState.Open;
                Vector3.Lerp(closedPos, openPos, Duration * Time.deltaTime);
            } else
            {
                doorState = DoorState.Closed;
                Vector3.Lerp(openPos, closedPos, Duration * Time.deltaTime);
            }*/
        }
    }

    IEnumerator AnimateDoor(DoorState newState)
    {
        doorState = DoorState.Animating;
        float time = 0.0f;
        Vector3 startPos = (newState == DoorState.Open) ? closedPos : openPos;
        Vector3 endPos = (newState == DoorState.Open) ? openPos : closedPos;
        while(time <= Duration)
        {
            float t = time / Duration;
            transform.position = Vector3.Lerp(startPos, endPos, JumpCurve.Evaluate(t));
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;
        doorState = newState; 
    }
}
