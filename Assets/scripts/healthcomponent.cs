using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class HealthComponent : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    public UnityEvent onDeath;
    public UnityEvent<float> onHealthChanged;
    public GameObject deathEffect;

    private DeathCameraManager cameraManager;
    private PlayerDamageFlash damageFlash;
    public float delayBeforeGameOver = 3f;

    // Fall damage settings
    private float fallThresholdY = -5f;
    private float fallDamageAmount = 10f;
    private float fallCheckInterval = 2f;
    private float nextFallDamageTime = 0f;

    [System.Obsolete]
    void Awake()
    {
        if (onHealthChanged == null)
            onHealthChanged = new UnityEvent<float>();

        if (onDeath == null)
            onDeath = new UnityEvent();

        cameraManager = FindObjectOfType<DeathCameraManager>();
        damageFlash = FindObjectOfType<PlayerDamageFlash>(); // 👈 Find the flash system
    }

    void Start()
    {
        currentHealth = maxHealth;
        onHealthChanged.Invoke(currentHealth);
    }

    [System.Obsolete]
    void Update()
    {
        CheckFallDamage();
    }

    [System.Obsolete]
    public void TakeDamage(float amount)
    {
        if (currentHealth <= 0) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        onHealthChanged?.Invoke(currentHealth);

        // 🔴 Trigger damage flash effect
        if (damageFlash != null)
            damageFlash.TriggerFlash();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    [System.Obsolete]
    public void Heal(float amount)
    {
        if (currentHealth <= 0) return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        onHealthChanged?.Invoke(currentHealth);
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

        GameOverManager gameOver = FindObjectOfType<GameOverManager>();
        if (gameOver != null)
            gameOver.ShowGameOver();

        Destroy(gameObject);
    }

    [System.Obsolete]
    private void CheckFallDamage()
    {
        if (currentHealth <= 0) return;

        if (transform.position.y < fallThresholdY && Time.time >= nextFallDamageTime)
        {
            TakeDamage(fallDamageAmount);
            nextFallDamageTime = Time.time + fallCheckInterval;
        }
    }
}
