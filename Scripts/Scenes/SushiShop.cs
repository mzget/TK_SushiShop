using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ShopTutor {
	public GameObject greeting_textSprite;
	public GameObject greeting_textmesh;
	public GameObject goaway_button_obj;

    public enum TutorStatus {
        None = 0,
        Greeting,
        AcceptOrders,
        CheckAccuracy,
        Billing,
    };
    public TutorStatus currentTutorState;
};

public class SushiShop : Mz_BaseScene {
	
	public Transform shop_background;
    public GameObject bakeryShop_backgroup_group;
    public ExtendAudioDescribeData audioDescriptionData = new ExtendAudioDescribeData();
	public CharacterAnimationManager TK_animationManager;
	public tk2dSprite shopLogo_sprite;
	public GoodDataStore goodDataStore;
	public static List<int> NumberOfCansellItem = new List<int>(GoodDataStore.FoodDatabaseCapacity);
	public List<Food> CanSellGoodLists = new List<Food>();
	public AudioClip[] en_greeting_clip = new AudioClip[6];
    public AudioClip[] th_greeting_clip = new AudioClip[7];
    private AudioClip[] apologize_clip = new AudioClip[5];
    private AudioClip[] thanksCustomer_clips = new AudioClip[2];
	
	//<!-- in game button.
	public GameObject close_button;	
	public GameObject billingMachine;
    private tk2dAnimatedSprite billingAnimatedSprite;
	private AnimationState billingMachine_animState;
	private const string TH_001 = "TH_001";
	private const string TH_002 = "TH_002";
	private const string TH_003 = "TH_003";
	private const string TH_004 = "TH_004";
	private const string TH_005 = "TH_005";
	private const string TH_006 = "TH_006";
	private const string EN_001 = "EN_001";
	private const string EN_002 = "EN_002";
	private const string EN_003 = "EN_003";
	private const string EN_004 = "EN_004";
	private const string EN_005 = "EN_005";
	private const string EN_006 = "EN_006";
	private const string EN_007 = "EN_007";
	
	//<!-- Miscellaneous game objects.	
	public BinBeh bin_behavior_obj;
	public GameObject foodsTray_obj;
	internal FoodTrayBeh foodTrayBeh;
	public SushiProduction sushiProduction;
    public GameObject calculator_group_instance;
    public GameObject receiptGUIForm_groupObj;
    public GameObject giveTheChangeGUIForm_groupObj;
    public tk2dTextMesh totalPrice_textmesh;
    public tk2dTextMesh receiveMoney_textmesh;
    public tk2dTextMesh change_textmesh;
    public tk2dTextMesh displayAnswer_textmesh;
    public GameObject baseOrderUI_Obj;
	public GameObject greetingMessage_ObjGroup;
	public GameObject darkShadowPlane;
	public GameObject rollingDoor_Obj;
	
	public GameObject[] arr_addNotations = new GameObject[2];
	public GameObject[] arr_goodsLabel = new GameObject[3];
	public tk2dSprite[] arr_GoodsTag = new tk2dSprite[3];
	public tk2dTextMesh[] arr_GoodsPrice_textmesh = new tk2dTextMesh[3];
    public tk2dSprite[] arr_orderingBaseItems = new tk2dSprite[3];
	private const string BASE_ORDER_ITEM_NORMAL = "Order_BaseItem";
	private const string BASE_ORDER_ITEM_COMPLETE = "Order_BaseItem_complete";
	public tk2dSprite[] arr_orderingItems = new tk2dSprite[3];
	private Mz_CalculatorBeh calculatorBeh;
    private GameObject cash_obj;
	private tk2dSprite cash_sprite;
    private GameObject packaging_Obj;
	public ShopTutor shopTutor;

    //<!-- Core data
    public enum GamePlayState { 
		none = 0,
		GreetingCustomer = 1,
        Ordering,
		calculationPrice,
		receiveMoney,
		giveTheChange, 
		TradeComplete,
	};
    public GamePlayState currentGamePlayState;

	#region <!-- Customer data group.

    public GameObject customerMenu_group_Obj;
    internal CustomerBeh currentCustomer;

    public event EventHandler nullCustomer_event;
    private void OnNullCustomer_event(EventArgs e) {
        if(nullCustomer_event != null) {
            nullCustomer_event(this, e);
		
			Debug.Log("Callback :: nullCustomer_event");
        }
    }
	
	#endregion	
    
    //<!-- Manage goods complete Event handle.
    public event System.EventHandler manageGoodsComplete_event;
    private void OnManageGoodComplete(System.EventArgs e)
    {
        if (manageGoodsComplete_event != null)
        {
            manageGoodsComplete_event(this, e);
        }
    }
	
	
	// Use this for initialization
	IEnumerator Start () {		
		Mz_ResizeScale.ResizingScale(shop_background);
		this.sushiProduction = this.GetComponent<SushiProduction>();
		
        yield return StartCoroutine(this.InitailizeSceneObject());
		
        this.OpenShop();
	}

    private IEnumerator InitailizeSceneObject()
    {
//		Mz_ResizeScale.ResizingScale(bakeryShop_backgroup_group.transform);

        StartCoroutine(this.ChangeShopLogoIcon());
        StartCoroutine(this.InitailizeShopLabelGUI());
        StartCoroutine(this.InitializeGameEffect());
        StartCoroutine(this.SceneInitializeAudio());
        StartCoroutine(this.InitializeObjectAnimation());

        yield return null;

		foodTrayBeh = new FoodTrayBeh();
        goodDataStore = new GoodDataStore();        
        calculator_group_instance.SetActiveRecursively(false);

        // Debug can sell list.
        StartCoroutine(this.InitializeCanSellGoodslist());
		Debug.Log("CanSellGoodLists.Count : " + CanSellGoodLists.Count + " :: " + "NumberOfCansellItem.Count : " + NumberOfCansellItem.Count);
		
		close_button.active = true;
    }

    private void OpenShop() 
    {
        iTween.MoveTo(rollingDoor_Obj, iTween.Hash("position", new Vector3(0, 220f, 1), "islocal", true, "time", 1f, "easetype", iTween.EaseType.easeInSine));
        audioEffect.PlayOnecSound(base.soundEffect_clips[0]);
				
        nullCustomer_event += new EventHandler(Handle_nullCustomer_event);
        OnNullCustomer_event(EventArgs.Empty);
    }
   
	private IEnumerator SceneInitializeAudio()
	{
        base.InitializeAudio();
		
        audioBackground_Obj.audio.clip = base.background_clip;
        audioBackground_Obj.audio.loop = true;
        audioBackground_Obj.audio.volume = 0.8f;
        audioBackground_Obj.audio.Play();
		
		yield return null;
	}

