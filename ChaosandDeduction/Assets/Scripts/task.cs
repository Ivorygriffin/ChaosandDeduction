using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class task : MonoBehaviour
{
    public List<string> villagerTasks;
    public List<string> traitorTasks;
    public static string[] selectedTraitorTasks;

    private void Start()
    {
        //UIManager.Instance.traitorCurrentTaskList = selectedTraitorTasks[0];

        ChangeVillagerTaskList();

    }

    public void ChangeVillagerTaskList()
    {
        for (int i = 0; i < 100; i++)
        {
            villagerTasks[i] = string.Format("List string : {0}", i);
            UIManager.Instance.villagerCurrentTaskList = villagerTasks[i];
            Debug.Log(villagerTasks[i]);
        }

    }
    public void ChangeTraitorTaskList()
    {

    }
}
