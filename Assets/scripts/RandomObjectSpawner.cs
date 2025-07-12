using UnityEngine;

[System.Serializable]
public class SpawnableObject
{
    public GameObject prefab;
    public int spawnCount = 10;
    public float checkRadius = 1f; // Prevent overlap
}

public class RandomObjectSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public SpawnableObject[] objectsToSpawn;
    public float spawnRadius = 50f;
    public LayerMask groundLayer;
    public LayerMask objectLayer; // Layer for spawned objects

    [Header("Raycast Settings")]
    public float raycastHeight = 100f;
    public float spawnYOffset = 0.1f;
    public int maxTriesPerObject = 100;

    void Start()
    {
        SpawnAllObjects();
    }

    void SpawnAllObjects()
    {
        foreach (SpawnableObject item in objectsToSpawn)
        {
            int spawned = 0;
            int attempts = 0;

            while (spawned < item.spawnCount && attempts < item.spawnCount * maxTriesPerObject)
            {
                attempts++;

                Vector3 randomPosition = transform.position + new Vector3(
                    Random.Range(-spawnRadius, spawnRadius),
                    raycastHeight,
                    Random.Range(-spawnRadius, spawnRadius)
                );

                if (Physics.Raycast(randomPosition, Vector3.down, out RaycastHit hit, raycastHeight * 2, groundLayer))
                {
                    Vector3 spawnPosition = hit.point + Vector3.up * spawnYOffset;

                    if (Physics.CheckSphere(spawnPosition, item.checkRadius, objectLayer))
                        continue;

                    GameObject obj = Instantiate(item.prefab, spawnPosition, Quaternion.Euler(0, Random.Range(0f, 360f), 0));
                    obj.layer = objectLayer == (objectLayer | (1 << obj.layer)) ? obj.layer : LayerMaskToLayer(objectLayer);
                    spawned++;
                }
            }
        }
    }

    int LayerMaskToLayer(LayerMask mask)
    {
        int layer = 0;
        int maskVal = mask.value;
        while (maskVal > 1)
        {
            maskVal = maskVal >> 1;
            layer++;
        }
        return layer;
    }
}
