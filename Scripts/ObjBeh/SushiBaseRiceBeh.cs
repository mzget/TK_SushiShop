using UnityEngine;
using System.Collections;

public class SushiBaseRiceBeh : ObjectsBeh {


	// Use this for initialization
	protected override void Start ()
	{
		base.Start ();

        base.destroyObj_Event += new System.EventHandler(Base_Handle_destroyObj_Event);
	}
	
	// Update is called once per frame
	protected override void Update ()
	{
		base.Update ();
	}

    protected override void ImplementDraggableObject()
    {
        base.ImplementDraggableObject();

        Ray cursorRay;
        RaycastHit hit;
        cursorRay = new Ray(this.transform.position, Vector3.forward);

        if (Physics.Raycast(cursorRay, out hit))
        {
            if (hit.collider.name == sceneManager.bin_behavior_obj.name)
            {
                if (this._isDropObject == true)
                {
                    sceneManager.bin_behavior_obj.PlayOpenAnimation();
                    this.OnDispose();
                    OnDestroyObject_event(System.EventArgs.Empty);
                }
            }
            else
            {
                if (this._isDropObject)
                {
                    this.transform.position = originalPosition;
                    this._isDropObject = false;
                    base._isDraggable = false;
                }
            }
        }
        else
        {
            if (this._isDropObject)
            {
                //            if(_isDraggable == false) {
                this.transform.position = originalPosition;
                this._isDropObject = false;
                base._isDraggable = false;
            }
        }

        Debug.DrawRay(cursorRay.origin, Vector3.forward, Color.red);
    }

}
