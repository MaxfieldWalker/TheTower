
public abstract class MainPlayerBase : CollisionReceiverBase {
    public abstract void ChangeState(ControlState state);
    public abstract void ToggleLockLeft();
    public abstract void ToggleLockRight();
}
