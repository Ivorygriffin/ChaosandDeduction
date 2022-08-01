using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class votingSystem : NetworkBehaviour
{
    //bool 

    public bool traitorDetected;
    

    //int

    public int numVoted;
    public int numTraitorVote; 
    public int numVillagerVote;

    //Gameobjects

    public GameObject voteObjects;

    [ServerCallback]
    void Update()
    {
        RpcConfirmVote();
    }

    
    [ClientRpc]
    void RpcVote()
    {
        if (traitorDetected == true && GetComponent<task>().vTaskComplete == true)
        {
            UIManager.Instance.winner = "The Villagers Win";
            UIManager.Instance.WinScreen();
        }
        if (traitorDetected == false && GetComponent<task>().vTaskComplete == true && GetComponent<task>().tTaskComplete == true)
        {
            UIManager.Instance.winner = "The Traitor Wins";
            UIManager.Instance.WinScreen();
        } 
        else
        {
            UIManager.Instance.winner = "The Game Has Beaten you all";
            UIManager.Instance.WinScreen();
        }
        
    }

    [Command(requiresAuthority = false)]
    public void CmdSelectPlayer()
    {
        Debug.Log("selected");
        switch (GetComponent<voteobject>().playerType)
        {
            case PlayerType.villager:
                numVillagerVote += 1;
                Debug.Log(numVillagerVote);
                break;
            case PlayerType.traitor:
                numTraitorVote += 1;
                numVoted += 1;
                Debug.Log(numTraitorVote);
                break ;
            default:
                
                break;
        }

    }

    [Command(requiresAuthority = false)]
    public void CmdConfirmPlayerVote()
    {
        numVoted += 1;
        voteObjects.SetActive(false);
    }

   
    void RpcConfirmVote()
    {
        if (numVoted == 4)
        {
            if(numVillagerVote >= 3) 
            {
                traitorDetected = false;
                RpcVote();
            }
            if(numVillagerVote >= 3)
            {
                traitorDetected = true;
                RpcVote();
            }
        }
        else
        {
            
        }
    }
}
