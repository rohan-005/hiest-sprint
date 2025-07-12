using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public float rotationSpeed = 90f;
    public float pulseScale = 0.05f;
    public float pulseSpeed = 2f;
    public AudioClip pickupSound; // 🔊 Assign in Inspector

    private Vector3 initialScale;
    private AudioSource audioSource;

    void Start()
    {
        initialScale = transform.localScale;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
        float pulse = Mathf.Sin(Time.time * pulseSpeed) * pulseScale;
        transform.localScale = initialScale + Vector3.one * pulse;
    }

    [System.Obsolete]
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ItemBarManager bar = FindObjectOfType<ItemBarManager>();
            if (bar != null)
            {
                bar.AddOneItem();
            }

            if (audioSource != null && pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position); // 🔊 Play once at location
            }

            Destroy(gameObject);
        }
    }
}
