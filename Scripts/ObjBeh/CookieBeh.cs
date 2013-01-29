using UnityEngine;
using System.Collections;

public class CookieBeh : GoodsBeh {

	// Use this for initialization
    protected override void Start()
    {
        base.Start();
        base.offsetPos = Vector3.up * -0.05f;
        
        base._canDragaable = true;
    }

    protected override void ImplementDraggableObject()
    {
        base.ImplementDraggableObject();
    }
}
