using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KyleController : MonoBehaviour {
    public GameObject Kyle;

    private GameObject wristBone;

	// Use this for initialization
	void Start () {
        this.wristBone = GameObject.Find("Left_Shoulder_Joint_01");
	}
	
	// Update is called once per frame
	void Update () {
        this.wristBone.transform.Rotate(new Vector3(0, 0, -1f));
	}
}
