using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectEmitter : MonoBehaviour
{
    [field: SerializeField] public GameObject ObjectPrefab { get; private set; }

    private ParticleSystem ps;
    private List<ParticleSystem.Particle> exitParticles = new();

    void Start() {
        ps = GetComponent<ParticleSystem>();
    }

    void OnParticleTrigger() {
        ps.GetTriggerParticles(ParticleSystemTriggerEventType.Exit, exitParticles);

        foreach (ParticleSystem.Particle p in exitParticles) {
            GameObject spawnedObject = Instantiate(ObjectPrefab);
            spawnedObject.transform.position = p.position;
        }
    }
}
