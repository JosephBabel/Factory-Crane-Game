using UnityEngine;

public class CannonBall : MonoBehaviour
{
    // Inspector settings
    public GameObject explosionParticlePrefab;

    [Header("Cannon Ball Settings")]
    [SerializeField] private bool explodeOnImpact = true;
    [SerializeField] private float explosionForce = 10.0f;
    [SerializeField] private float verticalForce = -100.0f;
    [SerializeField] private float radius = 5.0f;

    private CameraController cameraController => Camera.main.GetComponent<CameraController>();

    void OnCollisionEnter(Collision collision)
    {
        if (explodeOnImpact)
            Explode();
    }

    public void Explode()
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);

        foreach (Collider hit in colliders)
        {
            if (!hit.CompareTag("Cannon Ball"))
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                // Add explosion force to all nearby objects
                if (rb != null)
                    rb.AddExplosionForce(explosionForce, explosionPos, radius, verticalForce, ForceMode.Impulse);

                // Check if crane was hit
                if (hit.CompareTag("Claw"))
                {
                    GameManager.instance.IncrementBombsExploded();

                    if (cameraController)
                        cameraController.ShakeCamera();
                }
            }
        }

        // Play audio
        AudioManager.instance.PlayClip("Explosion", transform.position);

        // Play particle effect
        GameObject particleInstance = Instantiate(explosionParticlePrefab, transform.position, Quaternion.identity);
        Destroy(particleInstance, 3f);

        gameObject.SetActive(false);
    }
}
