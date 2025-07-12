using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public float healAmount = 20f;
    public float rotationSpeed = 90f;

    public float floatAmplitude = 0.25f;
    public float floatFrequency = 2f;

    public float pulseScale = 0.05f;
    public float pulseSpeed = 2f;

    public AudioClip pickupSound; // 🔊 Assign in Inspector

    private Vector3 startPos;
    private Vector3 initialScale;
    private AudioSource audioSource;

    void Start()
    {
        startPos = transform.position;
        initialScale = transform.localScale;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
        float newY = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = startPos + Vector3.up * newY;

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

            if (audioSource != null && pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position); // 🔊 Play once
            }

            Destroy(gameObject);
        }
    }
}
