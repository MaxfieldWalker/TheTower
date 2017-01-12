using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public GameStates GameState;
    public int ForceGameClearSeconds;

    public GameObject GameOverUI;
    public GameObject GameClearUI;
    public GameObject VRGameOverUI;
    public GameObject VRGameClearUI;

    public MainPlayer mainPlayer;
    public SubPlayer subPlayer;

    public Camera mainCamera;
    public BlurScript vrPlayer1Blur;
    public BlurScript vrPlayer2Blur;

    public Text countdownText;
    public Text vrCountdonwText;

    public Timer timer;
    public Timer vrTimer;

    public Animator gameClearUIAnimator;

    private bool gettingReady = false;

    // Use this for initialization
    void Start() {
        useMainCamera();
    }

    // Update is called once per frame
    void Update() {
        if (this.GameState == GameStates.BeforeGame && !gettingReady) {
            getReady();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            useMainCamera();
        }

        // TODO: ゲームクリア条件を作る
        // デバッグ用に60秒後にゲームクリアとしている
        if (this.GameState != GameStates.GameClear
         && this.timer.currentTimeSpan() > new System.TimeSpan(0, 0, ForceGameClearSeconds)) {
            gotoGameClearState();
        }
    }

    public void gotoGameOverState() {
        this.GameState = GameStates.GameOver;
        this.timer.pauseTimer();
        this.vrTimer.pauseTimer();
        this.mainPlayer.GoToGameOverState();

        this.mainCamera.SendMessage("activateBlurWithAnim");
        this.vrPlayer1Blur.activateBlurWithAnim();
        this.vrPlayer2Blur.activateBlurWithAnim();

        this.GameOverUI.SetActive(true);
        this.VRGameOverUI.SetActive(true);
    }

    public void gotoGameClearState() {
        this.GameState = GameStates.GameClear;
        this.timer.pauseTimer();
        this.vrTimer.pauseTimer();
        this.mainPlayer.GoToGameClearState();

        this.mainCamera.SendMessage("activateBlurWithAnim");
        this.vrPlayer1Blur.activateBlurWithAnim();
        this.vrPlayer2Blur.activateBlurWithAnim();

        this.timer.hide();
        this.vrTimer.hide();

        this.GameClearUI.SetActive(true);
        this.VRGameClearUI.SetActive(true);
        this.GameClearUI.GetComponentsInChildren<Text>()
            .Where(x => x.name == "ClearTimeText")
            .FirstOrDefault()
            .text = "Clear Time: " + this.timer.currentTimeAsString() + "\n  Medal: Gold";
    }

    private void gotoGameState() {
        // this.mainPlayer.Respawn();
        getReady();
    }

    public void onBackToTitlePageClick() {
        Debug.Log("Back to title page button was clicked");
        SceneManager.LoadScene("TitleScene");
    }

    public void onContinueButtonClick() {
        this.gotoGameState();
        Debug.Log("Continue button was clicked");
    }

    private void useMainCamera() {
        this.mainCamera.gameObject.SetActive(true);
    }

    private void getReady() {
        gettingReady = true;

        this.mainCamera.SendMessage("activateBlurWithAnim");
        this.vrPlayer1Blur.activateBlurWithAnim();
        this.vrPlayer2Blur.activateBlurWithAnim();

        this.GameState = GameStates.BeforeGame;

        this.GameOverUI.SetActive(false);
        this.VRGameOverUI.SetActive(false);
        this.GameClearUI.SetActive(false);
        this.VRGameClearUI.SetActive(false);

        this.timer.pauseTimer();
        this.timer.resetTimer();
        this.timer.hide();
        this.vrTimer.pauseTimer();
        this.vrTimer.resetTimer();
        this.vrTimer.hide();

        StartCoroutine(countdownCoroutine());
    }

    IEnumerator countdownCoroutine() {
        // 3, 2, 1とカウントダウンする
        this.countdownText.gameObject.SetActive(true);
        this.vrCountdonwText.gameObject.SetActive(true);
        this.countdownText.text = "3";
        this.vrCountdonwText.text = "3";
        yield return new WaitForSeconds(1.0f);
        this.countdownText.text = "2";
        this.vrCountdonwText.text = "2";
        yield return new WaitForSeconds(1.0f);
        this.countdownText.text = "1";
        this.vrCountdonwText.text = "1";
        yield return new WaitForSeconds(1.0f);

        this.countdownText.text = "GO!";
        this.vrCountdonwText.text = "GO!";

        this.mainCamera.SendMessage("deactivateBlurWithAnim");
        this.vrPlayer1Blur.deactivateBlurWithAnim();
        this.vrPlayer2Blur.deactivateBlurWithAnim();

        this.GameState = GameStates.Game;
        this.gettingReady = false;

        this.timer.show();
        this.timer.resumeTimer();
        this.vrTimer.show();
        this.vrTimer.resumeTimer();

        yield return new WaitForSeconds(0.5f);

        // カウントダウンを非表示にする
        this.countdownText.gameObject.SetActive(false);
        this.vrCountdonwText.gameObject.SetActive(false);
    }

    public enum GameStates {
        BeforeGame,
        Game,
        GameOver,
        GameClear
    }
}

