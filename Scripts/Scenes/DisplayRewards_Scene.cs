using UnityEngine;
using System.Collections;

public class DisplayRewards_Scene : Mz_BaseScene {

    internal const string MEDAL_Bronze = "Bronze";
    internal const string MEDAL_Copper = "Copper";
    internal const string MEDAL_Silver = "Silver";
    internal const string MEDAL_Gold = "Gold";
    internal const string MEDAL_Crystal = "Crystal";

	const string BACK_BUTTON_NAME = "Back_button";
	const string NEXT_BUTTON_NAME = "Next_button";
	const string PREVIOUS_BUTTON_NAME = "Previous_button";

    public Transform sceneBackground_transform;
    public GameObject[] cloudAndFog_Objs;


	public RewardManager rewardManager;
	public CharacterAnimationManager tk_animationManager;


	// Use this for initialization
	void Start () {
		StartCoroutine(this.InitializeAudio());
		StartCoroutine(base.InitializeIdentityGUI());

        Mz_ResizeScale.ResizingScale(sceneBackground_transform);
        iTween.MoveTo(cloudAndFog_Objs[0].gameObject, iTween.Hash("y", 0f, "islocal", true, "time", 3f, "easetype", iTween.EaseType.easeInSine, "looptype", iTween.LoopType.pingPong));
        iTween.MoveTo(cloudAndFog_Objs[1].gameObject, iTween.Hash("y", 20f, "islocal", true, "time", 4f, "easetype", iTween.EaseType.easeInSine, "looptype", iTween.LoopType.pingPong));
        iTween.MoveTo(cloudAndFog_Objs[2].gameObject, iTween.Hash("y", 40f, "islocal", true, "time", 5f, "easetype", iTween.EaseType.easeInSine, "looptype", iTween.LoopType.pingPong)); 
	}
	
	private new IEnumerator InitializeAudio() {
		base.InitializeAudio();		
		
        audioBackground_Obj.audio.clip = base.background_clip;
        audioBackground_Obj.audio.loop = true;
        audioBackground_Obj.audio.Play();

        yield return null;
	}
	
	// Update is called once per frame
//	void Update () {
//	
//	}
	
	public override void OnInput (string nameInput)
	{
//		base.OnInput (nameInput);
		
		switch (nameInput) {
		case BACK_BUTTON_NAME : if(!Application.isLoadingLevel) {
				Mz_LoadingScreen.LoadSceneName = Mz_BaseScene.SceneNames.Town.ToString();
				Application.LoadLevel(Mz_BaseScene.SceneNames.LoadingScene.ToString());
			}
			break;
		case NEXT_BUTTON_NAME:
			rewardManager.HaveNextPageCommand();
			tk_animationManager.PlayGoodAnimation();
			break;
		case PREVIOUS_BUTTON_NAME : 
			rewardManager.HavePreviousCommand();
			tk_animationManager.PlayGoodAnimation();
			break;
		default:
		break;
		}
	}
}
