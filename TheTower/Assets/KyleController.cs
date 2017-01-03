using Assets.Scripts;
using UnityEngine;

public class KyleController : MonoBehaviour
{
    public GameObject KylePrefab;

    private Kyle kyle;

    // Use this for initialization
    void Start()
    {
        this.kyle = GetComponent<Kyle>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            this.kyle.currentMode = PlayerControlMode.MoveLeftHand;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            this.kyle.currentMode = PlayerControlMode.MoveRightHand;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            this.kyle.currentMode = PlayerControlMode.MoveLeftFoot;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            this.kyle.currentMode = PlayerControlMode.MoveRightFoot;
        }

        if (Input.GetKey(KeyCode.W))
        {
            this.kyle.moveLimbUp();
        }
        else if (Input.GetKey(KeyCode.S))
        {
            this.kyle.moveLimbDown();
        }

        if (Input.GetKey(KeyCode.D))
        {
            this.kyle.moveLimbRight();
        }
        else if (Input.GetKey(KeyCode.A))
        {
            this.kyle.moveLimbLeft();
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            this.kyle.moveBodyUp();
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            this.kyle.moveBodyDown();
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            this.kyle.moveBodyLeft();
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            this.kyle.moveBodyRight();
        }
    }
}
