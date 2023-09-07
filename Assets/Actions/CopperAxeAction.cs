using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopperAxeAction : ActionBarAction
{
    [field: SerializeField] public EquipTool EquipTool { get; private set; }

    IEnumerator DestroySelf() {
        yield return new WaitForSeconds(.8f);

        Destroy(gameObject);
    }

    public override void Execute() {
        EquipTool.ChangeTool();
        print("Copper Axe action was executed!");

        StartCoroutine(DestroySelf());
    }
}
