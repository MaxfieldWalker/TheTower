using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//腕のばね部分に充てるスクリプト
//ArmScriptから自動生成

public class SpringScript : MonoBehaviour {

	private double length0 = 44.85f; //ばねの元となるオブジェクトの自然長。いじるべからず
    public double length_x;       //現在のx方向の長さ
    public double length_y;       //現在のy方向の長さ
    public double length_z;       //現在のz方向の長さ
    public double ini_length_x;   //初期位置のx方向の長さ
    public double ini_length_y;   //初期位置のy方向の長さ
    public double ini_length_z;   //初期位置のz方向の長さ
    private double init_scale;    //初期のscake
    private bool first = true;

    void Start(){
        init_scale = transform.localScale.z;
    }

    public void moveSpring(GameObject SphObj)
    {

        if (first)
        {
            ini_length_x = SphObj.transform.position.x - transform.position.x;
            ini_length_y = SphObj.transform.position.y - transform.position.y;
            ini_length_z = SphObj.transform.position.z - transform.position.z;
            first = false;
        }

        Vector3 scale = transform.localScale;
        double l0 = length0 * scale.z;
        //ばねの付け根の座標
        double x_0 = transform.position.x;
        double y_0 = transform.position.y;
        double z_0 = transform.position.z;
        //足先の球の座標
        double x_g = SphObj.transform.position.x;
        double y_g = SphObj.transform.position.y;
        double z_g = SphObj.transform.position.z;

        length_x = x_g - x_0;
        length_y = y_g - y_0;
        length_z = z_g - z_0;


        //ばねの長さを求めscaleを求める
        double l = Math.Sqrt(length_x * length_x + length_y * length_y + length_z * length_z);
        double scale1 = (l / l0) * scale.z;

        //ばねを伸ばす
        transform.localScale = new Vector3((float)scale.x, (float)scale.y, (float)scale1);
        //ばねを球の方へ向ける
        transform.LookAt(new Vector3((float)x_g, (float)y_g, (float)z_g));

    }
}
