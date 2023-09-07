using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupResources : MonoBehaviour
{
    [field: SerializeField] public Inventory ResourcesInventory { get; private set; }
    
    void OnTriggerEnter2D(Collider2D collider) {
        ResourcePickup pickup = collider.gameObject.GetComponent<ResourcePickup>();

        if (pickup) {
            ResourcesInventory.AddResource(pickup.Target, 1);
            Destroy(pickup.gameObject);
        }
    }
}
