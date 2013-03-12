using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UE = UnityEngine;


[System.Serializable]
public class TownTutorDataStore {

    public GameObject roof_00_button_obj;

	public Transform topRightAnchor_transform;
	public GameObject decoration_audioTip;
	public GameObject dress_audioTip;
	public GameObject trophy_audioTip;

    public TownTutorDataStore() { }

    public void OnDestroy() { }
};

public class Town : Mz_BaseScene {

    //<@--- Constance button name.
    const string YES_BUTTON_NAME = "Yes_button";
    const string NO_BUTTON_NAME = "No_button";
	const string MoveCameraToTutorPointComplete_FUNC = "MoveCameraToTutorPointComplete";

	public GameObject town_bg_group;
	public GameObject backgroup_group_transform;
	public GameObject movingCloud;
    public GameObject flyingBird_group;
	public GameObject shop_body_sprite;
    public GameObject SheepbankDoor;
	const string SHOP_DOOR_NAME = "Shop_door";
    public tk2dAnimatedSprite shopDoor_anim;
    public tk2dAnimatedSprite sheepBank_door_animated;
    public Transform midcenter_anchor;
	public GameObject upgradeOutside_baseAnchor;
	public UpgradeOutsideManager upgradeOutsideManager;
    public CharacterAnimationManager characterAnimatedManage;
	public DogBeh bullDog;
    public TownTutorDataStore townTutorData;
    internal GameObject pet;

    private bool _updatable = true;
	public enum OnGUIState { none = 0, DrawEditShopname, };
	public OnGUIState currentGUIState;
	string shopname = "";
	Rect editShop_Textfield_rect = new Rect( 50, 60, 200, 50);
	Rect editShop_OKButton_rect = new Rect(10, 150, 100, 40);
	Rect editShop_CancelButton_rect = new Rect(160, 150, 100, 40);

	#region <@-- Event Handles Data section

	public static event EventHandler newGameStartup_Event;
	private void OnnewGameStartup_Event (EventArgs e)
	{
		EventHandler handler = Town.newGameStartup_Event;
		if (handler != null)
			handler (this, e);
	}

	public static GameObject StartingTrucks;
	public static void Handle_NewGameStartupEvent (object sender, EventArgs e)
	{
		Town.newGameStartup_Event -= Town.Handle_NewGameStartupEvent;

		if(StartingTrucks == null) {
			StartingTrucks = Instantiate(Resources.Load("StartingTrucks", typeof(GameObject)), new Vector3(-200f, -62f, -6f), Quaternion.identity)  as GameObject;
		}
		
		iTween.MoveTo(StartingTrucks, iTween.Hash("x", 500f, "Time", 15f, "easetype", iTween.EaseType.linear,
		                                       "oncompletetarget", GameObject.FindGameObjectWithTag("GameController"), "oncomplete", "OnStartingCarComplete"));
        DogBeh.ChaseBite();
	}
	
	void OnStartingCarComplete ()
	{
		Destroy(Town.StartingTrucks);
		StartingTrucks = null;
	}

	#endregion

    protected override void Initialization()
    {
        base.Initialization();

        StartCoroutine(this.CreatePetAtRuntime());
    }

