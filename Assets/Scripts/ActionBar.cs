using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBar : MonoBehaviour
{
    [field: SerializeField] public SerializableDictionary<ActionBarSlot, ActionBarAction> ActionSet;
    [field: SerializeField] public ActionBarSlot SelectedSlot { 
        get {
            return selectedSlot;
        }
        set {
            if (value == null) {
                selectedSlot = value;
            }
        }
    }

    private ActionBarSlot selectedSlot;

    void UpdateSelectedSlot(ActionBarSlot slot) {

    }
}
