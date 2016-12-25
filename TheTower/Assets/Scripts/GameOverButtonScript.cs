using UnityEngine;

public class GameOverButtonScript : MonoBehaviour
{
    public GameObject gameManagerObject;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void onClick()
    {
        this.gameObject.SetActive(false);
        this.gameManagerObject.SendMessage("gotoGameOverState");
    }
}
