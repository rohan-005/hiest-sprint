using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public float walkRange = 10f;
    public float attackRange = 5f;
    public float walkSpeed = 2f;
    public float runSpeed = 4f;

    public GameObject bulletPrefab;
    public Transform shootOrigin;
    public float bulletSpeed = 20f;
    public float shootCooldown = 1.5f;
    public LayerMask groundLayer;

    public float separationRadius = 1.5f;
    public float separationStrength = 2f;

    private Transform player;
    private Animator anim;
    private float lastShotTime;
    private PlayerController playerController;


    void Start()
{
    anim = GetComponent<Animator>();
    GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
    if (playerObj != null)
    {
        player = playerObj.transform;
        playerController = playerObj.GetComponent<PlayerController>(); // 👈 Get controller
    }
}


   void Update()
{
    if (player == null || (playerController != null && playerController.isDead))
        return; // 🛑 Stop everything if player is dead

    float distance = Vector3.Distance(transform.position, player.position);
    Vector3 direction = (player.position - transform.position).normalized;
    direction.y = 0;

    direction += GetSeparationOffset();
    direction = direction.normalized;

    if (direction != Vector3.zero)
    {
        Quaternion lookRot = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 5f);
    }

    if (IsGrounded())
    {
        if (distance <= attackRange)
        {
            anim.SetBool("walk", false);
            anim.SetBool("run", false);
            TryShoot();
        }
        else if (distance <= walkRange)
        {
            anim.SetBool("walk", true);
            anim.SetBool("run", false);
            transform.Translate(direction * walkSpeed * Time.deltaTime, Space.World);
        }
        else
        {
            anim.SetBool("walk", false);
            anim.SetBool("run", true);
            transform.Translate(direction * runSpeed * Time.deltaTime, Space.World);
        }
    }
}


    void TryShoot()
    {
        if (Time.time - lastShotTime < shootCooldown) return;

        if (bulletPrefab != null && shootOrigin != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, shootOrigin.position, shootOrigin.rotation);
        }

        lastShotTime = Time.time;
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f, groundLayer);
    }

    Vector3 GetSeparationOffset()
    {
        Collider[] nearby = Physics.OverlapSphere(transform.position, separationRadius);
        Vector3 repulsion = Vector3.zero;

        foreach (Collider col in nearby)
        {
            if (col.gameObject != gameObject && col.CompareTag("Enemy"))
            {
                Vector3 away = transform.position - col.transform.position;
                float dist = away.magnitude;
                if (dist > 0)
                    repulsion += away.normalized / dist;
            }
        }

        return repulsion * separationStrength;
    }

    // Optional: Debug separation radius in Scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, separationRadius);
    }
}
