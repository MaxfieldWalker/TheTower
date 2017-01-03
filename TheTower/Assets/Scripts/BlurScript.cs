using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class BlurScript : MonoBehaviour
{
    private Blur blurEffect;
    private bool blurAmountSholdChange = false;
    private int targetBlurAmount = 0;
    private float elapsedTime = 0.0f;
    private const float BlurChangeSpeed = 20.0f;

    private float BlurAmount
    {
        get { return this.blurEffect.iterations; }
        set { this.blurEffect.iterations = (int)value; }
    }

    // Use this for initialization
    void Start()
    {
        this.blurEffect = this.gameObject.GetComponent<Blur>();
        this.blurEffect.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (blurAmountSholdChange)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > 1.0f / BlurChangeSpeed)
            {
                if (this.BlurAmount != this.targetBlurAmount)
                {
                    float changeAmount = (this.targetBlurAmount - this.BlurAmount);
                    this.BlurAmount += changeAmount > 0 ? 1 : -1;
                }
                if (this.BlurAmount == this.targetBlurAmount)
                {
                    this.blurAmountSholdChange = false;
                    this.blurEffect.enabled = this.BlurAmount != 0;
                }

                elapsedTime = 0.0f;
            }
        }
    }



    public void activateBlurWithAnim()
    {
        this.blurEffect.enabled = true;
        changeBlurAmoutTo(10);
    }

    public void deactivateBlurWithAnim()
    {
        changeBlurAmoutTo(0);
    }

    private void changeBlurAmoutTo(int amount)
    {
        this.blurAmountSholdChange = true;
        this.targetBlurAmount = amount;
    }
}
