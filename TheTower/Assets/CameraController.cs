using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Playerに追従するカメラ
/// 参考: http://qiita.com/valbeat/items/bab5cb649fe0cf6756d4
/// </summary>
public class CameraController : MonoBehaviour {
    private GameObject player;
    private Vector3 offset;

	// Use this for initialization
	void Start () {
        this.player = GameObject.FindGameObjectWithTag("Player");
        // プレイヤーとカメラの距離を取得しておく
        // これ以降プレイヤーの位置とオフセットを足したものをカメラの位置とすることで
        // 最初のプレイヤーとカメラの位置関係を維持する
        this.offset = this.transform.position - player.transform.position;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        Vector3 newPosition = this.transform.position;
        newPosition.x = player.transform.position.x + offset.x;
        newPosition.y = player.transform.position.y + offset.y;
        newPosition.z = player.transform.position.z + offset.z;

        this.transform.position = newPosition;
    }
}
