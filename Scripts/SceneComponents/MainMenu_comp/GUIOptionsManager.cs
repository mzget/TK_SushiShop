using UnityEngine;
using System.Collections;

[System.Serializable]
public class GUIOptionsManager {

	internal const int AUDIO_ON_ID = 1;
	internal const int AUDIO_OFF_ID = 0;

	public tk2dSprite audio_ui_sprite;
	public tk2dSprite flag_ui_sprite;
	public GameObject selectLanguage_Obj;

	public AudioClip english_clip;
	public AudioClip thai_clip;

	// Use this for initialization
	public GUIOptionsManager() {}
	
	// Update is called once per frame
	void OnDestroy () {
	
	}
}
