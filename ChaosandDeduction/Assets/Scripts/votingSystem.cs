using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class votingSystem : NetworkBehaviour
{
    public static votingSystem instance;
    //bool
    bool voted = false;
    bool finishedVoting = false; //used server only to determine if the voting has been finished

    bool confirmTimer = false; //server only

    //int
    public SyncList<byte> numVoted = new SyncList<byte>();

    public GameObject[] playerIconFrames;
    public GameObject[] playerIconPrefabs;
    //Gameobjects
    int selectedPlayer = -1;
    public GameObject voteObjects;
    //public Timer timer;
    public TMP_Text timerText;

    [SyncVar]
    float voteTimer = 5;
    const float maxVoteTime = 5;
    bool voteUIActive = false;

    private void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        if (isServer)
            numVoted.AddRange(new byte[4]);

        timerText.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    void Update()
    {
        //RpcConfirmVote();

        if (confirmTimer && isServer) //have a delay until tallying the votes
        {
            voteTimer -= Time.deltaTime;
            if (voteTimer <= 0)
                TallyVotes();
        }
        if (voteTimer < maxVoteTime != voteUIActive)
        {
            voteUIActive = voteTimer < maxVoteTime;
            timerText.gameObject.SetActive(voteUIActive);
        }
        timerText.text = ((int)voteTimer).ToString();
    }

    [ClientRpc]
    void RpcResults(short voted, short traitor, short villagerTasks, short villagerTasksDone, short traitorTasks, short traitorTasksDone)
    {
        CustomNetworkManager customNetworkManager = (CustomNetworkManager)NetworkManager.singleton;
        customNetworkManager.AssignResults(voted, traitor, villagerTasks, villagerTasksDone, traitorTasks, traitorTasksDone);
        //PlayerPrefs.SetInt("VotedWitch", voted); //TODO: determine if we should use playerPrefs for this
        //PlayerPrefs.SetInt("TraitorWitch", traitor);

        //PlayerPrefs.SetInt("VillagerTasksDone", villagerTasks);
        //PlayerPrefs.SetInt("VillagerTasks", villagerTasksDone);

        //PlayerPrefs.SetInt("TraitorTasksDone", traitorTasks);
        //PlayerPrefs.SetInt("TraitorTasks", traitorTasksDone);

        UIManager.Instance.WinScreen();
    }

    public void SelectPlayer(int playerIndex)
    {
        //if (PlayerManager.Instance.playersJoined < 4) //if not 4 players, stop
        //    return;

        CmdSelect(selectedPlayer, playerIndex);

        selectedPlayer = playerIndex;
        //TODO: UI stuff to indicate player is selected
    }

    [Command(requiresAuthority = false)]
    public void CmdSelect(int previous, int current)
    {
        //Debug.Log("Client has selected");

        if (previous != -1)
            numVoted[previous]--;
        numVoted[current]++;

        for (int i = 0; i < numVoted.Count; i++)
        {
            if (numVoted[i] >= 3)
            {
                confirmTimer = true;
                return;
            }
        }

        confirmTimer = false;
        voteTimer = maxVoteTime;
    }

    [Server]
    void TallyVotes()
    {
        if (finishedVoting)
            return;

        for (int i = 0; i < numVoted.Count; i++)
        {
            if (numVoted[i] >= 3) //if 3 or more votes on any one player, consider game ended
            {
                finishedVoting = true;
                CustomNetworkManager customNetworkManager = (CustomNetworkManager)NetworkManager.singleton;
                //PlayerManager.Instance.allPlayers[selectedPlayer].GetComponent<CharacterInteraction>().alignment

                short vTasks = (short)(DifficultyManager.instance.GetVTasks());
                short vTasksDone = (short)(TaskManager.instance.CheckVillagerTasks());

                short tTasks = (short)(DifficultyManager.instance.GetTTasks());
                short tTasksDone = (short)(TaskManager.instance.CheckTraitorTasks());

                short traitor = -1;
                for (short j = 0; j < 4; j++)
                    if (customNetworkManager.playerArray[j].alignment == Alignment.Traitor)
                        traitor = customNetworkManager.playerArray[j].modelIndex;

                RpcResults(customNetworkManager.playerArray[i].modelIndex, traitor, vTasks, vTasksDone, tTasks, tTasksDone);

                return; //Halt rest of script (as the rest will handle what happens if draw / spread out votes
            }
        }

        numVoted[0] = 0;
        numVoted[1] = 0;
        numVoted[2] = 0;
        numVoted[3] = 0;
        RpcResetVote();
    }

    [ClientRpc]
    void RpcResetVote()
    {
        voted = false;
        voteObjects.SetActive(true);
        selectedPlayer = -1;
    }

    [Command(requiresAuthority = false)]
    public void CmdGetIcons(NetworkConnectionToClient conn = null)
    {
        CustomNetworkManager customNetworkManager = (CustomNetworkManager)NetworkManager.singleton;

        byte[] data = new byte[customNetworkManager.playerArray.Length]; //using short rather than byte to support -1 as undefined
        for (int i = 0; i < data.Length; i++)
        {
            if (customNetworkManager.playerArray[i].alignment != Alignment.Undefined)
                data[i] = customNetworkManager.playerArray[i].modelIndex;
        }

        TargetGetIcons(conn, data);
    }
    [TargetRpc]
    void TargetGetIcons(NetworkConnection conn, byte[] models)
    {
        for (int i = 0; i < 4; i++)
            Instantiate(playerIconPrefabs[models[i]], playerIconFrames[i].transform).transform.SetSiblingIndex(1); //spawn icon and set as just above picture frame
    }
}
