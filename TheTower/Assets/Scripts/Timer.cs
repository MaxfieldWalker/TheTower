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
        this.text.text = "";
    }

    public void show()
    {
        updateView();
    }

    private void updateView()
    {
        // 経過時間を表示する
        this.text.text = currentTime().ToString(@"m\'ss.fff");
    }

    public DateTime currentTime()
    {
        return new DateTime(0).Add(TimeSpan.FromSeconds(countTime));
    }
}
