using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [field: SerializeField] public GameObject EquipmentPanel;

    void Start() {
        EquipmentPanel.SetActive(false);
    }

    void OnInventory() {
        GameManager.PauseUnpause();
        GameManager.IsInMenu = !GameManager.IsInMenu;

        EquipmentPanel.SetActive(!EquipmentPanel.activeSelf);
    }
}
