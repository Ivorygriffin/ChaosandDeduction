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

    public Vector3 currentPoint
    {
        get
        {
            int index = currentIndex;
            if (index == -1)
                return points[0];
            return points[currentIndex];
        }
    }

    public int currentIndex
    {
        get
        {
            for (int i = 0; i < isComplete.Length; i++)
            {
                if (!isComplete[i])
                    return i;
            }
            return -1;
        }
    }
}
