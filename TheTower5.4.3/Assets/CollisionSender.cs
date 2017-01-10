using UnityEngine;

public class CollisionSender : MonoBehaviour {
    public CollisionReceiverBase collisionReceiver;

    private void OnTriggerEnter(Collider other)
    {
        // TODO: 衝突相手を制御点だけに絞ればバチバチが直る?
        this.collisionReceiver.DelegateOnTriggerEnter(this.gameObject, other);
    }

    private void OnTriggerExit(Collider other)
    {
        this.collisionReceiver.DelegateOnTriggerExit(this.gameObject, other);
    }
}
