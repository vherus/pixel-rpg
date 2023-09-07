using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePickup : MonoBehaviour
{
    [field: SerializeField] public Resource Target { get; private set; }
}
