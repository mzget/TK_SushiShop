using UnityEngine;
using System.Collections;


[AddComponentMenu("Mz_ScriptLib/GUI/Mz_GUITexture")]
public class Mz_GUITexture : MonoBehaviour {
	
	private GUITexture myGUITexture;
	
	void Awake()
	{
	    myGUITexture = this.gameObject.GetComponent("GUITexture") as GUITexture;
	}
	
	// Use this for initialization
	void Start()
	{
	    // Position the billboard in the center, 
	    // but respect the picture aspect ratio
	    int textureHeight = myGUITexture.texture.height;
	    int textureWidth = myGUITexture.texture.width;
	    int screenHeight = Screen.height;
	    int screenWidth = Screen.width;
	
	    int screenAspectRatio = (screenWidth / screenHeight);
	    int textureAspectRatio = (textureWidth / textureHeight);
	
		
		
	    int scaledHeight;
	    int scaledWidth;
	    if (textureAspectRatio <= screenAspectRatio)
	    {
	        // The scaled size is based on the height
	        scaledHeight = screenHeight;
	        scaledWidth = (screenHeight * textureAspectRatio);
	    }
	    else
	    {
	        // The scaled size is based on the width
	        scaledWidth = screenWidth;
	        scaledHeight = (scaledWidth / textureAspectRatio);
	    }
		
	    float xPosition = screenWidth / 2 - (scaledWidth / 2);
	    myGUITexture.pixelInset = new Rect(xPosition, scaledHeight - scaledHeight, textureWidth, textureHeight);
		
		
		Debug.Log(myGUITexture.texture.height);
	}
}
