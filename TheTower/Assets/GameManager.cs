using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject GameOverUI;

    public float time = 5.0f;
    private float elapsed = 0.0f;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        this.elapsed += Time.deltaTime;
        if (this.elapsed > this.time)
            this.GameOverUI.SendMessage("gameOver");
    }
}
