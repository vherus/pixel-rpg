using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipWeapon : MonoBehaviour
{
    [field: SerializeField] public Weapon TargetWeapon { get; set; } = null;

    private WeaponSlot weaponSlot;

    public void ChangeWeapon() {
        weaponSlot = FindObjectOfType<PlayerController>().GetComponentInChildren<WeaponSlot>();

        weaponSlot.EquippedWeapon = TargetWeapon;

        return;
    }
}
