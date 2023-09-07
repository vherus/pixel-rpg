using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupGravity : MonoBehaviour
{
    [field: SerializeField] public float GravitySpeed { get; private set; } = 0.6f;

    private List<ResourcePickup> nearbyResources = new();

    void OnTriggerEnter2D(Collider2D collider) {
        ResourcePickup pickup = collider.gameObject.GetComponent<ResourcePickup>();

        if (pickup) {
            nearbyResources.Add(pickup);
        }
    }

    void OnTriggerExit2D(Collider2D collider) {
        ResourcePickup pickup = collider.gameObject.GetComponent<ResourcePickup>();

        if (pickup) {
            nearbyResources.Remove(pickup);
        }
    }

    void FixedUpdate() {
        nearbyResources.RemoveAll(pickup => pickup == null);

        foreach (ResourcePickup pickup in nearbyResources) {
            Vector2 directionToCenter = (transform.position - pickup.transform.position).normalized;

            pickup.transform.Translate(directionToCenter * GravitySpeed * Time.fixedDeltaTime);
        }
    }
}
