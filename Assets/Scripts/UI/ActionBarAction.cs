using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBarAction : MonoBehaviour
{
    public virtual void Execute() {
        print("Action was executed!");
    }
}
