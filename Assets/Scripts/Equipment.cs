using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    [field: SerializeField] public SerializableDictionary<EquipSlot, Equippable> Equipped { get; set; }
}
