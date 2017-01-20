using UnityEngine;

public class MainCharacterKinectController : MonoBehaviour {
    public MainPlayer mainPlayer;
    private const int JointCount = 15;

    /// <summary>
    /// ジャンプなどの縦の動きを許可するか?
    /// </summary>
    public bool MoveVertically = false;

    /// <summary>
    /// 鏡に映したようにマッピングを反転させるか? (ユーザーの右手がキャラクターの左手にマッピングされる)
    /// </summary>
    public bool MirroredMovement = false;

    // 体の各部位を対応させるGameObject
    // Unity側で指定する
    //public GameObject Head;
    //public GameObject Neck;
    //public GameObject Shoulder_Left;
    //public GameObject Shoulder_Right;
    //public GameObject Elbow_Left;
    //public GameObject Elbow_Right;
    public GameObject Hand_Left;
    public GameObject Hand_Right;
    //public GameObject Hip_Center;
    //public GameObject Hip_Left;
    //public GameObject Hip_Right;
    //public GameObject Knee_Left;
    //public GameObject Knee_Right;
    public GameObject Foot_Left;
    public GameObject Foot_Right;

    // 体の各部位を配列で管理する
    private BoneInfo[] bInfo;
    private ControlState Asignment;

    // 初期位置
    private Vector3 initialPosition;
    // 初期回転
    private Quaternion initialRotation;
    private Vector3 initialPosOffset = Vector3.zero;
    private uint initialPosUserID = 0;

    void Start() {
        bInfo = new BoneInfo[JointCount]
        {
            null,
            null,
            null,
            null,
            null,
            null,
            new BoneInfo(Hand_Left),
            new BoneInfo(Hand_Right),
            null,
            null,
            null,
            null,
            null,
            new BoneInfo(Foot_Left),
            new BoneInfo(Foot_Right),
        };

        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="info">動かしたいボーン</param>
    /// <param name="playerID"></param>
    /// <param name="posPointMan"></param>
    /// <param name="i">入力にするボーンのインデックス</param>
    private void moveBone(BoneInfo info, uint playerID, Vector3 posPointMan, int i) {
        if (info.Bone == null) return;

        // このジョイントはトラッキングされているか?
        if (KinectManager.Instance.IsJointPositionTracked(playerID, i)) {
            info.Bone.gameObject.SetActive(true);

            int joint = MirroredMovement ? KinectWrapper.GetSkeletonMirroredJoint(i) : i;
            // Kinectからジョイントの座標を取得する
            Vector3 posJoint = KinectManager.Instance.GetJointPosition(playerID, joint);
            // Kinectからジョイントの回転を取得する
            Quaternion rotJoint = KinectManager.Instance.GetJointOrientation(playerID, joint, !MirroredMovement);

            posJoint.x = MirroredMovement ? posJoint.x : -posJoint.x;
            posJoint.z = MirroredMovement ? posJoint.z : -posJoint.z;

            // ジョイントの座標からユーザーの座標を引いて相対座標にする
            posJoint -= posPointMan;

            info.SetPosition(posJoint);
            info.SetRotation(rotJoint);
        }
    }

    void Update() {
        // メインプレイヤーがなければこのスクリプトでモードを切り替える
        if (this.mainPlayer == null) {
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                this.Asignment = ControlState.Hands;
            } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
                this.Asignment = ControlState.Feet;
            }
        } else {
            // メインプレイヤーの割当てを使う
            this.Asignment = this.mainPlayer.CurrentControlState;
        }

        // get 1st player
        uint playerID = KinectManager.Instance != null ? KinectManager.Instance.GetPlayer1ID() : 0;

        if (playerID <= 0) {
            return;
        }

        //if (playerID <= 0) {
        //    // 位置と回転を初期値にリセットする
        //    if (transform.position != initialPosition) transform.position = initialPosition;
        //    if (transform.rotation != initialRotation) transform.rotation = initialRotation;
        //    return;
        //}

        // Kinectからユーザーの座標(尻の座標)を取得する
        Vector3 posPointMan = KinectManager.Instance.GetUserPosition(playerID);
        posPointMan.z = MirroredMovement ? posPointMan.z : -posPointMan.z;

        // store the initial position
        if (initialPosUserID != playerID) {
            initialPosUserID = playerID;
            initialPosOffset = transform.position - (MoveVertically ? posPointMan : new Vector3(posPointMan.x, 0, posPointMan.z));
        }

        // transform.position = initialPosOffset + (MoveVertically ? posPointMan : new Vector3(posPointMan.x, 0, posPointMan.z));

        if (this.Asignment == ControlState.Hands) {
            int hand_L_Index = (int)KinectWrapper.SkeletonJoint.LEFT_HAND;
            int hand_R_Index = (int)KinectWrapper.SkeletonJoint.RIGHT_HAND;

            if (this.mainPlayer == null || this.mainPlayer.CanMoveHand_L()) {
                BoneInfo info = bInfo[hand_L_Index];
                moveBone(info, playerID, posPointMan, hand_L_Index);
            }

            if (this.mainPlayer == null || this.mainPlayer.CanMoveHand_R()) {
                BoneInfo info = bInfo[hand_R_Index];
                moveBone(info, playerID, posPointMan, hand_R_Index);
            }
        }

        if (this.Asignment == ControlState.Feet) {
            int hand_L_Index = (int)KinectWrapper.SkeletonJoint.LEFT_HAND;
            int hand_R_Index = (int)KinectWrapper.SkeletonJoint.RIGHT_HAND;
            int foot_L_Index = (int)KinectWrapper.SkeletonJoint.LEFT_FOOT;
            int foot_R_Index = (int)KinectWrapper.SkeletonJoint.RIGHT_FOOT;

            if (this.mainPlayer == null || this.mainPlayer.CanMoveFoot_L()) {
                BoneInfo info = bInfo[foot_L_Index];
                moveBone(info, playerID, posPointMan, hand_L_Index);
            }

            if (this.mainPlayer == null || this.mainPlayer.CanMoveFoot_R()) {
                BoneInfo info = bInfo[foot_R_Index];
                moveBone(info, playerID, posPointMan, hand_R_Index);
            }
        }
    }
}
