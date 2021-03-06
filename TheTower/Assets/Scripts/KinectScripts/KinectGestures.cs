﻿using UnityEngine;
using System.Collections.Generic;

public class KinectGestures
{

    public interface IGestureListener
    {
        /// <summary>
        /// 新規のユーザーが検知されトラッキングが開始された時に呼び出されます
        /// この時からKinectManager.DetectGesture()でジェスチャーの認識が可能になります
        /// Invoked when a new user is detected and tracking starts
        /// Here you can start gesture detection with KinectManager.DetectGesture()
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userIndex"></param>
        void UserDetected(uint userId, int userIndex);

        /// <summary>
        /// ユーザーがロストした時に呼び出されます
        /// Invoked when a user is lost
        /// Gestures for this user are cleared automatically, but you can free the used resources
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userIndex"></param>
        void UserLost(uint userId, int userIndex);

        /// <summary>
        /// ジェスチャーの途中で呼び出されます
        /// Invoked when a gesture is in progress
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userIndex"></param>
        /// <param name="gesture"></param>
        /// <param name="progress"></param>
        /// <param name="joint"></param>
        /// <param name="screenPos"></param>
        void GestureInProgress(
            uint userId,
            int userIndex,
            Gestures gesture,
            float progress,
            KinectWrapper.SkeletonJoint joint,
            Vector3 screenPos);

        /// <summary>
        /// ジェスチャーが完了した時に呼び出されます
        /// ジェスチャー検知が再開される必要があるときはtrue，その他の場合はfalseを返します
        /// Invoked if a gesture is completed.
        /// Returns true, if the gesture detection must be restarted, false otherwise
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userIndex"></param>
        /// <param name="gesture"></param>
        /// <param name="joint"></param>
        /// <param name="screenPos"></param>
        /// <returns></returns>
        bool GestureCompleted(
            uint userId,
            int userIndex,
            Gestures gesture,
            KinectWrapper.SkeletonJoint joint,
            Vector3 screenPos);

        /// <summary>
        /// ジェスチャーが途中でキャンセルされた時に呼び出されます
        /// ジェスチャー検知が再開される必要があるときはtrue，その他の場合はfalseを返します
        /// Invoked if a gesture is cancelled.
        /// Returns true, if the gesture detection must be retarted, false otherwise
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userIndex"></param>
        /// <param name="gesture"></param>
        /// <param name="joint"></param>
        /// <returns></returns>
        bool GestureCancelled(
            uint userId,
            int userIndex,
            Gestures gesture,
            KinectWrapper.SkeletonJoint joint);
    }


    public enum Gestures
    {
        None = 0,
        RaiseRightHand,
        RaiseLeftHand,
        Psi,
        Tpose,
        Stop,

        /// <summary>
        /// 手を左右に何度か振る
        /// </summary>
        Wave,

        Click,
        SwipeLeft,
        SwipeRight,
        SwipeUp,
        SwipeDown,
        RightHandCursor,
        LeftHandCursor,
        ZoomOut,
        ZoomIn,
        Wheel,
        Jump,
        Squat,
        Push,
        Pull
    }

    /// <summary>
    /// ジェスチャーに関する情報を格納する
    /// </summary>
    public struct GestureData
    {
        public uint userId;
        public Gestures gesture;

        /// <summary>
        /// ジェスチャーの状態遷移の状態番号
        /// </summary>
        public int state;

        /// <summary>
        /// タイムスタンプ
        /// </summary>
        public float timestamp;

        public int joint;
        public Vector3 jointPos;

        /// <summary>
        /// 肩と腰の4隅で出来る長方形を基準にした
        /// 手の位置の座標。成分の範囲は0 ~ 1
        /// </summary>
        public Vector3 handScreenPos_R;
        public Vector3 handScreenPos_L;

        public float tagFloat;

        /// <summary>
        /// screenPosの計算するのに使うベクトル
        /// x成分には腰の中心から肩までのx成分の差
        /// y成分には腰の中心のx成分が入る
        /// </summary>
        public Vector3 tagVector_R;

        public Vector3 tagVector_L;


        /// <summary>
        /// screenPosの計算するのに使うベクトル
        /// x成分には肩幅
        /// y成分には肩と尻の距離が入る
        /// </summary>
        public Vector3 tagVector2;

        /// <summary>
        /// プログレス(進捗)
        /// </summary>
        public float progress;

        /// <summary>
        /// ジェスチャーが完了しているかのフラグ
        /// </summary>
        public bool complete;

        /// <summary>
        /// ジェスチャーが失敗したかのフラグ
        /// </summary>
        public bool cancelled;

        /// <summary>
        /// ??
        /// </summary>
        public List<Gestures> checkForGestures;


        public float startTrackingAtTime;
    }

    private const int headIndex = (int)KinectWrapper.SkeletonJoint.HEAD;

    private const int leftHandIndex = (int)KinectWrapper.SkeletonJoint.LEFT_HAND;
    private const int rightHandIndex = (int)KinectWrapper.SkeletonJoint.RIGHT_HAND;

    private const int leftElbowIndex = (int)KinectWrapper.SkeletonJoint.LEFT_ELBOW;
    private const int rightElbowIndex = (int)KinectWrapper.SkeletonJoint.RIGHT_ELBOW;

    private const int leftShoulderIndex = (int)KinectWrapper.SkeletonJoint.LEFT_SHOULDER;
    private const int rightShoulderIndex = (int)KinectWrapper.SkeletonJoint.RIGHT_SHOULDER;

