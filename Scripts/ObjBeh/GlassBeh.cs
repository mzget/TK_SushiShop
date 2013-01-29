using UnityEngine;
using System.Collections;

public class GlassBeh : GoodsBeh {



	// Use this for initialization
    protected override void Start()
    {
        base.Start();

        _canDragaable = true;
    }
	
	protected override void ImplementDraggableObject ()
	{
		base.ImplementDraggableObject ();
	}
	
	// Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
	
	
    protected override void OnTouchEnded()
    {
		base.OnTouchEnded();
		
		if(base._isDraggable)
			_isDropObject = true;
    }
}
