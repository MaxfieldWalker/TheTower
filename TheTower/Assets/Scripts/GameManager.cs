using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameStates GameState;

    public GameObject GameOverUI;
    public GameObject player;
    public Camera mainCamera;
    public Camera fpsCamera;
    public Text countdownText;

    private float time = 5.0f;
    private float elapsed = 0.0f;

    // Use this for initialization
    void Start()
    {
        useMainCamera();
        getReady();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            useMainCamera();
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            useFPSCamera();
        }
    }

    public void gotoGameOverState()
    {
        this.player.SendMessage("gotoGameOverState");
        this.mainCamera.SendMessage("activateBlurWithAnim");
        this.GameOverUI.SetActive(true);
    }

    private void gotoGameState()
    {
        this.player.SendMessage("respawn");
        getReady();
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

    private void useMainCamera()
    {
        this.mainCamera.gameObject.SetActive(true);
        this.fpsCamera.gameObject.SetActive(false);
    }

    private void useFPSCamera()
    {
        this.mainCamera.gameObject.SetActive(false);
        this.fpsCamera.gameObject.SetActive(true);
    }

    private void getReady()
    {
        this.mainCamera.SendMessage("activateBlurWithAnim");
        this.GameState = GameStates.BeforeGame;
        this.GameOverUI.SetActive(false);
        StartCoroutine(countdownCoroutine());
    }

    IEnumerator countdownCoroutine()
    {
        this.countdownText.gameObject.SetActive(true);
        this.countdownText.text = "3";
        yield return new WaitForSeconds(1.0f);
        this.countdownText.text = "2";
        yield return new WaitForSeconds(1.0f);
        this.countdownText.text = "1";
        yield return new WaitForSeconds(1.0f);

        this.countdownText.text = "GO!";
        this.mainCamera.SendMessage("deactivateBlurWithAnim");
        this.GameState = GameStates.Game;

        yield return new WaitForSeconds(0.5f);
        this.countdownText.gameObject.SetActive(false);
    }

    public enum GameStates
    {
        BeforeGame,
        Game,
        GameOver,
        GameClear
    }
}

