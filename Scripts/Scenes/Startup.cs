using UnityEngine;
using System.Collections;

public class Startup : Mz_BaseScene {

	// Use this for initialization
	void Start () {
		this.Initialization();
		//<!-- get name quality.
		//		qualities_list = QualitySettings.names;
		this.AutomaticSetup_QualitySetting();
		Handheld.PlayFullScreenMovie("Movies/LogoVista3D.mp4", Color.white, FullScreenMovieControlMode.Hidden, FullScreenMovieScalingMode.AspectFit);
		
		if(Application.isLoadingLevel == false) {
			Application.LoadLevelAsync(Mz_BaseScene.SceneNames.WaitForStart.ToString());
		}
	}
	
	protected override void Initialization ()
	{
		base.Initialization ();
		
		Mz_OnGUIManager.CalculateViewportScreen();
		Mz_StorageManage.Language_id = PlayerPrefs.GetInt(Mz_StorageManage.KEY_SYSTEM_LANGUAGE, 0);
		Main.Mz_AppLanguage.appLanguage = (Main.Mz_AppLanguage.SupportLanguage)Mz_StorageManage.Language_id;
	}
	
	private void AutomaticSetup_QualitySetting() {
		#if UNITY_IPHONE
		if(iPhone.generation == iPhoneGeneration.iPad1Gen ||
		   iPhone.generation == iPhoneGeneration.iPhone3G || iPhone.generation == iPhoneGeneration.iPhone3GS) {
			QualitySettings.SetQualityLevel(0);	
			Application.targetFrameRate = 30;
		}
		else {
			QualitySettings.SetQualityLevel(1);
			Application.targetFrameRate = 60;
		}
		#elif UNITY_ANDROID
		if(Screen.height < Main.HD_HEIGHT) {			
			QualitySettings.SetQualityLevel(0);	
			Application.targetFrameRate = 30;
		}
		else {
			QualitySettings.SetQualityLevel(1);
			Application.targetFrameRate = 60;
		}
		#else 
		QualitySettings.SetQualityLevel(3);
		Application.targetFrameRate = 60;
		#endif
	}
	
	// Update is called once per frame
	protected override void Update ()
	{
		base.Update ();
	}
}
