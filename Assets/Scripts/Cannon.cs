using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public GameObject cannonBall;

    [SerializeField] private float cannonBallForce = 100.0f;
    [SerializeField] private float cannonBallTorque = 50.0f;

    private float minSpawnTimer => GameManager.instance.cannonBallMinSpawnTimer;
    private float maxSpawnTimer => GameManager.instance.cannonBallMaxSpawnTimer;

    private float nextSpawnTime;

    void Start()
    {
        nextSpawnTime = Random.Range(minSpawnTimer, maxSpawnTimer);
        StartCoroutine(SpawnCannonBalls());
    }

    IEnumerator SpawnCannonBalls()
    {
        while (true)
        {
            while (GameManager.instance.isRunning)
            {
                yield return new WaitForSeconds(nextSpawnTime);
                GameObject newCannonBall = Instantiate(cannonBall, transform.position, Quaternion.identity);
                Rigidbody cannonBallRb = newCannonBall.GetComponent<Rigidbody>();
                cannonBallRb.AddForce(transform.TransformDirection(Vector3.right) * cannonBallForce, ForceMode.Impulse);
                cannonBallRb.AddTorque(transform.TransformDirection(Vector3.back) * cannonBallTorque, ForceMode.Impulse);
                nextSpawnTime = Random.Range(minSpawnTimer, maxSpawnTimer);
            }

            yield return null;
        }
    }
}
