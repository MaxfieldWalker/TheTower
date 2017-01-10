using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScript : MonoBehaviour {
    public bool IsEnabled;

    private GameObject[] ArmObj = new GameObject[4];
    private ArmScript[] ArmScr = new ArmScript[4];

    private int mode=-1;
    private float k = 1.00f;//ばね定数

    void Start () {
        ArmObj[0] = transform.FindChild("1p").gameObject.transform.FindChild("armsLeft").gameObject;
        ArmObj[1] = transform.FindChild("1p").gameObject.transform.FindChild("armsRight").gameObject;
        ArmObj[2] = transform.FindChild("2p").gameObject.transform.FindChild("armsLeft").gameObject;
        ArmObj[3] = transform.FindChild("2p").gameObject.transform.FindChild("armsRight").gameObject;

        int i = 0;
        for (i = 0; i < ArmObj.Length; i++) {
            ArmObj[i].AddComponent<ArmScript>();
            ArmScr[i] = ArmObj[i].GetComponent<ArmScript>();
        }
    }

    // Update is called once per frame
    void Update()
    {

        //switchMode();

        //足の移動
        int i = 0;
        for (i = 0; i < ArmScr.Length; i++)
        {
            ArmScr[i].moveArm(i == mode);
        }

        //キャラクターの移動
        //キャラの座標、1pの左足先の座標、1pの右足先の座標、2pの左足先の座標、2pの右足先の座標
        float[][] position = new float[][] {
                new float[] {transform.position.x , transform.position.y- 0.299968f, transform.position.z },
                ArmScr[0].getSpherePosition(),
                new float[] {ArmScr[1].SphScr.transform.position.x, ArmScr[1].SphScr.transform.position.y, ArmScr[1].SphScr.transform.position.z },
                new float[] {ArmScr[2].SphScr.transform.position.x, ArmScr[2].SphScr.transform.position.y, ArmScr[2].SphScr.transform.position.z },
                new float[] {ArmScr[3].SphScr.transform.position.x, ArmScr[3].SphScr.transform.position.y, ArmScr[3].SphScr.transform.position.z }};

        for (i = 0; i < 2; i++)//x,y,z方向でそれぞれ行う
        {

            //エネルギー式
            float Ene = Energy(position[0][i], position[1][i], position[2][i], position[3][i], position[4][i]) - minEnergy(position[1][i], position[2][i], position[3][i], position[4][i]);
            float dx = div(position[0][i], position[1][i], position[2][i], position[3][i], position[4][i]) * 0.28f;
            //誤差の都合で0になりきらないので、この範囲ならゼロとみなす
            if (dx < 0.000001 && dx > -0.000001)
            {
                Ene = 0;
                dx = 1;
            }

            //x,y,z座標それぞれでキャラを移動
            //キャラが移動すると相対的に足先も動いてしまうので足先は元の位置に戻す
            if (i == 0) {
                transform.Translate(-Ene / dx, 0, 0, Space.World);
                for (i = 0; i < 4; i++) ArmScr[i].SphScr.transform.Translate(Ene / dx, 0, 0, Space.World);
            }
            if (i == 1) {
                transform.Translate(0, -Ene / dx, 0, Space.World);
                for (i = 0; i < 4; i++) ArmScr[i].SphScr.transform.Translate(0, Ene / dx, 0, Space.World);
            }
            if (i == 2) {
                transform.Translate(0, 0, -Ene / dx, Space.World);
                for (i = 0; i < 4; i++) ArmScr[i].SphScr.transform.Translate(0, 0, Ene / dx, Space.World);
            }

        }

            
    }
    /*
    //動かす腕を指定する関数。
    //mode=0 →　1p左腕
    //mode=1 →　1p右腕
    //mode=2 →　2p左腕
    //mode=3 →　2p右腕
    //mode=-1→　動かさない
    void switchMode()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            if (mode == 0) mode = -1;
            else mode = 0;
            return;
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            if (mode == 1) mode = -1;
            else mode = 1;
            return;
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            if (mode == 2) mode = -1;
            else mode = 2;
            return;
        }
        if (Input.GetKey(KeyCode.Alpha4))
        {
            if (mode == 3) mode = -1;
            else mode = 3;
            return;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            mode = -1;
            return;
        }
    }
    */

    //現在地における弾性エネルギーの値を返す
    float Energy(float x0, float x1, float x2, float x3, float x4)
    {
        return ( k * ((x0 - x1) * (x0 - x1)
                  + (x0 - x2) * (x0 - x2)
                  + (x0 - x3) * (x0 - x3)
                  + (x0 - x4) * (x0 - x4)));
    }

    //Energy(x0,x1,x2,x3,x4)-realEnergy(x1,x2,x3,x4)の関数をx0で微分した値を返す
    float div(float x0, float x1, float x2, float x3, float x4)
    {
        return k * (8 * x0 - 2 * (x1 + x2 + x3 + x4));
    }

    //エネルギーの理論上の最小値を返す
    float minEnergy(float x1, float x2, float x3, float x4)
    {
        return 0.75f* k * (x1 * x1 + x2 * x2 + x3 * x3 + x4 * x4) - 0.5f  * (x1 * x2 + x1 * x3 + x1 * x4 + x2 * x3 + x2 * x4 + x3 * x4);
    }
}
