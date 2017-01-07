using System;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private Text text;
    private float countTime;
    private bool paused;

    // Use this for initialization
    void Start()
    {
        countTime = 0f;
        paused = false;
        this.text = this.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (paused) return;

        countTime += Time.deltaTime;

        updateView();
    }

    public void pauseTimer()
    {
        paused = true;
    }

    public void resumeTimer()
    {
        paused = false;
    }

    public void resetTimer()
    {
        this.countTime = 0f;
        updateView();
    }

    public void hide()
    {
        if (this.text == null) return;

        this.text.text = "";
    }

    public void show()
    {
        updateView();
    }

    private void updateView()
    {
        if (this.text == null) return;

        // 経過時間を表示する
        this.text.text = currentTimeAsString();
    }

    public DateTime currentTime()
    {
        return new DateTime(0).Add(currentTimeSpan());
    }

    public TimeSpan currentTimeSpan()
    {
        return TimeSpan.FromSeconds(countTime);
    }

    public string currentTimeAsString()
    {
        return currentTime().ToString(@"m\'ss.fff");
    }
}
