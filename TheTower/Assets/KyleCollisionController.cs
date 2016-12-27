using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KyleCollisionController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger" + DateTime.Now.ToString());

    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("trigger exit" + DateTime.Now.ToString());
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collide" + DateTime.Now.ToString());
    }

    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("collide exit" + DateTime.Now.ToString());
    }
}
