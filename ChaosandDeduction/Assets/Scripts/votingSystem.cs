using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class votingSystem : NetworkBehaviour
{
    //bool 
    [SyncVar]
    bool traitorDetected;
    bool voted;


    //int

    byte[] numVoted = new byte[4];

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


    [ClientRpc]
    void RpcResults()
    {
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
        UIManager.Instance.RpcVoting(false);
    }

    public void SelectPlayer(int playerIndex)
    {
        if (PlayerManager.Instance.allPlayers.Count < 4) //if not 4 players, stop
            return;

        Debug.Log("selected");
        selectedPlayer = playerIndex;
        //TODO: UI stuff to indicate player is selected
    }


    public void ConfirmPlayerVote()
    {
        if (voted)
            return;

        voted = true;
        voteObjects.SetActive(false);
        CmdConfirmPlayerVote(); 
    }

    [Command(requiresAuthority = false)]
    public void CmdConfirmPlayerVote() //TODO: add effects to indicate a player has locked in, to other players
    {
        if (selectedPlayer == -1)
            return;

        numVoted[selectedPlayer]++;

        TallyVotes(); //check if voting is complete
        //TODO: handle spread out votes (I.E. no definitive target of the vote)

        
    }

    [Server]
    void TallyVotes()
    {
        int votes = 0;
        for(int i = 0; i < numVoted.Length; i++)
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
                RpcResults();
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
