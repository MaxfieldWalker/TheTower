using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        var audioSource = this.gameObject.GetComponent<AudioSource>();
        audioSource.Play();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
