using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public float fillAmountOnPickup = 0.2f; 
    public float rotationSpeed = 90f;

    public float pulseScale = 0.05f;       // Scale variation
    public float pulseSpeed = 2f;          // Speed of pulsing

    private Vector3 initialScale;

    void Start()
    {
        initialScale = transform.localScale;
    }

    void Update()
    {
        // Rotate
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);

        // Pulse scale
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
                bar.AddToBar(fillAmountOnPickup);
            }

            Destroy(gameObject);
        }
    }
}
