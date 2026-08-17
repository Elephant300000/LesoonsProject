using UnityEngine;
using UnityEngine.UI;

public class HudBur : MonoBehaviour
{
    public Image readBur;
    public Image greenBur;

    public Image healthBur;

    public float readBurAmount = 0;
    public float greenBurAmount = 100;
    public float healthBurAmount = 100;

    private void Start() => UpdateBur();

    public void Increment(float amount)
    {
        readBurAmount -= amount;
        greenBurAmount += amount;
        UpdateBur();
    }

    public void Decrement(float amount)
    {
        readBurAmount += amount;
        greenBurAmount -= amount;
        UpdateBur();
    }

    public void UpdateBur()
    {
        readBurAmount = Mathf.Clamp(readBurAmount, 0, 100);
        greenBurAmount = Mathf.Clamp(greenBurAmount, 0, 100);

        if (readBur != null)
            readBur.fillAmount = readBurAmount / 100f;
        if (greenBur != null)
            greenBur.fillAmount = greenBurAmount / 100f;
    }
    public void UpdateHealthBur(float amount)
    { 
        healthBurAmount = Mathf.Clamp(amount, 0, 100);
         
        if (healthBur != null)
            healthBur.fillAmount = healthBurAmount / 100f; 
    }
}
