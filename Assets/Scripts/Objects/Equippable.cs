using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Equippable", menuName = "ScriptableObjects/Equippable")]
public class Equippable : ScriptableObject
{
    [field: SerializeField] public string DisplayName { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public EquipSlot EquipmentSlot { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public int Value { get; private set; }
    [field: SerializeField] public int Damage { get; private set; } = 0;
    [field: SerializeField] public int Defense { get; private set; } = 0;
}
