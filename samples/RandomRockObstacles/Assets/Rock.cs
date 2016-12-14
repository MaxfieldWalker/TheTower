using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// 衝突が起きたらこのメソッドが呼び出される
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter(Collision collision)
    {
        // 衝突した相手のオブジェクトのタグが"Floor"なら
        // 岩(障害物)を消す
        if (collision.gameObject.CompareTag("Floor"))
        {
            Destroy(this.gameObject);
        }
    }
}
