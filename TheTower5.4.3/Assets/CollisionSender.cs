using UnityEngine;

public class CollisionSender : MonoBehaviour {
    public MainPlayer mainPlayer;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void OnTriggerEnter(Collider other)
    {
        this.mainPlayer.delegateOnTriggerEnter(this.gameObject, other);
    }

    private void OnTriggerExit(Collider other)
    {
        this.mainPlayer.delegateOnTriggerExit(this.gameObject, other);
    }
}
