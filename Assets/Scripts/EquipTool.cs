using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipTool : MonoBehaviour
{
    [field: SerializeField] public Tool TargetTool { get; set; } = null;

    private ToolSlot toolSlot;
    private Tool previousTool;

    IEnumerator EquipPreviousTool() {
        yield return new WaitForSeconds(0.7f);
        toolSlot.EquippedTool = previousTool;
    }

    public void ChangeTool() {
        toolSlot = FindObjectOfType<PlayerController>().GetComponentInChildren<ToolSlot>();
        previousTool = toolSlot.EquippedTool;

        toolSlot.EquippedTool = TargetTool;

        StartCoroutine(EquipPreviousTool());

        return;
    }
}
