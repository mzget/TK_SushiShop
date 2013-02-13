using UnityEngine;
using System.Collections;

public class CheckingUnityAnimationComplete {

    public static IEnumerator ICheckAnimationComplete(Animation targetAnimation, string targetAnimatedName, GameObject callbackObj, string callbackFunction)
    {
        do
        {
            yield return null;
        } while (targetAnimation.IsPlaying(targetAnimatedName));

        Debug.Log(targetAnimatedName + " finish !");

        // here ! do some things.
        callbackObj.SendMessage(callbackFunction, SendMessageOptions.DontRequireReceiver);        
    }
}
