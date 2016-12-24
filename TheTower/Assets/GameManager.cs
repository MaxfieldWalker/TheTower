using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject GameOverUI;
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
        this.mainCamera.SendMessage("activateBlur");
        this.GameOverUI.SetActive(true);
    }

    public void onBackToTitlePageClick()
    {
        Debug.Log("Back to title page button was clicked");
        SceneManager.LoadScene("TitleScene");
    }

    public void onContinueButtonClick()
    {
        Debug.Log("Continue button was clicked");
    }
}
