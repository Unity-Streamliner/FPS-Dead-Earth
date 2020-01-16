﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;

[CustomEditor(typeof(AIWaypointNetwork))]
public class AIWaypointNetworkEditor : Editor
{

    public override void OnInspectorGUI()
    {
        AIWaypointNetwork network = (AIWaypointNetwork)target;
        network.DisplayMode = (PathDisplayMode)EditorGUILayout.EnumPopup("Display Mode", network.DisplayMode);
        if (network.DisplayMode == PathDisplayMode.Paths)
        {
            network.UIStart = EditorGUILayout.IntSlider("Waypoint Start", network.UIStart, 0, network.Waypoints.Count - 1);
            network.UIEnd = EditorGUILayout.IntSlider("Waypoint End", network.UIEnd, 0, network.Waypoints.Count - 1);
        }
        base.OnInspectorGUI();
    }

    private void OnSceneGUI()
    {
        AIWaypointNetwork network = (AIWaypointNetwork)target;

        for (int i = 0; i < network.Waypoints.Count; i++)
        {
            if (network.Waypoints[i] != null)
                Handles.Label(network.Waypoints[i].position, network.Waypoints[i].name);
        }


        if (network.DisplayMode == PathDisplayMode.Connections)
        {
            ShowConnectionsMode(network);
        } else if (network.DisplayMode == PathDisplayMode.Paths)
        {
            ShowNavMeshPath(network);
        } else
        {

        }
    }

    void ShowConnectionsMode(AIWaypointNetwork network)
    {
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

    void ShowNavMeshPath(AIWaypointNetwork network)
    {
        NavMeshPath path = new NavMeshPath();
        if (network.Waypoints[network.UIStart] == null || network.Waypoints[network.UIEnd] == null) return;
        Vector3 from = network.Waypoints[network.UIStart].position;
        Vector3 to = network.Waypoints[network.UIEnd].position;

        NavMesh.CalculatePath(from, to, NavMesh.AllAreas, path);
        Handles.color = Color.yellow;
        Handles.DrawPolyLine(path.corners);
    }
}
