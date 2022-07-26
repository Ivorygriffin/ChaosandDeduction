using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Mirror;
public class TaskManager : NetworkBehaviour
{

    // STILL NEED TO MAKE ALL OF THIS WORK WITH MULTIPLAYER CONSIDERATIONS


    public static TaskManager instance;

    //string lists

    public List<TaskScriptableObject> potentialVillagerTasks;//use if extra tasks made and we want to randomise
                                                             // public List<TaskScriptableObject> uncompleteVillagerTasks; //tasks seen of task screen, whatever isnt complete
    public List<TaskScriptableObject> villagerTasks;//list of selected tasks for the round

    public List<TaskScriptableObject> potentialTraitorTasks; //ones to pick from
                                                             // public List<TaskScriptableObject> uncompleteTraitorTasks; //the uncomplete ones
    public List<TaskScriptableObject> traitorTasks; //the picked ones

    //Strings

    private TaskScriptableObject randomTask;
    private TaskScriptableObject randomVTask;

    //ints

    [Header("Traitor Counts")]
    private int currentTraitorTaskNum;

    [Header("Villager Counts")]
    private int currentVillagerTaskNum;

    //bools

    private bool hasDisplayedTraitor;
    private bool hasDisplayedVillager;
    public bool vTaskComplete, tTaskComplete;

    public UnityEvent TaskUpdate;

    bool initialised = false;

    public void Start()
    {
        if (isServer)
        {
            if (!instance)
                instance = this;
            else
                Destroy(this);

            int numVillagerTasks = DifficultyManager.instance.GetVTasks();
            int numTraitorTasks = DifficultyManager.instance.GetTTasks();

            for (int i = 0; i < numVillagerTasks; i++)
                AddVillagerTask();
            for (int i = 0; i < numTraitorTasks; i++)
                AddTraitorTask();
        }

    }

    public void Update()
    {
        if (!isServer)
            return;
        if (UIManager.Instance && !initialised) //just run this whenever the instance is set up (only runs once)
        {
            initialised = true;
            //UpdateTraitorString();
            //UpdateVillagerString();
            CheckTaskComplete();
        }
        //ConvertTraitorToString();
        //ConvertVillagerToString();

        //gotta move outta update to allow for network behaviour

    }
    public void AddVillagerTask() //Adds one random task from potential task list to the todo task list
    {
        if (potentialVillagerTasks.Count == 0)
            return; //if no tasks left, ignore

        int numTasksSelected = Random.Range(0, potentialVillagerTasks.Count);
        TaskScriptableObject temp = potentialVillagerTasks[numTasksSelected];
        for (int i = 0; i < temp.isComplete.Length; i++) //reset stage completions
            temp.isComplete[i] = false;

        villagerTasks.Add(temp);
        potentialVillagerTasks.RemoveAt(numTasksSelected);
        currentVillagerTaskNum += 1;

        //AddVSelectedTaskToUncompleteList();
    }

    public void AddTraitorTask() //Adds one random task from potential task list to the todo task list
    {
        if (potentialTraitorTasks.Count == 0)
            return; //if no tasks left, ignore

        int numTaskSelected = Random.Range(0, potentialTraitorTasks.Count);
        TaskScriptableObject temp = potentialTraitorTasks[numTaskSelected];
        for (int i = 0; i < temp.isComplete.Length; i++) //reset stage completions
            temp.isComplete[i] = false;

        traitorTasks.Add(temp);
        potentialTraitorTasks.RemoveAt(numTaskSelected);
        currentTraitorTaskNum += 1;

        //AddSelectedTaskToUncompleteList();
    }

    //public void UpdateTraitorString()
    //{
    //    if (!hasDisplayedTraitor)
    //    {
    //        UIManager.Instance.traitorCurrentTaskList = ""; //clear the UI text

    //        for (int i = 0; i < traitorTasks.Count; i++) //Iterate through all current tasks and add them as a string to the UI
    //        {
    //            string par = traitorTasks[i].description;
    //            UIManager.Instance.traitorCurrentTaskList += (traitorTasks[i].description + "\n" + "\n");
    //            Debug.Log(UIManager.Instance.traitorCurrentTaskList);

    //            //hasDisplayedTraitor = true;
    //        }
    //    }
    //}
    //public void UpdateVillagerString()
    //{
    //    if (!hasDisplayedVillager)
    //    {
    //        UIManager.Instance.villagerCurrentTaskList = "";  //clear the UI text

    //        for (int i = 0; i < villagerTasks.Count; i++) //Iterate through all current tasks and add them as a string to the UI
    //        {
    //            string task = villagerTasks[i].description;
    //            UIManager.Instance.villagerCurrentTaskList += (villagerTasks[i].description + "\n" + "\n");
    //            Debug.Log(UIManager.Instance.villagerCurrentTaskList);

    //            //hasDisplayedVillager = true;
    //        }
    //    }
    //}

    //Should only get called when a task is completed TODO: ensure this is correct by checking all tasks
    public int CheckTraitorTasks()
    {
        int traitorTasksComplete = 0;
        for (int i = 0; i < traitorTasks.Count; i++)
        {

            if (traitorTasks[i].isComplete[traitorTasks[i].isComplete.Length - 1])
            {
                //traitorTasks.RemoveAt(i);
                //i--;
                traitorTasksComplete++;
                //UpdateTraitorString();
            }
        }
        return traitorTasksComplete;
    }
    public int CheckVillagerTasks()
    {
        int villagerTasksComplete = 0;
        for (int i = 0; i < villagerTasks.Count; i++)
        {
            if (villagerTasks[i].isComplete[villagerTasks[i].isComplete.Length - 1])
            {
                //villagerTasks.RemoveAt(i);
                //i--;
                villagerTasksComplete++;
                //UpdateVillagerString();
            }
        }
        return villagerTasksComplete;
    }
    public void CheckTaskComplete()
    {
        if (traitorTasks.Count <= CheckTraitorTasks())
            tTaskComplete = true;
        if (villagerTasks.Count <= CheckVillagerTasks())
            vTaskComplete = true;

        TaskUpdate.Invoke();
    }


    public bool RequiredTask(TaskScriptableObject task)
    {
        return villagerTasks.Contains(task) || traitorTasks.Contains(task);
    }
}