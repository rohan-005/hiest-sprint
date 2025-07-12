using UnityEngine;
using System.Collections;

public class GameOverManager : MonoBehaviour
{
    public CanvasGroup gameOverImage;   // Assign the CanvasGroup of the Game Over image
    public GameObject buttonsGroup;     // Assign your button group
    public float fadeDuration = 1.5f;
    public float delayBeforeButtons = 1.5f;

    public AudioClip gameOverSound;     // 🎵 Assign a Game Over sound in the Inspector
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
    }

    public void ShowGameOver()
    {
        StartCoroutine(FadeInGameOver());
    }

    private IEnumerator FadeInGameOver()
    {
        float elapsed = 0f;

        Time.timeScale = 0f; // Stop gameplay

        // 🎵 Play Game Over Sound using unscaled time
        if (gameOverSound != null)
        {
            audioSource.clip = gameOverSound;
            audioSource.Play(); // AudioSource still works even when Time.timeScale is 0
        }

        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            gameOverImage.alpha = Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }

        yield return new WaitForSecondsRealtime(delayBeforeButtons);

        buttonsGroup.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        gameOverImage.blocksRaycasts = true;
        gameOverImage.interactable = true;
    }
}
