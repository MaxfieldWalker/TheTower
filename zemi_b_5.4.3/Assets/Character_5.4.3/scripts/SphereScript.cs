using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//腕の先端部分に充てるスクリプト
//ArmScriptから自動生成

public class SphereScript : MonoBehaviour {

	public double s = 0.1;  //キー入力により移動する速度

    // キー入力で足先であるSphereを移動
    public void moveSphere(bool mode) {

        double dx =0.0;
		double dy =0.0;
		double dz =0.0;

        // →キーでX軸方向に +s 移動
        // ←キーでX軸方向に -s 移動
        if (Input.GetKey(KeyCode.RightArrow)) {
			dx = s;
		}
		if (Input.GetKey(KeyCode.LeftArrow)) {
			dx = -s;
		}
        if (mode) transform.Translate((float)dx, 0, 0);

        // Z キーでY軸方向に +s 移動
        // X キーでY軸方向に -s 移動

        if (Input.GetKey(KeyCode.Z)) {
			dy = s;
		}
		if (Input.GetKey(KeyCode.X)) {
			dy = -s;
		}
        if (mode) transform.Translate(0, (float)dy, 0);

        // ↑キーでZ軸方向に +s 移動
        // ↓キーでZ軸方向に -s 移動
        if (Input.GetKey(KeyCode.UpArrow)) {
			dz = s;
		}
		if (Input.GetKey(KeyCode.DownArrow)) {
			dz = -s;
		}

        // 動かしてよいなら動かす
        if (mode) transform.Translate(0, 0, (float)dz);
	}
}