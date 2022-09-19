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
    public GameObject set1, set2;
    bool barricadesShown = false;

    enum audioValue
    {
        gameplay,
        voting
    }

    void Start()
    {
        timeRemaining = startTime;
        if (isServer)
            gameplayMusic.Play();
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
                //VotingMusic.Play(); //TODO: determine if host needs to call the voting music themselves or are they included in clientRPC
                RpcStartMusic(audioValue.voting, 0);
            }
            if (timeRemaining <= 90 && !barricadesShown)
            {
                barricadesShown = true;
                RpcShowB();
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
    public void CmdStartMusic(NetworkConnectionToClient conn = null)  //round about way of forcing the client to sync music?
    {
        TargetStartMusic(conn, audioValue.gameplay, timeRemaining);
    }
    [TargetRpc]
    void TargetStartMusic(NetworkConnection conn, audioValue audio, float time)
    {
        PlayMusic(audio, time);
    }
    [ClientRpc]
    void RpcStartMusic(audioValue audio, float time)
    {
        PlayMusic(audio, time);
    }

    void PlayMusic(audioValue audio, float time)
    {
        AudioSource audioSource = null;

        switch (audio)
        {
            case audioValue.gameplay:
                audioSource = gameplayMusic;
                break;
            case audioValue.voting:
                audioSource = VotingMusic;
                break;

        }
        audioSource.Play();
        audioSource.time = (startTime - time);
    }

    [ClientRpc]
    public void RpcShowB()
    {
        set1.SetActive(true);
        set2.SetActive(true);
    }
}
