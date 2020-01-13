using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AIWaypointNetwork))]
public class AIWaypointNetworkEditor : Editor
{
    private void OnSceneGUI()
    {
        AIWaypointNetwork network = (AIWaypointNetwork)target;

        for (int i = 0; i < network.Waypoints.Count; i++)
        {
            if (network.Waypoints[i] != null)
                Handles.Label(network.Waypoints[i].position, network.Waypoints[i].name);
        }

        Vector3[] linePoints = new Vector3[network.Waypoints.Count + 1];

        for (int i = 0; i <= network.Waypoints.Count; i++)
        {
            int index = i != network.Waypoints.Count ? i : 0;
            if (network.Waypoints[index] != null) linePoints[i] = network.Waypoints[index].position;
            else linePoints[i] = new Vector3(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity);
        }
        Handles.color = Color.cyan;
        Handles.DrawAAPolyLine(linePoints);
    }
}
