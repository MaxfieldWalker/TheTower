﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenFloorController : MonoBehaviour {
    public GameObject gameManager;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            this.gameManager.SendMessage("gotoGameOverState");
        }
    }
}
