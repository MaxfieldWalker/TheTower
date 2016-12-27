using UnityEngine;
using System;

public class SimpleGestureListener : MonoBehaviour, KinectGestures.IGestureListener
{
    // GUI Text to display the gesture messages.
    public GUIText GestureInfo;

    // private bool to track if progress message has been displayed
    private bool progressDisplayed;

    // the single instance of this class
    private static SimpleGestureListener instance;


    // returns the single SimpleGestureListener instance
    public static SimpleGestureListener Instance { get { return instance; } }


    public void UserDetected(uint userId, int userIndex)
    {
        // 検知したいジェスチャーを登録する
        KinectManager manager = KinectManager.Instance;
        manager.DetectGesture(userId, KinectGestures.Gestures.Jump);
        manager.DetectGesture(userId, KinectGestures.Gestures.Squat);
        manager.DetectGesture(userId, KinectGestures.Gestures.Tpose);

        if (GestureInfo != null)
        {
            GestureInfo.GetComponent<GUIText>().text = "SwipeLeft, SwipeRight, Jump, Squat or T-pose.";
        }
    }

    public void UserLost(uint userId, int userIndex)
    {
        if (GestureInfo != null)
        {
            GestureInfo.GetComponent<GUIText>().text = string.Empty;
        }
    }

    public void GestureInProgress(
        uint userId,
        int userIndex,
        KinectGestures.Gestures gesture,
        float progress,
        KinectWrapper.SkeletonJoint joint,
        Vector3 screenPos)
    {
        if (gesture == KinectGestures.Gestures.Click && progress > 0.3f)
        {
            // クリックのプログレスをパーセントで表示する
            string sGestureText = string.Format("{0} {1:F1}% complete", gesture, progress * 100);
            if (GestureInfo != null)
                GestureInfo.GetComponent<GUIText>().text = sGestureText;

            progressDisplayed = true;
        }
    }

    public bool GestureCompleted(
        uint userId,
        int userIndex,
        KinectGestures.Gestures gesture,
        KinectWrapper.SkeletonJoint joint,
        Vector3 screenPos)
    {
        string sGestureText = gesture + " detected";
        if (gesture == KinectGestures.Gestures.Click)
            sGestureText += string.Format(" at ({0:F1}, {1:F1})", screenPos.x, screenPos.y);

        if (GestureInfo != null)
            GestureInfo.GetComponent<GUIText>().text = sGestureText;

        progressDisplayed = false;

        return true;
    }

    public bool GestureCancelled(
        uint userId,
        int userIndex,
        KinectGestures.Gestures gesture,
        KinectWrapper.SkeletonJoint joint)
    {
        if (progressDisplayed)
        {
            // clear the progress info
            if (GestureInfo != null)
                GestureInfo.GetComponent<GUIText>().text = String.Empty;

            progressDisplayed = false;
        }

        return true;
    }


    void Awake()
    {
        // シングルトンなので自分のインスタンスを登録する
        instance = this;
    }
}
