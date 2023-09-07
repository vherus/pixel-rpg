using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolSlot : MonoBehaviour
{
    public Tool EquippedTool {
        get { return tool; }

        set {
            if (tool != value) {
                tool = value;

                UpdateSprite();
            }
        }
    }

    [SerializeField] private Tool tool;

    private SpriteRenderer sr;

    void UpdateSprite() {
        if (tool != null) {
            sr.sprite = tool.Sprite;
            return;
        }

        sr.sprite = null;
    }

    void Start() {
        sr = GetComponent<SpriteRenderer>();
        UpdateSprite();
    }

    void OnTriggerEnter2D(Collider2D collision) {
        Harvestable harvestable = collision.GetComponent<Harvestable>();

        if (harvestable != null) {
            int amountToHarvest = Random.Range(EquippedTool.MinHarvest, EquippedTool.MaxHarvest);
            harvestable.TryHarvest(EquippedTool.Type, amountToHarvest);
        }
    }
}
