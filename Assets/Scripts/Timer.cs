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
    public float CompletedDisplayTime;
    float MaxDisplayTime;
    public TMP_Text CompletedText;
    float ExtraTime;

    public AudioSource MusicSource;
    float StarterVolume;

    // Start is called before the first frame update
    void Start()
    {
        MaxDisplayTime = CompletedDisplayTime;
        if (PlayerPrefs.GetFloat("FastestTime") != 0)
        {
            LeaderboardText.text = ("Best Time:    " + Mathf.Round(PlayerPrefs.GetFloat("FastestTime") * 1000) / 1000);
            BestTime = PlayerPrefs.GetFloat("FastestTime");
        }
        StarterVolume = MusicSource.volume;
            
    }

    // Update is called once per frame
    void Update()
    {
        if (TimerRunning)
        {
            Timed += Time.deltaTime;
            TimerText.text = ("" + Mathf.Round(Timed * 1000) / 1000);
            MusicSource.volume = StarterVolume * 2;
        }

        if (IsStarting())
            StartTimer();

        if (transform.position.y <= -19 || Input.GetKeyDown(KeyCode.R))
        {
            TimerRunning = false;
            transform.position = new Vector3(90, 3, -17);
            CompletedText.text = "";
            MusicSource.volume = StarterVolume;
        }
            
        


        if (IsEnding() && TimerRunning)
        {
            TimerRunning = false;
            MusicSource.volume = StarterVolume;
            GameObject[] Enemies = GameObject.FindGameObjectsWithTag("Enemy");
            int EnemyCount = Enemies.Length;
            for (int i = 0; i < EnemyCount; i++)
            {
                if (Enemies[i].GetComponent<Renderer>().enabled == true)
                {
                    Timed = Timed + 5;
                    ExtraTime = ExtraTime + 5;
                }
            }
            if (BestTime == 0 || BestTime > Timed)
                BestTime = Timed;
                PlayerPrefs.SetFloat("FastestTime", BestTime);
                LeaderboardText.text = ("Best Time:    " + Mathf.Round(BestTime * 1000) / 1000);
            CompletedText.text = "Completed \n +" + ExtraTime + "seconds for " + ExtraTime / 5 + " missed enemies";
            ExtraTime = 0;
            CompletedDisplayTime -= Time.deltaTime;
            if (CompletedDisplayTime < 0)
                CompletedText.text = "";


        }



    }

    void StartTimer()
    {
        GameObject[] Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        int EnemyCount = Enemies.Length;
        for (int i = 0; i < EnemyCount; i++)
        {
            Enemies[i].GetComponent<Renderer>().enabled = true;
            Enemies[i].GetComponent<Collider>().enabled = true;
        }
        TimerRunning = true;
        Timed = 0;
        CompletedDisplayTime = MaxDisplayTime;
        CompletedText.text = "";
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
