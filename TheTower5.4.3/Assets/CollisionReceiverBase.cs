using UnityEngine;

public abstract class CollisionReceiverBase : MonoBehaviour {
    // 両手足の衝突をまとめて受け取る
    public abstract void DelegateOnTriggerEnter(GameObject self, Collider other);
    public abstract void DelegateOnTriggerExit(GameObject self, Collider other);
}
