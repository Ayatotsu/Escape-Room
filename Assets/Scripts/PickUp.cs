using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public Item item = new Item("Item Name", 1);

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Inventory.instance.AddItem(item);
            Destroy(gameObject);
        }
    }
}
