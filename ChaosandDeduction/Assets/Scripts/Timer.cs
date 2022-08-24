using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class Timer : NetworkBehaviour
{
    [SyncVar]
    public float timeRemaining, startTime;
    public TMP_Text timerText;

    bool revealedVotingScreen = false; //has the server revealed the voting screen yet? TODO: turn this on when early vote is called?

    public AudioSource gameplayMusic; 
    public AudioSource VotingMusic;
    public GameObject set1,set2;
    void Start()
    {
        timeRemaining = startTime;
    }

    void Update()
    {
        if (isServer) //only crunch the numbers on the server
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining <= 0 && !revealedVotingScreen)
            {
                revealedVotingScreen = true;
                UIManager.Instance.CmdVoting(true);
                VotingMusic.Play();
            }
            if(timeRemaining <= 90)
            {
                set1.SetActive(true);
                set2.SetActive(true);
            }
        }

        timerText.text = timeRemaining.ToString("F0");
    }
    public override void OnStartClient()
    {
        base.OnStartClient();
        CmdStartMusic();
    }
    [Command(requiresAuthority = false)]
    public void CmdStartMusic(NetworkConnectionToClient conn = null) //round about way of forcing the client to sync music?
    {
        TargetStartMusic(conn, timeRemaining);
    }
    [TargetRpc]
    void TargetStartMusic(NetworkConnection conn, float time)
    {
        gameplayMusic.Play();
        gameplayMusic.time = (startTime - time);
    }
}
