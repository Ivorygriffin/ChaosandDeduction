using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Task", menuName = "ScriptableObjects/Task", order = 1)]
public class TaskScriptableObject : ScriptableObject
{
    public string description;
    public bool[] isComplete;
    public GameObject tasksticker;

    [Header("Waypoint Stuff")]
    public Vector3 firstPoint;
    public Vector3[] points;
}
