using Assets.Scripts;
using UnityEngine;

public class KyleCollisionController : MonoBehaviour {
    public GameObject RootObj;
    private Kyle kyle;

	// Use this for initialization
	void Start () {
        this.kyle = this.RootObj.GetComponent<Kyle>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        this.kyle.delegateOnTriggerEnter(this.gameObject, other);
    }

    private void OnTriggerExit(Collider other)
    {
        this.kyle.delegateOnTriggerExit(this.gameObject, other);
    }
}
