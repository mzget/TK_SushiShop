using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UE = UnityEngine;

public class Dressing : Mz_BaseScene {

    public Transform background_transform;
	public GameObject cloudAndFogs;
	public GameObject back_button_Obj;
	public CharacterAnimationManager TK_animationManager;
    public CostumeManager costomeManager;

    public Transform gameEffect_transform;
    public static List<int> CanEquipClothe_list = new List<int>();
	public static List<int> CanEquipHat_list = new List<int>();
	

	protected override void Initialization ()
	{
		if (Dressing.CanEquipClothe_list.Count == 0) {
			extendsStorageManager.LoadCostumeData();
		}
		
		this.gameObject.AddComponent<GameEffectManager>();
		gameEffectManager = this.gameObject.GetComponent<GameEffectManager>();
	}

    // Use this for initialization
	void Start () {
		StartCoroutine_Auto (this.ReInitializeAudioClipData ());
        StartCoroutine(InitializeAudio());
		base.InitializeIdentityGUI();
		
        Mz_ResizeScale.ResizingScale(background_transform);
        iTween.MoveTo(cloudAndFogs, iTween.Hash("x", -150f, "islocal", true, "time", 10f, "easetype", iTween.EaseType.easeInSine, "looptype", iTween.LoopType.pingPong)); 
		
		audioDescribe.PlayOnecSound(description_clips[0]);
	}

    protected new IEnumerator InitializeAudio()
    {
        base.InitializeAudio();
        
        audioBackground_Obj.audio.clip = base.background_clip;
        audioBackground_Obj.audio.loop = true;
        audioBackground_Obj.audio.Play();

		base.audioManager = ScriptableObject.CreateInstance<Base_AudioManager> ();
		if (Main.Mz_AppLanguage.appLanguage == Main.Mz_AppLanguage.SupportLanguage.EN) {
			for (int i = 0; i < arr_EN_AppreciateClipName.Length; i++)
			{
				base.audioManager.appreciate_Clips.Add(Resources.Load(PATH_OF_APPRECIATE_CLIP + arr_EN_AppreciateClipName[i], typeof(AudioClip)) as AudioClip);
			}
		}
		else if(Main.Mz_AppLanguage.appLanguage == Main.Mz_AppLanguage.SupportLanguage.TH) {
			for (int i = 0; i < arr_TH_AppreciateClipName.Length; i++)
			{
				base.audioManager.appreciate_Clips.Add(Resources.Load(PATH_OF_APPRECIATE_CLIP + arr_TH_AppreciateClipName[i], typeof(AudioClip)) as AudioClip);
			}
		}
		 
		yield return null; 
    }
	
	private const string PATH_OF_DYNAMIC_CLIP = "AudioClips/SceneInfo/";	
	private readonly string[] arr_TH_AppreciateClipName = new string[] {
		"TH_appreciate_01", 
		"TH_appreciate_02",
		"TH_appreciate_03",
		"TH_appreciate_04",
	};
	private readonly string[] arr_EN_AppreciateClipName = new string[] {
		"EN_appreciate_001", 
		"EN_appreciate_002",
		"EN_appreciate_003",
		"EN_appreciate_004",
	};
	private IEnumerator ReInitializeAudioClipData()
	{
		description_clips.Clear();
		if (Main.Mz_AppLanguage.appLanguage == Main.Mz_AppLanguage.SupportLanguage.TH)
		{
			description_clips.Add(Resources.Load(PATH_OF_DYNAMIC_CLIP + "TH_suit", typeof(AudioClip)) as AudioClip);
		}
		else if (Main.Mz_AppLanguage.appLanguage == Main.Mz_AppLanguage.SupportLanguage.EN)
		{
			description_clips.Add(Resources.Load(PATH_OF_DYNAMIC_CLIP + "EN_suit", typeof(AudioClip)) as AudioClip);
		}
		
		yield return 0;
	}

	public override void OnInput (string nameInput)
	{
		base.OnInput (nameInput);

        switch (nameInput)
        {
            case "shirt_button":
                TK_animationManager.PlayTalkingAnimation();
                costomeManager.ShowTab(CostumeManager.TabMenuState.clothes);
                break;
            case "hat_button":
                TK_animationManager.PlayTalkingAnimation();
                costomeManager.ShowTab(CostumeManager.TabMenuState.hat);
                break;
            case "Previous_button":
                costomeManager.BackToPreviousPage();
                break;
            case "Next_button":
                costomeManager.GotoNextPage();
                break;
            case "Low0_1":
                costomeManager.HaveChooseClotheCommand(nameInput);
                break;
            case "Low0_2": 
                costomeManager.HaveChooseClotheCommand(nameInput);
                break;
            case "Low0_3":
                costomeManager.HaveChooseClotheCommand(nameInput);
                break;
            case "Low1_1": 
                costomeManager.HaveChooseClotheCommand(nameInput);
                break;
            case "Low1_2": 
                costomeManager.HaveChooseClotheCommand(nameInput);
                break;
            case "Low1_3":
                costomeManager.HaveChooseClotheCommand(nameInput);
                break;
            case "Yes_button":
                costomeManager.UserConfirmTransaction();
                break;
            case "No_button":
                costomeManager.UserCancleTransaction();
                break;
            default:
                break;
        }

		if(nameInput == back_button_Obj.name) {
			if(Application.isLoadingLevel == false && _onDestroyScene == false) {
				_onDestroyScene = true;
				
				base.extendsStorageManager.SaveDataToPermanentMemory();
				
				Mz_LoadingScreen.LoadSceneName = Mz_BaseScene.SceneNames.Town.ToString();
				Application.LoadLevelAsync(Mz_BaseScene.SceneNames.LoadingScene.ToString());
			}
		}
	}

    internal void PlayGreatEffect()
    {
        TK_animationManager.PlayGoodAnimation();
        gameEffectManager.Create2DSpriteAnimationEffect(GameEffectManager.IRIDESCENT_EFFECT_PATH, gameEffect_transform);
        audioEffect.PlayOnecWithOutStop(audioEffect.longBring_clip);
    }

    internal void PlaySoundWarning()
    {
        this.audioEffect.PlayOnecWithOutStop(audioEffect.wrong_Clip);
    }

    internal void RandomPlayAppreciateClip()
    {
        int r = UE.Random.Range(0, audioManager.appreciate_Clips.Count);
        audioDescribe.PlayOnecSound(audioManager.appreciate_Clips[r]);
    }
}
