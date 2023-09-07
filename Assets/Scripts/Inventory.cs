using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [field: SerializeField] private SerializableDictionary<Resource, int> Resources { get; set; }

    public int GetResourceCount(Resource resource) {
        if (Resources.TryGetValue(resource, out int currentCount)) {
            return currentCount;
        }

        return 0;
    }

    public int AddResource(Resource resource, int count) {
        if (Resources.TryGetValue(resource, out int currentCount)) {
            return Resources[resource] += count;
        }
        
        Resources.Add(resource, count);
        return count;
    }
}
