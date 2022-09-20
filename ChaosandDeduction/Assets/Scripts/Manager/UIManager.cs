using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class UIManager : NetworkBehaviour
{
    public static UIManager Instance;

    //UI Screens
    public Canvas voteScreen;
    public Canvas winScreen;
    public Canvas settingsCanvas;
    public Canvas taskCanvas;
    public GameObject VillagerTaskScreen;
    public GameObject TraitorTaskScreen;
    public GameObject TraitorTaskTab;
    public Canvas InitialMap;
    public Canvas InteractCanvas;

    public Canvas roleRevealCanvas;
    public TMP_Text roleText;
    float roleTimer = 1;
    const float roleMaxTime = 5;

    float replayTimer = -1;
    const float replayMaxTime = 15;

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

    bool initalised = false;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);


            voteScreen.enabled = false;
            winScreen.enabled = false;
            settingsCanvas.enabled = false;
            taskCanvas.enabled = false;
            InitialMap.enabled = false;
            InteractCanvas.enabled = false;

            roleRevealCanvas.enabled = false;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (!initalised && PlayerManager.Instance && PlayerManager.Instance.localPlayerData.alignment != Alignment.Undefined)
        {
            roleRevealCanvas.enabled = true;
            switch (PlayerManager.Instance.localPlayerData.alignment)
            {
                case Alignment.Traitor:
                    TraitorTaskTab.SetActive(true);
                    VillagerTaskScreen.SetActive(false);
                    TraitorTaskScreen.SetActive(true);
                    roleText.text = "You are the traitor!";
                    break;
                case Alignment.Villager:
                    TraitorTaskTab.SetActive(false);
                    VillagerTaskScreen.SetActive(true);
                    TraitorTaskScreen.SetActive(false);
                    roleText.text = "You are a villager.";
                    break;

                default:
                    roleText.text = "Broken: localPlayerData not assigned properly";
                    break;
            }

            initalised = true;
            roleTimer = roleMaxTime;
        }

        if (roleTimer > 0)
        {
            roleTimer -= Time.deltaTime;
            if (roleTimer <= 0)
            {
                roleRevealCanvas.enabled = false;
                InteractCanvas.enabled = true;
            }
        }

        if (replayTimer > 0)
        {
            replayTimer -= Time.deltaTime;
            if (replayTimer <= 0)
            {
                NetworkManager.singleton.ServerChangeScene("Lobby");
            }
        }
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
        voteScreen.enabled = active;
        InteractCanvas.enabled = !active;
    }

    public void WinScreen()
    {
        winScreen.enabled = true;
        voteScreen.enabled = false;
        InteractCanvas.enabled = false;

        replayTimer = replayMaxTime;
    }

    public void Map(bool show)
    {
        InitialMap.enabled = show;
        InteractCanvas.enabled = !show;
    }

    public void Settings(bool show)
    {
        settingsCanvas.enabled = show;
        InteractCanvas.enabled = !show;
    }

    public void TaskNotifications()
    {
        taskNotificationText.text = completedTask;

    }
    public void TaskList(bool show)
    {
        taskCanvas.enabled = show;
        InteractCanvas.enabled = !show;
    }
    public void SwitchList(bool villager)
    {
        VillagerTaskScreen.SetActive(villager);
        TraitorTaskScreen.SetActive(!villager);
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
