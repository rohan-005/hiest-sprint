using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public float healAmount = 20f;
    public float rotationSpeed = 90f;

    public float floatAmplitude = 0.25f;
    public float floatFrequency = 2f;

    public float pulseScale = 0.05f;
    public float pulseSpeed = 2f;

    private Vector3 startPos;
    private Vector3 initialScale;

    void Start()
    {
        startPos = transform.position;
        initialScale = transform.localScale;
    }

    void Update()
    {
        // Rotate
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);

        // Float
        float newY = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = startPos + Vector3.up * newY;

        // Pulse
        float pulse = Mathf.Sin(Time.time * pulseSpeed) * pulseScale;
        transform.localScale = initialScale + Vector3.one * pulse;
    }

    [System.Obsolete]
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HealthComponent health = other.GetComponent<HealthComponent>();
            if (health != null && health.currentHealth < health.maxHealth)
            {
                health.Heal(healAmount);

                PlayerHealFlash flash = FindObjectOfType<PlayerHealFlash>();
                if (flash != null)
                    flash.TriggerFlash();
            }

            Destroy(gameObject);
        }
    }
}
