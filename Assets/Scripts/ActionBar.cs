using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBar : MonoBehaviour
{
    [field: SerializeField] public SerializableDictionary<ActionBarSlot, ActionBarAction> ActionSet;
}
