using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class UIManager : NetworkBehaviour
{
    public static UIManager Instance;

    //UI Screens
    [Header("Canvases")]
    public Canvas voteScreen;
    public Canvas winScreen;
    public Canvas settingsCanvas;
    public Canvas taskCanvas;
    public GameObject VillagerTaskScreen;
    public GameObject TraitorTaskScreen;
    public GameObject TraitorTaskTab;
    public Canvas InitialMap;
    public Canvas InteractCanvas;

    [Header("Role Stuff")]
    public Canvas roleRevealCanvas;
    public TMP_Text roleText, roleText2;
    float roleTimer = 1;
    const float roleMaxTime = 5;

    float replayTimer = -1;
    const float replayMaxTime = 15;

    [Header("FPS Stuff")]
    public GameObject fireButton;
    public GameObject spellBook;

    [SerializeField] GameObject[] wandProgress;
    int wandProgressIndex;

    //Texts
    public TMP_Text winScreenText;
    //public TMP_Text taskScreenText;
    //public TMP_Text villagerTaskScreenText;  
    //public TMP_Text villagerTaskScreenText2;
    public TMP_Text taskNotificationText;

    //Strings
    //public string winner; // this string will change upon voting/or timer end
    //public string completedTask;// will change depending on which task a player has completed

    bool initalised = false;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;

            voteScreen.enabled = false;
            winScreen.enabled = false;
            settingsCanvas.enabled = false;
            taskCanvas.enabled = false;
            InitialMap.enabled = false;
            InteractCanvas.enabled = false;

            roleRevealCanvas.enabled = false;
            //fireButton.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }


        ResetWandProgress();
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
                    roleText.text = "You are the traitor";
                    roleText2.text = "LIE \n MISLEAD \n BLAME \n do your tasks without being detected";
                    break;
                case Alignment.Villager:
                    TraitorTaskTab.SetActive(false);
                    VillagerTaskScreen.SetActive(true);
                    TraitorTaskScreen.SetActive(false);
                    roleText.text = "You are a villager";
                    roleText2.text = "COMMUNICATE\n WORK TOGETHER\n AVOID BEING FRAMED \n complete your tasks";
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
        votingSystem.instance.CmdGetIcons();
        InteractCanvas.enabled = !active;
    }

    public void WinScreen()
    {
        //winScreen.enabled = true;
        //voteScreen.enabled = false;
        //InteractCanvas.enabled = false;

        //TODO: add loading screen
        NetworkManager.singleton.ServerChangeScene("EndingCutscene");
    }

    public void ThirdPerson(bool show)
    {
        fireButton.SetActive(!show);
        spellBook.SetActive(!show);
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
        //taskNotificationText.text = completedTask;

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


    public void SetWandProgress(int index)
    {
        for (int i = 0; i < wandProgress.Length; i++)
            wandProgress[i].SetActive(i < index);

        StopCoroutine(delayResetWand());
        if (index == 2) //if its completed progress, wait set amount then reset
            StartCoroutine(delayResetWand());
    }

    [ContextMenu("Reset Wand")]
    public void ResetWandProgress()
    {
        for (int i = 0; i < wandProgress.Length; i++)
            wandProgress[i].SetActive(false);

        wandProgressIndex = 0;
    }
    IEnumerator delayResetWand()
    {
        yield return new WaitForSeconds(2);
        ResetWandProgress();
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