    private const string PATH_OF_DYNAMIC_CLIP = "AudioClips/GameIntroduce/Shop/";
    private const string PATH_OF_MERCHANDISC_CLIP = "AudioClips/AudioDescribe/";
    private const string PATH_OD_APOLOGIZE_CLIP = "AudioClips/ApologizeClips/";
    private const string PATH_OF_APPRECIATE_CLIP = "AudioClips/AppreciateClips/";
    private const string PATH_OF_THANKS_CLIP = "AudioClips/ThanksClips/";
    private IEnumerator ReInitializeAudioClipData()
    {
        description_clips.Clear();
        if (Main.Mz_AppLanguage.appLanguage == Main.Mz_AppLanguage.SupportLanguage.TH)
        {
            description_clips.Add(Resources.Load(PATH_OF_DYNAMIC_CLIP + "1.TH_greeting", typeof(AudioClip)) as AudioClip);
            description_clips.Add(Resources.Load(PATH_OF_DYNAMIC_CLIP + "2.TH_ordering", typeof(AudioClip)) as AudioClip);
            description_clips.Add(Resources.Load(PATH_OF_DYNAMIC_CLIP + "3.TH_dragGoodsToTray", typeof(AudioClip)) as AudioClip);
            description_clips.Add(Resources.Load(PATH_OF_DYNAMIC_CLIP + "4.TH_checkingAccuracy", typeof(AudioClip)) as AudioClip);
            description_clips.Add(Resources.Load(PATH_OF_DYNAMIC_CLIP + "5.TH_billing", typeof(AudioClip)) as AudioClip);
            description_clips.Add(Resources.Load(PATH_OF_DYNAMIC_CLIP + "6.TH_calculationPrice", typeof(AudioClip)) as AudioClip);
            description_clips.Add(Resources.Load(PATH_OF_DYNAMIC_CLIP + "7.TH_giveTheChange", typeof(AudioClip)) as AudioClip);
            description_clips.Add(Resources.Load(PATH_OF_DYNAMIC_CLIP + "8.TH_completeTutor", typeof(AudioClip)) as AudioClip);
            description_clips.Add(Resources.Load(PATH_OF_DYNAMIC_CLIP + "TH_noticeUserToUpgrade", typeof(AudioClip)) as AudioClip);

            apologize_clip[0] = Resources.Load(PATH_OD_APOLOGIZE_CLIP + "TH_shortSorry_0001", typeof(AudioClip)) as AudioClip;
            apologize_clip[1] = Resources.Load(PATH_OD_APOLOGIZE_CLIP + "TH_shortSorry_0002", typeof(AudioClip)) as AudioClip;
            apologize_clip[2] = Resources.Load(PATH_OD_APOLOGIZE_CLIP + "TH_longSorry_0001", typeof(AudioClip)) as AudioClip;
            apologize_clip[3] = Resources.Load(PATH_OD_APOLOGIZE_CLIP + "TH_longSorry_0002", typeof(AudioClip)) as AudioClip;
            apologize_clip[4] = Resources.Load(PATH_OD_APOLOGIZE_CLIP + "TH_longSorry_0003", typeof(AudioClip)) as AudioClip;

            thanksCustomer_clips[0] = Resources.Load(PATH_OF_THANKS_CLIP + "TH_Thank_0001", typeof(AudioClip)) as AudioClip;
            thanksCustomer_clips[1] = Resources.Load(PATH_OF_THANKS_CLIP + "TH_Thank_0002", typeof(AudioClip)) as AudioClip;

            //			appreciate_clips[0] = Resources.Load(PATH_OF_APPRECIATE_CLIP + "EN_appreciate_001", typeof(AudioClip)) as AudioClip;
            //			appreciate_clips[1] = Resources.Load(PATH_OF_APPRECIATE_CLIP + "EN_appreciate_002", typeof(AudioClip)) as AudioClip;
            //			appreciate_clips[2] = Resources.Load(PATH_OF_APPRECIATE_CLIP + "EN_appreciate_003", typeof(AudioClip)) as AudioClip;
            //			appreciate_clips[3] = Resources.Load(PATH_OF_APPRECIATE_CLIP + "EN_appreciate_004", typeof(AudioClip)) as AudioClip;
            //			appreciate_clips[4] = Resources.Load(PATH_OF_APPRECIATE_CLIP + "EN_appreciate_005", typeof(AudioClip)) as AudioClip;
        }
        else if (Main.Mz_AppLanguage.appLanguage == Main.Mz_AppLanguage.SupportLanguage.EN)
        {
            description_clips.Add(Resources.Load(PATH_OF_DYNAMIC_CLIP + "1.EN_greeting", typeof(AudioClip)) as AudioClip);
            description_clips.Add(Resources.Load(PATH_OF_DYNAMIC_CLIP + "2.EN_ordering", typeof(AudioClip)) as AudioClip);
            description_clips.Add(Resources.Load(PATH_OF_DYNAMIC_CLIP + "3.EN_dragGoodsToTray", typeof(AudioClip)) as AudioClip);
            description_clips.Add(Resources.Load(PATH_OF_DYNAMIC_CLIP + "4.EN_checkingAccuracy", typeof(AudioClip)) as AudioClip);
            description_clips.Add(Resources.Load(PATH_OF_DYNAMIC_CLIP + "5.EN_billing", typeof(AudioClip)) as AudioClip);
            description_clips.Add(Resources.Load(PATH_OF_DYNAMIC_CLIP + "6.EN_calculationPrice", typeof(AudioClip)) as AudioClip);
            description_clips.Add(Resources.Load(PATH_OF_DYNAMIC_CLIP + "7.EN_giveTheChange", typeof(AudioClip)) as AudioClip);
            description_clips.Add(Resources.Load(PATH_OF_DYNAMIC_CLIP + "8.EN_completeTutor", typeof(AudioClip)) as AudioClip);
            description_clips.Add(Resources.Load(PATH_OF_DYNAMIC_CLIP + "EN_noticeUserToUpgrade", typeof(AudioClip)) as AudioClip);

            apologize_clip[0] = Resources.Load(PATH_OD_APOLOGIZE_CLIP + "EN_shortSorry_0001", typeof(AudioClip)) as AudioClip;
            apologize_clip[1] = Resources.Load(PATH_OD_APOLOGIZE_CLIP + "EN_shortSorry_0002", typeof(AudioClip)) as AudioClip;
            apologize_clip[2] = Resources.Load(PATH_OD_APOLOGIZE_CLIP + "EN_longSorry_0001", typeof(AudioClip)) as AudioClip;
            apologize_clip[3] = Resources.Load(PATH_OD_APOLOGIZE_CLIP + "EN_longSorry_0002", typeof(AudioClip)) as AudioClip;
            apologize_clip[4] = Resources.Load(PATH_OD_APOLOGIZE_CLIP + "EN_longSorry_0003", typeof(AudioClip)) as AudioClip;

            thanksCustomer_clips[0] = Resources.Load(PATH_OF_THANKS_CLIP + "EN_Thank_0001", typeof(AudioClip)) as AudioClip;
            thanksCustomer_clips[1] = Resources.Load(PATH_OF_THANKS_CLIP + "EN_Thank_0002", typeof(AudioClip)) as AudioClip;

            //			appreciate_clips[0] = Resources.Load(PATH_OF_APPRECIATE_CLIP + "EN_appreciate_001", typeof(AudioClip)) as AudioClip;
            //			appreciate_clips[1] = Resources.Load(PATH_OF_APPRECIATE_CLIP + "EN_appreciate_002", typeof(AudioClip)) as AudioClip;
            //			appreciate_clips[2] = Resources.Load(PATH_OF_APPRECIATE_CLIP + "EN_appreciate_003", typeof(AudioClip)) as AudioClip;
            //			appreciate_clips[3] = Resources.Load(PATH_OF_APPRECIATE_CLIP + "EN_appreciate_004", typeof(AudioClip)) as AudioClip;
            //			appreciate_clips[4] = Resources.Load(PATH_OF_APPRECIATE_CLIP + "EN_appreciate_005", typeof(AudioClip)) as AudioClip;
        }

        this.ReInitializingMerchandiseNameAudio();

        yield return 0;
    }

