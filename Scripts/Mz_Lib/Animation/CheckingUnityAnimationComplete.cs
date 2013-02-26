using UnityEngine;
using System;
using System.Collections;

public class CheckingUnityAnimationComplete {

	public static event EventHandler TargetAnimationComplete_event;
	private static void OnTargetAnimationComplete_event (EventArgs e)
	{
		var handler = TargetAnimationComplete_event;
		if (handler != null)
			handler (null, e);
	}

    public static IEnumerator ICheckAnimationComplete(Animation targetAnimation, string targetAnimatedName, GameObject callbackObj, string callbackFunction)
    {
        do
        {
            yield return null;
        } while (targetAnimation.IsPlaying(targetAnimatedName));

        Debug.Log(targetAnimatedName + " finish !");

        // here ! call back to delegation obj.
		if (callbackObj != null)
			callbackObj.SendMessage (callbackFunction, SendMessageOptions.DontRequireReceiver);
		else
			OnTargetAnimationComplete_event (EventArgs.Empty);

    }
}
