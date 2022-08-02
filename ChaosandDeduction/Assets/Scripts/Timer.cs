using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class Timer : MonoBehaviour
{
    public float timeRemaining, startTime;
    public TMP_Text timerText;
    void Start()
    {
        timeRemaining = startTime;
    }
    
    void Update()
    {
        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0)
        {
            UIManager.Instance.RpcVoting(true);
        }

        timerText.text = timeRemaining.ToString("F0");
    }
}
