using UnityEngine;

public class LaserBullet : MonoBehaviour
{
    public float speed = 30f;
    public float lifeTime = 2f;
    public AudioClip fireSound;
    public float dom = 2f;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = fireSound;
        audioSource.playOnAwake = false;
        audioSource.Play();

        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    [System.Obsolete]
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HealthComponent health = other.GetComponent<HealthComponent>();
            if (health != null)
            {
                health.TakeDamage(dom);
                Debug.Log("Hit player!");

                // 🔁 Apply recoil
                PlayerController controller = other.GetComponent<PlayerController>();
                if (controller != null)
                {
                    Vector3 recoilDir = other.transform.position - transform.position;
                    controller.ApplyRecoil(recoilDir, 5f); // Adjust force as needed
                }

                // 🎨 Flash red UI effect
                PlayerDamageFlash flash = FindObjectOfType<PlayerDamageFlash>();
                if (flash != null)
                {
                    flash.TriggerFlash();
                }
            }

            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