    private void ReInitializingMerchandiseNameAudio()
    {
        audioDescriptionData.merchandiseNameDescribes = new AudioClip[30];

        if (Main.Mz_AppLanguage.appLanguage == Main.Mz_AppLanguage.SupportLanguage.EN)
        {
            for (int i = 0; i < 30; i++)
            {
                audioDescriptionData.merchandiseNameDescribes[i] =
                    Resources.Load(PATH_OF_MERCHANDISC_CLIP + "EN/" + goodDataStore.FoodDatabase_list[i].name, typeof(AudioClip)) as AudioClip;
            }
        }
        else if (Main.Mz_AppLanguage.appLanguage == Main.Mz_AppLanguage.SupportLanguage.TH)
        {
            for (int i = 0; i < 30; i++)
            {
                audioDescriptionData.merchandiseNameDescribes[i] = Resources.Load(PATH_OF_MERCHANDISC_CLIP + "TH/" + goodDataStore.FoodDatabase_list[i].name, typeof(AudioClip)) as AudioClip;
            }
        }
    }

    private IEnumerator InitializeGameEffect()
    {
        if (gameEffectManager == null) {
            this.gameObject.AddComponent<GameEffectManager>();
            gameEffectManager = this.GetComponent<GameEffectManager>();
        }
        else
            yield return null;
    }

	#region <!-- Tutor systems.
	
	void CreateTutorObjectAtRuntime ()
	{
		cameraTutor_Obj = GameObject.FindGameObjectWithTag("MainCamera");
		
		handTutor = Instantiate(Resources.Load("Tutor_Objs/Town/HandTutor", typeof(GameObject))) as GameObject;
		handTutor.transform.parent = cameraTutor_Obj.transform;
		handTutor.transform.localPosition = new Vector3(30f, 75f, 3f);
		handTutor.transform.localScale = Vector3.one;
		
		GameObject tutorText_0 = Instantiate(Resources.Load("Tutor_Objs/SheepBank/Tutor_description", typeof(GameObject))) as GameObject;
		tutorText_0.transform.parent = cameraTutor_Obj.transform;
		tutorText_0.transform.localPosition = new Vector3(65f, 80f, 3f);
		tutorText_0.transform.localScale = Vector3.one;

		base.tutorDescriptions = new List<GameObject>();
		tutorDescriptions.Add(tutorText_0);
		//<@-- Animated hand with tweening.
		iTween.MoveTo(handTutor.gameObject, iTween.Hash("y", 65f, "Time", .5f, "easetype", iTween.EaseType.easeInSine, "looptype", iTween.LoopType.pingPong));
	}

	void CreateGreetingCustomerTutorEvent()
    {
		shopTutor.greeting_textSprite.active = false;
        shopTutor.greeting_textmesh.active = true;

        tutorDescriptions[0].GetComponent<tk2dTextMesh>().text = "GREETING";
        tutorDescriptions[0].GetComponent<tk2dTextMesh>().Commit();

        audioDescribe.PlayOnecSound(description_clips[0]);
	}

	void CreateAcceptOrdersTutorEvent ()
    {
		this.SetActivateTotorObject(true);
		shopTutor.goaway_button_obj.active = false;
		darkShadowPlane.transform.position += Vector3.forward * 3;
		
		handTutor.transform.localPosition = new Vector3(-62f, -13f, 3f);
		
		tutorDescriptions[0].transform.localPosition = new Vector3(0f, 0f, 3f);
		tutorDescriptions[0].GetComponent<tk2dTextMesh>().text = "ACCEPT ORDERS";
		tutorDescriptions[0].GetComponent<tk2dTextMesh>().Commit();
		//<@-- Animated hand with tweening.
        iTween.MoveTo(handTutor.gameObject, iTween.Hash("y", -20f, "Time", .5f, "easetype", iTween.EaseType.easeInSine, "looptype", iTween.LoopType.pingPong));
		
		if(_isPlayAcceptOuderSound == false)
			StartCoroutine(this.WaitForHelloCustomer());
	}

	void GenerateGoodOrderTutorEvent ()
	{
		base.SetActivateTotorObject(true);

        handTutor.transform.localPosition = new Vector3(-0.68f, 0.38f, 3f);
		
		tutorDescriptions[0].transform.localPosition = new Vector3(-0.48f, 0.5f, 3f);
		tutorDescriptions[0].GetComponent<tk2dTextMesh>().text = "TAP";
		tutorDescriptions[0].GetComponent<tk2dTextMesh>().Commit();
		//<@-- Animated hand with tweening.
		iTween.MoveTo(handTutor.gameObject, iTween.Hash("y", 0.45f, "Time", .5f, "easetype", iTween.EaseType.easeInSine, "looptype", iTween.LoopType.pingPong));
	}

    internal void CreateDragGoodsToTrayTutorEvent()
    {
        Vector3 originalFoodTrayPos = foodsTray_obj.transform.position;
        foodsTray_obj.transform.position = new Vector3(originalFoodTrayPos.x, originalFoodTrayPos.y, -5.5f);

        base.SetActivateTotorObject(true);

        handTutor.transform.localPosition = new Vector3(0f, -0.5f, 3f);

        tutorDescriptions[0].transform.localPosition = new Vector3(0.6f, -0.3f, 3f);
        tutorDescriptions[0].GetComponent<tk2dTextMesh>().text = "DRAG GOOD TO TRAY";
        tutorDescriptions[0].GetComponent<tk2dTextMesh>().Commit();
        //<@-- Animated hand with tweening.
        iTween.MoveTo(handTutor.gameObject, iTween.Hash("y", -.6f, "Time", .5f, "easetype", iTween.EaseType.easeInSine, "looptype", iTween.LoopType.pingPong));
    }
	
	private bool _isPlayAcceptOuderSound = false;
    private IEnumerator WaitForHelloCustomer()
    {
		_isPlayAcceptOuderSound = true;
		
        yield return new WaitForSeconds(en_greeting_clip[0].length);
        audioDescribe.PlayOnecSound(description_clips[1]);
    }

    private void CreateCheckingAccuracyTutorEvent()
    {
        this.SetActivateTotorObject(true);
        shopTutor.goaway_button_obj.active = false;
        Vector3 originalFoodTrayPos = foodsTray_obj.transform.position;
        foodsTray_obj.transform.position = new Vector3(originalFoodTrayPos.x, originalFoodTrayPos.y, -2f);

        handTutor.transform.localPosition = new Vector3(-0.62f, -0.13f, 3f);

        tutorDescriptions[0].transform.localPosition = new Vector3(0f, 0f, 3f);
        tutorDescriptions[0].GetComponent<tk2dTextMesh>().text = "CHECK ACCURACY";
        tutorDescriptions[0].GetComponent<tk2dTextMesh>().Commit();
        //<@-- Animated hand with tweening.
        iTween.MoveTo(handTutor.gameObject, iTween.Hash("y", -0.2f, "Time", .5f, "easetype", iTween.EaseType.easeInSine, "looptype", iTween.LoopType.pingPong));
    }

    private void CreateBillingTutorEvent()
    {
        shopTutor.currentTutorState = ShopTutor.TutorStatus.Billing;

        base.SetActivateTotorObject(true);
		darkShadowPlane.active = true;
        billingMachine.transform.position += Vector3.back * 5f;

        handTutor.transform.localPosition = new Vector3(-0.25f, -0.1f, 3f);

        tutorDescriptions[0].transform.localPosition = new Vector3(0.1f, 0f, 3f);
        tutorDescriptions[0].GetComponent<tk2dTextMesh>().text = "BILLING";
        tutorDescriptions[0].GetComponent<tk2dTextMesh>().Commit();
        //<@-- Animated hand with tweening.
        iTween.MoveTo(handTutor.gameObject, iTween.Hash("y", 0f, "Time", .5f, "easetype", iTween.EaseType.easeInSine, "looptype", iTween.LoopType.pingPong));

        audioDescribe.PlayOnecSound(description_clips[2]);
    }

	#endregion

