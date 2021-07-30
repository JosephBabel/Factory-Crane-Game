using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Instantiates objects ahead of time and retrieves them for reuse.
/// </summary>
public class ObjectPooling : MonoBehaviour
{
    // Inspector settings
    [Header("Object Pooling Settings")]
    [Range(1, 100)]
    public int numberOfCannonBalls = 10;
    [Range(1, 20)]
    public int numberOfEachItem = 10;

    // Prefabs
    public GameObject cannonBallPrefab;
    public GameObject[] itemPrefabs;

    // GameObject pool queues
    private Queue<GameObject> cannonBalls;
    private Queue<GameObject>[] items;

    /// <summary>Number of unique item types.</summary>
    public int UniqueItemTypeCount => itemPrefabs.Length;

    void Start()
    {
        cannonBalls = new Queue<GameObject>();

        items = new Queue<GameObject>[itemPrefabs.Length];

        // Create unique queue for each item type and store in array of queues
        for (int i = 0; i < itemPrefabs.Length; i++)
            items[i] = new Queue<GameObject>();

        InstantiateCannonBalls();
        InstantiateItems();
    }

    void InstantiateCannonBalls()
    {
        // Fill cannonBall pool queue
        for (int i = 0; i < numberOfCannonBalls; i++)
        {
            GameObject newCannonBall = Instantiate(cannonBallPrefab);
            newCannonBall.SetActive(false);
            cannonBalls.Enqueue(newCannonBall);
        }
    }

    void InstantiateItems()
    {
        // Fill all item pool queues
        for (int i = 0; i < itemPrefabs.Length; i++)
        {
            for (int j = 0; j < numberOfEachItem; j++)
            {
                GameObject newItem = Instantiate(itemPrefabs[i]);
                newItem.SetActive(false);
                items[i].Enqueue(newItem);
            }
        }
    }

    void CleanCannonBall(GameObject gameObject)
    {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        gameObject.transform.rotation = Quaternion.identity;
    }

    void CleanItem(GameObject gameObject)
    {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.useGravity = true;
        gameObject.transform.rotation = Quaternion.identity;
    }

    /// <summary>
    /// Retrieves cannon ball GameObject
    /// </summary>
    /// <returns>Non-active cannonBall GameObject.</returns>
    public GameObject RetrieveCannonBall()
    {
        if (!cannonBalls.Peek().activeSelf)
        {
            GameObject retrievedCannonBall = cannonBalls.Dequeue();
            cannonBalls.Enqueue(retrievedCannonBall);
            CleanCannonBall(retrievedCannonBall);
            retrievedCannonBall.SetActive(true);
            return retrievedCannonBall;
        }

        return null;
    }

    /// <summary>
    /// Retrieves item GameObject
    /// </summary>
    /// <param name="itemType">corresponds to type of item to retrieve. Each item type
    /// belongs to unique queue.</param>
    /// <returns>Item GameObject</returns>
    public GameObject RetrieveItem(int itemType)
    {
        if (itemType >= 0 && itemType < itemPrefabs.Length)
        {
            GameObject retrievedItem = items[itemType].Dequeue();
            items[itemType].Enqueue(retrievedItem);
            CleanItem(retrievedItem);
            retrievedItem.SetActive(true);
            return retrievedItem;
        }
        else
        {
            Debug.LogError($"Error: argument itemType is OutOfBounds. (Limited between [0, {itemPrefabs.Length - 1}])");
            return null;
        }
    }
}
