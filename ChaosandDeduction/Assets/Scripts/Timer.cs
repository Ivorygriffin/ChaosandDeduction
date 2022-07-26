using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


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
    }

    public void TimerStart()
    {
        timeRemaining -= Time.deltaTime;
    }
    public void UpdateTimerText()
    {
        timerText.text = timeRemaining.ToString("F0");
    }

}
