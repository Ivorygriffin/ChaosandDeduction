using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class task : MonoBehaviour
{

    // STILL NEED TO MAKE ALL OF THIS WORK WITH MULTIPLAYER CONSIDERATIONS


    //string lists

    public List<TaskScriptableObject> potentialVillagerTasks;//use if extra tasks made and we want to randomise
    public List<TaskScriptableObject> villagerTasks;//list of selected tasks for the round
    public List<TaskScriptableObject> uncompleteVillagerTasks; //tasks seen of task screen, whatever isnt complete
    public List<TaskScriptableObject> potentialTraitorTasks; //ones to pick from
    public List<TaskScriptableObject> uncompleteTraitorTasks; //the uncomplete ones
    public List<TaskScriptableObject> selectedTraitorTasks; //the picked ones

    //Strings

    private TaskScriptableObject randomTask;
    private TaskScriptableObject randomVTask;

    //ints

    private int currentTaskNum;
    public int numTraitorTasks;
    private int currentVillagerTaskNum;
    public int numVillagerTasks;
    public int villagerTasksComplete;
    public int traitorTasksComplete;

    //bools

    private bool hasDisplayed;
    private bool hasDisplayedVillager;
    public  bool vTaskComplete, tTaskComplete;
    

 
    public void Update()
    {
        ChangeTraitorTaskList();
        ConvertTraitorToString();
        ChangeVillagerTaskList();
        ConvertVillagerToString();
        CheckTaskComplete();
        ConvertVillagerToString();
        ChangeVillagerTaskList();

        //gotta move outta update to allow for network behaviour
        
    }
    public void ChangeVillagerTaskList()
    {
        if (currentVillagerTaskNum != numVillagerTasks)
        {
            int numTasksSelected = Random.Range(0, potentialVillagerTasks.Count);
            randomVTask = potentialVillagerTasks[numTasksSelected];

            villagerTasks.Add(randomVTask);
            potentialVillagerTasks.RemoveAt(numTasksSelected);
            currentVillagerTaskNum += 1;
            Debug.Log(currentVillagerTaskNum);
            Debug.Log(randomVTask);
            AddVSelectedTaskToUncompleteList();
        }
    }

    public void ChangeTraitorTaskList()
    {
        if(currentTaskNum != numTraitorTasks)
        {
            int numTaskSelected = Random.Range(0, potentialTraitorTasks.Count);
            randomTask = potentialTraitorTasks[numTaskSelected];

            selectedTraitorTasks.Add(randomTask);
            potentialTraitorTasks.RemoveAt(numTaskSelected);
            currentTaskNum += 1;
            Debug.Log(currentTaskNum);
            Debug.Log(randomTask);
            AddSelectedTaskToUncompleteList();
        }
      
    }
    public void AddVSelectedTaskToUncompleteList()//adding 10???
    {

        foreach(TaskScriptableObject listItem in villagerTasks)
        {
            uncompleteVillagerTasks.Add(listItem);
            villagerTasks.Remove(listItem);
        }
    }
    public void AddSelectedTaskToUncompleteList()//adding 10???
    {

        foreach(TaskScriptableObject listItem in selectedTraitorTasks)
        {
            uncompleteTraitorTasks.Add(listItem);
            selectedTraitorTasks.Remove(listItem);
        }
    }
    

    public void ConvertTraitorToString()
    {
        if (currentTaskNum == numTraitorTasks && hasDisplayed == false)
        {
            for (int i = 0; i < uncompleteTraitorTasks.Count; i++)
            {
                string par = uncompleteTraitorTasks[i].description;
                UIManager.Instance.traitorCurrentTaskList += (uncompleteTraitorTasks[i].description + "\n" + "\n");
                Debug.Log(UIManager.Instance.traitorCurrentTaskList);
                hasDisplayed = true;
                
            }
        }
    }    
    public void ConvertVillagerToString()
    {
        if (currentVillagerTaskNum == numVillagerTasks && hasDisplayedVillager == false)
        {
            
            for (int i = 0; i < uncompleteVillagerTasks.Count; i++)
            {
            
                string task = uncompleteVillagerTasks[i].description;
                UIManager.Instance.villagerCurrentTaskList += (uncompleteVillagerTasks[i].description + "\n"+ "\n");
                Debug.Log(UIManager.Instance.villagerCurrentTaskList);

                hasDisplayedVillager = true;
            }
           


        }
       
    }

    public void CheckTaskComplete() //run upon task completion
    {
        uncompleteVillagerTasks.Clear();
        foreach(TaskScriptableObject task in villagerTasks)
        {

            if (task.isComplete != true)
            {
                uncompleteVillagerTasks.Add(task);
                ConvertVillagerToString();
            }
            if(task.isComplete == true)
            {
                villagerTasksComplete += 1;
            }
        }
        uncompleteTraitorTasks.Clear();
        foreach(TaskScriptableObject tasks in uncompleteTraitorTasks)
        {
            if (tasks.isComplete != true)
            {
                uncompleteTraitorTasks.Add(tasks);
                ConvertTraitorToString();
            }
            if(tasks.isComplete == true)
            {
                traitorTasksComplete += 1;
            }
        }
        if(traitorTasksComplete == selectedTraitorTasks.Count)
        {
            tTaskComplete = true;
        }
        if(villagerTasksComplete == villagerTasks.Count)
        {
            vTaskComplete = true;
        }
    }
    
  
}











