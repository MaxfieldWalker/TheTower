using UnityEngine;
using System;
using System.Collections;

public class BoneInfo
{
    private GameObject bone;
    private Vector3 initPostion;

    public GameObject Bone { get { return this.bone; } }

    public BoneInfo(GameObject bone)
    {
        if (bone != null)
        {
            this.bone = bone;
            this.initPostion = bone.transform.position;
        }
    }

    public void SetPosition(Vector3 pos)
    {

        this.bone.transform.localPosition = this.initPostion + new Vector3(-pos.x, pos.y, pos.z);
    }

    public void SetRotation(Quaternion q)
    {
        this.bone.transform.localRotation = q;
    }
}

public enum ControlAsignment
{
    Hands,
    Feet
}

public class MainCharacterKinectController : MonoBehaviour
{
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
    public ControlAsignment Asignment;

    // 初期位置
    private Vector3 initialPosition;
    // 初期回転
    private Quaternion initialRotation;
    private Vector3 initialPosOffset = Vector3.zero;
    private uint initialPosUserID = 0;

    void Start()
    {
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
    private void moveBone(BoneInfo info, uint playerID, Vector3 posPointMan, int i)
    {
        if (info.Bone == null) return;

        // このジョイントはトラッキングされているか?
        if (KinectManager.Instance.IsJointPositionTracked(playerID, i))
        {
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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            this.Asignment = ControlAsignment.Hands;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            this.Asignment = ControlAsignment.Feet;
        }

        // get 1st player
        uint playerID = KinectManager.Instance != null ? KinectManager.Instance.GetPlayer1ID() : 0;

        if (playerID <= 0)
        {
            // 位置と回転を初期値にリセットする
            if (transform.position != initialPosition) transform.position = initialPosition;
            if (transform.rotation != initialRotation) transform.rotation = initialRotation;
            return;
        }

        // Kinectからユーザーの座標(尻の座標)を取得する
        Vector3 posPointMan = KinectManager.Instance.GetUserPosition(playerID);
        posPointMan.z = MirroredMovement ? posPointMan.z : -posPointMan.z;

        // store the initial position
        if (initialPosUserID != playerID)
        {
            initialPosUserID = playerID;
            initialPosOffset = transform.position - (MoveVertically ? posPointMan : new Vector3(posPointMan.x, 0, posPointMan.z));
        }

        // transform.position = initialPosOffset + (MoveVertically ? posPointMan : new Vector3(posPointMan.x, 0, posPointMan.z));

        if (this.Asignment == ControlAsignment.Hands)
        {
            int hand_L_Index = (int)KinectWrapper.SkeletonJoint.LEFT_HAND;
            int hand_R_Index = (int)KinectWrapper.SkeletonJoint.RIGHT_HAND;

            int[] indexes = { hand_L_Index, hand_R_Index };
            foreach (int i in indexes)
            {
                BoneInfo info = bInfo[i];
                moveBone(info, playerID, posPointMan, i);
            }
        }

        if (this.Asignment == ControlAsignment.Feet)
        {
            int hand_L_Index = (int)KinectWrapper.SkeletonJoint.LEFT_HAND;
            int hand_R_Index = (int)KinectWrapper.SkeletonJoint.RIGHT_HAND;
            int foot_L_Index = (int)KinectWrapper.SkeletonJoint.LEFT_FOOT;
            int foot_R_Index = (int)KinectWrapper.SkeletonJoint.RIGHT_FOOT;

            BoneInfo info = bInfo[foot_L_Index];
            moveBone(info, playerID, posPointMan, hand_L_Index);

            info = bInfo[foot_R_Index];
            moveBone(info, playerID, posPointMan, hand_R_Index);
        }
    }
}
