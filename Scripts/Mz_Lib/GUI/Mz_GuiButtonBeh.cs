using UnityEngine;
using System.Collections;

[AddComponentMenu("Mz_ScriptLib/GUI/Mz_GuiButtonBeh")]
public class Mz_GuiButtonBeh : Base_ObjectBeh {
	
	public bool enablePlayAudio = true;
	
	private Mz_BaseScene gameController;
    private Vector3 originalScale;
		
	
	// Use this for initialization
    void Start ()
	{
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<Mz_BaseScene> ();
		
        originalScale = this.transform.localScale;
	}

	void OnApplicationPause (bool pause) {
		collider.enabled = !pause;
	}
	
	#region <!-- Handle OnInput Events.
	
	protected override void OnTouchBegan ()
	{
		base.OnTouchBegan ();
		
		if(this.enablePlayAudio)
			gameController.audioEffect.PlayOnecSound(gameController.audioEffect.buttonDown_Clip);

        iTween.ShakeScale(this.gameObject, new Vector3(0.1f, 0.1f, 0), 0.3f);
	}
	protected override void OnTouchDown ()
	{
        gameController.OnInput(this.gameObject.name);
		
		base.OnTouchDown ();
	}
	protected override void OnTouchEnded ()
	{
		base.OnTouchEnded ();		
		
        this.transform.localScale = originalScale;
	}
	
	#endregion
}
