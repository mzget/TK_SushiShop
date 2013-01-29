using UnityEngine;
using System.Collections;
 
// Use this on a guiText or guiTexture object to automatically have them
// adjust their aspect ratio when the game starts.
 
public class GUITexture_RatioFixer
{
	private const float gameWidth = 1024f;
	private const float gameHeight = 768f;
	float defaultPosX;
	float defaultPosY;
	float scaleX;
	float scaleY;

    private Vector2 position;
    private Vector2 scale;

    public static Rect ResizingTextureScale(Rect pixel_inset)
    {
        Vector2 newPos = new Vector2(pixel_inset.x, pixel_inset.y);
        Vector2 newScale = new Vector2(pixel_inset.width, pixel_inset.height);

//        if (currentRatio == Main.fixedratio)
//        {
//            Debug.Log("automatic scale by engine.");
//        }
//        else
//        {
//            Vector2 scale = new Vector3(pixel_inset.width, pixel_inset.height) * (currentRatio / Main.fixedratio);
//            newScale.x = scale.x;
//        }
		
		if(Screen.width == 1024) {
			
		}
		else {			
			newPos.x = (Screen.width * pixel_inset.x) / gameWidth;
			newScale.x = (Screen.width * pixel_inset.width) / gameWidth;
		}
		
        if (Screen.height == 768)
        {
				
        }
        else
        {
			newPos.y = (Screen.height * pixel_inset.y) / gameHeight;
			newScale.y = (Screen.height * pixel_inset.height) / gameHeight;
        }

        pixel_inset = new Rect(newPos.x, newPos.y, newScale.x, newScale.y);
		
		return pixel_inset;
    }
}