    internal IEnumerator CreatePetAtRuntime()
    {
        if (pet != null)
        {
            Destroy(pet);
            yield return new WaitForFixedUpdate();
        }

        switch (Mz_StorageManage.Pet_id)
        {
            case 0:
                pet = Instantiate(Resources.Load("Pets/Bulldog", typeof(GameObject))) as GameObject;
                pet.transform.parent = midcenter_anchor;
				pet.transform.position = PetBeh.InitializePosition;
                break;
            case 1:
                pet = Instantiate(Resources.Load("Pets/Dog", typeof(GameObject))) as GameObject;
                pet.transform.parent = midcenter_anchor;
				pet.transform.position = PetBeh.InitializePosition;
                break;
            case 2:
                pet = Instantiate(Resources.Load("Pets/Ewe", typeof(GameObject))) as GameObject;
                pet.transform.parent = midcenter_anchor;
				pet.transform.position = PetBeh.InitializePosition;
                break;
            case 3:
                pet = Instantiate(Resources.Load("Pets/Ram", typeof(GameObject))) as GameObject;
                pet.transform.parent = midcenter_anchor;
				pet.transform.position = PetBeh.InitializePosition;
                break;
            case 4:
                pet = Instantiate(Resources.Load("Pets/Nanny", typeof(GameObject))) as GameObject;
                pet.transform.parent = midcenter_anchor;
				pet.transform.position = PetBeh.InitializePosition;
                break;
            case 5:
                pet = Instantiate(Resources.Load("Pets/BillyGoat", typeof(GameObject))) as GameObject;
                pet.transform.parent = midcenter_anchor;
				pet.transform.position = PetBeh.InitializePosition;
                break;
            case 6:
                pet = Instantiate(Resources.Load("Pets/Cow", typeof(GameObject))) as GameObject;
                pet.transform.parent = midcenter_anchor;
				pet.transform.position = PetBeh.InitializePosition;
                break;
            default:
                break;
        }

        yield return 0;
    }

	// Use this for initialization
	void Start ()
    {
        Mz_ResizeScale.ResizingScale(town_bg_group.transform);

		StartCoroutine(ReInitializeAudioClipData());
		StartCoroutine(this.InitAudio());
        StartCoroutine(base.InitializeIdentityGUI());

        this.upgradeOutsideManager.InitializeDecorationObjects();
		if (SheepBank.HaveUpgradeOutSide) {
            StartCoroutine(this.ActiveDecorationBar());
		}
		else
			StartCoroutine(this.UnActiveDecorationBar());
      
		iTween.MoveTo(flyingBird_group, iTween.Hash("x", 550f, "time", 24f, "easetype", iTween.EaseType.linear, "looptype", iTween.LoopType.loop));
		iTween.MoveTo(movingCloud.gameObject, iTween.Hash("x", -85f, "islocal", true, "time", 20f, "easetype", iTween.EaseType.easeInSine, "looptype", iTween.LoopType.pingPong));
		iTween.MoveTo(backgroup_group_transform, iTween.Hash("y", 64f, "time", 16f, "easetype", iTween.EaseType.linear, "looptype", iTween.LoopType.pingPong));

        if(MainMenu._HasNewGameEvent == false) {
         	if(Town.IntroduceGameUI_Event == null) {
				this.Checking_HasNewStartingTruckEvent();

				townTutorData = null;
			}
			else  {
				this.audioDescribe.PlayOnecSound(description_clips[5]);
				this.OnIntroduceGameUI_Event(EventArgs.Empty);
				this.CreateAudioTipObj();
			}
		}
        else if(MainMenu._HasNewGameEvent && SheepBank.HaveUpgradeOutSide == false && Town.IntroduceGameUI_Event == null) {
            plane_darkShadow.active = true;
            SheepbankDoor.transform.position += Vector3.back * 10;
            this.CreateTutorObjectAtRuntime();
        }
        else if (MainMenu._HasNewGameEvent && SheepBank.HaveUpgradeOutSide && Town.IntroduceGameUI_Event == null) {           
            plane_darkShadow.active = true;
            plane_darkShadow.transform.position -= Vector3.forward * 2.5f;
            townTutorData.roof_00_button_obj.transform.position -= Vector3.forward * 2;
            this.CreateBuyDecoratuionTutorEvent();
        }
	}

    #region <@-- Tutor system.

