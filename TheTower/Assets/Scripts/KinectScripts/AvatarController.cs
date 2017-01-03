using UnityEngine;

public class AvatarController : MonoBehaviour
{
    // 鏡に映したようにマッピングを反転させるか? (ユーザーの右手がキャラクターの左手にマッピングされる)
    // Bool that has the characters (facing the player) actions become mirrored. Default false.
    public bool MirroredMovement = false;

    // ジャンプのような縦方向の動きを許可するか
    // Bool that determines whether the avatar is allowed to jump -- vertical movement
    // can cause some models to behave strangely, so use at your own discretion.
    public bool VerticalMovement = false;

    // アバターの動きの度合いを調整する
    // Rate at which avatar will move through the scene. The rate multiplies the movement speed (.001f, i.e dividing by 1000, unity's framerate).
    private int MoveRate = 1;

    public Transform Hips;
    public Transform Neck;
    public Transform Head;

    public Transform LeftUpperArm;
    public Transform LeftElbow;
    public Transform LeftHand;

    public Transform RightUpperArm;
    public Transform RightElbow;
    public Transform RightHand;

    public Transform LeftThigh;
    public Transform LeftKnee;
    public Transform LeftFoot;

    public Transform RightThigh;
    public Transform RightKnee;
    public Transform RightFoot;

    // キャラクターのジョイントのルート?
    public Transform Root;

    /// <summary>
    /// キャラクターの基準になるオブジェクト
    /// A required variable if you want to rotate the model in space.
    /// </summary>
    public GameObject offsetNode;

    // Variable to hold all them bones. It will initialize the same size as initialRotations.
    private Transform[] bones;

    // Rotations of the bones when the Kinect tracking starts.
    private Quaternion[] initialRotations;

    // Calibration Offset Variables for Character Position.
    bool OffsetCalibrated = false;
    float XOffset, YOffset, ZOffset;
    Quaternion originalRotation;

    public void Start()
    {
        bones = new Transform[15];

        //体の各部位をbones配列に格納する
        MapBones();

        initialRotations = new Quaternion[15];

        // 回転の初期値を保持しておく
        CaptureInitialRotations();

        // キャラクターをキャリブレーションポーズにする
        RotateToCalibrationPose(0, KinectManager.IsCalibrationNeeded());
    }

    /// <summary>
    /// アバターのジョイントの位置や回転を更新する
    /// Update the avatar each frame.
    /// </summary>
    /// <param name="UserID"></param>
    /// <param name="IsNearMode"></param>
    public void UpdateAvatar(uint UserID, bool IsNearMode)
    {
        TransformBone(UserID, KinectWrapper.SkeletonJoint.HIPS, MirroredMovement);
        TransformBone(UserID, KinectWrapper.SkeletonJoint.NECK, MirroredMovement);
        TransformBone(UserID, KinectWrapper.SkeletonJoint.HEAD, MirroredMovement);

        TransformBone(UserID, KinectWrapper.SkeletonJoint.LEFT_SHOULDER, MirroredMovement);
        TransformBone(UserID, KinectWrapper.SkeletonJoint.LEFT_ELBOW, MirroredMovement);
        TransformBone(UserID, KinectWrapper.SkeletonJoint.LEFT_HAND, MirroredMovement);

        TransformBone(UserID, KinectWrapper.SkeletonJoint.RIGHT_SHOULDER, MirroredMovement);
        TransformBone(UserID, KinectWrapper.SkeletonJoint.RIGHT_ELBOW, MirroredMovement);
        TransformBone(UserID, KinectWrapper.SkeletonJoint.RIGHT_HAND, MirroredMovement);

        if (!IsNearMode)
        {
            TransformBone(UserID, KinectWrapper.SkeletonJoint.LEFT_HIP, MirroredMovement);
            TransformBone(UserID, KinectWrapper.SkeletonJoint.LEFT_KNEE, MirroredMovement);
            TransformBone(UserID, KinectWrapper.SkeletonJoint.LEFT_FOOT, MirroredMovement);

            TransformBone(UserID, KinectWrapper.SkeletonJoint.RIGHT_HIP, MirroredMovement);
            TransformBone(UserID, KinectWrapper.SkeletonJoint.RIGHT_KNEE, MirroredMovement);
            TransformBone(UserID, KinectWrapper.SkeletonJoint.RIGHT_FOOT, MirroredMovement);
        }

        MoveAvatar(UserID);
    }

    /// <summary>
    /// 回転を0,0,0にしてキャリブレーションポーズにする
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="needCalibration"></param>
    public void RotateToCalibrationPose(uint userId, bool needCalibration)
    {
        // Reset the rest of the model to the original position.
        RotateToInitialPosition();

        if (needCalibration)
        {
            if (offsetNode != null)
            {
                // Set the offset's rotation to 0.
                offsetNode.transform.rotation = Quaternion.Euler(Vector3.zero);
            }

            if (offsetNode != null)
            {
                // Restore the offset's rotation
                offsetNode.transform.rotation = originalRotation;
            }
        }
    }

