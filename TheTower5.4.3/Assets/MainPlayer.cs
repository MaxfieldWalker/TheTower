using Assets.Scripts;
using UnityEngine;

public class MainPlayer : MonoBehaviour {
    public ControlState CurrentControlState;

    private ArmInfo Hand_L;
    private ArmInfo Hand_R;
    private ArmInfo Foot_L;
    private ArmInfo Foot_R;    

    // Use this for initialization
    void Start () {
        // 両手にアサインされた状態から始める
        this.CurrentControlState = ControlState.Hands;

        this.Hand_L = new ArmInfo(GameObject.Find("Hand_L").transform);
        this.Hand_R = new ArmInfo(GameObject.Find("Hand_R").transform);
        this.Foot_L = new ArmInfo(GameObject.Find("Foot_L").transform);
        this.Foot_R = new ArmInfo(GameObject.Find("Foot_R").transform);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ChangeState(ControlState state)
    {
        this.CurrentControlState = state;
    }

    public void MoveLeft(MoveDirection direction)
    {
        switch (this.CurrentControlState)
        {
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

    public void MoveRight(MoveDirection direction)
    {
        switch (this.CurrentControlState)
        {
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

    public void ToggleLockLeft()
    {
        switch (this.CurrentControlState)
        {
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

    public void ToggleLockRight()
    {
        switch (this.CurrentControlState)
        {
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

    public void delegateOnTriggerEnter(GameObject self, Collider other)
    {
        Debug.Log("trigger enter: " + self.name);

        if (self.name == "Hand_L") this.Hand_L.Grab(other.gameObject);
        if (self.name == "Hand_R") this.Hand_R.Grab(other.gameObject);
        if (self.name == "Foot_L") this.Foot_L.Grab(other.gameObject);
        if (self.name == "Foot_R") this.Foot_R.Grab(other.gameObject);
    }


    public void delegateOnTriggerExit(GameObject self, Collider other)
    {
        Debug.Log("trigger exit: " + self.name);

        if (self.name == "Hand_L") this.Hand_L.Ungrab();
        if (self.name == "Hand_R") this.Hand_R.Ungrab();
        if (self.name == "Foot_L") this.Foot_L.Ungrab();
        if (self.name == "Foot_R") this.Foot_R.Ungrab();
    }
}
