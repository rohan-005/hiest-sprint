using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    public UnityEvent onDeath;
    public GameObject deathEffect;

    private DeathCameraManager cameraManager;
    public UnityEvent<float> onHealthChanged;

    [System.Obsolete]
    void Awake()
{
    if (onHealthChanged == null)
        onHealthChanged = new UnityEvent<float>();

    if (onDeath == null)
        onDeath = new UnityEvent();
}




   void Start()
{
    currentHealth = maxHealth;
    onHealthChanged.Invoke(currentHealth);
}


    public void TakeDamage(float amount)
{
    currentHealth -= amount;
    currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

    if (onHealthChanged != null)
        onHealthChanged.Invoke(currentHealth); // 👈 this now works after Awake()

    if (currentHealth <= 0)
    {
        Die();
    }
}


    public void Die()
    {
        Debug.Log("Player Died");

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

        // 👇 Use the manager to activate the death camera
        if (cameraManager != null)
            cameraManager.ActivateDeathCamera();

        Invoke(nameof(DisablePlayer), 3f);
    }

    void DisablePlayer()
    {
        gameObject.SetActive(false);
    }
    
}
