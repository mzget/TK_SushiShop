using UnityEngine;
using System.Collections;

public class Base_ObjectBeh : MonoBehaviour {
    
    protected bool _OnTouchBegin = false;
//	protected bool _OnTouchMove = false;
	protected bool _OnTouchRelease = false;
	
	protected virtual void Update() {       
        if (_OnTouchBegin && _OnTouchRelease) {
            OnTouchDown();
        }
	}

	#region <!-- On Mouse Events.

	protected virtual void OnTouchBegan() {
//		Debug.Log("Class : Base_ObjectBeh." + "OnTouchBegan");

        if(_OnTouchBegin == false)
			_OnTouchBegin = true;
	}
	protected virtual void OnTouchDrag() {
//		Debug.Log("Class : Base_ObjectBeh." + "OnTouchDrag");

//		_OnTouchMove = true;
	}
    protected virtual void OnTouchDown()
    {
//		Debug.Log("Class : Base_ObjectBeh." + "OnTouchDown");

        /// do something.
		
        _OnTouchBegin = false;
        _OnTouchRelease = false;
//		_OnTouchMove = false;
    }
    protected virtual void OnTouchEnded ()
	{
//		Debug.Log ("Class : Base_ObjectBeh." + "OnTouchEnded");

//		if (_OnTouchBegin && _OnTouchMove == false)
		if(_OnTouchBegin)
			_OnTouchRelease = true;
		
//		_OnTouchMove = false;
    }

    #endregion

	public virtual void OnDispose() {}
}
