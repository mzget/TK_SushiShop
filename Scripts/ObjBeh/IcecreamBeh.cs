using UnityEngine;
using System.Collections;

public class IcecreamBeh : GoodsBeh {
	
	
	// Use this for initialization
	protected override void Start ()
	{
		base.Start ();
		
		Debug.Log("Starting : IcecreamBeh");

		base._canDragaable = true;
		base.originalPosition = this.transform.position;
        base.offsetPos = new Vector3(0, -0.075f, 0);
	}

	protected override void OnTouchEnded()
    {
        base.OnTouchEnded();

		if(base._isDraggable) 
			base._isDropObject = true;
    }
}
