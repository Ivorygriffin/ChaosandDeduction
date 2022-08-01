using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    //UI Screens
    public GameObject voteScreen;
    public GameObject winScreen;
    public GameObject VillagerTaskScreen;
    public GameObject TraitorTaskScreen;


    //Texts
    public TMP_Text winScreenText;
    public TMP_Text taskScreenText;
    public TMP_Text villagerTaskScreenText;  
    public TMP_Text villagerTaskScreenText2;
    public TMP_Text taskNotificationText;

    //Strings
    public string winner; // this string will change upon voting/or timer end
    public string completedTask;// will change depending on which task a player has completed
    public string villagerCurrentTaskList; 
    public string traitorCurrentTaskList;

    //need variable to differenciate between villager or traitor interaction with UI,if the traitor clicks on the task list then traitor task list will open
    //for the sake of getting rid of compile errors variable
    public bool villager;
    public bool traitor;
    
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Update()
    {
        TraitorTaskListText();
        VillagerTaskListText();
    }


    public void Voting()
    {
        voteScreen.SetActive(true);
    }
    public void WinScreen()
    {
        winScreen.SetActive(true);
    }
    public void WinScreenText()
    {
        winScreenText.text = winner; 
    }

    public void TaskNotifications()
    {
        taskNotificationText.text = completedTask;
    }
    public void OpenTaskList()
    {
        if (villager)
        {
            VillagerTaskScreen.SetActive(true);
        }
        if (traitor)
        {
            TraitorTaskScreen.SetActive(true);
        }

    }
    public void CloseTaskList()
    {
        if (villager)
        {
            VillagerTaskScreen.SetActive(false);
        }
        if (traitor)
        {
            TraitorTaskScreen.SetActive(false);
        }
    }
    public void TraitorTaskListText()
    {
        taskScreenText.text = traitorCurrentTaskList;
    }  
    public void VillagerTaskListText()
    {
        villagerTaskScreenText.text = villagerCurrentTaskList;
        villagerTaskScreenText2.text = villagerCurrentTaskList;
        

    }

}
