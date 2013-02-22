using UnityEngine;
using System;
using System.Collections;

public class GoodsBeh : ObjectsBeh {    
    public const string ClassName = "GoodsBeh";

    protected SushiShop sceneManager;

    public Vector3 offsetPos;	
	private string animationName_001 = string.Empty;
	private string animationName_002 = string.Empty;
	internal int costs;

    #region <!-- Events data.

    //<!-- WaitForIngredientEvent.
	protected bool _isWaitFotIngredient = false;	
	protected event EventHandler waitForIngredientEvent;
    protected void CheckingDelegationOfWaitFotIngredientEvent(object sender, EventArgs e) {
        if (waitForIngredientEvent != null)
            waitForIngredientEvent(sender, System.EventArgs.Empty);
    }
	protected virtual void Handle_waitForIngredientEvent (object sender, System.EventArgs e)
	{
		_isWaitFotIngredient = true;
	}
	public virtual void WaitForIngredient(string ingredientName) {			
		Debug.Log("WaitForIngredient :: " + ingredientName);
	}
	
	//<!-- Put goods objects intance on food tray.
    public class PutGoodsToTrayEventArgs : EventArgs
    {
//        public GoodsBeh food;
        public GameObject foodInstance;
    };
	private event EventHandler<PutGoodsToTrayEventArgs> putObjectOnTray_Event;
    internal EventHandler<PutGoodsToTrayEventArgs> GoodsBeh_putObjectOnTray_Event;
	protected void OnPutOnTray_event (PutGoodsToTrayEventArgs e) {
		if (putObjectOnTray_Event != null) 
        {
			putObjectOnTray_Event (this, e);
            sceneManager.audioEffect.PlayOnecWithOutStop(sceneManager.soundEffect_clips[5]);
			
            Debug.Log(putObjectOnTray_Event + ":: OnPutOnTray_event : " + this.name);

            if(MainMenu._HasNewGameEvent)
                sceneManager.CheckingGoodsObjInTray("newgame_event");
		}
	}

    #endregion

    protected override void ImplementDraggableObject ()
	{
		base.ImplementDraggableObject ();
		
		Ray cursorRay;
		RaycastHit hit;		
		cursorRay = new Ray(this.transform.position, Vector3.forward);
		
		if(Physics.Raycast(cursorRay, out hit)) 
        {
			if(hit.collider.name == sceneManager.binBeh.name) {			
				if(this._isDropObject == true) {
                    sceneManager.binBeh.PlayOpenAnimation();
                    this.OnDispose();
                    OnDestroyObject_event(System.EventArgs.Empty);
				}
			}
			else if(hit.collider.name == sceneManager.foodsTray_obj.name) {
                if(this._isDropObject) {
					this._isDropObject = false;
	                base._isDraggable = false;
					base._canActive = false;
					this._isWaitFotIngredient = false;
					this.waitForIngredientEvent -= this.Handle_waitForIngredientEvent;

                    OnPutOnTray_event(new PutGoodsToTrayEventArgs() { foodInstance = this.gameObject });
                }
            }
        	else {
	            if(this._isDropObject) {
	                this.transform.position = originalPosition;
	                this._isDropObject = false;
	                base._isDraggable = false;
	            }
        	}
		}
		else {
            if(this._isDropObject) {
                this.transform.position = originalPosition;
                this._isDropObject = false;
                base._isDraggable = false;
            }
		}
		
		Debug.DrawRay(cursorRay.origin, Vector3.forward, Color.red);
	}

    private void StopActiveAnimation() {
		//<!--- On object active.
        if(animation) {
			this.animation.Stop();
		}
		else if(animatedSprite) {
            animatedSprite.Stop();
		}
    }

	public void animationCompleteDelegate(tk2dAnimatedSprite sprite, int clipId) {
		if(animationName_002 != "") {
			animatedSprite.Play(animationName_002);
			animatedSprite.animationCompleteDelegate -= animationCompleteDelegate;
		}
	}

    protected override void Awake()
    {
        base.Awake();

        sceneManager = baseScene.GetComponent<SushiShop>();
    }

    protected override void Start()
    {
        base.Start();

        putObjectOnTray_Event += new EventHandler<PutGoodsToTrayEventArgs>(GoodsBeh_putObjectOnTray_Event);
    }

    protected override void OnTouchBegan()
    {
        base.OnTouchBegan();

        this.transform.localScale += new Vector3(1.2f, 1.2f, 1);
        baseScene.audioEffect.PlayOnecWithOutStop(baseScene.audioEffect.pop_clip);
    }

	protected override void OnTouchDown ()
	{
		if(_canActive && _isWaitFotIngredient) {
			//<!--- On object active.
			if(animatedSprite && animationName_001 != string.Empty) {
				animatedSprite.Play(animationName_001);				
				animatedSprite.animationCompleteDelegate = animationCompleteDelegate;
			}
			else { 
				iTween.PunchPosition(this.gameObject, iTween.Hash("y", 0.2f, "time", 1f, "looptype", iTween.LoopType.loop));
			}
		}

		base.OnTouchDown();
	}

    protected override void OnTouchEnded()
    {
        base.OnTouchEnded();

        this.transform.localScale -= new Vector3(1.2f, 1.2f, 1);
    }

    public override void OnDispose()
    {
        base.OnDispose();

        Destroy(this.gameObject);
        this.waitForIngredientEvent -= this.Handle_waitForIngredientEvent;
    }
}