using UnityEngine;
using UnityEngine.UI;

public class HPIndicatorController : MonoBehaviour {
    Slider slider;
    float hp = 0.0f;

	// Use this for initialization
	void Start () {
        slider = GameObject.Find("Slider").GetComponent<Slider>();
	}
	
	// Update is called once per frame
	void Update () {
        hp += 0.01f;
        if (hp > 1.0f) hp = 0.0f;

        slider.value = hp;
	}
}
