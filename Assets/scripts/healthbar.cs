using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUIManager : MonoBehaviour
{
    public Image healthFill;
    private HealthComponent playerHealth;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            playerHealth = player.GetComponent<HealthComponent>();
            if (playerHealth != null)
            {
                playerHealth.onHealthChanged.AddListener(UpdateHealthBar);
                playerHealth.onDeath.AddListener(DisableHealthBar); // 👈 Stop UI update on death
                UpdateHealthBar(playerHealth.currentHealth); // Initial fill
            }
        }
    }

   void UpdateHealthBar(float currentHealth)
{
    // Debug.Log("Health: " + currentHealth + " / " + playerHealth.maxHealth);

    if (playerHealth != null && healthFill != null)
    {
        float fillAmount = Mathf.InverseLerp(0, playerHealth.maxHealth, currentHealth);
        Debug.Log("Fill Amount: " + fillAmount); // Add this
        healthFill.fillAmount = fillAmount;
    }
}



    void DisableHealthBar()
    {
        // Option A: Just hide the UI
        // gameObject.SetActive(false);

        // Option B: Disable just the health fill image
        if (healthFill != null)
            healthFill.enabled = false;

        // Optional: Unsubscribe from event to avoid warnings/errors
        if (playerHealth != null)
            playerHealth.onHealthChanged.RemoveListener(UpdateHealthBar);
    }
}
