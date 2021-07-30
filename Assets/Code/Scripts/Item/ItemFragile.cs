using UnityEngine;

/// <summary>
/// Special item that breaks upon dropping.
/// </summary>
// INHERITANCE
public class ItemFragile : Item
{
    public GameObject smokeParticle;

    // POLYMORPHISM
    protected override void OnDrop()
    {
        AudioManager.instance.PlayClip("Drop", transform.position);
        GameObject particleInstace = Instantiate(smokeParticle, transform.position, Quaternion.identity);
        Destroy(particleInstace, particleInstace.GetComponent<ParticleSystem>().main.duration);
        gameObject.SetActive(false);
    }
}
