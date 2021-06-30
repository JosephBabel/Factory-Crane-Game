using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    public GameObject explosionParticlePrefab;

    [SerializeField] private float explosionForce = 10.0f;
    [SerializeField] private float verticalForce = -100.0f;
    [SerializeField] private float radius = 5.0f;

    void OnCollisionEnter(Collision collision)
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);

        foreach (Collider hit in colliders)
        {
            if (!hit.CompareTag("Cannon Ball"))
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    rb.AddExplosionForce(explosionForce, explosionPos, radius, verticalForce, ForceMode.Impulse);
                }
            }
        }

        AudioManager.instance.PlayExplosionSound(Camera.main.transform.position + (transform.position.normalized * 2));
        GameObject particleInstance = Instantiate(explosionParticlePrefab, transform.position, Quaternion.identity);
        Destroy(particleInstance, 1.0f);
        Destroy(gameObject);
    }
}
