using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScript : MonoBehaviour {
    public GameObject gameOverUI;

	// Use this for initialization
	void Start () {
        this.gameOverUI.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void gameOver()
    {
        // GAME OVERのUIを表示する
        this.gameOverUI.SetActive(true);
    }
}
