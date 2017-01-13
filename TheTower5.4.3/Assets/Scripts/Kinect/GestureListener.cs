using UnityEngine;

public class GestureListener : MonoBehaviour, KinectGestures.IGestureListener
{
    // GUI Text to display the gesture messages.
    public GUIText GestureInfo;

    private bool swipeLeft;
    private bool swipeRight;

    // the single instance of this class
    private static GestureListener instance;

    // returns the single GestureListener instance
    public static GestureListener Instance { get { return instance; } }

    public bool IsSwipeLeft()
    {
        if (swipeLeft)
        {
            swipeLeft = false;
            return true;
        }

        return false;
    }

    public bool IsSwipeRight()
    {
        if (swipeRight)
        {
            swipeRight = false;
            return true;
        }

        return false;
    }

    public void UserDetected(uint userId, int userIndex)
    {
        // detect these user specific gestures
        KinectManager manager = KinectManager.Instance;

        // 取得したいジェスチャーを指定する
        manager.DetectGesture(userId, KinectGestures.Gestures.SwipeLeft);
        manager.DetectGesture(userId, KinectGestures.Gestures.SwipeRight);
        manager.DetectGesture(userId, KinectGestures.Gestures.Wave);

        GestureInfo.GetComponent<GUIText>().text = "Swipe left or right to change the slides.";
    }

    public void UserLost(uint userId, int userIndex)
    {
        GestureInfo.GetComponent<GUIText>().text = "User lost. User ID: " + userId;
    }

    public void GestureInProgress(
        uint userId,
        int userIndex,
        KinectGestures.Gestures gesture,
        float progress,
        KinectWrapper.SkeletonJoint joint,
        Vector3 screenPos)
    {
        // don't do anything here
    }

    public bool GestureCompleted(uint userId, int userIndex, KinectGestures.Gestures gesture,
        KinectWrapper.SkeletonJoint joint, Vector3 screenPos)
    {
        string sGestureText = gesture + " detected";
        if (GestureInfo != null)
        {
            GestureInfo.GetComponent<GUIText>().text = sGestureText;
        }

        if (gesture == KinectGestures.Gestures.SwipeLeft)
            swipeLeft = true;
        else if (gesture == KinectGestures.Gestures.SwipeRight)
            swipeRight = true;

        return true;
    }

    public bool GestureCancelled(
        uint userId,
        int userIndex,
        KinectGestures.Gestures gesture,
        KinectWrapper.SkeletonJoint joint)
    {
        // don't do anything here, just reset the gesture state
        return true;
    }

    void Awake()
    {
        // GestureListenerはシングルトンで扱うためインスタンスを渡しておく
        instance = this;
    }
}
