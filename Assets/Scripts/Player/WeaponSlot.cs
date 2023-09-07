using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlot : MonoBehaviour
{
    public Weapon EquippedWeapon {
        get { return weapon; }

        set {
            if (weapon != value) {
                weapon = value;

                UpdateSprite();
            }
        }
    }

    [SerializeField] private Weapon weapon;

    private SpriteRenderer sr;

    void UpdateSprite() {
        if (weapon != null) {
            sr.sprite = weapon.Sprite;
            return;
        }

        sr.sprite = null;
    }

    void Start() {
        sr = GetComponent<SpriteRenderer>();
        UpdateSprite();
    }
}
