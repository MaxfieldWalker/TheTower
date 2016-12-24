using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class BlurScript : MonoBehaviour
{
    private Blur blurEffect;

    // Use this for initialization
    void Start()
    {
        this.blurEffect = this.gameObject.GetComponent<Blur>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void activateBlur()
    {
        changeBlurAmoutTo(4);
        this.blurEffect.enabled = true;
    }

    public void deactivateBlur()
    {
        changeBlurAmoutTo(0);
        this.blurEffect.enabled = false;
    }

    private void changeBlurAmoutTo(float amount)
    {

    }
}
