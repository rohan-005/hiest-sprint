using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyTypeA;
    public GameObject enemyTypeB;

    public int enemyTypeACount = 5;
    public int enemyTypeBCount = 5;

    public float spawnRadius = 10f;
    public float spawnHeightAboveGround = 0.5f;
    public float spawnDelay = 0.3f; // Delay between each enemy spawn

    public LayerMask groundLayer;
    public LayerMask objectLayer;

    void Start()
    {
        StartCoroutine(SpawnEnemiesWithDelay(enemyTypeA, enemyTypeACount));
        StartCoroutine(SpawnEnemiesWithDelay(enemyTypeB, enemyTypeBCount));
    }

    IEnumerator SpawnEnemiesWithDelay(GameObject prefab, int count)
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
                    yield return new WaitForSeconds(spawnDelay); // delay between spawns
                }
            }
            yield return null; // Avoid freezing the game loop
        }
    }

    Vector3 GetRandomPointInRadius()
    {
        Vector2 circle = Random.insideUnitCircle * spawnRadius;
        return transform.position + new Vector3(circle.x, 0, circle.y);
    }

    bool IsValidSpawnPosition(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, 0.4f, objectLayer);
        return colliders.Length == 0;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
