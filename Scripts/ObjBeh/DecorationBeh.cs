using UnityEngine;
using System.Collections;

public class DecorationBeh : ObjectsBeh {

	protected override void OnTouchDown ()
	{
		base.animatedSprite.Play();
		base.animatedSprite.animationCompleteDelegate = AnimationComplete;

		base.OnTouchDown ();
	}
	
	public void AnimationComplete(tk2dAnimatedSprite sprite, int clipId) {
		animatedSprite.StopAndResetFrame();
		
		base.animatedSprite.animationCompleteDelegate -= AnimationComplete;
	}
}
