using UnityEngine;
using UnityEngine.UI;

public class UpButton : MonoBehaviour {
    Slider slider;

    // Use this for initialization
    void Start () {
        slider = GameObject.Find("Slider").GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void OnClick()
    {
        if (slider.value < 0.9f) slider.value += 0.05f;
    }
}
