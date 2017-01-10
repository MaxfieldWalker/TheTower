using UnityEngine;

public interface ICollisionReceiver {
    // 両手足の衝突をまとめて受け取る
    void DelegateOnTriggerEnter(GameObject self, Collider other);
    void DelegateOnTriggerExit(GameObject self, Collider other);
}
