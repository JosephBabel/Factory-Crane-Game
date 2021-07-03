using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public List<GameObject> items;

    [SerializeField] private float spawnTimer => GameManager.instance.itemSpawnTimer;

    void Start()
    {
        StartCoroutine(SpawnItems());
    }

    IEnumerator SpawnItems()
    {
        while (true) 
        {
            while (GameManager.instance.isRunning) // While game is running
            {
                int randomIndex = Random.Range(0, items.Count);
                Instantiate(items[randomIndex], transform.position, Quaternion.identity);
                yield return new WaitForSeconds(spawnTimer);
            }

            yield return null;
        }
    }
}
