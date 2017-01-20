using UnityEngine;
using System.Collections;

public class PlayingGameSE : MonoBehaviour {

    public AudioClip seChangeCamera;
    public AudioClip seButtonClick;
    public AudioClip seGameOver;
    public AudioClip seGameClear;
    public AudioClip seLockArm;
    public AudioClip seCanLockArm;
    private AudioSource audioSource;

    private bool start_flag = false;

    // Use this for initialization
    void Start() {
        if (start_flag) return;
        audioSource = gameObject.GetComponent<AudioSource>();
        this.audioSource.playOnAwake = false;
        start_flag = true;
    }

    // Update is called once per frame
    void Update() {

    }

    public void PlaySEChangeCamera() {
        Start();
        //this.audioSource.Stop();
        this.audioSource.clip = this.seChangeCamera;
        this.audioSource.Play();
    }

    public void PlaySEButtonClick() {
        Start();
        //this.audioSource.Stop();
        this.audioSource.clip = this.seButtonClick;
        this.audioSource.Play();
    }

    public void PlaySEGameOver() {
        Start();
        //this.audioSource.Stop();
        this.audioSource.clip = this.seGameOver;
        this.audioSource.Play();
    }

    public void PlaySEGameClear() {
        Start();
        //this.audioSource.Stop();
        this.audioSource.clip = this.seGameClear;
        this.audioSource.Play();
    }

    public void PlaySELockArm() {
        Start();
        //this.audioSource.Stop();
        this.audioSource.clip = this.seLockArm;
        this.audioSource.Play();
    }

    public void PlaySECanLockArm() {
        Start();
        //this.audioSource.Stop();
        this.audioSource.clip = this.seCanLockArm;
        this.audioSource.Play();
    }
}
