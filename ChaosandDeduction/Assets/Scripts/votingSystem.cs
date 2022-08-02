using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class votingSystem : NetworkBehaviour
{
    //bool
    [SyncVar(hook = "Results")]
    bool traitorDetected;
    bool voted = false;


    //int

    public byte[] numVoted = new byte[4];

    //Gameobjects
    int selectedPlayer = -1;
    public GameObject voteObjects;
    public task taskManager;
    public Timer timer;

    [ServerCallback]
    void Update()
    {
        //RpcConfirmVote();
    }


    void Results(bool oldVal, bool newVal)
    {
        traitorDetected = newVal;
        if (traitorDetected && ((taskManager.vTaskComplete == true) || timer.timeRemaining > 0)) //if traitor is found and, either all villager tasks are complete or the timer hasnt run out, villagers win
        {
            UIManager.Instance.winScreenText.text = "The Villagers Win";
            UIManager.Instance.WinScreen();
        }
        else if (!traitorDetected && ((taskManager.tTaskComplete == true) || timer.timeRemaining > 0)) //if traitor is found and, either all villager tasks are complete or the timer hasnt run out, villagers win
        {
            UIManager.Instance.winScreenText.text = "The Traitor Wins";
            UIManager.Instance.WinScreen();
        }
        else
        {
            UIManager.Instance.winScreenText.text = "The Game Has Beaten you all";
            UIManager.Instance.WinScreen();
        }
        UIManager.Instance.CmdVoting(false);
    }

    public void SelectPlayer(int playerIndex)
    {
        if (PlayerManager.Instance.playersJoined < 4) //if not 4 players, stop
            return;

        Debug.Log("selected");
        selectedPlayer = playerIndex;
        //TODO: UI stuff to indicate player is selected
    }


    public void ConfirmPlayerVote()
    {
        if (voted || selectedPlayer == -1)
            return;

        voted = true;
        voteObjects.SetActive(false);
        CmdConfirmPlayerVote(selectedPlayer);
    }

    [Command(requiresAuthority = false)]
    public void CmdConfirmPlayerVote(int select) //TODO: add effects to indicate a player has locked in, to other players
    {
        if (select == -1)
            return;

        numVoted[select]++;

        TallyVotes(); //check if voting is complete
                      //TODO: handle spread out votes (I.E. no definitive target of the vote)


    }

    [Server]
    void TallyVotes()
    {
        int votes = 0;
        for (int i = 0; i < numVoted.Length; i++)
        {
            votes += numVoted[i];
        }

        if (votes < 4) //if not enough votes yet
            return;

        for (int i = 0; i < numVoted.Length; i++)
        {
            if (numVoted[i] >= 3) //if 3 or more votes on any one player, consider game ended
            {
                switch (PlayerManager.Instance.allPlayers[selectedPlayer].GetComponent<CharacterInteraction>().alignment)
                {
                    case Alignment.Villager:
                        //lost
                        traitorDetected = false;
                        break;
                    case Alignment.Traitor:
                        //win
                        traitorDetected = true;
                        break;
                }
                return; //Halt rest of script (as the rest will handle what happens if draw / spread out votes
            }
        }

        RpcResetVote();
    }

    [ClientRpc]
    void RpcResetVote()
    {
        voted = false;
        voteObjects.SetActive(false);
        selectedPlayer = -1;
    }
}
