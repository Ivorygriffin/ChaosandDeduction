using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class votingSystem : MonoBehaviour
{
    public bool traitorDetected;
    public bool p1, p2, p3, p4;
    public int numVoted;
    //attach traitor script to a player if the selected player has the traitor script on them then traitorDetected = true
    //attach villager script to others if the selected player has the villager script on them then traitorDetected = false
    void Start()
    {
        
    } 
    void Update()
    {
        
    }

    [ClientRpc]
    void RpcVote()
    {
        if (traitorDetected == true && GetComponent<task>().vTasksComplete == true)
        {
            UIManager.Instance.winner = "The Villagers Win";
            UIManager.Instance.WinScreen();
        }
        if (traitorDetected == false && GetComponent<task>().vTasksComplete == true && GetComponent<task>().tTasksComplete == true)
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

    public void SelectPlayer()
    {
        if (p1)
        {

        }
        if (p2)
        {

        }
        if (p3)
        {

        }
        if (p4)
        {
            
        }
    }
    [Command]
    void CmdConfirmVote()
    {
        if (p1)
        {
            RpcVote();
        }
        if (p2)
        {
            RpcVote();
        }
        if (p3)
        {
            RpcVote();
        }
        if (p4)
        {
            RpcVote();
        }
    }
}
