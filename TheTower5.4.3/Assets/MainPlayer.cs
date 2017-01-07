using UnityEngine;
using System.Collections;

public class MainPlayer : MonoBehaviour {
    public ControlState CurrentControlState;
    public bool Hand_L_Locked;
    public bool Hand_R_Locked;
    public bool Foot_L_Locked;
    public bool Foot_R_Locked;

    private Transform Hand_L;
    private Transform Hand_R;
    private Transform Foot_L;
    private Transform Foot_R;

    // Use this for initialization
    void Start () {
        // 両手にアサインされた状態から始める
        this.CurrentControlState = ControlState.Hands;
        // 両手足がロックされた状態から始める
        this.Hand_L_Locked = true;
        this.Hand_R_Locked = true;
        this.Foot_L_Locked = true;
        this.Foot_R_Locked = true;

        this.Hand_L = GameObject.Find("Hand_L").transform;
        this.Hand_R = GameObject.Find("Hand_R").transform;
        this.Foot_L = GameObject.Find("Foot_L").transform;
        this.Foot_R = GameObject.Find("Foot_R").transform;
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
                Move_Hand_L(direction);
                break;
            case ControlState.Feet:
                Move_Foot_L(direction);
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
                Move_Hand_R(direction);
                break;
            case ControlState.Feet:
                Move_Foot_R(direction);
                break;
            default:
                break;
        }
    }

    public void Move_Hand_L(MoveDirection direction)
    {
        if (this.Hand_L_Locked) return;

        moveObject(this.Hand_L, direction);
    }

    public void Move_Hand_R(MoveDirection direction)
    {
        if (this.Hand_R_Locked) return;

        moveObject(this.Hand_R, direction);
    }

    public void Move_Foot_L(MoveDirection direction)
    {
        if (this.Foot_L_Locked) return;

        moveObject(this.Foot_L, direction);
    }

    public void Move_Foot_R(MoveDirection direction)
    {
        if (this.Foot_R_Locked) return;

        moveObject(this.Foot_R, direction);
    }

    private void moveObject(Transform obj, MoveDirection direction)
    {
        switch (direction)
        {
            case MoveDirection.Up:
                obj.Translate(0f, 0f, 0.1f);
                break;
            case MoveDirection.Right:
                obj.Translate(0.1f, 0f, 0f);
                break;
            case MoveDirection.Down:
                obj.Translate(0f, 0f, -0.1f);
                break;
            case MoveDirection.Left:
                obj.Translate(-0.1f, 0f, 0f);
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
                this.Hand_L_Locked = !this.Hand_L_Locked;
                break;
            case ControlState.Feet:
                this.Foot_L_Locked = !this.Foot_L_Locked;
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
                this.Hand_R_Locked = !this.Hand_R_Locked;
                break;
            case ControlState.Feet:
                this.Foot_R_Locked = !this.Foot_R_Locked;
                break;
            default:
                break;
        }
    }

    public enum MoveDirection
    {
        Up,
        Right,
        Down,
        Left
    }
}