	IEnumerator ChangeShopLogoIcon ()
	{
		shopLogo_sprite.spriteId = shopLogo_sprite.GetSpriteIdByName(InitializeNewShop.shopLogo_NameSpecify[Mz_StorageManage.ShopLogo]);
		shopLogo_sprite.color = InitializeNewShop.shopLogos_Color[Mz_StorageManage.ShopLogoColor];

		yield return 0;
	}

	IEnumerator InitailizeShopLabelGUI ()
	{		
		if(Mz_StorageManage.Username != string.Empty) {
			base.shopnameTextmesh.text = Mz_StorageManage.ShopName;
			base.shopnameTextmesh.Commit();

			base.availableMoney.text = Mz_StorageManage.AvailableMoney.ToString();
			base.availableMoney.Commit();
		}
		yield return null;
	}

	IEnumerator InitializeObjectAnimation ()
	{
        billingMachine_animState = billingMachine.animation["billingMachine_anim"];
        billingMachine_animState.wrapMode = WrapMode.Once;
        billingAnimatedSprite = billingMachine.GetComponent<tk2dAnimatedSprite>();

		yield return 0;
	}

    private IEnumerator InitializeCanSellGoodslist()
    {
		if(Mz_StorageManage.Username == string.Empty) {
	        SushiShop.NumberOfCansellItem.Clear();
            for (int i = 0; i < GoodDataStore.FoodDatabaseCapacity; i++)
            {
                SushiShop.NumberOfCansellItem.Add(i);
            }
            base.extendsStorageManager.SaveCanSellGoodListData();
		}
        if(SushiShop.NumberOfCansellItem.Count == 0)
            base.extendsStorageManager.LoadCanSellGoodsListData();		

        yield return new WaitForFixedUpdate();

        foreach (int id in NumberOfCansellItem)
        {
            CanSellGoodLists.Add(goodDataStore.FoodDatabase_list[id]);
        }
    }

    #region <!-- Customer manage system.

    private void Handle_nullCustomer_event(object sender, EventArgs e) {
    	StartCoroutine(CreateCustomer());
    }

    private IEnumerator CreateCustomer() { 
		yield return new WaitForSeconds(1f);
		
        if(currentCustomer == null) {
            audioEffect.PlayOnecSound(audioEffect.dingdong_clip);
            this.manageGoodsComplete_event += Handle_manageGoodsComplete_event;
			
			GameObject customer = Instantiate(Resources.Load("Customers/CustomerBeh_obj", typeof(GameObject))) as GameObject;
            currentCustomer = customer.GetComponent<CustomerBeh>();
			
			currentCustomer.customerSprite_Obj = Instantiate(Resources.Load("Customers/Customer_AnimatedSprite", typeof(GameObject))) as GameObject;
			currentCustomer.customerSprite_Obj.transform.parent = customerMenu_group_Obj.transform;
			currentCustomer.customerSprite_Obj.transform.localPosition = new Vector3(-6f, -77f, -.1f);
			
			currentCustomer.customerOrderingIcon_Obj = Instantiate(Resources.Load("Customers/CustomerOrdering_icon", typeof(GameObject))) as GameObject;
			currentCustomer.customerOrderingIcon_Obj.transform.parent = customerMenu_group_Obj.transform;
			currentCustomer.customerOrderingIcon_Obj.transform.localPosition = new Vector3(35f, -60f, -2f);
			currentCustomer.customerOrderingIcon_Obj.name = "OrderingIcon";
			
			currentCustomer.customerOrderingIcon_Obj.active = false;
        }
		else {
			Debug.LogError("Current Cusstomer does not correct destroying..." + " :: " + currentCustomer);
		}

		currentGamePlayState = GamePlayState.GreetingCustomer;
		this.SetActiveGreetingMessage(true);

		if(MainMenu._HasNewGameEvent) {
			darkShadowPlane.active = true;
//            darkShadowPlane.transform.position += Vector3.back * 2f;
            this.CreateTutorObjectAtRuntime();
            this.CreateGreetingCustomerTutorEvent();
		}
		else {
			darkShadowPlane.active = false;
            Destroy(shopTutor.greeting_textmesh);
        }
    }

    private IEnumerator ExpelCustomer() {
        yield return new WaitForSeconds(1f);

	    if(currentCustomer != null) {
	        currentCustomer.Dispose();
			foreach (GoodsBeh item in foodTrayBeh.goodsOnTray_List) {
				item.OnDispose();
			}
			foodTrayBeh.goodsOnTray_List.Clear();
			StartCoroutine(this.CollapseOrderingGUI());
			this.manageGoodsComplete_event -= Handle_manageGoodsComplete_event;
	    }
		
		yield return new WaitForFixedUpdate();	
		
		OnNullCustomer_event(EventArgs.Empty);
    }

	#endregion

    public void Handle_manageGoodsComplete_event(object sender, System.EventArgs eventArgs)
    {
        //		int r = UE.Random.Range(0, appreciate_clips.Length);
        //		this.PlayAppreciateAudioClip(appreciate_clips[r]);

        audioEffect.PlayOnecWithOutStop(audioEffect.correctBring_clip);

        currentGamePlayState = GamePlayState.calculationPrice;

        TK_animationManager.PlayGoodAnimation();
        currentCustomer.customerOrderingIcon_Obj.active = false;

        StartCoroutine(this.ShowReceiptGUIForm());
    }

    private IEnumerator ShowReceiptGUIForm()
    {
		yield return new WaitForSeconds(0.5f);

		darkShadowPlane.active = true;
        
        audioEffect.PlayOnecSound(audioEffect.receiptCash_clip);
		
		this.CreateTKCalculator();
		calculatorBeh.result_Textmesh = displayAnswer_textmesh;
		receiptGUIForm_groupObj.SetActiveRecursively(true);
		this.DeActiveCalculationPriceGUI();
		this.ManageCalculationPriceGUI();

        if(MainMenu._HasNewGameEvent) {
            audioDescribe.PlayOnecSound(description_clips[3]);
        }
    }

	void DeActiveCalculationPriceGUI ()
	{
		for (int i = 0; i < arr_addNotations.Length; i++) {
			arr_addNotations[i].active = false;
		}
		for (int i = 0; i < arr_goodsLabel.Length; i++) {
			arr_goodsLabel[i].SetActiveRecursively(false);
		}
	}

	void ManageCalculationPriceGUI ()
	{		
		for (int i = 0; i < currentCustomer.customerOrderRequire.Count; i++) {
			arr_goodsLabel[i].SetActiveRecursively(true);
			arr_GoodsTag[i].spriteId = arr_GoodsTag[i].GetSpriteIdByName(currentCustomer.customerOrderRequire[i].food.name);
			arr_GoodsPrice_textmesh[i].text = currentCustomer.customerOrderRequire[i].food.price.ToString();
			arr_GoodsPrice_textmesh[i].Commit();
			if(i != 0)
				arr_addNotations[i - 1].active = true;
		}
	}

    internal void GenerateOrderGUI ()
	{
		foreach (tk2dSprite item in arr_orderingBaseItems) {
			item.spriteId = item.GetSpriteIdByName(BASE_ORDER_ITEM_NORMAL);
			item.gameObject.SetActiveRecursively (false);
		}

		for (int i = 0; i < currentCustomer.customerOrderRequire.Count; i++) {
			arr_orderingBaseItems[i].gameObject.SetActiveRecursively(true);	
			arr_orderingItems[i].spriteId = arr_orderingItems[i].GetSpriteIdByName(currentCustomer.customerOrderRequire[i].food.name);
            arr_orderingItems[i].gameObject.name = currentCustomer.customerOrderRequire[i].food.name;
		}

		StartCoroutine(this.ShowOrderingGUI());
		currentGamePlayState = GamePlayState.Ordering;
    }

