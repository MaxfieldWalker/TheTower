using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//腕に充てるスクリプト
//CharacterScriptから自動生成
public class ArmScript : MonoBehaviour {

    public GameObject SphObj;
    public GameObject SprObj;
    public SpringScript SprScr;
    public SphereScript SphScr;

    //ばねと足先のスクリプトを設定
    void Start() {
        SphObj = transform.FindChild("Sphere").gameObject;
        SphObj.AddComponent<SphereScript>();
        SphScr = SphObj.GetComponent<SphereScript>();
        SprObj = transform.FindChild("Spring").gameObject;
        SprObj.AddComponent<SpringScript>();
        SprScr = SprObj.GetComponent<SpringScript>();
    }

    //腕を動かす関数　CharacterScriptから呼ばれる
    public void moveArm(bool mode)
    {
        //球とばねをそれぞれ動かす
        SphScr.moveSphere(mode);
        SprScr.moveSpring(SphObj);
    }

    public float[] getSpherePosition() {
        float[] f = new float[3];
        f[0] = SphScr.transform.position.x;
        f[1] = SphScr.transform.position.y;
        f[2] = SphScr.transform.position.z;
        return f;
    }

}
