using System.Collections;
using UnityEngine;
using TMPro;

/// <summary>
/// Adds profit of items to score and adds force to launch items into space.
/// </summary>
public class ItemCollection : MonoBehaviour
{
    // Inspector settings
    [SerializeField] private float launchForce = 20f;

    public GameObject profitTextPrefab;

    private Rigidbody itemRb;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            int profit = other.gameObject.GetComponent<Item>().profit;
            GameManager.instance.AddToProfit(profit);
            GameObject profitText = Instantiate(profitTextPrefab, transform.position, Quaternion.identity);
            TextMeshPro textMeshPro = profitText.GetComponent<TextMeshPro>();
            textMeshPro.text = "+$" + profit.ToString("N0");

            AudioManager.instance.PlayClip("Money");

            itemRb = other.gameObject.GetComponent<Rigidbody>();
            itemRb.useGravity = false;
            itemRb.velocity = Vector3.zero;
            itemRb.angularVelocity = Vector3.zero;
            StartCoroutine(ShootItem(other));
        }
    }

    IEnumerator ShootItem(Collider other)
    {
        GameManager.instance.IncrementItemsLaunched();
        AudioManager.instance.PlayClip("Shoot", transform.position);
        yield return new WaitForSeconds(2f);
        itemRb.AddForce(transform.TransformDirection(Vector3.forward + Vector3.up) * launchForce, ForceMode.Impulse);
        yield return new WaitForSeconds(5f);
        other.gameObject.SetActive(false);
    }
}
