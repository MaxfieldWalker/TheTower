using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSceneManager : MonoBehaviour
{
    // GameObjecs
    public GameObject onePlayerButton;
    public GameObject twoPlayersButton;
    public GameObject level1Button;
    public GameObject level2Button;
    public GameObject level3Button;

    private static GameInfo gameInfo;
    private GameLevel gameLevel = GameLevel.Unknwon;
    private GamePlayerMode playerMode = GamePlayerMode.Unknown;

    // Use this for initialization
    void Start()
    {
        // 起動してすぐレベル選択のボタンを非表示にする
        setActiveAllLevelButtons(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void on1PlayerButtonClick()
    {
        this.playerMode = GamePlayerMode.OnePlayer;
        setActiveAllPlayerButtons(false);
        setActiveAllLevelButtons(true);
    }

    public void on2PlayerButtonClick()
    {
        this.playerMode = GamePlayerMode.TwoPlayers;
        setActiveAllPlayerButtons(false);
        setActiveAllLevelButtons(true);
    }

    public void onLevel1ButtonClick()
    {
        this.gameLevel = GameLevel.Level1;
        goGameScene();
    }

    public void onLevel2ButtonClick()
    {
        this.gameLevel = GameLevel.Level2;
        goGameScene();
    }

    public void onLevel3ButtonClick()
    {
        this.gameLevel = GameLevel.Level3;
        goGameScene();
    }

    /// <summary>
    /// ゲームシーンへ移動する
    /// </summary>
    private void goGameScene()
    {
        Debug.Log(this.playerMode.ToString() + ", " + this.gameLevel.ToString());

        if (this.playerMode == GamePlayerMode.OnePlayer)
        {

        }
        if (this.playerMode == GamePlayerMode.TwoPlayers)
        {

        }

        if (this.gameLevel == GameLevel.Level1)
        {

        }

        if (this.gameLevel == GameLevel.Level2)
        {

        }

        if (this.gameLevel == GameLevel.Level3)
        {

        }
    }

    public void setActiveAllLevelButtons(bool value)
    {
        this.level1Button.SetActive(value);
        this.level2Button.SetActive(value);
        this.level3Button.SetActive(value);
    }

    public void setActiveAllPlayerButtons(bool value)
    {
        this.onePlayerButton.SetActive(value);
        this.twoPlayersButton.SetActive(value);
    }
}
