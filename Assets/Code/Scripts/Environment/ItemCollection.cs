using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollection : MonoBehaviour
{
    [SerializeField] private float launchForce = 20.0f;

    private Rigidbody itemRb;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            int profit = other.gameObject.GetComponent<Item>().profit;
            GameManager.instance.AddToProfit(profit);
            itemRb = other.gameObject.GetComponent<Rigidbody>();
            itemRb.useGravity = false;
            itemRb.velocity = Vector3.zero;
            StartCoroutine(ShootItem(other));
        }
    }

    IEnumerator ShootItem(Collider other)
    {
        AudioManager.instance.PlayClipAt("Shoot", transform.position);
        yield return new WaitForSeconds(2f);
        itemRb.AddForce(transform.TransformDirection(Vector3.forward + Vector3.up) * launchForce, ForceMode.Impulse);
        Destroy(other.gameObject, 5.0f);
    }
}
