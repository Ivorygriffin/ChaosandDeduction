using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class votingSystem : NetworkBehaviour
{
    //bool
    bool voted = false;
    bool finishedVoting = false; //used server only to determine if the voting has been finished

    bool confirmTimer = false; //server only

    //int
    public SyncList<byte> numVoted = new SyncList<byte>();

    //Gameobjects
    int selectedPlayer = -1;
    public GameObject voteObjects;
    //public Timer timer;
    public TMP_Text timerText;

    [SyncVar]
    float voteTimer = 5;
    const float maxVoteTime = 5;

    private void Start()
    {
        if (isServer)
            numVoted.AddRange(new byte[4]);
    }

    void Update()
    {
        //RpcConfirmVote();

        if (confirmTimer && isServer) //have a delay until tallying the votes
        {
            voteTimer -= Time.deltaTime;
            if (voteTimer < 0)
                TallyVotes();
        }

        timerText.text = voteTimer.ToString();
    }

    [ClientRpc]
    void RpcResults(bool traitor, bool vTaskComp, bool tTaskComp)
    {
        if (traitor && vTaskComp) //if traitor is found and, either all villager tasks are complete or the timer hasnt run out, villagers win
        {
            UIManager.Instance.winScreenText.text = "The Witches Win";
            //UIManager.Instance.WinScreen();
        }
        else if (!traitor && tTaskComp) //if traitor is found and, either all villager tasks are complete or the timer hasnt run out, villagers win
        {
            UIManager.Instance.winScreenText.text = "The Traitor Wins";
            //UIManager.Instance.WinScreen();
        }
        else
        {
            UIManager.Instance.winScreenText.text = "The Game Has Beaten you all";
            //UIManager.Instance.WinScreen();
        }
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

        int votes = 0;
        for (int i = 0; i < numVoted.Count; i++)
        {
            votes += numVoted[i];
        }

        if (votes < 4) //if not enough votes yet
            return;

        for (int i = 0; i < numVoted.Count; i++)
        {
            if (numVoted[i] >= 3) //if 3 or more votes on any one player, consider game ended
            {
                finishedVoting = true;
                CustomNetworkManager customNetworkManager = (CustomNetworkManager)NetworkManager.singleton;
                //PlayerManager.Instance.allPlayers[selectedPlayer].GetComponent<CharacterInteraction>().alignment

                switch (customNetworkManager.playerArray[i].alignment)
                {
                    case Alignment.Villager:
                        //lost
                        RpcResults(false, TaskManager.instance.vTaskComplete, TaskManager.instance.tTaskComplete);
                        break;
                    case Alignment.Traitor:
                        //win
                        RpcResults(true, TaskManager.instance.vTaskComplete, TaskManager.instance.tTaskComplete);
                        break;
                    default:
                        //RpcResults(false);
                        break;
                }
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
}
