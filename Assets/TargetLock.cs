using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLock : MonoBehaviour
{
    [field: SerializeField] public Enemy Target { get; set; }
    private Transform player;

    void Start() {
        player = GetComponentInParent<PlayerController>().gameObject.transform;
    }

    void Update() {
        if (Target) {
            transform.position = (Target.transform.position + player.position) / 2.0f;
            return;
        }

        transform.position = player.position;
    }
}
