using UnityEngine;
using UnityEngine.UI;

public class SprintBar : MonoBehaviour
{
    Image image;
    public float fillAmount = 1f;
    float drainRate = 0.33f;
    float regenRate = 0.25f;
    void Start()
    {
        image = GetComponent<Image>();
    }

    public void UpdateImage()
    {
        fillAmount -= drainRate * Time.deltaTime;
        fillAmount = Mathf.Clamp01(fillAmount);
        image.fillAmount = fillAmount;
    }

    public void UpdateImage_Regen()
    {
        fillAmount += regenRate * Time.deltaTime;
        fillAmount = Mathf.Clamp01(fillAmount);
        image.fillAmount = fillAmount;
    }
}
