using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScript : MonoBehaviour {
    public GameObject gameOverUI;

	// Use this for initialization
	void Start () {
        // 最初は非表示
        this.gameObject.GetComponent<Text>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void gameOver()
    {
        // GAME OVERのテキストを表示する
        this.gameObject.GetComponent<Text>().enabled = true;
    }
}
