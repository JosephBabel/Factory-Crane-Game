using UnityEngine;

/// <summary>
/// Spawns cannonballs and launches them.
/// </summary>
public class Cannon : MonoBehaviour
{
    // Inspector settings
    public ObjectPooling objectPool;

    [Header("Cannon Ball Settings")]
    [SerializeField] private float cannonBallForce = 100f;
    [SerializeField] private float cannonBallTorque = 50f;

    // Get GameManager difficulty settings
    private float minSpawnTimer => GameManager.instance.cannonBallMinSpawnTimer;
    private float maxSpawnTimer => GameManager.instance.cannonBallMaxSpawnTimer;

    private float nextSpawnTime;
    private float timer;

    void Start()
    {
        nextSpawnTime = Random.Range(2, 5);
    }

    void Update()
    {
        if (GameManager.instance.isRunning)
        {
            timer += Time.deltaTime;
            if (timer >= nextSpawnTime)
            {
                SpawnCannonBall();
                nextSpawnTime = Random.Range(minSpawnTimer, maxSpawnTimer);
                timer = 0f;
            }
        }
    }

    void SpawnCannonBall()
    {
        GameObject newCannonBall = objectPool.RetrieveCannonBall();

        if (newCannonBall != null)
        {
            newCannonBall.transform.position = gameObject.transform.position;
            Rigidbody cannonBallRb = newCannonBall.GetComponent<Rigidbody>();
            cannonBallRb.AddForce(transform.TransformDirection(Vector3.right) * cannonBallForce, ForceMode.Impulse);
            cannonBallRb.AddTorque(transform.TransformDirection(Vector3.back) * cannonBallTorque, ForceMode.Impulse);
        }
    }
}
