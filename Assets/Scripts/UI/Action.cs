using System;
using UnityEngine;

[Serializable]
public class Action : MonoBehaviour
{
    public string DisplayName { get; set; }
    public Sprite Icon { get; set; }

    public virtual bool Execute() {
        return false;
    }
}
