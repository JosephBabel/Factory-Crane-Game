using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollection : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            int profit = other.gameObject.GetComponent<Item>().profit;
            GameManager.instance.AddToProfit(profit);
            Destroy(other.gameObject);
        }
    }
}
