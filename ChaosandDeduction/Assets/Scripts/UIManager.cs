using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class UIManager : NetworkBehaviour
{
    public static UIManager Instance;

    //UI Screens
    public GameObject voteScreen;
    public GameObject winScreen;
    public GameObject VillagerTaskScreen;
    public GameObject TraitorTaskScreen;
    public GameObject InitialMap;
    public GameObject InteractCanvas;


    //Texts
    public TMP_Text winScreenText;
    //public TMP_Text taskScreenText;
    //public TMP_Text villagerTaskScreenText;  
    //public TMP_Text villagerTaskScreenText2;
    public TMP_Text taskNotificationText;

    //Strings
    //public string winner; // this string will change upon voting/or timer end
    public string completedTask;// will change depending on which task a player has completed
    [TextArea]
    [SyncVar]
    public string villagerCurrentTaskList;
    [TextArea]
    [SyncVar]
    public string traitorCurrentTaskList;

    //need variable to differenciate between villager or traitor interaction with UI,if the traitor clicks on the task list then traitor task list will open
    //for the sake of getting rid of compile errors variable
    //public bool villager;
    //public bool traitor;
    
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Update()
    {
        //TraitorTaskListText();
        //VillagerTaskListText();
    }

    [Command(requiresAuthority = false)]
    public void CmdVoting(bool active)
    {
        RpcVoting(active);
        
    }
    [ClientRpc]
    void RpcVoting(bool active)
    {
        voteScreen.SetActive(active);
        InteractCanvas.SetActive(!active);
    }
    
    public void WinScreen()
    {
        winScreen.SetActive(true);
        InteractCanvas.SetActive(false);
    }

    public void ShowMap()
    {
        InitialMap.SetActive(true);
        InteractCanvas.SetActive(false);
    }

    public void TaskNotifications()
    {
        taskNotificationText.text = completedTask;

    }
    public void OpenTaskList()
    {
        if (PlayerManager.Instance.localPlayerData.alignment == Alignment.Villager)
        {
            VillagerTaskScreen.SetActive(true);
            InteractCanvas.SetActive(false);
        }
        if (PlayerManager.Instance.localPlayerData.alignment == Alignment.Traitor)
        {
            TraitorTaskScreen.SetActive(true);
            InteractCanvas.SetActive(false);
        }

    }
    public void CloseTaskList()
    {
        if (PlayerManager.Instance.localPlayerData.alignment == Alignment.Villager)
        {
            VillagerTaskScreen.SetActive(false);
            InteractCanvas.SetActive(true);
        }
        if (PlayerManager.Instance.localPlayerData.alignment == Alignment.Traitor)
        {
            TraitorTaskScreen.SetActive(false);
            InteractCanvas.SetActive(true);
        }
    }
    //public void TraitorTaskListText()
    //{
    //    //taskScreenText.text = traitorCurrentTaskList;
    //}  
    //public void VillagerTaskListText()
    //{
    //    //villagerTaskScreenText.text = villagerCurrentTaskList;
    //    //villagerTaskScreenText2.text = villagerCurrentTaskList;
        

    //}

}
