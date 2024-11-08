using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TMP_Text TimerText;
    bool TimerRunning = false;
    float Timed = 0;
    public LayerMask StartTimerMask;
    public LayerMask StopTimerMask;
    public float BestTime;
    public TMP_Text LeaderboardText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (TimerRunning)
        {
            Timed += Time.deltaTime;
            TimerText.text = ("" + Mathf.Round(Timed * 1000) / 1000);
        }

        if (IsStarting())
            StartTimer();

        if (IsEnding() || transform.position.y <= -20)
            TimerRunning = false;



        if (IsEnding())
        {
            if (BestTime == 0 || BestTime > Timed)
                BestTime = Timed;
                LeaderboardText.text = ("Best Time:    " + Mathf.Round(BestTime * 1000) / 1000);
        }



    }

    void StartTimer()
    {
        TimerRunning = true;
        Timed = 0;
    }

    bool IsStarting()
    {
        
        return Physics.CheckSphere(transform.position, 1.5f, StartTimerMask);
    }

    bool IsEnding()
    {
        return Physics.CheckSphere(transform.position, 1.5f, StopTimerMask);
    }

}
