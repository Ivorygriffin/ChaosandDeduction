using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class task : MonoBehaviour
{

    // STILL NEED TO MAKE ALL OF THIS WORK WITH MULTIPLAYER CONSIDERATIONS


    //string lists

    public List<TaskScriptableObject> potentialVillagerTasks;//use if extra tasks made and we want to randomise
    public List<TaskScriptableObject> uncompleteVillagerTasks; //tasks seen of task screen, whatever isnt complete
    public List<TaskScriptableObject> villagerTasks;//list of selected tasks for the round

    public List<TaskScriptableObject> potentialTraitorTasks; //ones to pick from
    public List<TaskScriptableObject> uncompleteTraitorTasks; //the uncomplete ones
    public List<TaskScriptableObject> traitorTasks; //the picked ones

    //Strings

    private TaskScriptableObject randomTask;
    private TaskScriptableObject randomVTask;

    //ints

    [Header("Traitor Counts")]
    private int currentTraitorTaskNum;
    public int numTraitorTasks;
    public int traitorTasksComplete;

    [Header("Villager Counts")]
    private int currentVillagerTaskNum;
    public int numVillagerTasks;
    public int villagerTasksComplete;

    //bools

    private bool hasDisplayedTraitor;
    private bool hasDisplayedVillager;
    public bool vTaskComplete, tTaskComplete;

    public void Start()
    {
        for (int i = 0; i < numVillagerTasks; i++)
            AddVillagerTask();
        for (int i = 0; i < numTraitorTasks; i++)
            AddTraitorTask();
    }

    public void Update()
    {
        //ConvertTraitorToString();
        //ConvertVillagerToString();

        CheckTaskComplete();

        //gotta move outta update to allow for network behaviour

    }
    public void AddVillagerTask() //Adds one random task from potential task list to the todo task list
    {
        int numTasksSelected = Random.Range(0, potentialVillagerTasks.Count);
        TaskScriptableObject temp = potentialVillagerTasks[numTasksSelected];
        temp.isComplete = false;

        villagerTasks.Add(temp);
        potentialVillagerTasks.RemoveAt(numTasksSelected);
        currentVillagerTaskNum += 1;

        //AddVSelectedTaskToUncompleteList();
    }

    public void AddTraitorTask() //Adds one random task from potential task list to the todo task list
    {
        int numTaskSelected = Random.Range(0, potentialTraitorTasks.Count);
        TaskScriptableObject temp = potentialTraitorTasks[numTaskSelected];
        temp.isComplete = false;

        traitorTasks.Add(temp);
        potentialTraitorTasks.RemoveAt(numTaskSelected);
        currentTraitorTaskNum += 1;

        //AddSelectedTaskToUncompleteList();
    }

    public void ConvertTraitorToString()
    {
        if (!hasDisplayedTraitor)
        {
            for (int i = 0; i < traitorTasks.Count; i++)
            {
                string par = traitorTasks[i].description;
                UIManager.Instance.traitorCurrentTaskList += (traitorTasks[i].description + "\n" + "\n");
                Debug.Log(UIManager.Instance.traitorCurrentTaskList);

                //hasDisplayedTraitor = true;
            }
        }
    }
    public void ConvertVillagerToString()
    {
        if (!hasDisplayedVillager)
        {
            for (int i = 0; i < villagerTasks.Count; i++)
            {
                string task = villagerTasks[i].description;
                UIManager.Instance.villagerCurrentTaskList += (villagerTasks[i].description + "\n" + "\n");
                Debug.Log(UIManager.Instance.villagerCurrentTaskList);

                //hasDisplayedVillager = true;
            }
        }
    }

    public void CheckTaskComplete() //run upon task completion
    {
        //uncompleteVillagerTasks.Clear();
        //foreach (TaskScriptableObject task in villagerTasks)
        //{

        //    if (task.isComplete != true)
        //    {
        //        uncompleteVillagerTasks.Add(task);
        //        ConvertVillagerToString();
        //    }
        //    if (task.isComplete == true)
        //    {
        //        villagerTasksComplete += 1;
        //    }
        //}
        for (int i = 0; i < villagerTasks.Count; i++)
        {

            if (villagerTasks[i].isComplete)
            {
                villagerTasks.RemoveAt(i);
                i--;
                villagerTasksComplete += 1;
                ConvertVillagerToString();
            }
            else
            {
                //uncompleteVillagerTasks.Add(villagerTasks[i]);
            }
        }
        //uncompleteTraitorTasks.Clear();
        //foreach (TaskScriptableObject tasks in uncompleteTraitorTasks)
        //{
        //    if (tasks.isComplete != true)
        //    {
        //        uncompleteTraitorTasks.Add(tasks);
        //        ConvertTraitorToString();
        //    }
        //    if (tasks.isComplete == true)
        //    {
        //        traitorTasksComplete += 1;
        //    }
        //}

        for (int i = 0; i < traitorTasks.Count; i++)
        {

            if (traitorTasks[i].isComplete)
            {
                traitorTasks.RemoveAt(i);
                i--;
                traitorTasksComplete += 1;
                ConvertTraitorToString();
            }
            else
            {
                //uncompleteVillagerTasks.Add(villagerTasks[i]);
            }
        }





        if (traitorTasksComplete == traitorTasks.Count)
        {
            tTaskComplete = true;
        }
        if (villagerTasksComplete == villagerTasks.Count)
        {
            vTaskComplete = true;
        }
    }


}











