using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class HealthComponent : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    public UnityEvent onDeath;
    public GameObject deathEffect;

    private DeathCameraManager cameraManager;
    public UnityEvent<float> onHealthChanged;

    public float delayBeforeGameOver = 3f; // Delay after death before Game Over

    [System.Obsolete]
    void Awake()
    {
        if (onHealthChanged == null)
            onHealthChanged = new UnityEvent<float>();

        if (onDeath == null)
            onDeath = new UnityEvent();

        cameraManager = FindObjectOfType<DeathCameraManager>();
    }

    void Start()
    {
        currentHealth = maxHealth;
        onHealthChanged.Invoke(currentHealth);
    }

    [System.Obsolete]
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        onHealthChanged?.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    [System.Obsolete]
    public void Die()
    {
        if (deathEffect != null)
            Instantiate(deathEffect, transform.position, Quaternion.identity);

        PlayerController controller = GetComponent<PlayerController>();
        if (controller != null)
        {
            controller.isDead = true;
            controller.enabled = false;
        }

        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        Animator anim = GetComponent<Animator>();
        if (anim != null) anim.SetTrigger("Die");

        if (cameraManager != null)
            cameraManager.SwitchToDeathCamera();

        onDeath?.Invoke();

        StartCoroutine(HandleDeathSequence());
    }

    [System.Obsolete]
    private IEnumerator HandleDeathSequence()
    {
        yield return new WaitForSeconds(delayBeforeGameOver);

        // 🎬 Show Game Over fade-in screen
        GameOverManager gameOver = FindObjectOfType<GameOverManager>();
        if (gameOver != null)
            gameOver.ShowGameOver();

        // 💥 Then destroy the player GameObject
        Destroy(gameObject);
    }
    [System.Obsolete]
public void Heal(float amount)
{
    if (currentHealth <= 0) return; // Don't heal if dead

    currentHealth += amount;
    currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

    onHealthChanged?.Invoke(currentHealth);
}

}
