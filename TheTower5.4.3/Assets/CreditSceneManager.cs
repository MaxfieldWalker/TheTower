using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CreditSceneManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void onBackToTitleClick() {
        SceneManager.LoadScene("TitleScene");
    }
}
