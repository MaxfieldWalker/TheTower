using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {
    public Camera MainCamera;
    public Camera Player1Camera;
    public Camera Player2Camera;

    public PlayingGameSE se;

	// Use this for initialization
	void Start () {
        // Vive側の映像をディスプレイにミラーしないようにする
        UnityEngine.VR.VRSettings.showDeviceView = false;

        UsePlayer1Camera();
    }

    public void UseMainCamera() {
        this.MainCamera.gameObject.SetActive(true);
        this.Player1Camera.gameObject.SetActive(false);
        this.Player2Camera.gameObject.SetActive(false);
    }

    public void UsePlayer1Camera() {
        se.PlaySEChangeCamera();
        // this.MainCamera.gameObject.SetActive(false);
        this.Player1Camera.gameObject.SetActive(true);
        this.Player2Camera.gameObject.SetActive(false);
    }

    public void UsePlayer2Camera() {
        se.PlaySEChangeCamera();
        //this.MainCamera.gameObject.SetActive(false);
        this.Player1Camera.gameObject.SetActive(false);
        this.Player2Camera.gameObject.SetActive(true);
        this.Player2Camera.gameObject.GetComponent<BlurScript>().deactivateBlur();
    }
}