	IEnumerator ShowOrderingGUI ()
	{
		if(MainMenu._HasNewGameEvent) {
			iTween.MoveTo(baseOrderUI_Obj.gameObject, 
			              iTween.Hash("position", new Vector3(-85f, 6f, -2.5f), "islocal", true, "time", .5f, "easetype", iTween.EaseType.spring));
			
            if (shopTutor.currentTutorState == ShopTutor.TutorStatus.AcceptOrders) {
                this.CreateAcceptOrdersTutorEvent();
			}
            else if (shopTutor.currentTutorState == ShopTutor.TutorStatus.CheckAccuracy) {
                base.SetActivateTotorObject(false);
                this.CreateCheckingAccuracyTutorEvent();
            }
		}
		else {
			iTween.MoveTo(baseOrderUI_Obj.gameObject, 
		              iTween.Hash("position", new Vector3(-85f, 6f, -10f), "islocal", true, "time", .5f, "easetype", iTween.EaseType.spring));
		}

		yield return new WaitForFixedUpdate();
		
		this.CheckingGoodsObjInTray(GoodsBeh.ClassName);
		currentCustomer.customerOrderingIcon_Obj.active = false;
		darkShadowPlane.active = true;
		
		foreach (var item in arr_orderingItems) {
			iTween.Resume(item.gameObject);
            iTween.MoveTo(item.gameObject, iTween.Hash("y", 10f, "islocal", true, "time", .3f, "looptype", iTween.LoopType.pingPong));
		}
	}

	IEnumerator CollapseOrderingGUI ()
	{
        if (MainMenu._HasNewGameEvent)
        {
			base.SetActivateTotorObject(false);
            
			iTween.MoveTo(baseOrderUI_Obj.gameObject,
                      iTween.Hash("position", new Vector3(-85f, -200f, 0f), "islocal", true, "time", 0.5f, "easetype", iTween.EaseType.linear));

            if (shopTutor.currentTutorState == ShopTutor.TutorStatus.AcceptOrders)
            {
                this.GenerateGoodOrderTutorEvent();
                yield return new WaitForSeconds(0.5f);
                foreach (var item in arr_orderingItems)
                {
                    iTween.Pause(item.gameObject);
                }

                if (currentCustomer)
                {
                    currentCustomer.customerOrderingIcon_Obj.active = true;

                    iTween.PunchPosition(currentCustomer.customerOrderingIcon_Obj,
                        iTween.Hash("x", 10f, "y", 10f, "delay", 1f, "time", .5f, "looptype", iTween.LoopType.pingPong));
                }
            }
            else if(shopTutor.currentTutorState == ShopTutor.TutorStatus.CheckAccuracy) {
				this.CreateBillingTutorEvent();
			}
        }
        else
        {
            iTween.MoveTo(baseOrderUI_Obj.gameObject,
                      iTween.Hash("position", new Vector3(-85f, -200f, 0f), "islocal", true, "time", 0.5f, "easetype", iTween.EaseType.linear));

            yield return new WaitForSeconds(0.5f);

            darkShadowPlane.active = false;
            foreach (var item in arr_orderingItems)
            {
                iTween.Pause(item.gameObject);
            }

            if (currentCustomer)
            {
                currentCustomer.customerOrderingIcon_Obj.active = true;

                iTween.PunchPosition(currentCustomer.customerOrderingIcon_Obj,
                    iTween.Hash("x", 10f, "y", 10f, "delay", 1f, "time", .5f, "looptype", iTween.LoopType.pingPong));
            }
        }
	}

	private void SetActiveGreetingMessage (bool activeState)
	{
		if(activeState) {
			greetingMessage_ObjGroup.SetActiveRecursively(true);
			iTween.ScaleTo(greetingMessage_ObjGroup, iTween.Hash("x", 1f, "y", 1f, "time", 0.5f, "easetype", iTween.EaseType.easeInSine));
		}
		else {
			iTween.ScaleTo(greetingMessage_ObjGroup, iTween.Hash("x", 0f, "y", 0f, "time", 0.5f, "easetype", iTween.EaseType.easeInExpo,
				"oncomplete", "UnActiveGreetingMessage", "oncompletetarget", this.gameObject));
		}
	}
	
	void UnActiveGreetingMessage() {		
		greetingMessage_ObjGroup.SetActiveRecursively(false);
		
		if(MainMenu._HasNewGameEvent) {
			currentCustomer.GenerateTutorGoodOrderEvent();
		}
		else
			currentCustomer.GenerateGoodOrder();
	}
	
	IEnumerator PlayApologizeCustomer (AudioClip clip)
	{
		this.TK_animationManager.PlayEyeAnimation(CharacterAnimationManager.NameAnimationsList.agape);
		currentCustomer.PlayRampage_animation();
		
		while (TK_animationManager._isPlayingAnimation) {
			yield return null;
		}
		
		TK_animationManager.PlayTalkingAnimation();
		this.PlayApologizeAudioClip(clip);
	}

	#region <@-- Play Audio method.

	IEnumerator PlayGreetingAudioClip (AudioClip clip)
	{
		audioDescribe.audio.clip = clip;		
		audioDescribe.audio.Play();
		this.SetActiveGreetingMessage(false);
		
		yield return null;
	}

	void PlayAppreciateAudioClip (AudioClip audioClip)
	{
		this.audioDescribe.PlayOnecSound(audioClip);
	}

	void PlayApologizeAudioClip (AudioClip audioClip)
	{
		this.audioDescribe.PlayOnecSound(audioClip);
	}

	#endregion
    
    private void CreateTKCalculator() {
        if(calculator_group_instance) {
            calculator_group_instance.SetActiveRecursively(true);
			
			if(calculatorBeh == null) {
				calculatorBeh = calculator_group_instance.GetComponent<Mz_CalculatorBeh>();
			}
        }

        if (calculatorBeh == null)
            Debug.LogError(calculatorBeh);
    }
	
    private IEnumerator ReceiveMoneyFromCustomer() {
        currentGamePlayState = GamePlayState.receiveMoney;
        
		if (cash_obj == null)
        {
            cash_obj = Instantiate(Resources.Load("Money/Cash", typeof(GameObject))) as GameObject;
			cash_obj.transform.position = new Vector3(0, 0, -5);
			cash_sprite = cash_obj.GetComponent<tk2dSprite>();
			
            if(currentCustomer.amount < 20) {
				cash_sprite.spriteId = cash_sprite.GetSpriteIdByName("cash_20");
				currentCustomer.payMoney = 20;
            }
            else if(currentCustomer.amount < 50) {
				cash_sprite.spriteId = cash_sprite.GetSpriteIdByName("cash_50");
				currentCustomer.payMoney = 50;
            }
            else if(currentCustomer.amount <= 100) {
				cash_sprite.spriteId = cash_sprite.GetSpriteIdByName("cash_100");
				currentCustomer.payMoney = 100;
            }
        }
		
		yield return new WaitForSeconds(3);
		
		Destroy(cash_obj.gameObject);
		calculator_group_instance.SetActiveRecursively(true);
		this.DeActiveCalculationPriceGUI();

		this.ShowGiveTheChangeForm();
		currentGamePlayState = GamePlayState.giveTheChange;

        if(MainMenu._HasNewGameEvent) {
            audioDescribe.PlayOnecSound(description_clips[4]);
        }
    }

	void ShowGiveTheChangeForm ()
	{
        giveTheChangeGUIForm_groupObj.SetActiveRecursively(true);
		darkShadowPlane.active = true;
		
		audioEffect.PlayOnecSound(audioEffect.giveTheChange_clip);

        totalPrice_textmesh.text = currentCustomer.amount.ToString();
        totalPrice_textmesh.Commit();
        receiveMoney_textmesh.text = currentCustomer.payMoney.ToString();
        receiveMoney_textmesh.Commit();

        calculatorBeh.result_Textmesh = change_textmesh;
	}

