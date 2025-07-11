using UnityEngine;
using UnityEngine.UI;

public class PlayerDamageFlash : MonoBehaviour
{
    public Image flashImage;
    public Color flashColor = new Color(1f, 0f, 0f, 0.4f);
    public float fadeSpeed = 2f;

    private bool flashActive = false;

    void Update()
    {
        if (flashActive)
        {
            flashImage.color = Color.Lerp(flashImage.color, Color.clear, fadeSpeed * Time.deltaTime);
            if (flashImage.color.a < 0.01f)
            {
                flashImage.color = Color.clear;
                flashActive = false;
            }
        }
    }

    public void TriggerFlash()
    {
        flashImage.color = flashColor;
        flashActive = true;
    }
}
