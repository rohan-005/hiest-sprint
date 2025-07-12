using UnityEngine;

public class RewardSpawner : MonoBehaviour
{
    public GameObject coinPrefab;
    public GameObject heartPrefab;

    public int coinCount = 10;
    public int heartCount = 5;

    public float spawnRadius = 10f;
    public float spawnHeightAboveGround = 0.5f;
    public float objectPadding = 0.5f; // Padding distance from object walls

    public LayerMask groundLayer;
    public LayerMask objectLayer;

    void Start()
    {
        SpawnRewards(coinPrefab, coinCount);
        SpawnRewards(heartPrefab, heartCount);
    }

    void SpawnRewards(GameObject prefab, int count)
    {
        int spawned = 0;
        int attempts = 0;

        while (spawned < count && attempts < count * 10)
        {
            attempts++;

            Vector3 randomPos = GetRandomPointInRadius();

            if (Physics.Raycast(randomPos + Vector3.up * 10f, Vector3.down, out RaycastHit hit, 20f, groundLayer))
            {
                Vector3 spawnPos = hit.point + Vector3.up * spawnHeightAboveGround;

                if (IsValidSpawnPosition(spawnPos))
                {
                    Instantiate(prefab, spawnPos, Quaternion.identity);
                    spawned++;
                }
            }
        }
    }

    Vector3 GetRandomPointInRadius()
    {
        Vector2 circle = Random.insideUnitCircle * spawnRadius;
        return transform.position + new Vector3(circle.x, 0, circle.y);
    }

    bool IsValidSpawnPosition(Vector3 position)
    {
        // Increase radius to check padding from walls/obstacles
        Collider[] colliders = Physics.OverlapSphere(position, objectPadding, objectLayer);
        return colliders.Length == 0;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