    private void TradingComplete() {
        currentGamePlayState = GamePlayState.TradeComplete;

        foreach(var good in foodTrayBeh.goodsOnTray_List) {
            Destroy(good.gameObject);
        }

        foodTrayBeh.goodsOnTray_List.Clear();

        StartCoroutine(this.PackagingGoods());

        if(MainMenu._HasNewGameEvent) {
            MainMenu._HasNewGameEvent = false;
            Destroy(shopTutor.greeting_textmesh);
            shopTutor.goaway_button_obj.active = true;
            shopTutor = null;
            darkShadowPlane.transform.position += Vector3.forward * 2f;

            audioDescribe.PlayOnecSound(description_clips[5]);
        }
    }

    private IEnumerator PackagingGoods()
    {
        if(packaging_Obj == null) {
            packaging_Obj = Instantiate(Resources.Load(ObjectsBeh.Packages_ResourcePath + "Packages_Sprite", typeof(GameObject))) as GameObject;
            packaging_Obj.transform.parent = foodsTray_obj.transform;
            packaging_Obj.transform.localPosition = new Vector3(0, .1f, -.1f);
        }
		
		TK_animationManager.RandomPlayGoodAnimation();

        yield return new WaitForSeconds(2);
        
        StartCoroutine(this.CreateGameEffect());
		audioEffect.PlayOnecSound(audioEffect.longBring_clip);

		TK_animationManager.RandomPlayGoodAnimation();

        billingAnimatedSprite.Play("Thanks");
        billingAnimatedSprite.animationCompleteDelegate = delegate(tk2dAnimatedSprite sprite, int clipId) {
			billingAnimatedSprite.Play("Billing");
		};
        
        Mz_StorageManage.AvailableMoney += currentCustomer.amount;
        base.availableMoney.text = Mz_StorageManage.AvailableMoney.ToString();
        base.availableMoney.Commit();

        //<!-- Clare resource data.
		Destroy(packaging_Obj);
		StartCoroutine(ExpelCustomer());
    }

    private IEnumerator CreateGameEffect()
    {
        gameEffectManager.Create2DSpriteAnimationEffect(GameEffectManager.BLOOMSTAR_EFFECT_PATH, foodsTray_obj.transform);

        yield return 0;
    }
	
