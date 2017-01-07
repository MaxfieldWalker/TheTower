using UnityEngine;

public class SubPlayer : MonoBehaviour {
    public MainPlayer MainPlayer;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ChangeState(ControlState state)
    {
        this.MainPlayer.ChangeState(state);
    }

    public void ToggleLockLeft()
    {
        this.MainPlayer.ToggleLockLeft();
    }

    public void ToggleLockRight()
    {
        this.MainPlayer.ToggleLockRight();
    }
}

public enum ControlState
{
    Hands, // 両手にアサインされている状態
    Feet   // 両足にアサインされている状態
}
