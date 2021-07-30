using UnityEngine;

/// <summary>
/// Base item class. By default item profit is cut in half upon dropping it.
/// </summary>
public class Item : MonoBehaviour
{
    // Inspector settings
    public int profit;

    private bool wasGrabbed;
    private bool wasDropped;

    protected virtual void OnDrop() 
    {
        GameManager.instance.IncrementItemsDropped();
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