	public override void OnInput(string nameInput)
	{	
		if(MainMenu._HasNewGameEvent) {
			if(nameInput == "EN_001_textmesh") {
				StartCoroutine(this.PlayGreetingAudioClip(en_greeting_clip[0]));
//                base.SetActivateTotorObject(false);
                shopTutor.currentTutorState = ShopTutor.TutorStatus.AcceptOrders;

                return;
			}
		}

        //<!-- Close shop button.
		if(nameInput == close_button.name) {
			if(Application.isLoadingLevel == false && _onDestroyScene == false) {
				_onDestroyScene = true;
				
                base.extendsStorageManager.SaveDataToPermanentMemory();
                this.PreparingToCloseShop();		
				
				return;
			}
		}
		
		if (calculator_group_instance.active) 
        {
			if(currentGamePlayState == GamePlayState.calculationPrice) {
				if(nameInput == "ok_button") {
					this.CallCheckAnswerOfTotalPrice();
					return;
				}
			}
			else if(currentGamePlayState == GamePlayState.giveTheChange) {
				if(nameInput == "ok_button") {
					this.CallCheckAnswerOfGiveTheChange();
					return;
				}
			}
			
			calculatorBeh.GetInput(nameInput);
		}

		if(currentGamePlayState == GamePlayState.GreetingCustomer) {
			switch (nameInput) {
                case TH_001: StartCoroutine(this.PlayGreetingAudioClip(th_greeting_clip[0]));
				break;
            case TH_002: StartCoroutine(this.PlayGreetingAudioClip(th_greeting_clip[1]));
				break;
            case TH_003: StartCoroutine(this.PlayGreetingAudioClip(th_greeting_clip[2]));
				break;
            case TH_004: StartCoroutine(this.PlayGreetingAudioClip(th_greeting_clip[3]));
				break;
            case TH_005: StartCoroutine(this.PlayGreetingAudioClip(th_greeting_clip[4]));
				break;
            case TH_006: StartCoroutine(this.PlayGreetingAudioClip(th_greeting_clip[5]));
				break;
            case EN_001: StartCoroutine(this.PlayGreetingAudioClip(en_greeting_clip[0]));
				break;
            case EN_002: StartCoroutine(this.PlayGreetingAudioClip(en_greeting_clip[1]));
				break;
            case EN_003: StartCoroutine(this.PlayGreetingAudioClip(en_greeting_clip[2]));
				break;
            case EN_004: StartCoroutine(this.PlayGreetingAudioClip(en_greeting_clip[3]));
				break;
            case EN_005: StartCoroutine(this.PlayGreetingAudioClip(en_greeting_clip[4]));
				break;
            case EN_006: StartCoroutine(this.PlayGreetingAudioClip(en_greeting_clip[5]));
				break;
            case EN_007: StartCoroutine(this.PlayGreetingAudioClip(en_greeting_clip[6]));
				break;
			default:
			break;
			}
		}
        else if (currentGamePlayState == GamePlayState.Ordering)
        {
            if (nameInput == GoodDataStore.FoodMenuList.Octopus_sushi.ToString())
                audioDescribe.PlayOnecWithOutStop(audioDescriptionData.merchandiseNameDescribes[(int)GoodDataStore.FoodMenuList.Octopus_sushi]);
            else if (nameInput == GoodDataStore.FoodMenuList.Tempura.ToString())
                audioDescribe.PlayOnecWithOutStop(audioDescriptionData.merchandiseNameDescribes[(int)GoodDataStore.FoodMenuList.Tempura]);
            else if (nameInput == GoodDataStore.FoodMenuList.Prawn_brown_maki.ToString())
                audioDescribe.PlayOnecWithOutStop(audioDescriptionData.merchandiseNameDescribes[(int)GoodDataStore.FoodMenuList.Prawn_brown_maki]);
            else if (nameInput == GoodDataStore.FoodMenuList.Miso_soup.ToString())
                audioDescribe.PlayOnecWithOutStop(audioDescriptionData.merchandiseNameDescribes[(int)GoodDataStore.FoodMenuList.Miso_soup]);
            else if (nameInput == GoodDataStore.FoodMenuList.Kimji.ToString())
                audioDescribe.PlayOnecWithOutStop(audioDescriptionData.merchandiseNameDescribes[(int)GoodDataStore.FoodMenuList.Kimji]);
            else if (nameInput == GoodDataStore.FoodMenuList.Prawn_maki.ToString())
                audioDescribe.PlayOnecWithOutStop(audioDescriptionData.merchandiseNameDescribes[(int)GoodDataStore.FoodMenuList.Prawn_maki]);
            else if (nameInput == GoodDataStore.FoodMenuList.Yaki_soba.ToString())
                audioDescribe.PlayOnecWithOutStop(audioDescriptionData.merchandiseNameDescribes[(int)GoodDataStore.FoodMenuList.Yaki_soba]);
            else if (nameInput == GoodDataStore.FoodMenuList.Pickling_cucumber_filled_maki.ToString())
                audioDescribe.PlayOnecWithOutStop(audioDescriptionData.merchandiseNameDescribes[(int)GoodDataStore.FoodMenuList.Pickling_cucumber_filled_maki]);
            else if (nameInput == GoodDataStore.FoodMenuList.Sweetened_egg_sushi.ToString())
                audioDescribe.PlayOnecWithOutStop(audioDescriptionData.merchandiseNameDescribes[(int)GoodDataStore.FoodMenuList.Sweetened_egg_sushi]);
            else if (nameInput == GoodDataStore.FoodMenuList.GreenTea_icecream.ToString())
                audioDescribe.PlayOnecWithOutStop(audioDescriptionData.merchandiseNameDescribes[(int)GoodDataStore.FoodMenuList.GreenTea_icecream]);
            else if (nameInput == GoodDataStore.FoodMenuList.Iced_greenTea.ToString())
                audioDescribe.PlayOnecWithOutStop(audioDescriptionData.merchandiseNameDescribes[(int)GoodDataStore.FoodMenuList.Iced_greenTea]);
            else if (nameInput == GoodDataStore.FoodMenuList.Spicy_shell_sushi.ToString())
                audioDescribe.PlayOnecWithOutStop(audioDescriptionData.merchandiseNameDescribes[(int)GoodDataStore.FoodMenuList.Spicy_shell_sushi]);
            else if (nameInput == GoodDataStore.FoodMenuList.Hot_greenTea.ToString())
                audioDescribe.PlayOnecWithOutStop(audioDescriptionData.merchandiseNameDescribes[(int)GoodDataStore.FoodMenuList.Hot_greenTea]);
            else if (nameInput == GoodDataStore.FoodMenuList.Crab_sushi.ToString())
                audioDescribe.PlayOnecWithOutStop(audioDescriptionData.merchandiseNameDescribes[(int)GoodDataStore.FoodMenuList.Crab_sushi]);
            else if (nameInput == GoodDataStore.FoodMenuList.Prawn_sushi.ToString())
                audioDescribe.PlayOnecWithOutStop(audioDescriptionData.merchandiseNameDescribes[(int)GoodDataStore.FoodMenuList.Prawn_sushi]);
            else if (nameInput == GoodDataStore.FoodMenuList.Zaru_soba.ToString())
                audioDescribe.PlayOnecWithOutStop(audioDescriptionData.merchandiseNameDescribes[(int)GoodDataStore.FoodMenuList.Zaru_soba]);
            else if (nameInput == GoodDataStore.FoodMenuList.California_maki.ToString())
                audioDescribe.PlayOnecWithOutStop(audioDescriptionData.merchandiseNameDescribes[(int)GoodDataStore.FoodMenuList.California_maki]);
            else if (nameInput == GoodDataStore.FoodMenuList.Ramen.ToString())
                audioDescribe.PlayOnecWithOutStop(audioDescriptionData.merchandiseNameDescribes[(int)GoodDataStore.FoodMenuList.Ramen]);
            else if (nameInput == GoodDataStore.FoodMenuList.Skipjack_tuna_sushi.ToString())
                audioDescribe.PlayOnecWithOutStop(audioDescriptionData.merchandiseNameDescribes[(int)GoodDataStore.FoodMenuList.Skipjack_tuna_sushi]);
            else if (nameInput == GoodDataStore.FoodMenuList.Eel_sushi.ToString())
                audioDescribe.PlayOnecWithOutStop(audioDescriptionData.merchandiseNameDescribes[(int)GoodDataStore.FoodMenuList.Eel_sushi]);
            else if (nameInput == GoodDataStore.FoodMenuList.Fatty_tuna_sushi.ToString())
                audioDescribe.PlayOnecWithOutStop(audioDescriptionData.merchandiseNameDescribes[(int)GoodDataStore.FoodMenuList.Fatty_tuna_sushi]);
            else if (nameInput == GoodDataStore.FoodMenuList.Salmon_sushi.ToString())
                audioDescribe.PlayOnecWithOutStop(audioDescriptionData.merchandiseNameDescribes[(int)GoodDataStore.FoodMenuList.Salmon_sushi]);
            else if (nameInput == GoodDataStore.FoodMenuList.Bean_ice_jam_on_crunching.ToString())
                audioDescribe.PlayOnecWithOutStop(audioDescriptionData.merchandiseNameDescribes[(int)GoodDataStore.FoodMenuList.Bean_ice_jam_on_crunching]);
            else if (nameInput == GoodDataStore.FoodMenuList.Curry_with_rice.ToString())
                audioDescribe.PlayOnecWithOutStop(audioDescriptionData.merchandiseNameDescribes[(int)GoodDataStore.FoodMenuList.Curry_with_rice]);
            else
            {
                switch (nameInput)
                {
                    case "OK_button":
                        StartCoroutine(this.CollapseOrderingGUI());
                        break;
                    case "Goaway_button":
                        currentCustomer.PlayRampage_animation();
                        StartCoroutine(this.ExpelCustomer());
                        break;
                    case "OrderingIcon": StartCoroutine(this.ShowOrderingGUI());
                        break;
                    case "Billing_machine":
                        if (MainMenu._HasNewGameEvent) {
                            base.SetActivateTotorObject(false);
                        }
                        audioEffect.PlayOnecSound(audioEffect.calc_clip);
                        billingMachine.animation.Play(billingMachine_animState.name);
                        StartCoroutine(this.CheckingUNITYAnimationComplete(billingMachine.animation, billingMachine_animState.name));
                        break;
//                    case "SushiIngredientTray":
//                        // Create sushi popup creation.
//					sushiProduction.OnInput(ref nameInput);
//					break;
//                    case "ClosePopup":
//					sushiProduction.OnInput(ref nameInput);
//					break;
//                    case "BucketOfRice": 
//					sushiProduction.OnInput(ref nameInput);
//					break;
                    default:
                        break;
                }

				if(nameInput == SushiProduction.Crab_sushi_face || nameInput == SushiProduction.Eel_sushi_face ||
				   nameInput == SushiProduction.Fatty_tuna_sushi_face || nameInput == SushiProduction.Octopus_sushi_face ||
				   nameInput == SushiProduction.Prawn_sushi_face || nameInput == SushiProduction.Salmon_sushi_face ||
				   nameInput == SushiProduction.Skipjack_tuna_sushi_face || nameInput == SushiProduction.Spicy_shell_sushi_face ||
				   nameInput == SushiProduction.Sweetened_egg_sushi)
				{
					sushiProduction.OnInput(ref nameInput);
				}
				else if(nameInput == SushiProduction.SushiIngredientTray || nameInput == SushiProduction.ClosePopup || nameInput == SushiProduction.BucketOfRice) 
				{
					sushiProduction.OnInput(ref nameInput);
				}
            }
        }
	}

    private void CallCheckAnswerOfTotalPrice() {
        if(currentCustomer.amount == calculatorBeh.GetDisplayResultTextToInt()) {
			audioEffect.PlayOnecSound(audioEffect.correct_Clip);
		
			calculatorBeh.ClearCalcMechanism();
            calculator_group_instance.SetActiveRecursively(false);
            receiptGUIForm_groupObj.SetActiveRecursively(false);
            darkShadowPlane.active = false;

            StartCoroutine(this.ReceiveMoneyFromCustomer());
        }
        else {
			audioEffect.PlayOnecSound(audioEffect.wrong_Clip);
            calculatorBeh.ClearCalcMechanism();
			
            Debug.LogWarning("Wrong answer !. Please recalculate");
        }
    }	
	
	private void CallCheckAnswerOfGiveTheChange() {
		int correct_TheChange = currentCustomer.payMoney - currentCustomer.amount;
		if(correct_TheChange == calculatorBeh.GetDisplayResultTextToInt()) {
			calculatorBeh.ClearCalcMechanism();
			calculator_group_instance.SetActiveRecursively(false);
            giveTheChangeGUIForm_groupObj.SetActiveRecursively(false);
            darkShadowPlane.active = false;
			
			audioEffect.PlayOnecWithOutStop(audioEffect.correct_Clip);
			
			Debug.Log("give the change :: correct");

            this.TradingComplete();
		}
        else {			
			audioEffect.PlayOnecWithOutStop(audioEffect.wrong_Clip);
			calculatorBeh.ClearCalcMechanism();
            currentCustomer.PlayRampage_animation();
			
            Debug.Log("Wrong answer !. Please recalculate");
        }
	}