    // Invoked on the successful calibration of a player.
    /// <summary>
    /// プレイヤーのキャリブレーションに成功した時に呼ばれます
    /// </summary>
    /// <param name="userId"></param>
    public void SuccessfulCalibration(uint userId)
    {
        // reset the models position
        if (offsetNode != null)
        {
            offsetNode.transform.rotation = originalRotation;
        }

        // re-calibrate the position offset
        OffsetCalibrated = false;
    }

    /// <summary>
    /// ジョイントに対応する番号を返します
    /// </summary>
    /// <param name="joint"></param>
    /// <param name="isMirrored"></param>
    /// <returns></returns>
    int GetJointIndex(KinectWrapper.SkeletonJoint joint, bool isMirrored)
    {
        if (isMirrored)
        {
            switch (joint)
            {
                case KinectWrapper.SkeletonJoint.LEFT_SHOULDER:
                    return (int)KinectWrapper.SkeletonJoint.RIGHT_SHOULDER;
                case KinectWrapper.SkeletonJoint.LEFT_ELBOW:
                    return (int)KinectWrapper.SkeletonJoint.RIGHT_ELBOW;
                case KinectWrapper.SkeletonJoint.LEFT_HAND:
                    return (int)KinectWrapper.SkeletonJoint.RIGHT_HAND;
                case KinectWrapper.SkeletonJoint.RIGHT_SHOULDER:
                    return (int)KinectWrapper.SkeletonJoint.LEFT_SHOULDER;
                case KinectWrapper.SkeletonJoint.RIGHT_ELBOW:
                    return (int)KinectWrapper.SkeletonJoint.LEFT_ELBOW;
                case KinectWrapper.SkeletonJoint.RIGHT_HAND:
                    return (int)KinectWrapper.SkeletonJoint.LEFT_HAND;
                case KinectWrapper.SkeletonJoint.LEFT_HIP:
                    return (int)KinectWrapper.SkeletonJoint.RIGHT_HIP;
                case KinectWrapper.SkeletonJoint.LEFT_KNEE:
                    return (int)KinectWrapper.SkeletonJoint.RIGHT_KNEE;
                case KinectWrapper.SkeletonJoint.LEFT_FOOT:
                    return (int)KinectWrapper.SkeletonJoint.RIGHT_FOOT;
                case KinectWrapper.SkeletonJoint.RIGHT_HIP:
                    return (int)KinectWrapper.SkeletonJoint.LEFT_HIP;
                case KinectWrapper.SkeletonJoint.RIGHT_KNEE:
                    return (int)KinectWrapper.SkeletonJoint.LEFT_KNEE;
                case KinectWrapper.SkeletonJoint.RIGHT_FOOT:
                    return (int)KinectWrapper.SkeletonJoint.LEFT_FOOT;
            }
        }

        return (int)joint;
    }

    /// <summary>
    /// Kinectが取得した情報をジョイントに適用する
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="joint"></param>
    /// <param name="isMirrored"></param>
    void TransformBone(uint userId, KinectWrapper.SkeletonJoint joint, bool isMirrored)
    {
        int boneIndex = GetJointIndex(joint, isMirrored);
        if (boneIndex < 0) return;

        Transform boneToTransform = bones[boneIndex];
        if (boneToTransform == null) return;

        // Grab the bone we're moving.
        int jointIndex = (int)joint;
        if (jointIndex < 0) return;

        // Kinectからジョイントの回転を取得する
        Quaternion jointRotation = KinectManager.Instance.GetJointOrientation(userId, jointIndex, !isMirrored);
        // Debug.Log(jointRotation);
        if (jointRotation == Quaternion.identity) return;

        // Apply the new rotation.
        Quaternion newRotation = jointRotation * initialRotations[boneIndex];

        //If an offset node is specified, combine the transform with its
        //orientation to essentially make the skeleton relative to the node
        // OffsetNoteが指定されている場合はOffsetNoteのtransfromを組み合わせて
        // ジョイントの回転を相対化する
        if (offsetNode != null)
        {
            // Grab the total rotation by adding the Euler and offset's Euler.
            Vector3 totalRotation = newRotation.eulerAngles + offsetNode.transform.rotation.eulerAngles;
            // Grab our new rotation.
            newRotation = Quaternion.Euler(totalRotation);
        }

        // Smoothly transition to our new rotation.
        boneToTransform.rotation = Quaternion.Slerp(boneToTransform.rotation, newRotation, Time.deltaTime * 3.0f);
        Debug.Log("Rotate! " + boneToTransform.name);
    }