    void CreateTutorObjectAtRuntime ()
	{
        audioDescribe.PlayOnecWithOutStop(description_clips[0]);
		cameraTutor_Obj = GameObject.FindGameObjectWithTag("MainCamera");
		
		handTutor = Instantiate(Resources.Load("Tutor_Objs/HandTutor", typeof(GameObject))) as GameObject;
		handTutor.transform.parent = cameraTutor_Obj.transform;
		handTutor.transform.localPosition = new Vector3(-10f, 0, 6f);
		
		GameObject tutor_text = Instantiate(Resources.Load("Tutor_Objs/Tutor_description", typeof(GameObject))) as GameObject;
        tutor_text.transform.parent = cameraTutor_Obj.transform;
        tutor_text.transform.localPosition = new Vector3(10f, 0f, 6f);
        base.tutorDescriptions = new List<GameObject>();
        base.tutorDescriptions.Add(tutor_text);
		
		iTween.MoveTo(Camera.main.gameObject, iTween.Hash("x", 266f, "time", 1f, "easetype", iTween.EaseType.easeInOutSine,
		                                                  "oncomplete", MoveCameraToTutorPointComplete_FUNC, "oncompletetarget", this.gameObject));
	}

	void MoveCameraToTutorPointComplete() {		
		handTutor.active = true;
        this._updatable = false;
		iTween.MoveTo(handTutor.gameObject, iTween.Hash("y", 0.2f, "Time", .5f, "easetype", iTween.EaseType.easeInSine, "looptype", iTween.LoopType.pingPong));
	}

    private void CreateBuyDecoratuionTutorEvent()
    {
        cameraTutor_Obj = GameObject.FindGameObjectWithTag("MainCamera");

        handTutor = Instantiate(Resources.Load("Tutor_Objs/HandTutor", typeof(GameObject))) as GameObject;
        handTutor.transform.parent = cameraTutor_Obj.transform;
        handTutor.transform.localPosition = new Vector3(-78f, -55f, 4);

        GameObject tutor_text = Instantiate(Resources.Load("Tutor_Objs/Tutor_description", typeof(GameObject))) as GameObject;
        tutor_text.transform.parent = cameraTutor_Obj.transform;
        tutor_text.transform.localPosition = new Vector3(-65f, -45f, 4);
        tutor_text.GetComponent<tk2dTextMesh>().text = "BUY DECORATION";
        tutor_text.GetComponent<tk2dTextMesh>().Commit();
        base.tutorDescriptions = new List<GameObject>();
        base.tutorDescriptions.Add(tutor_text);

        this._updatable = false;
        iTween.MoveTo(handTutor.gameObject, iTween.Hash("y", -45f, "Time", .5f, "easetype", iTween.EaseType.easeInSine, "looptype", iTween.LoopType.pingPong));
    }
	
	private IEnumerator WaitForDecorationTweenDownComplete() {
		yield return StartCoroutine(this.UnActiveDecorationBar());
		this.CreateGoToShopTutorEvent();
	}

    private void CreateGoToShopTutorEvent()
    {
        base.SetActivateTotorObject(true);

        shopDoor_anim.gameObject.transform.position += Vector3.back * 13;
        characterAnimatedManage.gameObject.transform.position += Vector3.back * 13;

        handTutor.transform.localPosition = new Vector3(0f, 0f, 5f);
        tutorDescriptions[0].transform.localPosition = new Vector3(20f, 0f, 5f);
        tutorDescriptions[0].GetComponent<tk2dTextMesh>().text = "LET'S PLAY";
        tutorDescriptions[0].GetComponent<tk2dTextMesh>().Commit();

        this._updatable = false;
		audioDescribe.PlayOnecSound(description_clips[1]);
        iTween.MoveTo(handTutor.gameObject, iTween.Hash("y", 10f, "Time", .5f, "easetype", iTween.EaseType.easeInSine, "looptype", iTween.LoopType.pingPong));
    }
		
	public static event EventHandler IntroduceGameUI_Event;
	private void OnIntroduceGameUI_Event (EventArgs e)
	{
		Debug.Log("OnIntroduceGameUI_Event");

		if (IntroduceGameUI_Event != null)
			IntroduceGameUI_Event (this, e);
	}	
	internal static void Handle_IntroduceGameUI_Event (object sender, EventArgs e)
	{
		IntroduceGameUI_Event -= Handle_IntroduceGameUI_Event;
	}	
	
