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
        TimerStart();
        UpdateTimerText();
        CheckTime();
    }

    public void TimerStart()
    {
        timeRemaining -= Time.deltaTime;
    }
    public void UpdateTimerText()
    {
        timerText.text = timeRemaining.ToString("F0");
    }

    public void CheckTime()
    {
        if(timeRemaining <= 0)
        {
            TimerEnd();
        }
    }
    public void TimerEnd()
    {
        UIManager.Instance.Voting();
    }
}
