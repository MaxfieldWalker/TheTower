using UnityEngine;

public class MainPlayerController : MonoBehaviour {
    public GameManager gameManager;

    private Rigidbody rigidBody;
    private const float ForceAmount = 10.0f;
    private Vector3 initialPosition;
    private MainPlayer mainPlayer;

	// Use this for initialization
	void Start () {
        this.rigidBody = this.gameObject.GetComponent<Rigidbody>();
        this.initialPosition = this.gameObject.transform.position;
        this.mainPlayer = GetComponent<MainPlayer>();
	}
	
	// Update is called once per frame
	void Update () {
        if (this.gameManager.GameState != GameManager.GameStates.Game) return;

        // SZXCで左を動かす
        if (Input.GetKey(KeyCode.S))
        {
            this.mainPlayer.MoveLeft(MainPlayer.MoveDirection.Up);
        }
        if (Input.GetKey(KeyCode.Z))
        {
            this.mainPlayer.MoveLeft(MainPlayer.MoveDirection.Left);
        }
        if (Input.GetKey(KeyCode.X))
        {
            this.mainPlayer.MoveLeft(MainPlayer.MoveDirection.Down);
        }
        if (Input.GetKey(KeyCode.C))
        {
            this.mainPlayer.MoveLeft(MainPlayer.MoveDirection.Right);
        }

        // GVBNで右を動かす
        if (Input.GetKey(KeyCode.G))
        {
            this.mainPlayer.MoveRight(MainPlayer.MoveDirection.Up);
        }
        if (Input.GetKey(KeyCode.V))
        {
            this.mainPlayer.MoveRight(MainPlayer.MoveDirection.Left);
        }
        if (Input.GetKey(KeyCode.B))
        {
            this.mainPlayer.MoveRight(MainPlayer.MoveDirection.Down);
        }
        if (Input.GetKey(KeyCode.N))
        {
            this.mainPlayer.MoveRight(MainPlayer.MoveDirection.Right);
        }
    }

    public void respawn()
    {
        // 初期位置に戻す
        this.gameObject.transform.position = this.initialPosition;
        // 静止させる
        this.rigidBody.velocity = Vector3.zero;
        this.rigidBody.useGravity = true;
    }

    public void gotoGameOverState()
    {
        this.rigidBody.velocity *= 0.01f;
    }

    public void gotoGameClearState()
    {
        // 完全に静止させる
        this.rigidBody.useGravity = false;
        this.rigidBody.velocity = Vector3.zero;
        this.rigidBody.angularVelocity = Vector3.zero;
    }
}
