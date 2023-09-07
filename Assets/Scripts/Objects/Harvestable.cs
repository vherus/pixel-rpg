using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harvestable : MonoBehaviour
{
    [field: SerializeField] public ToolType RequiredToolType { get; private set; }
    [field: SerializeField] public int ResourceCount { get; private set; }
    [field: SerializeField] public ParticleSystem ParticleEmitter { get; private set; }
    [field: SerializeField] public GameObject EffectOnDestroyPrefab { get; private set; }

    private int amountHarvested = 0;

    public bool TryHarvest(ToolType toolType, int amount) {
        if (toolType == RequiredToolType) {
            Harvest(amount);
            return true;
        }

        return false;
    }

    void Harvest(int amount) {
        int amountToSpawn = Mathf.Min(amount, ResourceCount - amountHarvested);

        if (amountToSpawn > 0) {
            ParticleEmitter.Emit(amount);
            amountHarvested += amountToSpawn;
        }

        if (amountHarvested >= ResourceCount) {
            if (EffectOnDestroyPrefab) {
                Instantiate(EffectOnDestroyPrefab, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }
}
