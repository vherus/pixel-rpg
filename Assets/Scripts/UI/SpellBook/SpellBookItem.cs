using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellBookItem : MonoBehaviour
{
    [field: SerializeField] public Transform UIActionBar { get; set; }

    private ActionBarSlot[] actionSlots;

    void Start() {
        actionSlots = UIActionBar.GetComponentsInChildren<ActionBarSlot>();
    }

    public void OnSelect() {
        Selector selector = actionSlots[0].GetComponentInChildren<Selector>();
        Image selectorIcon = selector.GetComponent<Image>();

        selectorIcon.enabled = true;
    }
}