    private IEnumerator CheckingUNITYAnimationComplete(Animation targetAnimation, string targetAnimatedName)
    {
        do
        {
            yield return null;
        } while (targetAnimation.IsPlaying(targetAnimatedName));

		Debug.LogWarning(targetAnimatedName + " finish !");

		this.CheckingGoodsObjInTray(string.Empty);
    }

    internal void CheckingGoodsObjInTray(string callFrom)
    {
		Debug.Log("CheckingGoodsObjInTray");
		
        if (callFrom == GoodsBeh.ClassName) {
			// Check correctly of goods with arr_orderingItems.
			// and change color of arr_orderingBaseItems.
			foreach (tk2dSprite item in arr_orderingItems) 
				item.transform.parent.GetComponent<tk2dSprite>().spriteId = arr_orderingBaseItems[0].GetSpriteIdByName(BASE_ORDER_ITEM_NORMAL);
			if(foodTrayBeh.goodsOnTray_List.Count == 0) {
				return;
			}
			
            List<CustomerOrderRequire> list_goodsTemp = new List<CustomerOrderRequire>();
            Food temp_goods = null;

            for (int i = 0; i < currentCustomer.customerOrderRequire.Count; i++)
            {
                foreach (GoodsBeh item in this.foodTrayBeh.goodsOnTray_List)
                {
                    if (item.name == currentCustomer.customerOrderRequire[i].food.name)
                    {
                        temp_goods = currentCustomer.customerOrderRequire[i].food;
                    }
                }
				
				if(temp_goods != null) {
	                list_goodsTemp.Add(new CustomerOrderRequire() { food = temp_goods, });
					
					/// Check correctly of goods with arr_orderingItems.
					/// and change color of arr_orderingBaseItems.
					foreach (tk2dSprite item in arr_orderingItems) {		
						if(list_goodsTemp[i] != null) {
							if(item.gameObject.name == list_goodsTemp[i].food.name)
								item.transform.parent.GetComponent<tk2dSprite>().spriteId = arr_orderingBaseItems[0].GetSpriteIdByName(BASE_ORDER_ITEM_COMPLETE);
						}
					};
					
					if (list_goodsTemp.Count == currentCustomer.customerOrderRequire.Count)
						this.billingMachine.animation.Play();
	
	                temp_goods = null;
				}
				else {
					list_goodsTemp.Add(new CustomerOrderRequire() { food = null });
					audioEffect.PlayOnecWithOutStop(audioEffect.wrong_Clip);
				}
            }
        }
        else if (callFrom == "newgame_event") {
            // Check correctly of goods with arr_orderingItems.
            // and change color of arr_orderingBaseItems.
            foreach (tk2dSprite item in arr_orderingItems)
                item.transform.parent.GetComponent<tk2dSprite>().spriteId = arr_orderingBaseItems[0].GetSpriteIdByName(BASE_ORDER_ITEM_NORMAL);
            if (foodTrayBeh.goodsOnTray_List.Count == 0)
            {
                Debug.Log("food on tray is empty.");
                return;
            }
            else if (this.foodTrayBeh.goodsOnTray_List.Count != currentCustomer.customerOrderRequire.Count)
            {
                Debug.Log("food on tray != customer require.");
                return;
            }
            else {
                List<CustomerOrderRequire> list_goodsTemp = new List<CustomerOrderRequire>();
                Food temp_goods = null;

                for (int i = 0; i < currentCustomer.customerOrderRequire.Count; i++)
                {
                    foreach (GoodsBeh item in this.foodTrayBeh.goodsOnTray_List)
                    {
                        if (item.name == currentCustomer.customerOrderRequire[i].food.name)
                        {
                            temp_goods = currentCustomer.customerOrderRequire[i].food;
                        }
                    }

                    if (temp_goods != null)
                    {
                        list_goodsTemp.Add(new CustomerOrderRequire() { food = temp_goods, });

                        /// Check correctly of goods with arr_orderingItems.
                        /// and change color of arr_orderingBaseItems.
                        foreach (tk2dSprite item in arr_orderingItems)
                        {
                            if (list_goodsTemp[i] != null)
                            {
                                if (item.gameObject.name == list_goodsTemp[i].food.name)
                                    item.transform.parent.GetComponent<tk2dSprite>().spriteId = arr_orderingBaseItems[0].GetSpriteIdByName(BASE_ORDER_ITEM_COMPLETE);
                            }
                        };

                        if (list_goodsTemp.Count == currentCustomer.customerOrderRequire.Count)
                        {
                            shopTutor.currentTutorState = ShopTutor.TutorStatus.CheckAccuracy;
                            this.billingMachine.animation.Play();
                            StartCoroutine(this.ShowOrderingGUI());
                        }

                        temp_goods = null;
                    }
                    else
                    {
                        list_goodsTemp.Add(new CustomerOrderRequire() { food = null });
                    }
                }
            }
        }
        else if (callFrom == string.Empty) {
            if (this.foodTrayBeh.goodsOnTray_List.Count == 0)
            {
                Debug.Log("food on tray is empty.");

                StartCoroutine(this.PlayApologizeCustomer(apologize_clip[0]));
            }
            else if (this.foodTrayBeh.goodsOnTray_List.Count != currentCustomer.customerOrderRequire.Count)
            {
                Debug.Log("food on tray != customer require.");

                StartCoroutine(this.PlayApologizeCustomer(apologize_clip[1]));
            }
            else
            {
                List<CustomerOrderRequire> list_goodsTemp = new List<CustomerOrderRequire>();
                Food temp_goods = null;

                for (int i = 0; i < currentCustomer.customerOrderRequire.Count; i++)
                {
                    foreach (GoodsBeh item in this.foodTrayBeh.goodsOnTray_List)
                    {
                        if (item.name == currentCustomer.customerOrderRequire[i].food.name)
                        {
                            temp_goods = currentCustomer.customerOrderRequire[i].food;
                        }
                    }

                    if (temp_goods != null)
                    {
                        list_goodsTemp.Add(new CustomerOrderRequire() { food = temp_goods, });

                        Debug.Log(list_goodsTemp[i].food.name);

                        if (list_goodsTemp.Count == currentCustomer.customerOrderRequire.Count)
                            OnManageGoodComplete(EventArgs.Empty);

                        temp_goods = null;
                    }
                    else
                    {
                        list_goodsTemp.Add(new CustomerOrderRequire() { food = null });
                        StartCoroutine(this.PlayApologizeCustomer(apologize_clip[0]));
                        return;
                    }
                }
            }
        }
    }
    
    private void PreparingToCloseShop()
    {
        this.OnDispose();

        iTween.MoveTo(rollingDoor_Obj, iTween.Hash("position", new Vector3(0, 0, 1), "islocal", true, "time", 1f, "easetype", iTween.EaseType.easeOutSine,
            "oncomplete", "RollingDoor_close", "oncompletetarget", this.gameObject));
		audioEffect.PlayOnecWithOutStop(base.soundEffect_clips[0]);
        rollingDoor_Obj.SetActiveRecursively(true);
    }
    private void RollingDoor_close() {
        Mz_LoadingScreen.LoadSceneName = SceneNames.Town.ToString();
        Application.LoadLevel(SceneNames.LoadingScene.ToString());	
    }

    public override void OnDispose()
    {
        base.OnDispose();
    }
}
