using UnityEngine;
using System.Collections;

public class BinBeh : ObjectsBeh {

    const string OpenAnim = "open";
    const string CloseAnim = "close";
	string animationName_001;
	string animationName_002;

	// Use this for initialization
    protected override void Start()
    {
        base.Start();
		
		animationName_001 = OpenAnim;
		animationName_002 = CloseAnim;
    }
	
	public void PlayOpenAnimation() {
        animatedSprite.Play(animationName_001);	
		animatedSprite.animationCompleteDelegate = animationCompleteDelegate;
	}
	
	public void animationCompleteDelegate(tk2dAnimatedSprite sprite, int clipId) {
		if(animationName_002 != "") {
			animatedSprite.Play(animationName_002);
			animatedSprite.animationCompleteDelegate -= animationCompleteDelegate;
		}
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
