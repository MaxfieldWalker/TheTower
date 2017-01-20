using Assets.Scripts;
using UnityEngine;

public class MainPlayerMoq : MainPlayerBase {
    public GameManager gameManager;
    public ControlState CurrentControlState;

    public PlayingGameSE se;

    private ArmInfo Hand_L;
    private ArmInfo Hand_R;
    private ArmInfo Foot_L;
    private ArmInfo Foot_R;

    // Use this for initialization
    void Start() {
        // 両手にアサインされた状態から始める
        this.CurrentControlState = ControlState.Hands;

        this.Hand_L = new ArmInfo(GameObject.Find("Hand_L").transform,this.se,null);
        this.Hand_R = new ArmInfo(GameObject.Find("Hand_R").transform, this.se, null);
        this.Foot_L = new ArmInfo(GameObject.Find("Foot_L").transform, this.se, null);
        this.Foot_R = new ArmInfo(GameObject.Find("Foot_R").transform, this.se, null);
    }

    // Update is called once per frame
    void Update() {
        // 両手足の状態をチェックして体を支えられない状態になっていたら
        // ゲームオーバー状態に遷移する

        if (this.gameManager.GameState == GameManager.GameStates.Game && !canSupportBody()) {
            this.gameManager.gotoGameOverState();
        }
    }

    private bool canSupportBody() {
        // 両手が離れている
        if (this.Hand_L.IsFree && this.Hand_R.IsFree) return false;

        // TODO: 両手足がひねられすぎて上半身と下半身が分離する場合もゲームオーバーとする
        return true;
    }

    public override void ChangeState(ControlState state) {
        this.CurrentControlState = state;
    }

    public void MoveLeft(MoveDirection direction) {
        switch (this.CurrentControlState) {
            case ControlState.Hands:
                this.Hand_L.Move(direction);
                break;
            case ControlState.Feet:
                this.Foot_L.Move(direction);
                break;
            default:
                break;
        }
    }

    public void MoveRight(MoveDirection direction) {
        switch (this.CurrentControlState) {
            case ControlState.Hands:
                this.Hand_R.Move(direction);
                break;
            case ControlState.Feet:
                this.Foot_R.Move(direction);
                break;
            default:
                break;
        }
    }

    public override void ToggleLockLeft() {
        switch (this.CurrentControlState) {
            case ControlState.Hands:
                this.Hand_L.ToggleLock();
                break;
            case ControlState.Feet:
                this.Foot_L.ToggleLock();
                break;
            default:
                break;
        }
    }

    public override void ToggleLockRight() {
        switch (this.CurrentControlState) {
            case ControlState.Hands:
                this.Hand_R.ToggleLock();
                break;
            case ControlState.Feet:
                this.Foot_R.ToggleLock();
                break;
            default:
                break;
        }
    }

    // 両手足の衝突をまとめて受け取る
    public override void DelegateOnTriggerEnter(GameObject self, Collider other) {
        Debug.Log("trigger enter: " + self.name);

        if (self.name == "Hand_L") this.Hand_L.Grab(other.gameObject);
        if (self.name == "Hand_R") this.Hand_R.Grab(other.gameObject);
        if (self.name == "Foot_L") this.Foot_L.Grab(other.gameObject);
        if (self.name == "Foot_R") this.Foot_R.Grab(other.gameObject);
    }

    public override void DelegateOnTriggerExit(GameObject self, Collider other) {
        Debug.Log("trigger exit: " + self.name);

        if (self.name == "Hand_L") this.Hand_L.Ungrab();
        if (self.name == "Hand_R") this.Hand_R.Ungrab();
        if (self.name == "Foot_L") this.Foot_L.Ungrab();
        if (self.name == "Foot_R") this.Foot_R.Ungrab();
    }

    public void Respawn() {
        Debug.Log("PLAYER RESPAWN");

        // 初期位置に戻す
        this.Hand_L.Reset();
        this.Hand_R.Reset();
        this.Foot_L.Reset();
        this.Foot_R.Reset();
    }
}
