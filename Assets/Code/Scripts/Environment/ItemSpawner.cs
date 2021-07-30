using UnityEngine;

/// <summary>
/// Spawns items at set interval when game isRunning.
/// </summary>
public class ItemSpawner : MonoBehaviour
{
    public ObjectPooling objectPool;

    // GameManager difficulty settings
    [SerializeField] private float spawnTimer => GameManager.instance.itemSpawnTimer;

    private float timer = float.MaxValue;

    void Update()
    {
        if (GameManager.instance.isRunning)
        {
            timer += Time.deltaTime;
            if (timer >= spawnTimer)
            {
                SpawnItem();
                timer = 0f;
            }
        }
    }

    void SpawnItem()
    {
        int randomIndex = Random.Range(0, objectPool.UniqueItemTypeCount);
        GameObject newItem = objectPool.RetrieveItem(randomIndex);
        newItem.transform.position = gameObject.transform.position;
    }
}