	private void CreateAudioTipObj ()
	{		
		GameObject decoration_tip = Instantiate(Resources.Load("Tutor_Objs/TIP/Decoration_audioTip", typeof(GameObject))) as GameObject;
		decoration_tip.name = "Decoration_audioTip";
		decoration_tip.transform.parent = townTutorData.topRightAnchor_transform;
		decoration_tip.transform.localPosition = new Vector3(-20f, -25f, 0);
		townTutorData.decoration_audioTip = decoration_tip;
		iTween.PunchScale(townTutorData.decoration_audioTip, iTween.Hash("amount", Vector3.one, "time", 0.5f, "looptype", iTween.LoopType.pingPong));
		
		GameObject trophy_tip = Instantiate(Resources.Load("Tutor_Objs/TIP/Trophy_audioTip", typeof(GameObject))) as GameObject;
		trophy_tip.name = "Trophy_audioTip";
		trophy_tip.transform.parent = townTutorData.topRightAnchor_transform;
		trophy_tip.transform.localPosition = new Vector3(-58f, -25f, 0);
		townTutorData.trophy_audioTip = trophy_tip;			
		iTween.PunchScale(townTutorData.trophy_audioTip, iTween.Hash("amount", Vector3.one, "time", 0.5f, "looptype", iTween.LoopType.pingPong));
		
		GameObject dress_tip = Instantiate(Resources.Load("Tutor_Objs/TIP/Dress_audioTip", typeof(GameObject))) as GameObject;
		dress_tip.name = "Dress_audioTip";
		dress_tip.transform.parent = townTutorData.topRightAnchor_transform;
		dress_tip.transform.localPosition = new Vector3(-90f, -25f, 0);
		townTutorData.dress_audioTip = dress_tip;			
		iTween.PunchScale(townTutorData.dress_audioTip, iTween.Hash("amount", Vector3.one, "time", 0.5f, "looptype", iTween.LoopType.pingPong));
	}
	
    #endregion

    void Checking_HasNewStartingTruckEvent ()
	{
		OnnewGameStartup_Event(EventArgs.Empty);
	}

	protected IEnumerator InitAudio ()
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

    public const string PATH_OF_DYNAMIC_CLIP = "AudioClips/GameIntroduce/Town/";
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
		if(Main.Mz_AppLanguage.appLanguage == Main.Mz_AppLanguage.SupportLanguage.TH) {
			description_clips.Add(Resources.Load(PATH_OF_DYNAMIC_CLIP + "TH_tutor_01", typeof(AudioClip)) as AudioClip);
			description_clips.Add(Resources.Load(PATH_OF_DYNAMIC_CLIP + "TH_Letplay", typeof(AudioClip)) as AudioClip);
			description_clips.Add(Resources.Load(PATH_OF_DYNAMIC_CLIP + "TH_decoration", typeof(AudioClip)) as AudioClip);
			description_clips.Add(Resources.Load(PATH_OF_DYNAMIC_CLIP + "TH_trophy", typeof(AudioClip)) as AudioClip);
			description_clips.Add(Resources.Load(PATH_OF_DYNAMIC_CLIP + "TH_dress", typeof(AudioClip)) as AudioClip);
			description_clips.Add(Resources.Load(PATH_OF_DYNAMIC_CLIP + "TH_touchmove", typeof(AudioClip)) as AudioClip);
		}
		else if(Main.Mz_AppLanguage.appLanguage == Main.Mz_AppLanguage.SupportLanguage.EN) {
			description_clips.Add(Resources.Load(PATH_OF_DYNAMIC_CLIP + "EN_tutor_01", typeof(AudioClip)) as AudioClip);
			description_clips.Add(Resources.Load(PATH_OF_DYNAMIC_CLIP + "EN_Letplay", typeof(AudioClip)) as AudioClip);
			description_clips.Add(Resources.Load(PATH_OF_DYNAMIC_CLIP + "EN_decoration", typeof(AudioClip)) as AudioClip);
			description_clips.Add(Resources.Load(PATH_OF_DYNAMIC_CLIP + "EN_trophy", typeof(AudioClip)) as AudioClip);
			description_clips.Add(Resources.Load(PATH_OF_DYNAMIC_CLIP + "EN_dress", typeof(AudioClip)) as AudioClip);
			description_clips.Add(Resources.Load(PATH_OF_DYNAMIC_CLIP + "EN_touchmove", typeof(AudioClip)) as AudioClip);
		}		
		
