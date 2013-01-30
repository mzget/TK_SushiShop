using UnityEngine;
using System.Collections;

public class IcecreamBeh : GoodsBeh {
	
	
	// Use this for initialization
	protected override void Start ()
	{
		base.Start ();

		base._canDragaable = true;
        base.offsetPos = new Vector3(0, -70f, 0);
	}

	protected override void OnTouchEnded()
    {
        base.OnTouchEnded();

		if(base._isDraggable) 
			base._isDropObject = true;
    }
}
