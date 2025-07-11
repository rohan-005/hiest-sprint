using UnityEngine;
using System.Collections;

public class GameOverManager : MonoBehaviour
{
    public CanvasGroup gameOverImage;   // Assign the CanvasGroup of the Game Over image
    public GameObject buttonsGroup;     // Assign your button group
    public float fadeDuration = 1.5f;
    public float delayBeforeButtons = 1.5f;

    public void ShowGameOver()
    {
        StartCoroutine(FadeInGameOver());
    }

    private IEnumerator FadeInGameOver()
    {
        float elapsed = 0f;

        // Disable all gameplay by setting Time.timeScale to 0 (optional but helps prevent background actions)
        Time.timeScale = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime; // Use unscaled time since timeScale is 0
            gameOverImage.alpha = Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }

        yield return new WaitForSecondsRealtime(delayBeforeButtons); // Again, unscaled wait

        buttonsGroup.SetActive(true);

        // 👇 Show and unlock mouse cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // 👇 Block background interaction (optional but recommended)
        gameOverImage.blocksRaycasts = true;
        gameOverImage.interactable = true;
    }
}