    private const int hipCenterIndex = (int)KinectWrapper.SkeletonJoint.HIPS;
    private const int shoulderCenterIndex = (int)KinectWrapper.SkeletonJoint.NECK;
    private const int leftHipIndex = (int)KinectWrapper.SkeletonJoint.LEFT_HIP;
    private const int rightHipIndex = (int)KinectWrapper.SkeletonJoint.RIGHT_HIP;


    private static int[] neededJointIndexes = {
        leftHandIndex,
        rightHandIndex,
        leftElbowIndex,
        rightElbowIndex,
        leftShoulderIndex,
        rightShoulderIndex,
        hipCenterIndex,
        shoulderCenterIndex,
        leftHipIndex,
        rightHipIndex
    };


    /// <summary>
    /// Returns the list of the needed gesture joint indexes
    /// </summary>
    /// <returns></returns>
    public static int[] GetNeededJointIndexes()
    {
        return neededJointIndexes;
    }

    private static void SetGestureJoint(ref GestureData gestureData, float timestamp, int joint, Vector3 jointPos)
    {
        gestureData.joint = joint;
        gestureData.jointPos = jointPos;
        gestureData.timestamp = timestamp;
        gestureData.state++;
    }

    private static void SetGestureCancelled(ref GestureData gestureData)
    {
        gestureData.state = 0;
        gestureData.progress = 0f;
        gestureData.cancelled = true;
    }

    /// <summary>
    /// ポーズ(姿勢)が完了しているかチェックする
    /// </summary>
    /// <param name="gestureData"></param>
    /// <param name="timestamp"></param>
    /// <param name="jointPos"></param>
    /// <param name="isInPose"></param>
    /// <param name="durationToComplete">ポーズが完了とするまでの時間</param>
    private static void CheckPoseComplete(
        ref GestureData gestureData,
        float timestamp,
        Vector3 jointPos,
        bool isInPose,
        float durationToComplete)
    {
        if (isInPose)
        {
            float timeDelta = timestamp - gestureData.timestamp;
            gestureData.progress = durationToComplete > 0f ? Mathf.Clamp01(timeDelta / durationToComplete) : 1.0f;

            if (timeDelta >= durationToComplete)
            {
                gestureData.timestamp = timestamp;
                gestureData.jointPos = jointPos;
                gestureData.state++;
                gestureData.complete = true;
            }
        }
        else
        {
            SetGestureCancelled(ref gestureData);
        }
    }

    private static void SetScreenPos_R(uint userId, ref GestureData gestureData, ref Vector3[] jointsPos, ref bool[] jointsTracked)
    {
        // 右手の位置を取得する
        Vector3 handPos = jointsPos[rightHandIndex];
        bool calculateCoords = gestureData.joint == rightHandIndex
                            && jointsTracked[rightHandIndex];

        // 右手がトラッキングされていなければ意味がない
        if (!calculateCoords) return;

        // 尻の中央と肩の左右中央は全てトラッキングされているか??
        if (jointsTracked[hipCenterIndex] && jointsTracked[shoulderCenterIndex] &&
            jointsTracked[leftShoulderIndex] && jointsTracked[rightShoulderIndex])
        {
            // 肩と尻の位置の差を計算する
            Vector3 neckToHips = jointsPos[shoulderCenterIndex] - jointsPos[hipCenterIndex];
            // 右肩と左肩の位置の差を計算する
            Vector3 rightToLeft = jointsPos[rightShoulderIndex] - jointsPos[leftShoulderIndex];
            // 頭と尻の位置の差を計算する
            Vector3 headToHips = jointsPos[headIndex] - jointsPos[hipCenterIndex];

            gestureData.tagVector2.x = rightToLeft.x;
            gestureData.tagVector2.y = neckToHips.y;
            gestureData.tagVector2.z = headToHips.z / 3.5f;

            if (gestureData.joint == rightHandIndex)
            {
                gestureData.tagVector_R.x = jointsPos[rightShoulderIndex].x - gestureData.tagVector2.x / 2;
                gestureData.tagVector_R.y = jointsPos[hipCenterIndex].y;
                gestureData.tagVector_R.z = jointsPos[rightShoulderIndex].z;
            }
        }

        if (gestureData.tagVector2.x != 0 && gestureData.tagVector2.y != 0)
        {
            Vector3 relHandPos = handPos - gestureData.tagVector_R;
            // Mathf.Clamp01は0未満なら0，0以上1未満ならそのまま，1以上なら1が返る
            gestureData.handScreenPos_R.x = Mathf.Clamp01(relHandPos.x / gestureData.tagVector2.x);
            gestureData.handScreenPos_R.y = Mathf.Clamp01(relHandPos.y / gestureData.tagVector2.y);
            gestureData.handScreenPos_R.z = Mathf.Clamp01(relHandPos.z / gestureData.tagVector2.z);
        }
    }


