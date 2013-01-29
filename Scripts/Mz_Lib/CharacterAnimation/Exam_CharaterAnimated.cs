using UnityEngine;
using System.Collections;

public class Exam_CharaterAnimated : MonoBehaviour {

	public CharacterAnimationManager animationManager;

	CharacterAnimationManager.NameAnimationsList nameList_8;
	CharacterAnimationManager.NameAnimationsList nameList_9;
//	CharacterAnimationManager.NameAnimationsList nameList_10;

	// Use this for initialization
	void Start () {
		nameList_8 = (CharacterAnimationManager.NameAnimationsList)8;
		nameList_9 = (CharacterAnimationManager.NameAnimationsList)9;
//		nameList_10 = (CharacterAnimationManager.NameAnimationsList)10;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity,  new Vector3(Screen.width/ Main.FixedGameWidth, Screen.height/Main.FixedGameHeight, 1));

		if(GUI.Button(new Rect(0,0, 150,50), nameList_8.ToString())) {
			animationManager.PlayEyeAnimation(nameList_8);
		}
		else if(GUI.Button(new Rect(0,50, 150,50), nameList_9.ToString())) {
			animationManager.PlayEyeAnimation(nameList_9);
		}
		else if(GUI.Button(new Rect(0,100, 150,50), "Talking")) {
			animationManager.PlayTalkingAnimation();
		}
	}
}
