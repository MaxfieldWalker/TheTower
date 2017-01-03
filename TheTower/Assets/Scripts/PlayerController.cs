using UnityEngine;

public class PlayerController : MonoBehaviour {
    public GameManager gameManager;

    private Rigidbody rigidBody;
    private const float ForceAmount = 10.0f;
    private Vector3 initialPosition;

	// Use this for initialization
	void Start () {
        this.rigidBody = this.gameObject.GetComponent<Rigidbody>();
        this.initialPosition = this.gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (this.gameManager.GameState != GameManager.GameStates.Game) return;

        // WASDでプレイヤーを操作する
        if (Input.GetKey(KeyCode.W))
        {
            this.rigidBody.AddForce(new Vector3(0, ForceAmount, ForceAmount));
        }
        if (Input.GetKey(KeyCode.D))
        {
            this.rigidBody.AddForce(new Vector3(ForceAmount, 0.0f, 0.0f));
        }
        if (Input.GetKey(KeyCode.A))
        {
            this.rigidBody.AddForce(new Vector3(-ForceAmount, 0.0f, 0.0f));
        }
    }

    public void respawn()
    {
        // 初期位置に戻す
        this.gameObject.transform.position = this.initialPosition;
        // 静止させる
        this.rigidBody.velocity = Vector3.zero;
    }

    public void gotoGameOverState()
    {
        this.rigidBody.velocity *= 0.01f;
    }
}
