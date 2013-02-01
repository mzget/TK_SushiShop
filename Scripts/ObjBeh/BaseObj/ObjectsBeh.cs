using UnityEngine;
using System.Collections;

public class ObjectsBeh : Base_ObjectBeh {

	protected Mz_BaseScene baseScene;    
	protected SushiShop sceneManager;
	internal tk2dSprite sprite;
    internal tk2dAnimatedSprite animatedSprite;

    public bool _canDragaable = false;
	protected bool _isDraggable = false;
    protected bool _isDropObject = false;
	protected bool _canActive = false;	
    internal Vector3 originalPosition;

    /// <summary>
    /// destroyObj_Event.
    /// </summary>	
	public event System.EventHandler destroyObj_Event;
    protected void OnDestroyObject_event(System.EventArgs e) {
        if (destroyObj_Event != null)
        {
            destroyObj_Event(this, e);
            Debug.Log(destroyObj_Event + ": destroyObj_Event : " + this.name);
        }
    }

	protected virtual void Awake() {
		try {
			sprite = this.gameObject.GetComponent<tk2dSprite>();
			animatedSprite = this.gameObject.GetComponent<tk2dAnimatedSprite>();
		}
		catch { }
	}
	
	// Use this for initialization
	protected virtual void Start () {		
        baseScene = GameObject.FindGameObjectWithTag("GameController").GetComponent<Mz_BaseScene>();
        sceneManager = baseScene as SushiShop;

		this.originalPosition = this.transform.position;
	}
	
	protected virtual void ImplementDraggableObject() {
        Vector3 screenPoint;
		
        if (Input.touchCount >= 1) {
            screenPoint = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
        }
        else {
            screenPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        this.transform.position = new Vector3(screenPoint.x, screenPoint.y, -6f);
	}
	
	// Update is called once per frame
	protected override void Update ()
	{
		base.Update ();

		if(_isDraggable) {
			this.ImplementDraggableObject();
		}
		
		if(sceneManager.touch.phase == TouchPhase.Ended || sceneManager.touch.phase == TouchPhase.Canceled) {			
			if(this._isDraggable)
				_isDropObject = true;
		}
	}
	
    protected override void OnTouchDrag()
    {
        base.OnTouchDrag();
        
        if(this._canDragaable && base._OnTouchBegin) {
			this._isDraggable = true;
        }
    }
}
