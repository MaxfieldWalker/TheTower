using System;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private float countTime;
    private bool paused;

    // Use this for initialization
    void Start()
    {
        countTime = 0;
        paused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (paused) return;

        countTime += Time.deltaTime;

        // 経過時間を表示する
        this.GetComponent<Text>().text = CurrentTime().ToString(@"m\'ss.fff");
    }

    public void PauseTimer()
    {
        paused = true;
    }

    public void ResumeTimer()
    {
        paused = false;
    }

    public DateTime CurrentTime()
    {
        return new DateTime(0).Add(TimeSpan.FromSeconds(countTime));
    }
}
