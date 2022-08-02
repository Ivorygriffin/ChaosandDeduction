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
}