    private static void SetScreenPos_L(uint userId, ref GestureData gestureData, ref Vector3[] jointsPos, ref bool[] jointsTracked)
    {
        // 左手の位置を取得する
        Vector3 handPos = jointsPos[leftHandIndex];
        bool calculateCoords = gestureData.joint == leftHandIndex
                            && jointsTracked[leftHandIndex];

        // 左手がトラッキングされていなければ意味がない
        if (!calculateCoords) return;

        // 尻の中央と肩の左右中央は全てトラッキングされているか??
        if (jointsTracked[hipCenterIndex] && jointsTracked[shoulderCenterIndex] &&
            jointsTracked[leftShoulderIndex] && jointsTracked[rightShoulderIndex])
        {
            // 肩と尻の位置の差を計算する
            Vector3 neckToHips = jointsPos[shoulderCenterIndex] - jointsPos[hipCenterIndex];
            // 右肩と左肩の位置の差を計算する
            Vector3 rightToLeft = jointsPos[rightShoulderIndex] - jointsPos[leftShoulderIndex];
            // 頭と尻の位置の差を計算する
            Vector3 headToHips = jointsPos[headIndex] - jointsPos[hipCenterIndex];

            gestureData.tagVector2.x = rightToLeft.x;
            gestureData.tagVector2.y = neckToHips.y;
            gestureData.tagVector2.z = headToHips.z / 3.5f;

            if (gestureData.joint == leftHandIndex)
            {
                gestureData.tagVector_L.x = jointsPos[leftShoulderIndex].x - gestureData.tagVector2.x / 2;
                gestureData.tagVector_L.y = jointsPos[hipCenterIndex].y;
                gestureData.tagVector_L.z = jointsPos[leftShoulderIndex].z;
            }
        }

        if (gestureData.tagVector2.x != 0 && gestureData.tagVector2.y != 0)
        {
            Vector3 relHandPos = handPos - gestureData.tagVector_L;
            // Mathf.Clamp01は0未満なら0，0以上1未満ならそのまま，1以上なら1が返る
            gestureData.handScreenPos_L.x = Mathf.Clamp01(relHandPos.x / gestureData.tagVector2.x);
            gestureData.handScreenPos_L.y = Mathf.Clamp01(relHandPos.y / gestureData.tagVector2.y);
            gestureData.handScreenPos_L.z = Mathf.Clamp01(relHandPos.z / gestureData.tagVector2.z);
        }
    }

    private static void SetZoomFactor(uint userId, ref GestureData gestureData, float initialZoom, ref Vector3[] jointsPos, ref bool[] jointsTracked)
    {
        Vector3 vectorZooming = jointsPos[rightHandIndex] - jointsPos[leftHandIndex];

        if (gestureData.tagFloat == 0f || gestureData.userId != userId)
        {
            gestureData.tagFloat = 0.5f; // this is 100%
        }

        float distZooming = vectorZooming.magnitude;
        gestureData.handScreenPos_R.z = initialZoom + (distZooming / gestureData.tagFloat);
    }

    private static void SetWheelRotation(uint userId, ref GestureData gestureData, Vector3 initialPos, Vector3 currentPos)
    {
        float angle = Vector3.Angle(initialPos, currentPos) * Mathf.Sign(currentPos.y - initialPos.y);
        gestureData.handScreenPos_R.z = angle;
    }

    // estimate the next state and completeness of the gesture
    public static void CheckForGesture(uint userId, ref GestureData gestureData, float timestamp, ref Vector3[] jointsPos, ref bool[] jointsTracked)
    {
        if (gestureData.complete)
            return;

        switch (gestureData.gesture)
        {
            // 右腕が上がっているか判定する
            // check for RaiseRightHand
            case Gestures.RaiseRightHand:
                // ジェスチャーの状態遷移
                // 右腕を上げるというジェスチャーは2つの状態遷移で表す
                switch (gestureData.state)
                {
                    case 0:  // gesture detection
                             // 右手と右型のジョイントがトラックされている かつ
                             // 右手の高さが右肩の高さより高い
                        if (jointsTracked[rightHandIndex] && jointsTracked[rightShoulderIndex] &&
                           (jointsPos[rightHandIndex].y - jointsPos[rightShoulderIndex].y) > 0.1f)
                        {
                            // 状態を1つ進める
                            SetGestureJoint(ref gestureData, timestamp, rightHandIndex, jointsPos[rightHandIndex]);
                        }
                        break;

                    case 1:  // gesture complete
                        // 右手の高さが右肩の高さより高いか?
                        bool isInPose = jointsTracked[rightHandIndex] && jointsTracked[rightShoulderIndex] &&
                            (jointsPos[rightHandIndex].y - jointsPos[rightShoulderIndex].y) > 0.1f;

                        Vector3 jointPos = jointsPos[gestureData.joint];
                        CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, KinectWrapper.Constants.PoseCompleteDuration);
                        break;
                }
                break;

            // check for RaiseLeftHand
            case Gestures.RaiseLeftHand:
                switch (gestureData.state)
                {
                    case 0:  // gesture detection
                        if (jointsTracked[leftHandIndex] && jointsTracked[leftShoulderIndex] &&
                                (jointsPos[leftHandIndex].y - jointsPos[leftShoulderIndex].y) > 0.1f)
                        {
                            SetGestureJoint(ref gestureData, timestamp, leftHandIndex, jointsPos[leftHandIndex]);
                        }
                        break;

                    case 1:  // gesture complete
                        bool isInPose = jointsTracked[leftHandIndex] && jointsTracked[leftShoulderIndex] &&
                            (jointsPos[leftHandIndex].y - jointsPos[leftShoulderIndex].y) > 0.1f;

                        Vector3 jointPos = jointsPos[gestureData.joint];
                        CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, KinectWrapper.Constants.PoseCompleteDuration);
                        break;
                }
                break;

