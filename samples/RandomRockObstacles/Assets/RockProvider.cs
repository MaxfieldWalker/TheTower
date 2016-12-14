using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockProvider : MonoBehaviour
{
    // 岩(障害物)
    public GameObject rock;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 生成位置を適当にばらけさせる
        Vector3 p = transform.position;
        p.x = p.x + 30.0f * (Random.value - 0.5f);
        p.y = p.x + 10f * Random.value;

        if (Input.GetKey(KeyCode.Space))
        {
            Instantiate(rock, p, Random.rotation);
        }
    }
}
