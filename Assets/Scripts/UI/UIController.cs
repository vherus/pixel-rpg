using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [field: SerializeField] public GameObject EquipmentPanel;
    [field: SerializeField] public ActionBar ActionBar;

    void Start() {
        EquipmentPanel.SetActive(false);
    }

    void OnInventory() {
        GameManager.PauseUnpause();

        EquipmentPanel.SetActive(!EquipmentPanel.activeSelf);
    }
}
