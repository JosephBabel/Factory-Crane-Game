using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// INHERITANCE
public class ItemFragile : Item
{
    public GameObject smokeParticle;

    // POLYMORPHISM
    protected override void OnDrop()
    {
        AudioManager.instance.PlayClipAt("Drop", transform.position);
        GameObject particleInstace = Instantiate(smokeParticle, transform.position, Quaternion.identity);
        Destroy(particleInstace, 2.5f);
        Destroy(gameObject);
    }
}
