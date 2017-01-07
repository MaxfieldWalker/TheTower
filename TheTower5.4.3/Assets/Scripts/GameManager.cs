using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameStates GameState;

    public GameObject GameOverUI;
    public GameObject GameClearUI;
    public GameObject player;
    public Camera mainCamera;
    public Text countdownText;
    public Timer timer;
    public Animator gameClearUIAnimator;

    private bool gettingReady = false;

    // Use this for initialization
    void Start()
    {
        useMainCamera();
    }

    // Update is called once per frame
    void Update()
    {
        if(this.GameState == GameStates.BeforeGame && !gettingReady)
        {
            getReady();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            useMainCamera();
        }

        // TODO: ゲームクリア条件を作る
        // デバッグ用に60秒後にゲームクリアとしている
        if (this.GameState != GameStates.GameClear
         && this.timer.currentTimeSpan() > new System.TimeSpan(0, 0, 3))
        {
            gotoGameClearState();
        }
    }

    public void gotoGameOverState()
    {
        this.GameState = GameStates.GameOver;
        this.timer.pauseTimer();
        this.player.SendMessage("gotoGameOverState");
        this.mainCamera.SendMessage("activateBlurWithAnim");
        this.GameOverUI.SetActive(true);
    }

    public void gotoGameClearState()
    {
        this.GameState = GameStates.GameClear;
        this.timer.pauseTimer();
        this.player.SendMessage("gotoGameClearState");
        this.mainCamera.SendMessage("activateBlurWithAnim");
        this.timer.hide();
        this.GameClearUI.SetActive(true);
        this.GameClearUI.GetComponentsInChildren<Text>()
            .Where(x => x.name == "ClearTimeText")
            .FirstOrDefault()
            .text = "Clear Time: " + this.timer.currentTimeAsString() + "\n  Medal: Gold";
    }

    private void gotoGameState()
    {
        this.player.SendMessage("Respawn");
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
    }

    private void getReady()
    {
        gettingReady = true;
        this.mainCamera.SendMessage("activateBlurWithAnim");
        this.GameState = GameStates.BeforeGame;
        this.GameOverUI.SetActive(false);
        this.GameClearUI.SetActive(false);
        this.timer.pauseTimer();
        this.timer.resetTimer();
        this.timer.hide();
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
        this.gettingReady = false;
        this.timer.show();
        this.timer.resumeTimer();

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

