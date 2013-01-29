using UnityEngine;
using System.Collections;

public class BinBeh : ObjectsBeh {

    const string OpenAnim = "open";
    const string CloseAnim = "close";

	// Use this for initialization
    protected override void Start()
    {
        base.Start();
		
		base.animationName_001 = OpenAnim;
		base.animationName_002 = CloseAnim;
    }
	
	public void PlayOpenAnimation() {
        animatedSprite.Play(animationName_001);	
		animatedSprite.animationCompleteDelegate = animationCompleteDelegate;
	}

	#region <!-- On Mouse Events.

	protected override void OnTouchDown ()
	{
		// <!--- On object active.
		if(animation) {
			this.animation.Play();
		}
		else if(animatedSprite && animationName_001 != string.Empty) {
			PlayOpenAnimation();
		}

        baseScene.audioEffect.PlayOnecWithOutStop(baseScene.soundEffect_clips[1]);

		base.OnTouchDown ();
	}

	#endregion
}
