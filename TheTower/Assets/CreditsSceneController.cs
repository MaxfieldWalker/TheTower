using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsSceneController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            goTitleScene();
        }
	}

    public void goTitleScene()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
