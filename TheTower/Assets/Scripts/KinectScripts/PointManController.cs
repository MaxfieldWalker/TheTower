using UnityEngine;
using System;
using System.Collections;

public class PointManController : MonoBehaviour
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
    public GameObject Head;
    public GameObject Neck;
    public GameObject Shoulder_Left;
    public GameObject Shoulder_Right;
    public GameObject Elbow_Left;
    public GameObject Elbow_Right;
    public GameObject Hand_Left;
    public GameObject Hand_Right;
    public GameObject Hip_Center;
    public GameObject Hip_Left;
    public GameObject Hip_Right;
    public GameObject Knee_Left;
    public GameObject Knee_Right;
    public GameObject Foot_Left;
    public GameObject Foot_Right;

    // 体の各部位を配列で管理する
    private GameObject[] bones;

    // 初期位置
    private Vector3 initialPosition;
    // 初期回転
    private Quaternion initialRotation;
    private Vector3 initialPosOffset = Vector3.zero;
    private uint initialPosUserID = 0;

    void Start()
    {
        bones = new GameObject[JointCount] {
            Head,
            Neck,
            Shoulder_Left,
            Shoulder_Right,
            Elbow_Left,
            Elbow_Right,
            Hand_Left,
            Hand_Right,
            Hip_Center,
            Hip_Left,
            Hip_Right,
            Knee_Left,
            Knee_Right,
            Foot_Left,
            Foot_Right
        };

        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    void Update()
    {
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

        transform.position = initialPosOffset + (MoveVertically ? posPointMan : new Vector3(posPointMan.x, 0, posPointMan.z));

        for (int i = 0; i < JointCount; i++)
        {
            GameObject bone = bones[i];

            // このジョイントはトラッキングされているか?
            if (KinectManager.Instance.IsJointPositionTracked(playerID, i))
            {
                bone.gameObject.SetActive(true);

                int joint = MirroredMovement ? KinectWrapper.GetSkeletonMirroredJoint(i) : i;
                // Kinectからジョイントの座標を取得する
                Vector3 posJoint = KinectManager.Instance.GetJointPosition(playerID, joint);
                // Kinectからジョイントの回転を取得する
                Quaternion rotJoint = KinectManager.Instance.GetJointOrientation(playerID, joint, !MirroredMovement);

                posJoint.x = MirroredMovement ? posJoint.x : -posJoint.x;
                posJoint.z = MirroredMovement ? posJoint.z : -posJoint.z;

                // ジョイントの座標からユーザーの座標を引いて相対座標にする
                posJoint -= posPointMan;

                // 体の各部位の座標と回転を設定する
                bone.transform.localPosition = posJoint;
                bone.transform.localRotation = rotJoint;
            }
            else
            {
                // トラッキングされていなければ非表示にする
                bone.gameObject.SetActive(false);
            }
        }
    }
}
