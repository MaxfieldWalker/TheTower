using UnityEngine;

public class MainPlayer : MainPlayerBase {
    public GameManager gameManager;
    public CameraManager cameraManager;
    public ControlState CurrentControlState;

    private ArmInfo Hand_L;
    private ArmInfo Hand_R;
    private ArmInfo Foot_L;
    private ArmInfo Foot_R;

    void Start() {
        // 両手にアサインされた状態から始める
        this.CurrentControlState = ControlState.Hands;

        this.transform.FindChild("1p").gameObject.transform.FindChild("armsLeft").gameObject.transform.Find("Sphere");
        this.Hand_L = new ArmInfo(this.transform.FindChild("1p").gameObject.transform.FindChild("armsLeft").gameObject.transform.Find("Sphere"));
        this.Hand_R = new ArmInfo(this.transform.FindChild("1p").gameObject.transform.FindChild("armsRight").gameObject.transform.Find("Sphere"));
        this.Foot_L = new ArmInfo(this.transform.FindChild("2p").gameObject.transform.FindChild("armsLeft").gameObject.transform.Find("Sphere"));
        this.Foot_R = new ArmInfo(this.transform.FindChild("2p").gameObject.transform.FindChild("armsRight").gameObject.transform.Find("Sphere"));
    }

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

        // 使うカメラを切り替える
        if(state == ControlState.Hands) {
            this.cameraManager.UsePlayer1Camera();
        }
        if(state == ControlState.Feet) {
            this.cameraManager.UsePlayer2Camera();
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

    public override void DelegateOnTriggerEnter(GameObject self, Collider other) {
        Debug.Log("trigger enter: " + self.tag);

        if (self.tag == "Hand_L") this.Hand_L.Grab(other.gameObject);
        if (self.tag == "Hand_R") this.Hand_R.Grab(other.gameObject);
        if (self.tag == "Foot_L") this.Foot_L.Grab(other.gameObject);
        if (self.tag == "Foot_R") this.Foot_R.Grab(other.gameObject);
    }

    public override void DelegateOnTriggerExit(GameObject self, Collider other) {
        Debug.Log("trigger exit: " + self.tag);

        if (self.tag == "Hand_L") this.Hand_L.Ungrab();
        if (self.tag == "Hand_R") this.Hand_R.Ungrab();
        if (self.tag == "Foot_L") this.Foot_L.Ungrab();
        if (self.tag == "Foot_R") this.Foot_R.Ungrab();
    }

    public void Respawn() {
        Debug.Log("PLAYER RESPAWN");

        // 初期位置に戻す
        this.Hand_L.Reset();
        this.Hand_R.Reset();
        this.Foot_L.Reset();
        this.Foot_R.Reset();
    }

    public bool CanMoveHand_L() {
        return !this.Hand_L.IsLocked;
    }

    public bool CanMoveHand_R() {
        return !this.Hand_R.IsLocked;
    }

    public bool CanMoveFoot_L() {
        return !this.Foot_L.IsLocked;
    }

    public bool CanMoveFoot_R() {
        return !this.Foot_R.IsLocked;
    }
}