		yield return 0;
    }

	#region <!-- Decoration upgrade bar.

	IEnumerator ActiveDecorationBar ()
	{
		yield return StartCoroutine(this.SettingGUIMidcenter(true));

		if(ConservationAnimals.Level >= 1 && LoveDogConsortium.Level >= 1 && ExtendsStorageManager.TK_clothe_id == 13 && ExtendsStorageManager.TK_hat_id == 13)
			upgradeOutsideManager.pet_button_obj.active = true;
		else 
			upgradeOutsideManager.pet_button_obj.active = false;
		
		iTween.MoveTo(upgradeOutside_baseAnchor.gameObject, iTween.Hash("position", new Vector3(0, 0, 8), "islocal", true, "time", 1f, "easetype", iTween.EaseType.spring));

        upgradeOutsideManager.ActiveRoof();

        SheepBank.HaveUpgradeOutSide = false;
	}

	IEnumerator UnActiveDecorationBar ()
	{
		yield return new WaitForEndOfFrame();
		iTween.MoveTo(upgradeOutside_baseAnchor.gameObject,
			iTween.Hash("position", new Vector3(0, -200, 8), "islocal", true, "time", 1f, "easetype", iTween.EaseType.spring,
			"oncomplete", "TweenDownComplete", "oncompletetarget", this.gameObject));		 
	}

	void TweenDownComplete() {
		StartCoroutine(this.SettingGUIMidcenter(false));
	}

	IEnumerator SettingGUIMidcenter (bool active)
	{
		yield return new WaitForEndOfFrame();
		upgradeOutside_baseAnchor.SetActiveRecursively(active);
	}

	#endregion

    private void LateUpdate() {
        if (_updatable)
        {
            base.ImplementTouchPostion();
        }
    }

	protected override void MovingCameraTransform ()
	{	
		base.MovingCameraTransform();

		if(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
            float speed = (500) * Time.deltaTime * Time.fixedDeltaTime;
			// Get movement of the finger since last frame   
			Vector2 touchDeltaPosition = touch.deltaPosition;
			// Move object across XY plane       
			//transform.Translate(-touchDeltaPosition.x * speed, -touchDeltaPosition.y * speed, 0);
			Camera.main.transform.Translate(-touchDeltaPosition.x * speed, 0, 0);
		}
		else if(Application.isWebPlayer || Application.isEditor)
		{
			if(_isDragMove) {
				float vector = currentPos.x - originalPos.x;
				if(vector < 0)
					Camera.main.transform.position += Vector3.right * Time.deltaTime * 200;
				else if(vector > 0) 
					Camera.main.transform.position += Vector3.left * Time.deltaTime * 200;
			}
		}

        if (Camera.main.transform.position.x > 266f)
            Camera.main.transform.position = new Vector3(266f, Camera.main.transform.position.y, Camera.main.transform.position.z); 	//Vector3.left * Time.deltaTime;
        else if (Camera.main.transform.position.x < 0)
            Camera.main.transform.position = new Vector3(0, Camera.main.transform.position.y, Camera.main.transform.position.z);	 //Vector3.right * Time.deltaTime;     
	}

    /// <summary>
    /// <!-- Gui region.
    /// </summary>
    private void OnGUI() {
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1, Screen.height / Main.GAMEHEIGHT, 1));    
		
		/// OnGUIState.DrawEditShopname.
		if(currentGUIState == OnGUIState.DrawEditShopname)
			this.DrawEditShopnameWindow();

        if (GUI.Button(new Rect(0, Main.FixedGameHeight / 2 - 25, 150 * Mz_OnGUIManager.Extend_heightScale, 50), "Swindle"))
        {
            Mz_StorageManage.AvailableMoney = 100000;
            StartCoroutine(base.InitializeIdentityGUI());
        }
    }

	void DrawEditShopnameWindow ()
	{
		GUI.BeginGroup(new Rect (Screen.width / 2 - 150, Main.GAMEHEIGHT / 2 - 100, 300, 200), "Edit shopname !", GUI.skin.window);
		{
			shopname = GUI.TextField(editShop_Textfield_rect, shopname, 24);

			if(GUI.Button(editShop_OKButton_rect, "OK")) {
				if(shopname != "" && shopname.Length >= 3) {

					if(shopname == "Fulfill your greed") {
						Mz_StorageManage.AvailableMoney += 1000000;
                        characterAnimatedManage.PlayEyeAnimation(CharacterAnimationManager.NameAnimationsList.agape);

						shopname = string.Empty;
					}
					else if(shopname == "Greed is bad") {
                        SushiShop.NumberOfCansellItem.Clear();
                        for (int i = 0; i < 30; i++)
                        {
                            SushiShop.NumberOfCansellItem.Add(i);
                        }
                        base.extendsStorageManager.SaveCanSellGoodListData();
                        characterAnimatedManage.PlayEyeAnimation(CharacterAnimationManager.NameAnimationsList.agape);

						shopname = string.Empty;
					}
					else {
						Mz_StorageManage.ShopName = shopname;
					}

                    base.extendsStorageManager.SaveDataToPermanentMemory();

					currentGUIState = OnGUIState.none;
					base.UpdateTimeScale(1);
				}
			}
			else if(GUI.Button(editShop_CancelButton_rect, "Cancel")) {
				currentGUIState = OnGUIState.none;
				base.UpdateTimeScale(1);
			}
		}
		GUI.EndGroup();
	}

	public override void OnInput (string nameInput)
	{
		if (upgradeOutside_baseAnchor.active) {
			switch (nameInput) {
			case "Close_button": StartCoroutine(this.UnActiveDecorationBar());
				break;
			case UpgradeOutsideManager.Roof_button : upgradeOutsideManager.ActiveRoof();
				break;
			case UpgradeOutsideManager.Awning_button : upgradeOutsideManager.ActiveAwning();
				break;
			case UpgradeOutsideManager.Table_button: upgradeOutsideManager.ActiveTable();
				break;
			case UpgradeOutsideManager.Accessories_button: upgradeOutsideManager.ActiveAccessories();
				break;
            case UpgradeOutsideManager.Pet_button: upgradeOutsideManager.ActivePet();
                break;
			case "None_button" : upgradeOutsideManager.HaveNoneCommand();
				break;
			case "Previous_button" : upgradeOutsideManager.HavePreviousPageCommand();
				break;
			case "Next_button" : upgradeOutsideManager.HaveNextPageCommand();
				break;
            case "Block_00": 
                if (MainMenu._HasNewGameEvent)
                    base.SetActivateTotorObject(false);

                upgradeOutsideManager.BuyDecoration("Block_00"); 
                break;
            case "Block_01": upgradeOutsideManager.BuyDecoration("Block_01");
                break;
            case "Block_02": upgradeOutsideManager.BuyDecoration("Block_02");
                break;
            case "Block_03": upgradeOutsideManager.BuyDecoration("Block_03");
                break;
            case "Block_04": upgradeOutsideManager.BuyDecoration("Block_04");
                break;
            case "Block_05": upgradeOutsideManager.BuyDecoration("Block_05");
                break;
            case "Block_06": upgradeOutsideManager.BuyDecoration("Block_06");
                break;
            case YES_BUTTON_NAME:
                if (MainMenu._HasNewGameEvent)
                {
                    StartCoroutine(this.WaitForDecorationTweenDownComplete());
                }
                upgradeOutsideManager.UserConfirmTransaction();
                break;
            case NO_BUTTON_NAME:
                if (MainMenu._HasNewGameEvent)
                    this.audioDescribe.PlayOnecWithOutStop(this.audioEffect.wrong_Clip);
                else
                    upgradeOutsideManager.UserCancleTransaction();
                break;
			default:
			break;
			}
		} 
		else {
			switch (nameInput) {
			case SHOP_DOOR_NAME : this.PlayShopDoorOpenAnimation ();
				break;
			case "SheepbankDoor" : this.PlaySheepBankOpenAnimation ();
				break;
			case "Back_button" :  
				if(Application.isLoadingLevel == false) {
	                base.extendsStorageManager.SaveDataToPermanentMemory();
	                this.OnDispose();
					
	                Mz_LoadingScreen.LoadSceneName = Mz_BaseScene.SceneNames.MainMenu.ToString();
	                Application.LoadLevel(Mz_BaseScene.SceneNames.LoadingScene.ToString());
	            }
				break;
			case "Dress_button" : 
				if (Application.isLoadingLevel == false) {
	                Mz_LoadingScreen.LoadSceneName = Mz_BaseScene.SceneNames.Dressing.ToString();
	                Application.LoadLevel(Mz_BaseScene.SceneNames.LoadingScene.ToString());
				}
				break;
	        case "Decoration_button":
	            this.characterAnimatedManage.RandomPlayGoodAnimation();
	            StartCoroutine(ActiveDecorationBar());
				break;
			case "Trophy_button" : 
				if (!Application.isLoadingLevel) {
	                Mz_LoadingScreen.LoadSceneName = Mz_BaseScene.SceneNames.DisplayReward.ToString();
	                Application.LoadLevel(Mz_BaseScene.SceneNames.LoadingScene.ToString());
				}
				break;
			case "Decoration_audioTip" : 
				audioDescribe.PlayOnecSound(description_clips[2]);
				Destroy(townTutorData.decoration_audioTip);
				break;
			case "Trophy_audioTip" :
				audioDescribe.PlayOnecSound(description_clips[3]);
				Destroy(townTutorData.trophy_audioTip);
				break;
			case "Dress_audioTip" :
				audioDescribe.PlayOnecSound(description_clips[4]);
				Destroy(townTutorData.dress_audioTip);
				break;
			default:
				break;
			}
		}
	}

    void PlayShopDoorOpenAnimation()
    {
        shopDoor_anim.Play("Open");
        shopDoor_anim.animationCompleteDelegate = delegate(tk2dAnimatedSprite sprite, int clipId)
        {
            if (Application.isLoadingLevel == false) {
                Mz_LoadingScreen.LoadSceneName = Mz_BaseScene.SceneNames.Shop.ToString();
                Application.LoadLevel(Mz_BaseScene.SceneNames.LoadingScene.ToString());
            }
        };
    }

    void PlaySheepBankOpenAnimation() {
        sheepBank_door_animated.Play();
        sheepBank_door_animated.animationCompleteDelegate = delegate(tk2dAnimatedSprite sprite, int clipId)
        {
            if(Application.isLoadingLevel == false) {
                Mz_LoadingScreen.LoadSceneName = Mz_BaseScene.SceneNames.Sheepbank.ToString();
                Application.LoadLevel(Mz_BaseScene.SceneNames.LoadingScene.ToString());
            }
        };
    }

	internal void PlaySoundRejoice ()
	{
		audioEffect.PlayOnecWithOutStop(audioEffect.correct_Clip);
		characterAnimatedManage.RandomPlayGoodAnimation();
	}

    internal void PlayAppreciateAudioClip(bool p_random)
    {
        if (p_random) {
            int r = UE.Random.Range(0, audioManager.appreciate_Clips.Count);

            audioDescribe.PlayOnecSound(audioManager.appreciate_Clips[r]);
        }
    }

    public override void OnDispose()
    {
        base.OnDispose();
        //<!-- Clear static NumberOfCanSellItem.
        SushiShop.NumberOfCansellItem.Clear();
		UpgradeOutsideManager.CanDecorateRoof_list.Clear();
		UpgradeOutsideManager.CanDecorateAwning_list.Clear();
		UpgradeOutsideManager.CanDecoration_Table_list.Clear();
		UpgradeOutsideManager.CanDecoration_Accessories_list.Clear();
    }
}
