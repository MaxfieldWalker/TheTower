using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject GameOverUI;
    public GameObject player;
    public Camera mainCamera;

    private float time = 5.0f;
    private float elapsed = 0.0f;

    // Use this for initialization
    void Start()
    {
        this.GameOverUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void gotoGameOverState()
    {
        this.player.SendMessage("gotoGameOverState");
        this.mainCamera.SendMessage("activateBlur");
        this.GameOverUI.SetActive(true);
    }

    private void gotoGameState()
    {
        this.player.SendMessage("respawn");
        this.mainCamera.SendMessage("deactivateBlur");
        this.GameOverUI.SetActive(false);
    }

    public void onBackToTitlePageClick()
    {
        Debug.Log("Back to title page button was clicked");
        SceneManager.LoadScene("TitleScene");
    }

    public void onContinueButtonClick()
    {
        this.gotoGameState();
        Debug.Log("Continue button was clicked");
    }
}
