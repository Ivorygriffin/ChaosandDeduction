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
    public Path[] paths;

    [System.Serializable]
    public struct Path
    {
        public Vector3 startPosition;
        public Vector3 endPosition;
    }


    public Path currentPath
    {
        get
        {
            int index = currentIndex;
            if (index == -1)
                return paths[0];
            return paths[currentIndex];
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

#if UNITY_EDITOR
    private void OnValidate()
    {
        //for (int i = 0; i < paths.Length; i++)
        //{
        //    paths[i].startPosition = Vector3.zero;
        //    paths[i].endPosition = Vector3.zero;
        //}

    }
#endif
}
