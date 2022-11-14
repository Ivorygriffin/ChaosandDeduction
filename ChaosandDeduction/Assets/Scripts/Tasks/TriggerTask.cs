using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTask : MonoBehaviour
{
    public TaskScriptableObject task;
    public int taskStage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && task == pinManager.instance.pinnedTask)
            pinManager.instance.pinnedTask.isComplete[taskStage] = true;
    }

    private void OnValidate()
    {
        if (task)
            task.paths[taskStage].endPosition = transform.position;
    }
}
