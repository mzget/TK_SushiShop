using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

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
        StartCoroutine(InitializeAudio());
		StartCoroutine(base.InitializeIdentityGUI());
		
        Mz_ResizeScale.ResizingScale(background_transform);
        iTween.MoveTo(cloudAndFogs, iTween.Hash("x", -150f, "islocal", true, "time", 10f, "easetype", iTween.EaseType.easeInSine, "looptype", iTween.LoopType.pingPong)); 
	}

    protected new IEnumerator InitializeAudio()
    {
        base.InitializeAudio();
        
        audioBackground_Obj.audio.clip = base.background_clip;
        audioBackground_Obj.audio.loop = true;
        audioBackground_Obj.audio.Play();
		 
		yield return null; 
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

	internal void ReFreshAvailableMoney ()
	{
		base.availableMoney.text = Mz_StorageManage.AvailableMoney.ToString();
		base.availableMoney.Commit();
	}
}
