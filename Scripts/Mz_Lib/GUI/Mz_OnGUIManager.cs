using UnityEngine;
using System.Collections;

public class Mz_OnGUIManager : MonoBehaviour {

	public static Rect viewPort_rect;
	public static Rect midcenterGroup_rect = new Rect(0, 0, Main.GAMEWIDTH, Main.GAMEHEIGHT);	
	//<!--- Equation finding scale == x = screen.height/ main.fixed.
	public static float Extend_heightScale = 1;
	public static void CalculateViewportScreen() {    
		// Calculation height of screen.
		if(Screen.height == Main.FixedGameHeight) {
			Extend_heightScale = 1;
		}
		else {
			Extend_heightScale =  Screen.height / Main.FixedGameHeight;
			
			midcenterGroup_rect.height = Main.FixedGameHeight * Extend_heightScale;
			midcenterGroup_rect.width = Main.FixedGameWidth * Extend_heightScale;
		}
		
		viewPort_rect = new Rect(((Screen.width / 2) - (midcenterGroup_rect.width / 2)), 0, midcenterGroup_rect.width, Main.FixedGameHeight);
	}
}
