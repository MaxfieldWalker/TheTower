using UnityEngine;
using System.Collections;

public class PlayingGameMusic : MonoBehaviour {

    public AudioClip audioGameBgm;

    private AudioSource audioSource;

    private bool start_flag=false;

    // Use this for initialization
    void Start () {
        if (start_flag) return;
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.clip = audioGameBgm;
        this.audioSource.Play();
        start_flag = true;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

}
