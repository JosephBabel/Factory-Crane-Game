using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int profit;

    private bool wasGrabbed;
    private bool wasDropped;

    protected virtual void OnDrop() 
    {
        profit /= 2;
    }

    void OnTriggerStay(Collider other)
    {
        if (!wasGrabbed && other.CompareTag("Claw") && (transform.position.y > GameManager.instance.dropYHeight))
            wasGrabbed = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") && wasGrabbed && !wasDropped)
        {
            wasDropped = true;
            OnDrop();
        }
    }
}
