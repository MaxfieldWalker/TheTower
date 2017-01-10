using Assets.Scripts;
using UnityEngine;

public class ArmInfo {
    public bool IsLocked { get; set; }
    private Transform obj;
    // GrabbableObjに触れているか
    private bool Grabbing { get; set; }
    private GameObject GrabbableObject { get; set; }
    private Vector3 initialPostion;

    public ArmInfo(Transform obj) {
        this.IsLocked = true;
        this.Grabbing = true;
        this.GrabbableObject = null;
        this.obj = obj;
        this.initialPostion = obj.position;
    }

    public void Move(MoveDirection direction) {
        if (this.IsLocked) return;

        switch (direction) {
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

    public void Grab(GameObject grabbableObj) {
        this.Grabbing = true;
        this.GrabbableObject = grabbableObj;
    }

    public void Ungrab() {
        this.Grabbing = false;
        this.GrabbableObject = null;
    }

    public void ToggleLock() {
        // ロックされておらずGrabbableObjにも触れていない場合は何もしない
        if (!this.IsLocked && !Grabbing) return;

        Debug.Log("toggle lock: " + this.obj.name);
        this.IsLocked = !this.IsLocked;
    }

    // 宙に浮いてかを返す
    public bool IsFree {
        get {
            return !(IsLocked || Grabbing);
        }
    }

    public void SetPosition(Vector3 pos) {
        this.obj.position = pos;
    }

    public void Reset() {
        this.obj.position = this.initialPostion;
        this.IsLocked = true;
        this.Grabbing = true;
        this.GrabbableObject = null;
    }
}
