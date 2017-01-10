using UnityEngine;

public class BoneInfo {
    private GameObject bone;
    private Vector3 initPostion;

    public GameObject Bone { get { return this.bone; } }

    public BoneInfo(GameObject bone) {
        if (bone != null) {
            this.bone = bone;
            this.initPostion = bone.transform.localPosition;
        }
    }

    public void SetPosition(Vector3 pos, float factor = 1.0f) {
        this.bone.transform.localPosition = initPostion + factor * new Vector3(pos.x, 2.0f * pos.y, -1.0f * pos.z);
    }

    public void SetRotation(Quaternion q) {
        // this.bone.transform.rotation = q;
    }
}
