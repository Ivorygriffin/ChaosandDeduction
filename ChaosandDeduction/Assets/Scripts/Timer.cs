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

    public AudioSource gameplayMusic;
    void Start()
    {
        timeRemaining = startTime;
    }

    void Update()
    {
        if (isServer) //only crunch the numbers on the server
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining <= 0)
            {
                UIManager.Instance.CmdVoting(true);
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
