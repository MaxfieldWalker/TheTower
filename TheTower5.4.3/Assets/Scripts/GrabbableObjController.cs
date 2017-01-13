using System.Collections;
using UnityEngine;

public class GrabbableObjController : MonoBehaviour {
    public Material DefaultMaterial;
    public Material CollideMaterial;

    public float Threshold_ms = 10000.0f;

    private Material mat;
    private Color defaultColor;
    private Color collideColor;
    private Light light;

    private bool touching;
    private int touchingCount = 0;

    // Use this for initialization
    void Start() {
        gameObject.GetComponentInChildren<Light>().color = DefaultMaterial.color;
        this.touching = false;
        this.touchingCount = 0;

        this.mat = this.gameObject.GetComponent<Renderer>().material;
        this.light = this.gameObject.GetComponentInChildren<Light>();

        this.defaultColor = DefaultMaterial.GetColor("_EmissionColor");
        this.collideColor = CollideMaterial.GetColor("_EmissionColor");
    }

    // Update is called once per frame
    void Update() {

    }

    IEnumerator changeCol() {
        // 色をじわじわ変える
        yield return StartCoroutine(ChangeColor(defaultColor, Color.white, 0.5f));
        yield return StartCoroutine(ChangeColor(Color.white, Color.green, 1.0f));

        yield break;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Hand_L" ||
           other.tag == "Hand_R" ||
           other.tag == "Foot_L" ||
           other.tag == "Foot_R") {
            this.touchingCount++;

            if (touching) return;
            this.touching = true;

            // 色をじわじわ変える
            StartCoroutine(changeCol());
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Hand_L" ||
            other.tag == "Hand_R" ||
            other.tag == "Foot_L" ||
            other.tag == "Foot_R") {
            this.touchingCount--;

            if (this.touchingCount > 0) return;
            this.touching = false;
            this.mat.SetColor("_EmissionColor", defaultColor);
            this.light.color = defaultColor;
        }
    }

    IEnumerator ChangeColor(Color fromColor, Color toColor, float duration_sec) {
        if (!touching) yield break;

        float start = Time.time;
        float end = start + duration_sec;

        // 5割までしか色がじわじわ変わらないようにする
        // 残りの5割はパッと変わるようにしている
        float cut = 0.5f;

        float deltaR = cut * (toColor.r - fromColor.r);
        float deltaG = cut * (toColor.g - fromColor.g);
        float deltaB = cut * (toColor.b - fromColor.b);

        while (Time.time < end) {
            if (!touching) yield break;

            float ratio = Time.deltaTime / duration_sec;

            fromColor.r = fromColor.r + ratio * deltaR;
            fromColor.g = fromColor.g + ratio * deltaG;
            fromColor.b = fromColor.b + ratio * deltaB;

            this.mat.SetColor("_EmissionColor", fromColor);
            this.light.color = fromColor;

            yield return 0;
        }

        this.mat.SetColor("_EmissionColor", toColor);
        this.light.color = toColor;

        yield break;
    }
}
