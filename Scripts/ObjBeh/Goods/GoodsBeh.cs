using UnityEngine;
using System;
using System.Collections;

public class GoodsBeh : ObjectsBeh {    
    public const string ClassName = "GoodsBeh";
    
    public Vector3 offsetPos;	
	string animationName_001 = string.Empty;
	string animationName_002 = string.Empty;

	//<!-- WaitForIngredientEvent.
	protected bool _isWaitFotIngredient = false;	
	protected event System.EventHandler waitForIngredientEvent;
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
	public event System.EventHandler putObjectOnTray_Event;
	protected void OnPutOnTray_event (System.EventArgs eventArgs) {
		if (putObjectOnTray_Event != null) {
			putObjectOnTray_Event (this, eventArgs);
            Debug.Log(putObjectOnTray_Event + ":: OnPutOnTray_event : " + this.name);

            if(MainMenu._HasNewGameEvent)
                sceneManager.CheckingGoodsObjInTray("newgame_event");
		}
	}
    
	protected override void ImplementDraggableObject ()
	{
		base.ImplementDraggableObject ();
		
		Ray cursorRay;
		RaycastHit hit;		
		cursorRay = new Ray(this.transform.position, Vector3.forward);
		
		if(Physics.Raycast(cursorRay, out hit)) 
        {
			if(hit.collider.name == sceneManager.bin_behavior_obj.name) {			
				if(this._isDropObject == true) {
					sceneManager.bin_behavior_obj.PlayOpenAnimation();
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

                    OnPutOnTray_event(System.EventArgs.Empty);
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
//            if(_isDraggable == false) {
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

	/// <!-- OnInput Events.
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

    public override void OnDispose()
    {
        base.OnDispose();

        Destroy(this.gameObject);
        this.waitForIngredientEvent -= this.Handle_waitForIngredientEvent;
    }
}