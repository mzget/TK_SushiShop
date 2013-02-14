using UnityEngine;
using System.Collections;

public class SushiBaseRiceBeh : ObjectsBeh {

    private BinBeh bin_behavior_obj;


	protected override void Awake ()
	{
		base.Awake ();
	}

	// Use this for initialization
	protected override void Start ()
	{
		base.Start ();

        bin_behavior_obj = base.baseScene.GetComponent<SushiShop>().binBeh;        
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
            if (hit.collider.name == bin_behavior_obj.name)
            {
                if (this._isDropObject == true)
                {
                    bin_behavior_obj.PlayOpenAnimation();
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
