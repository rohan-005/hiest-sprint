using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    public Slider healthSlider; // Assign in Inspector if using UI
    public GameObject deathEffect; // Optional death effect

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        UpdateHealthUI();

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void UpdateHealthUI()
    {
        if (healthSlider != null)
            healthSlider.value = currentHealth / maxHealth;
    }

   public void Die()
{
    Debug.Log("Player Died!");

    // Spawn death effect if assigned
    if (deathEffect != null)
        Instantiate(deathEffect, transform.position, Quaternion.identity);

    // Disable player controller and collider
    PlayerController controller = GetComponent<PlayerController>();
    if (controller != null) controller.enabled = false;

    Collider col = GetComponent<Collider>();
    if (col != null) col.enabled = false;

    // Optionally disable renderer or trigger death animation
    Animator anim = GetComponent<Animator>();
    if (anim != null)
    {
        anim.SetTrigger("Die");
    }

    // Optionally delay GameObject deactivation
    // You can also show a Game Over screen instead
    Invoke(nameof(DisablePlayer), 2f);
}

void DisablePlayer()
{
    gameObject.SetActive(false);
}

}
