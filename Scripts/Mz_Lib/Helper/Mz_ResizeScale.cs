using UnityEngine;
using System.Collections;

public class Mz_ResizeScale : MonoBehaviour {

	public static void ResizingScale(Transform r_transform)
    {
        float currentRatio = (float)Screen.width / (float)Screen.height;
        Vector3 newScale = r_transform.localScale;

        if (currentRatio == Main.fixedratio)
        {			
			Debug.Log("automatic scale by engine.");
        }
        else
        {
            Vector3 vec3 = new Vector3((currentRatio / Main.fixedratio), r_transform.localScale.y, r_transform.localScale.z);
            newScale.x = vec3.x;
        }

        r_transform.localScale = newScale;
	}
}
