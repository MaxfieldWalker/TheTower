using UnityEngine;

public class CollisionSender : MonoBehaviour {
    public CollisionReceiverBase collisionReceiver;

    private void OnTriggerEnter(Collider other) {
        this.collisionReceiver.DelegateOnTriggerEnter(this.gameObject, other);
    }

    private void OnTriggerExit(Collider other) {
        this.collisionReceiver.DelegateOnTriggerExit(this.gameObject, other);
    }
}