    /// <summary>
    /// アバターを移動させる (位置のみ)
    /// Moves the avatar in 3D space - pulls the tracked position of the spine and applies it to root.
    /// </summary>
    /// <param name="UserID"></param>
    void MoveAvatar(uint UserID)
    {
        if (Root == null) return;

        // 尻はトラッキングされているか? 尻がトラッキングされていないと意味がない
        if (!KinectManager.Instance.IsJointPositionTracked(UserID, (int)KinectWrapper.SkeletonJoint.HIPS)) return;

        // Get the position of the body and store it.
        Vector3 userPosition = KinectManager.Instance.GetUserPosition(UserID);

        // If this is the first time we're moving the avatar, set the offset. Otherwise ignore it.
        // 初回だけ座標を相対化するためにx, y, z座標のオフセットを計算しておく
        if (!OffsetCalibrated)
        {
            OffsetCalibrated = true;

            XOffset = !MirroredMovement ? userPosition.x * MoveRate : -userPosition.x * MoveRate;
            YOffset = userPosition.y * MoveRate;
            ZOffset = -userPosition.z * MoveRate;
        }

        float xPos, yPos, zPos;

        // If movement is mirrored, reverse it.
        xPos = (MirroredMovement ? -userPosition.x : userPosition.x) * MoveRate - XOffset;
        yPos = userPosition.y * MoveRate - YOffset;
        zPos = -userPosition.z * MoveRate - ZOffset;

        // If we are tracking vertical movement, update the y. Otherwise leave it alone.
        Vector3 targetPosition = new Vector3(xPos, (VerticalMovement ? yPos : 0.0f), zPos);

        Root.localPosition = Vector3.Lerp(Root.localPosition, targetPosition, 3 * Time.deltaTime);
    }

    /// <summary>
    /// 体の各部位をbones配列に格納する
    /// </summary>
    void MapBones()
    {
        bones[(int)KinectWrapper.SkeletonJoint.HIPS] = Hips;
        bones[(int)KinectWrapper.SkeletonJoint.NECK] = Neck;
        bones[(int)KinectWrapper.SkeletonJoint.HEAD] = Head;

        bones[(int)KinectWrapper.SkeletonJoint.LEFT_SHOULDER] = LeftUpperArm;
        bones[(int)KinectWrapper.SkeletonJoint.LEFT_ELBOW] = LeftElbow;
        bones[(int)KinectWrapper.SkeletonJoint.LEFT_HAND] = LeftHand;

        bones[(int)KinectWrapper.SkeletonJoint.RIGHT_SHOULDER] = RightUpperArm;
        bones[(int)KinectWrapper.SkeletonJoint.RIGHT_ELBOW] = RightElbow;
        bones[(int)KinectWrapper.SkeletonJoint.RIGHT_HAND] = RightHand;

        bones[(int)KinectWrapper.SkeletonJoint.LEFT_HIP] = LeftThigh;
        bones[(int)KinectWrapper.SkeletonJoint.LEFT_KNEE] = LeftKnee;
        bones[(int)KinectWrapper.SkeletonJoint.LEFT_FOOT] = LeftFoot;

        bones[(int)KinectWrapper.SkeletonJoint.RIGHT_HIP] = RightThigh;
        bones[(int)KinectWrapper.SkeletonJoint.RIGHT_KNEE] = RightKnee;
        bones[(int)KinectWrapper.SkeletonJoint.RIGHT_FOOT] = RightFoot;
    }

    /// <summary>
    /// キャラクターの初期回転を保持する
    /// </summary>
    void CaptureInitialRotations()
    {
        if (offsetNode != null)
        {
            // Store the original offset's rotation.
            originalRotation = offsetNode.transform.rotation;
            // Set the offset's rotation to 0.
            offsetNode.transform.rotation = Quaternion.Euler(Vector3.zero);
        }

        for (int i = 0; i < bones.Length; i++)
            initialRotations[i] = bones[i].rotation;

        if (offsetNode != null)
        {
            // Restore the offset's rotation
            offsetNode.transform.rotation = originalRotation;
        }
    }

    /// <summary>
    /// ジョイントの回転を初期値に戻す
    /// </summary>
    public void RotateToInitialPosition()
    {
        if (bones == null) return;

        if (offsetNode != null)
        {
            // Set the offset's rotation to 0.
            offsetNode.transform.rotation = Quaternion.Euler(Vector3.zero);
        }

        for (int i = 0; i < bones.Length; i++)
        {
            if (bones[i] != null)
            {
                bones[i].rotation = initialRotations[i];
            }
        }

        if (Root != null)
        {
            Root.localPosition = Vector3.zero;
        }

        if (offsetNode != null)
        {
            offsetNode.transform.rotation = originalRotation;
        }
    }
}
