using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroyer : MonoBehaviour {
    public int time_ms;
    float elapsedTime = 0.0f;
    

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(elapsedTime > this.time_ms)
        {
            Destroy(this.gameObject);
        }

        this.elapsedTime += Time.deltaTime * 1000;
	}
}