            // check for Psi
            case Gestures.Psi:
                switch (gestureData.state)
                {
                    case 0:  // gesture detection
                        if (jointsTracked[rightHandIndex] && jointsTracked[rightShoulderIndex] &&
                           (jointsPos[rightHandIndex].y - jointsPos[rightShoulderIndex].y) > 0.1f &&
                           jointsTracked[leftHandIndex] && jointsTracked[leftShoulderIndex] &&
                           (jointsPos[leftHandIndex].y - jointsPos[leftShoulderIndex].y) > 0.1f)
                        {
                            SetGestureJoint(ref gestureData, timestamp, rightHandIndex, jointsPos[rightHandIndex]);
                        }
                        break;

                    case 1:  // gesture complete
                        bool isInPose = jointsTracked[rightHandIndex] && jointsTracked[rightShoulderIndex] &&
                            (jointsPos[rightHandIndex].y - jointsPos[rightShoulderIndex].y) > 0.1f &&
                            jointsTracked[leftHandIndex] && jointsTracked[leftShoulderIndex] &&
                            (jointsPos[leftHandIndex].y - jointsPos[leftShoulderIndex].y) > 0.1f;

                        Vector3 jointPos = jointsPos[gestureData.joint];
                        CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, KinectWrapper.Constants.PoseCompleteDuration);
                        break;
                }
                break;


            // check for Tpose (3DキャラクタによくとらせるT字型の姿勢)
            case Gestures.Tpose:
                switch (gestureData.state)
                {
                    case 0:  // gesture detection
                        if (jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] && jointsTracked[rightShoulderIndex] &&
                           Mathf.Abs(jointsPos[rightElbowIndex].y - jointsPos[rightShoulderIndex].y) < 0.1f &&  // 0.07f
                           Mathf.Abs(jointsPos[rightHandIndex].y - jointsPos[rightShoulderIndex].y) < 0.1f &&  // 0.7f
                           jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] && jointsTracked[leftShoulderIndex] &&
                           Mathf.Abs(jointsPos[leftElbowIndex].y - jointsPos[leftShoulderIndex].y) < 0.1f &&
                           Mathf.Abs(jointsPos[leftHandIndex].y - jointsPos[leftShoulderIndex].y) < 0.1f)
                        {
                            SetGestureJoint(ref gestureData, timestamp, rightHandIndex, jointsPos[rightHandIndex]);
                        }
                        break;

                    case 1:  // gesture complete
                        bool isInPose = jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] && jointsTracked[rightShoulderIndex] &&
                            Mathf.Abs(jointsPos[rightElbowIndex].y - jointsPos[rightShoulderIndex].y) < 0.1f &&  // 0.7f
                                Mathf.Abs(jointsPos[rightHandIndex].y - jointsPos[rightShoulderIndex].y) < 0.1f &&  // 0.7f
                                jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] && jointsTracked[leftShoulderIndex] &&
                                Mathf.Abs(jointsPos[leftElbowIndex].y - jointsPos[leftShoulderIndex].y) < 0.1f &&
                                Mathf.Abs(jointsPos[leftHandIndex].y - jointsPos[leftShoulderIndex].y) < 0.1f;

                        Vector3 jointPos = jointsPos[gestureData.joint];
                        CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, KinectWrapper.Constants.PoseCompleteDuration);
                        break;
                }
                break;

            // check for Stop
            case Gestures.Stop:
                switch (gestureData.state)
                {
                    case 0:  // gesture detection
                        if (jointsTracked[rightHandIndex] && jointsTracked[rightHipIndex] &&
                           (jointsPos[rightHandIndex].y - jointsPos[rightHipIndex].y) < 0f &&
                           jointsTracked[leftHandIndex] && jointsTracked[leftHipIndex] &&
                           (jointsPos[leftHandIndex].y - jointsPos[leftHipIndex].y) < 0f)
                        {
                            SetGestureJoint(ref gestureData, timestamp, rightHandIndex, jointsPos[rightHandIndex]);
                        }
                        break;

                    case 1:  // gesture complete
                        bool isInPose = jointsTracked[rightHandIndex] && jointsTracked[rightHipIndex] &&
                            (jointsPos[rightHandIndex].y - jointsPos[rightHipIndex].y) < 0f &&
                            jointsTracked[leftHandIndex] && jointsTracked[leftHipIndex] &&
                            (jointsPos[leftHandIndex].y - jointsPos[leftHipIndex].y) < 0f;

                        Vector3 jointPos = jointsPos[gestureData.joint];
                        CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, KinectWrapper.Constants.PoseCompleteDuration);
                        break;
                }
                break;

            // check for Wave (腕を左右に振る)
            case Gestures.Wave:
                switch (gestureData.state)
                {
                    case 0:  // gesture detection - phase 1
                        if (jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] &&
                           (jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) > 0.1f &&
                           (jointsPos[rightHandIndex].x - jointsPos[rightElbowIndex].x) > 0.05f)
                        {
                            SetGestureJoint(ref gestureData, timestamp, rightHandIndex, jointsPos[rightHandIndex]);
                            gestureData.progress = 0.3f;
                        }
                        else if (jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
                                (jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) > 0.1f &&
                                (jointsPos[leftHandIndex].x - jointsPos[leftElbowIndex].x) < -0.05f)
                        {
                            SetGestureJoint(ref gestureData, timestamp, leftHandIndex, jointsPos[leftHandIndex]);
                            gestureData.progress = 0.3f;
                        }
                        break;

                    case 1:  // gesture - phase 2
                        if ((timestamp - gestureData.timestamp) < 1.5f)
                        {
                            bool isInPose = gestureData.joint == rightHandIndex ?
                                jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] &&
                                (jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) > 0.1f &&
                                (jointsPos[rightHandIndex].x - jointsPos[rightElbowIndex].x) < -0.05f :
                                jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
                                (jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) > 0.1f &&
                                (jointsPos[leftHandIndex].x - jointsPos[leftElbowIndex].x) > 0.05f;

                            if (isInPose)
                            {
                                gestureData.timestamp = timestamp;
                                gestureData.state++;
                                gestureData.progress = 0.7f;
                            }
                        }
                        else
                        {
                            // cancel the gesture
                            SetGestureCancelled(ref gestureData);
                        }
                        break;

                    case 2:  // gesture phase 3 = complete
                        if ((timestamp - gestureData.timestamp) < 1.5f)
                        {
                            bool isInPose = gestureData.joint == rightHandIndex ?
                                jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] &&
                                (jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) > 0.1f &&
                                (jointsPos[rightHandIndex].x - jointsPos[rightElbowIndex].x) > 0.05f :
                                jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
                                (jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) > 0.1f &&
                                (jointsPos[leftHandIndex].x - jointsPos[leftElbowIndex].x) < -0.05f;

                            if (isInPose)
                            {
                                Vector3 jointPos = jointsPos[gestureData.joint];
                                CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 0f);
                            }
                        }
                        else
                        {
                            // cancel the gesture
                            SetGestureCancelled(ref gestureData);
                        }
                        break;
                }
                break;

            // check for Click
            case Gestures.Click:
                switch (gestureData.state)
                {
                    case 0:  // gesture detection - phase 1
                        if (jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] &&
                           (jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) > -0.1f)
                        {
                            SetGestureJoint(ref gestureData, timestamp, rightHandIndex, jointsPos[rightHandIndex]);
                            gestureData.progress = 0.3f;

                            // set screen position at the start, because this is the most accurate click position
                            SetScreenPos_R(userId, ref gestureData, ref jointsPos, ref jointsTracked);
                        }
                        else if (jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
                                (jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) > -0.1f)
                        {
                            SetGestureJoint(ref gestureData, timestamp, leftHandIndex, jointsPos[leftHandIndex]);
                            gestureData.progress = 0.3f;

                            // set screen position at the start, because this is the most accurate click position
                            SetScreenPos_R(userId, ref gestureData, ref jointsPos, ref jointsTracked);
                        }
                        break;

                    case 1:  // gesture - phase 2
                        {
                            // check for stay-in-place
                            Vector3 distVector = jointsPos[gestureData.joint] - gestureData.jointPos;
                            bool isInPose = distVector.magnitude < 0.05f;

                            Vector3 jointPos = jointsPos[gestureData.joint];
                            CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, KinectWrapper.Constants.ClickStayDuration);
                        }
                        break;
                }
                break;

            // check for SwipeLeft
            case Gestures.SwipeLeft:
                switch (gestureData.state)
                {
                    case 0:  // gesture detection - phase 1
                        if (jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] &&
                           (jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) > -0.05f &&
                           (jointsPos[rightHandIndex].x - jointsPos[rightElbowIndex].x) > 0f)
                        {
                            SetGestureJoint(ref gestureData, timestamp, rightHandIndex, jointsPos[rightHandIndex]);
                            gestureData.progress = 0.5f;
                        }
                        break;

                    case 1:  // gesture phase 2 = complete
                        if ((timestamp - gestureData.timestamp) < 1.5f)
                        {
                            bool isInPose = gestureData.joint == rightHandIndex ?
                                jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] &&
                                Mathf.Abs(jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) < 0.1f &&
                                Mathf.Abs(jointsPos[rightHandIndex].y - gestureData.jointPos.y) < 0.08f &&
                                (jointsPos[rightHandIndex].x - gestureData.jointPos.x) < -0.15f :
                                jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
                                Mathf.Abs(jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) < 0.1f &&
                                Mathf.Abs(jointsPos[leftHandIndex].y - gestureData.jointPos.y) < 0.08f &&
                                (jointsPos[leftHandIndex].x - gestureData.jointPos.x) < -0.15f;

                            if (isInPose)
                            {
                                Vector3 jointPos = jointsPos[gestureData.joint];
                                CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 0f);
                            }
                        }
                        else
                        {
                            // cancel the gesture
                            SetGestureCancelled(ref gestureData);
                        }
                        break;
                }
                break;

            // check for SwipeRight
            case Gestures.SwipeRight:
                switch (gestureData.state)
                {
                    case 0:  // gesture detection - phase 1
                        if (jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
                                (jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) > -0.05f &&
                                (jointsPos[leftHandIndex].x - jointsPos[leftElbowIndex].x) < 0f)
                        {
                            SetGestureJoint(ref gestureData, timestamp, leftHandIndex, jointsPos[leftHandIndex]);
                            gestureData.progress = 0.5f;
                        }
                        break;

                    case 1:  // gesture phase 2 = complete
                        if ((timestamp - gestureData.timestamp) < 1.5f)
                        {
                            bool isInPose = gestureData.joint == rightHandIndex ?
                                jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] &&
                                Mathf.Abs(jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) < 0.1f &&
                                Mathf.Abs(jointsPos[rightHandIndex].y - gestureData.jointPos.y) < 0.08f &&
                                (jointsPos[rightHandIndex].x - gestureData.jointPos.x) > 0.15f :
                                jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
                                Mathf.Abs(jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) < 0.1f &&
                                Mathf.Abs(jointsPos[leftHandIndex].y - gestureData.jointPos.y) < 0.08f &&
                                (jointsPos[leftHandIndex].x - gestureData.jointPos.x) > 0.15f;

                            if (isInPose)
                            {
                                Vector3 jointPos = jointsPos[gestureData.joint];
                                CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 0f);
                            }
                        }
                        else
                        {
                            // cancel the gesture
                            SetGestureCancelled(ref gestureData);
                        }
                        break;
                }
                break;

            // check for SwipeUp
            case Gestures.SwipeUp:
                switch (gestureData.state)
                {
                    case 0:  // gesture detection - phase 1
                        if (jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] &&
                           (jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) < -0.05f &&
                           (jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) > -0.15f)
                        {
                            SetGestureJoint(ref gestureData, timestamp, rightHandIndex, jointsPos[rightHandIndex]);
                            gestureData.progress = 0.5f;
                        }
                        else if (jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
                                (jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) < -0.05f &&
                                (jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) > -0.15f)
                        {
                            SetGestureJoint(ref gestureData, timestamp, leftHandIndex, jointsPos[leftHandIndex]);
                            gestureData.progress = 0.5f;
                        }
                        break;

                    case 1:  // gesture phase 2 = complete
                        if ((timestamp - gestureData.timestamp) < 1.5f)
                        {
                            bool isInPose = gestureData.joint == rightHandIndex ?
                                jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] && jointsTracked[leftShoulderIndex] &&
                                //(jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) > 0.1f && 
                                //(jointsPos[rightHandIndex].y - gestureData.jointPos.y) > 0.15f && 
                                (jointsPos[rightHandIndex].y - jointsPos[leftShoulderIndex].y) > 0.05f &&
                                Mathf.Abs(jointsPos[rightHandIndex].x - gestureData.jointPos.x) < 0.08f :
                                jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] && jointsTracked[rightShoulderIndex] &&
                                //(jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) > 0.1f &&
                                //(jointsPos[leftHandIndex].y - gestureData.jointPos.y) > 0.15f && 
                                (jointsPos[leftHandIndex].y - jointsPos[rightShoulderIndex].y) > 0.05f &&
                                Mathf.Abs(jointsPos[leftHandIndex].x - gestureData.jointPos.x) < 0.08f;

                            if (isInPose)
                            {
                                Vector3 jointPos = jointsPos[gestureData.joint];
                                CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 0f);
                            }
                        }
                        else
                        {
                            // cancel the gesture
                            SetGestureCancelled(ref gestureData);
                        }
                        break;
                }
                break;

            // check for SwipeDown
            case Gestures.SwipeDown:
                switch (gestureData.state)
                {
                    case 0:  // gesture detection - phase 1
                        if (jointsTracked[rightHandIndex] && jointsTracked[leftShoulderIndex] &&
                           (jointsPos[rightHandIndex].y - jointsPos[leftShoulderIndex].y) >= 0.05f)
                        {
                            SetGestureJoint(ref gestureData, timestamp, rightHandIndex, jointsPos[rightHandIndex]);
                            gestureData.progress = 0.5f;
                        }
                        else if (jointsTracked[leftHandIndex] && jointsTracked[rightShoulderIndex] &&
                                (jointsPos[leftHandIndex].y - jointsPos[rightShoulderIndex].y) >= 0.05f)
                        {
                            SetGestureJoint(ref gestureData, timestamp, leftHandIndex, jointsPos[leftHandIndex]);
                            gestureData.progress = 0.5f;
                        }
                        break;

                    case 1:  // gesture phase 2 = complete
                        if ((timestamp - gestureData.timestamp) < 1.5f)
                        {
                            bool isInPose = gestureData.joint == rightHandIndex ?
                                jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] &&
                                //(jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) < -0.1f && 
                                (jointsPos[rightHandIndex].y - gestureData.jointPos.y) < -0.2f &&
                                Mathf.Abs(jointsPos[rightHandIndex].x - gestureData.jointPos.x) < 0.08f :
                                jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
                                //(jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) < -0.1f &&
                                (jointsPos[leftHandIndex].y - gestureData.jointPos.y) < -0.2f &&
                                Mathf.Abs(jointsPos[leftHandIndex].x - gestureData.jointPos.x) < 0.08f;

                            if (isInPose)
                            {
                                Vector3 jointPos = jointsPos[gestureData.joint];
                                CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 0f);
                            }
                        }
                        else
                        {
                            // cancel the gesture
                            SetGestureCancelled(ref gestureData);
                        }
                        break;
                }
                break;

            // check for RightHandCursor
            case Gestures.RightHandCursor:
                switch (gestureData.state)
                {
                    case 0:  // gesture detection - phase 1 (perpetual)
                        if (jointsTracked[rightHandIndex] && jointsTracked[rightHipIndex] &&
                            (jointsPos[rightHandIndex].y - jointsPos[rightHipIndex].y) > -0.1f)
                        {
                            gestureData.joint = rightHandIndex;
                            gestureData.timestamp = timestamp;
                            SetScreenPos_R(userId, ref gestureData, ref jointsPos, ref jointsTracked);
                            gestureData.progress = 0.7f;
                        }
                        else
                        {
                            // cancel the gesture
                            gestureData.progress = 0f;
                        }
                        break;

                }
                break;

            // check for LeftHandCursor
            case Gestures.LeftHandCursor:
                switch (gestureData.state)
                {
                    case 0:  // gesture detection - phase 1 (perpetual)
                        if (jointsTracked[leftHandIndex] && jointsTracked[leftHipIndex] &&
                            (jointsPos[leftHandIndex].y - jointsPos[leftHipIndex].y) > -0.1f)
                        {
                            gestureData.joint = leftHandIndex;
                            gestureData.timestamp = timestamp;
                            SetScreenPos_L(userId, ref gestureData, ref jointsPos, ref jointsTracked);
                            gestureData.progress = 0.7f;
                        }
                        else
                        {
                            // cancel the gesture
                            gestureData.progress = 0f;
                        }
                        break;
                }
                break;

            // check for ZoomOut
            case Gestures.ZoomOut:
                switch (gestureData.state)
                {
                    case 0:  // gesture detection - phase 1
                        float distZoomOut = ((Vector3)(jointsPos[rightHandIndex] - jointsPos[leftHandIndex])).magnitude;

                        if (jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
                           jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] &&
                           (jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) > 0f &&
                           (jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) > 0f &&
                           distZoomOut < 0.2f)
                        {
                            SetGestureJoint(ref gestureData, timestamp, rightHandIndex, jointsPos[rightHandIndex]);
                            gestureData.progress = 0.3f;
                        }
                        break;

                    case 1:  // gesture phase 2 = zooming
                        if ((timestamp - gestureData.timestamp) < 1.0f)
                        {
                            bool isInPose = jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
                                   jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] &&
                                ((jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) > 0f ||
                                   (jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) > 0f);

                            if (isInPose)
                            {
                                SetZoomFactor(userId, ref gestureData, 1.0f, ref jointsPos, ref jointsTracked);
                                gestureData.timestamp = timestamp;
                                gestureData.progress = 0.7f;
                            }
                        }
                        else
                        {
                            // cancel the gesture
                            SetGestureCancelled(ref gestureData);
                        }
                        break;
                }
                break;

            // check for ZoomIn
            case Gestures.ZoomIn:
                switch (gestureData.state)
                {
                    case 0:  // gesture detection - phase 1
                        float distZoomIn = ((Vector3)jointsPos[rightHandIndex] - jointsPos[leftHandIndex]).magnitude;

                        if (jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
                           jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] &&
                           (jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) > 0f &&
                           (jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) > 0f &&
                           distZoomIn >= 0.7f)
                        {
                            SetGestureJoint(ref gestureData, timestamp, rightHandIndex, jointsPos[rightHandIndex]);
                            gestureData.tagFloat = distZoomIn;
                            gestureData.progress = 0.3f;
                        }
                        break;

                    case 1:  // gesture phase 2 = zooming
                        if ((timestamp - gestureData.timestamp) < 1.0f)
                        {
                            bool isInPose = jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
                                   jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] &&
                                ((jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) > 0f ||
                                   (jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) > 0f);

                            if (isInPose)
                            {
                                SetZoomFactor(userId, ref gestureData, 0.0f, ref jointsPos, ref jointsTracked);
                                gestureData.timestamp = timestamp;
                                gestureData.progress = 0.7f;
                            }
                        }
                        else
                        {
                            // cancel the gesture
                            SetGestureCancelled(ref gestureData);
                        }
                        break;
                }
                break;

            // check for Wheel
            case Gestures.Wheel:
                Vector3 vectorWheel = (Vector3)jointsPos[rightHandIndex] - jointsPos[leftHandIndex];
                float distWheel = vectorWheel.magnitude;

                switch (gestureData.state)
                {
                    case 0:  // gesture detection - phase 1
                        if (jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
                           jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] &&
                           (jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) > 0f &&
                           (jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) > 0f &&
                           distWheel > 0.2f && distWheel < 0.7f)
                        {
                            SetGestureJoint(ref gestureData, timestamp, rightHandIndex, jointsPos[rightHandIndex]);
                            gestureData.tagVector_R = vectorWheel;
                            gestureData.tagFloat = distWheel;
                            gestureData.progress = 0.3f;
                        }
                        break;

                    case 1:  // gesture phase 2 = zooming
                        if ((timestamp - gestureData.timestamp) < 1.5f)
                        {
                            bool isInPose = jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
                                   jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] &&
                                ((jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) > 0f ||
                                   (jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) > 0f &&
                                Mathf.Abs(distWheel - gestureData.tagFloat) < 0.1f);

                            if (isInPose)
                            {
                                SetWheelRotation(userId, ref gestureData, gestureData.tagVector_R, vectorWheel);
                                gestureData.timestamp = timestamp;
                                gestureData.tagFloat = distWheel;
                                gestureData.progress = 0.7f;
                            }
                        }
                        else
                        {
                            // cancel the gesture
                            SetGestureCancelled(ref gestureData);
                        }
                        break;
                }
                break;

            // check for Jump
            case Gestures.Jump:
                switch (gestureData.state)
                {
                    case 0:  // gesture detection - phase 1
                        if (jointsTracked[hipCenterIndex] &&
                            (jointsPos[hipCenterIndex].y > 0.9f) && (jointsPos[hipCenterIndex].y < 1.3f))
                        {
                            SetGestureJoint(ref gestureData, timestamp, hipCenterIndex, jointsPos[hipCenterIndex]);
                            gestureData.progress = 0.5f;
                        }
                        break;

                    case 1:  // gesture phase 2 = complete
                        if ((timestamp - gestureData.timestamp) < 1.5f)
                        {
                            bool isInPose = jointsTracked[hipCenterIndex] &&
                                (jointsPos[hipCenterIndex].y - gestureData.jointPos.y) > 0.15f &&
                                Mathf.Abs(jointsPos[hipCenterIndex].x - gestureData.jointPos.x) < 0.15f;

                            if (isInPose)
                            {
                                Vector3 jointPos = jointsPos[gestureData.joint];
                                CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 0f);
                            }
                        }
                        else
                        {
                            // cancel the gesture
                            SetGestureCancelled(ref gestureData);
                        }
                        break;
                }
                break;

            // check for Squat
            case Gestures.Squat:
                switch (gestureData.state)
                {
                    case 0:  // gesture detection - phase 1
                        if (jointsTracked[hipCenterIndex] &&
                            (jointsPos[hipCenterIndex].y < 0.9f))
                        {
                            SetGestureJoint(ref gestureData, timestamp, hipCenterIndex, jointsPos[hipCenterIndex]);
                            gestureData.progress = 0.5f;
                        }
                        break;

                    case 1:  // gesture phase 2 = complete
                        if ((timestamp - gestureData.timestamp) < 1.5f)
                        {
                            bool isInPose = jointsTracked[hipCenterIndex] &&
                                (jointsPos[hipCenterIndex].y - gestureData.jointPos.y) < -0.15f &&
                                Mathf.Abs(jointsPos[hipCenterIndex].x - gestureData.jointPos.x) < 0.15f;

                            if (isInPose)
                            {
                                Vector3 jointPos = jointsPos[gestureData.joint];
                                CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 0f);
                            }
                        }
                        else
                        {
                            // cancel the gesture
                            SetGestureCancelled(ref gestureData);
                        }
                        break;
                }
                break;

            // check for Push
            case Gestures.Push:
                switch (gestureData.state)
                {
                    case 0:  // gesture detection - phase 1
                        if (jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] &&
                           (jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) > -0.05f &&
                           Mathf.Abs(jointsPos[rightHandIndex].x - jointsPos[rightElbowIndex].x) < 0.15f &&
                           (jointsPos[rightHandIndex].z - jointsPos[rightElbowIndex].z) < -0.05f)
                        {
                            SetGestureJoint(ref gestureData, timestamp, rightHandIndex, jointsPos[rightHandIndex]);
                            gestureData.progress = 0.5f;
                        }
                        else if (jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
                                (jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) > -0.05f &&
                                Mathf.Abs(jointsPos[leftHandIndex].x - jointsPos[leftElbowIndex].x) < 0.15f &&
                                (jointsPos[leftHandIndex].z - jointsPos[leftElbowIndex].z) < -0.05f)
                        {
                            SetGestureJoint(ref gestureData, timestamp, leftHandIndex, jointsPos[leftHandIndex]);
                            gestureData.progress = 0.5f;
                        }
                        break;

                    case 1:  // gesture phase 2 = complete
                        if ((timestamp - gestureData.timestamp) < 1.5f)
                        {
                            bool isInPose = gestureData.joint == rightHandIndex ?
                                jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] &&
                                Mathf.Abs(jointsPos[rightHandIndex].x - gestureData.jointPos.x) < 0.15f &&
                                Mathf.Abs(jointsPos[rightHandIndex].y - gestureData.jointPos.y) < 0.15f &&
                                (jointsPos[rightHandIndex].z - gestureData.jointPos.z) < -0.15f :
                                jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
                                Mathf.Abs(jointsPos[leftHandIndex].x - gestureData.jointPos.x) < 0.15f &&
                                Mathf.Abs(jointsPos[leftHandIndex].y - gestureData.jointPos.y) < 0.15f &&
                                (jointsPos[leftHandIndex].z - gestureData.jointPos.z) < -0.15f;

                            if (isInPose)
                            {
                                Vector3 jointPos = jointsPos[gestureData.joint];
                                CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 0f);
                            }
                        }
                        else
                        {
                            // cancel the gesture
                            SetGestureCancelled(ref gestureData);
                        }
                        break;
                }
                break;

            // check for Pull
            case Gestures.Pull:
                switch (gestureData.state)
                {
                    case 0:  // gesture detection - phase 1
                        if (jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] &&
                           (jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) > -0.05f &&
                           Mathf.Abs(jointsPos[rightHandIndex].x - jointsPos[rightElbowIndex].x) < 0.15f &&
                           (jointsPos[rightHandIndex].z - jointsPos[rightElbowIndex].z) < -0.15f)
                        {
                            SetGestureJoint(ref gestureData, timestamp, rightHandIndex, jointsPos[rightHandIndex]);
                            gestureData.progress = 0.5f;
                        }
                        else if (jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
                                (jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) > -0.05f &&
                                Mathf.Abs(jointsPos[leftHandIndex].x - jointsPos[leftElbowIndex].x) < 0.15f &&
                                (jointsPos[leftHandIndex].z - jointsPos[leftElbowIndex].z) < -0.15f)
                        {
                            SetGestureJoint(ref gestureData, timestamp, leftHandIndex, jointsPos[leftHandIndex]);
                            gestureData.progress = 0.5f;
                        }
                        break;

                    case 1:  // gesture phase 2 = complete
                        if ((timestamp - gestureData.timestamp) < 1.5f)
                        {
                            bool isInPose = gestureData.joint == rightHandIndex ?
                                jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] &&
                                Mathf.Abs(jointsPos[rightHandIndex].x - gestureData.jointPos.x) < 0.15f &&
                                Mathf.Abs(jointsPos[rightHandIndex].y - gestureData.jointPos.y) < 0.15f &&
                                (jointsPos[rightHandIndex].z - gestureData.jointPos.z) > 0.15f :
                                jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
                                Mathf.Abs(jointsPos[leftHandIndex].x - gestureData.jointPos.x) < 0.15f &&
                                Mathf.Abs(jointsPos[leftHandIndex].y - gestureData.jointPos.y) < 0.15f &&
                                (jointsPos[leftHandIndex].z - gestureData.jointPos.z) > 0.15f;

                            if (isInPose)
                            {
                                Vector3 jointPos = jointsPos[gestureData.joint];
                                CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 0f);
                            }
                        }
                        else
                        {
                            // cancel the gesture
                            SetGestureCancelled(ref gestureData);
                        }
                        break;
                }
                break;

                // here come more gesture-cases
        }
    }

}
