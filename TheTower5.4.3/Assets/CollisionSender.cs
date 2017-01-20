using System.Collections;
using UnityEngine;

public class CollisionSender : MonoBehaviour {
    public CollisionReceiverBase collisionReceiver;

    private bool touching;
    private float collideTime;
    private Coroutine oldCoroutine;
    private const float AcceptTime = 1.5f;

    private void Start() {
        this.touching = false;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "ClearGrabbableObj") {
            this.collisionReceiver.DelegateOnTriggerEnter(this.gameObject, other);
            return;
        }

        if (other.tag == "GrabbableObj") {
            this.touching = true;
            if (this.oldCoroutine != null) {
                Debug.Log("Stop coroutine");
                StopCoroutine(this.oldCoroutine);
            }

            this.oldCoroutine = StartCoroutine(Trigger(other));
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "GrabbableObj") {
            this.touching = false;
            this.collisionReceiver.DelegateOnTriggerExit(this.gameObject, other);
        }
    }

    IEnumerator Trigger(Collider other) {
        yield return new WaitForSeconds(AcceptTime);

        // 1.5秒経っても触れたままなら衝突とする
        if (this.touching) {
            this.collisionReceiver.DelegateOnTriggerEnter(this.gameObject, other);
        }
    }
}
