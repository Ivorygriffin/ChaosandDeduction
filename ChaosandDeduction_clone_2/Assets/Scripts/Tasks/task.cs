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

    //ints

    private int currentTaskNum = 0;
    public int numTraitorTasks = 0;
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
        //gotta move outta update to allow for network behaviour
        
    }
    public void ChangeVillagerTaskList()
    {
  
        //if villager tasks random feature
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
        if (hasDisplayedVillager == false)
        {
            for (int i = 0; i < uncompleteVillagerTasks.Count; i++)
            {
                string task = uncompleteVillagerTasks[i].description;
                UIManager.Instance.villagerCurrentTaskList += (uncompleteVillagerTasks[i].description + "\n"+ "\n");
                Debug.Log(UIManager.Instance.villagerCurrentTaskList);
                hasDisplayedVillager = true;
                //will happen everytime task complete is checked
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




















//switch (GetComponent<VillagerTaskLabel>().taskType) 
//{
//    //when item with an emun enters a trigger it will call this function, function will detect which enum assigned and will change the task list and number of tasks completed
//    case VillagerTaskType.Tree:
//        villagerTasksComplete += 1;
//        selectedVillagerTasks.RemoveAt(0);
//        UIManager.Instance.villagerCurrentTaskList = string.Empty;
//        hasDisplayedVillager = false;
//        ConvertVillagerToString();
//        Debug.Log("Tree Task Complete");
//        break; 
//    case VillagerTaskType.Cave:
//        villagerTasksComplete += 1;
//        selectedVillagerTasks.RemoveAt(2);
//        UIManager.Instance.villagerCurrentTaskList = string.Empty;
//        hasDisplayedVillager = false;
//        ConvertVillagerToString();
//        Debug.Log("Cave Task Complete");
//        break; 
//    case VillagerTaskType.Well:
//        villagerTasksComplete += 1;
//        selectedVillagerTasks.RemoveAt(3);
//        UIManager.Instance.villagerCurrentTaskList = string.Empty;
//        hasDisplayedVillager = false;
//        ConvertVillagerToString();
//        Debug.Log("Well Task Complete");
//        break; 
//    case VillagerTaskType.Camp:
//        villagerTasksComplete += 1;
//        selectedVillagerTasks.RemoveAt(4);
//        UIManager.Instance.villagerCurrentTaskList = string.Empty;
//        hasDisplayedVillager = false;
//        ConvertVillagerToString();
//        Debug.Log("Camp Task Complete");
//        break; 
//    case VillagerTaskType.Library:
//        villagerTasksComplete += 1;
//        selectedVillagerTasks.RemoveAt(5);
//        UIManager.Instance.villagerCurrentTaskList = string.Empty;
//        hasDisplayedVillager = false;
//        ConvertVillagerToString();
//        Debug.Log("Library Task Complete");
//        break; 
//    case VillagerTaskType.School:
//        villagerTasksComplete += 1;
//        selectedVillagerTasks.RemoveAt(6);
//        UIManager.Instance.villagerCurrentTaskList = string.Empty;
//        hasDisplayedVillager = false;
//        ConvertVillagerToString();
//        Debug.Log("School Task Complete");
//        break; 
//    case VillagerTaskType.Foodspot:
//        villagerTasksComplete += 1;
//        selectedVillagerTasks.RemoveAt(7);
//        UIManager.Instance.villagerCurrentTaskList = string.Empty;
//        hasDisplayedVillager = false;
//        ConvertVillagerToString();
//        Debug.Log("Foodspot Task Complete");
//        break; 
//    case VillagerTaskType.Packaging:
//        villagerTasksComplete += 1;
//        selectedVillagerTasks.RemoveAt(8);
//        UIManager.Instance.villagerCurrentTaskList = string.Empty;
//        hasDisplayedVillager = false;
//        ConvertVillagerToString();
//        Debug.Log("Packaging Task Complete");
//        break; 
//    case VillagerTaskType.Graveyard:
//        villagerTasksComplete += 1;
//        selectedVillagerTasks.RemoveAt(9);
//        UIManager.Instance.villagerCurrentTaskList = string.Empty;
//        hasDisplayedVillager = false;
//        ConvertVillagerToString();
//        Debug.Log("Graveyard Task Complete");
//        break; 
//    case VillagerTaskType.Statue:
//        villagerTasksComplete += 1;
//        selectedVillagerTasks.RemoveAt(10);
//        UIManager.Instance.villagerCurrentTaskList = string.Empty;
//        hasDisplayedVillager = false;
//        ConvertVillagerToString();
//        Debug.Log("Statue Task Complete");
//        break;

//    default:

//        break;



//}

//int numTreeSelected = Random.Range(0, treeVillagerTasks.Count);
//ranTree = treeVillagerTasks[numTreeSelected];
//selectedVillagerTasks.Add(ranTree);

//int numCaveSelected = Random.Range(0, caveVillagerTasks.Count);
//ranCave = caveVillagerTasks[numCaveSelected];
//selectedVillagerTasks.Add(ranCave);

//int numWellSelected = Random.Range(0, wellVillagerTasks.Count);
//ranWell = wellVillagerTasks[numWellSelected];
//selectedVillagerTasks.Add(ranWell);

//int numCampSelected = Random.Range(0, campVillagerTasks.Count);
//ranCamp = campVillagerTasks[numCampSelected];
//selectedVillagerTasks.Add(ranCamp);

//int numLibrarySelected = Random.Range(0, libraryVillagerTasks.Count);
//ranLibrary = libraryVillagerTasks[numLibrarySelected];
//selectedVillagerTasks.Add(ranLibrary);

//int numSchoolSelected = Random.Range(0, schoolVillagerTasks.Count);
//ranSchool = schoolVillagerTasks[numSchoolSelected];
//selectedVillagerTasks.Add(ranSchool);

//int numFoodspotSelected = Random.Range(0, foodspotVillagerTasks.Count);
//ranFoodspot = foodspotVillagerTasks[numFoodspotSelected];
//selectedVillagerTasks.Add(ranFoodspot);

//int numPackagingSelected = Random.Range(0, packagingVillagerTasks.Count);
//ranPackaging = packagingVillagerTasks[numPackagingSelected];
//selectedVillagerTasks.Add(ranPackaging);

//int numGraveyardSelected = Random.Range(0, graveyardVillagerTasks.Count);
//ranGraveyard = graveyardVillagerTasks[numGraveyardSelected];
//selectedVillagerTasks.Add(ranGraveyard);

//int numStatueSelected = Random.Range(0, statueVillagerTasks.Count);
//ranStatue = statueVillagerTasks[numStatueSelected];
//selectedVillagerTasks.Add(ranStatue);