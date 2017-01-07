using UnityEngine;
using System.Collections;

public class SubPlayerController : MonoBehaviour {
    public GameManager gameManager;
    private SubPlayer subPlayer;

	// Use this for initialization
	void Start () {
        this.subPlayer = GetComponent<SubPlayer>();
	}
	
	// Update is called once per frame
	void Update () {
        if (this.gameManager.GameState != GameManager.GameStates.Game) return;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // 両手にアサインする
            Debug.Log("assign control hands");
            this.subPlayer.ChangeState(ControlState.Hands);
        }else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // 両足にアサインする
            Debug.Log("assign control feet");
            this.subPlayer.ChangeState(ControlState.Feet);
        }

        // Qで左のロックをトグルする
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("toggle left lock");
            this.subPlayer.ToggleLockLeft();
        }

        // Wで左のロックをトグルする
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("toggle right lock");
            this.subPlayer.ToggleLockRight();
        }
    }
}
