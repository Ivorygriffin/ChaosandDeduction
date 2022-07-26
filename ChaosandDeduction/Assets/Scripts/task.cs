using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class task : MonoBehaviour
{
    public List<string> villagerTasks;
    public List<string> traitorTasks;
    public List<string> traitorSelectedTasks;
    private string randomTask;
    private int currentTaskNum = 0;
    public int numTraitorTasks = 0;

    private bool hasDisplayed;
    private bool hasDisplayedVillager;
    public bool vTasksComplete;
    public bool tTasksComplete;

  
    public void Update()
    {
        ChangeTraitorTaskList();
        convertTraitorToString();
        convertVillagerToString();
    }
    public void ChangeVillagerTaskList()
    {
       
        //if we add more tasks/pcik random ones for villagers
    }

    public void ChangeTraitorTaskList()
    {
        if(currentTaskNum != numTraitorTasks)
        {
            int numTaskSelected = Random.Range(0, traitorTasks.Count);
            randomTask = traitorTasks[numTaskSelected];

            traitorSelectedTasks.Add(randomTask);
            traitorTasks.RemoveAt(numTaskSelected);
            currentTaskNum += 1;
            Debug.Log(currentTaskNum);
            Debug.Log(randomTask);

        }
      
    }
    public void convertTraitorToString()
    {
        if (currentTaskNum == numTraitorTasks && hasDisplayed == false)
        {
            for (int i = 0; i < traitorSelectedTasks.Count; i++)
            {
                string par = traitorSelectedTasks[i];
                UIManager.Instance.traitorCurrentTaskList += (traitorSelectedTasks[i] + "\n" + "\n");
                Debug.Log(UIManager.Instance.traitorCurrentTaskList);
                hasDisplayed = true;
                
            }
        }
    }    
    public void convertVillagerToString()
    {
        if (hasDisplayedVillager == false)
        {
            for (int i = 0; i < villagerTasks.Count; i++)
            {
                string task = villagerTasks[i];
                UIManager.Instance.villagerCurrentTaskList += (villagerTasks[i]+ "\n"+ "\n");
                Debug.Log(UIManager.Instance.villagerCurrentTaskList);
                hasDisplayedVillager = true;

            }
        }
       
    }
}
