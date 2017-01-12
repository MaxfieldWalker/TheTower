using UnityEngine;

public class GrabbableObjController : MonoBehaviour {
    public Material DefaultMaterial;
    public Material CollideMaterial;

    // Use this for initialization
    void Start() {
        gameObject.GetComponentInChildren<Light>().color = DefaultMaterial.color;
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Hand_L" ||
           other.tag == "Hand_R" ||
           other.tag == "Foot_L" ||
           other.tag == "Foot_R") {
            // マテリアルの色に合わせてライトの色も変える
            this.gameObject.GetComponent<Renderer>().material = CollideMaterial;
            gameObject.GetComponentInChildren<Light>().color = CollideMaterial.color;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Hand_L" ||
            other.tag == "Hand_R" ||
            other.tag == "Foot_L" ||
            other.tag == "Foot_R") {
            this.gameObject.GetComponent<Renderer>().material = DefaultMaterial;
            gameObject.GetComponentInChildren<Light>().color = DefaultMaterial.color;
        }
    }
}
