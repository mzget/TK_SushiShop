using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]	
public class BankOfficer {
	public const string WOMAN_IDLE = "woman_idle";
    public const string WOMAN_TALK = "woman_talk";
    public const string WOMAN_TALK_LOOP = "woman_talk_loop";
	public const string MAN_TALK = "man_talk";
	public const string MAN_IDLE = "man_idle";


	public tk2dAnimatedSprite woman_animated;
	public tk2dAnimatedSprite man_animated;
};

[System.Serializable]
public class SheepBankTutor {
	//<@-- Button name -->
	public GameObject Back_Button_obj;
	public GameObject Deposit_button_obj;
	public GameObject Withdrawal_button_obj;
	public GameObject UpgradeInside_button_obj;
    public GameObject UpgradeOutside_button_obj;
	
	public GameObject blueberry_sprite_icon;
	public GameObject upgrade_blueberryJam_button;

	public SheepBankTutor() {}

	public void OnDestroy() {
		Deposit_button_obj = null;
		Withdrawal_button_obj = null;
	}
};

public class SheepBank : Mz_BaseScene {

    public GameObject background_obj;
    public GameObject upgradeInside_window_Obj;
	public GameObject depositForm_Obj;
	public GameObject withdrawalForm_Obj;
	public GameObject transactionForm_Obj;
    public GameObject donationForm_group;
	public GameObject passbook_group;
	public GameObject shadowPlane_Obj;
	public tk2dTextMesh availableMoney_Textmesh;
	public tk2dTextMesh accountBalance_Textmesh;
	public tk2dTextMesh passbookAccountBalance_textmesh;
    public Mz_CalculatorBeh calculatorBeh;
	public GameObject availabelMoneyBillboard_Obj;
	public tk2dTextMesh availableMoneyBillboard_Textmesh;
	int resultValue = 0;

    //<!-- Constance Button Name.
	const string WITHDRAWAL_BUTTON_NAME = "Withdrawal_button";
	const string DEPOSIT_BUTTON_NAME = "Deposit_button";
	const string UpgradeInside_BUTTON_NAME = "UpgradeInside_button";
	const string UpgradeOutside_BUTTON_NAME = "UpgradeOutside_button";
    const string Donate_Button_Name = "Donate_button";
	const string PreviousButtonName = "Previous_button";
	const string NextButtonName = "Next_button";
    const string OKButtonName = "OK_button";
	const string BACK_BUTTON_NAME = "Back_button";
	const string PASSBOOKBUTTONNAME = "Passbook_button";
	const string YES_BUTTON_NAME = "Yes_button";
	const string NO_BUTTON_NAME = "No_button";
	
	const string ActiveDonationForm_function = "ActiveDonationForm";
    const string ActiveDepositForm_function = "ActiveDepositForm";
    const string ActiveWithdrawalForm_function = "ActiveWithdrawalForm";
	
	private Hashtable moveDown_Transaction_Hash;
    private Hashtable moveDownUpgradeInside = new Hashtable();
    private Hashtable moveUp_hashdata = new Hashtable();
	
	private DonationManager donationManager;
    private UpgradeInsideManager upgradeInsideManager;
	public SheepBankTutor sheepBankTutor;
    public BankOfficer offecer = new BankOfficer();
    public GameObject[] upgradeButtons = new GameObject[8];
	public AudioClip long_introduce_clip;
	public AudioClip short_introduce_clip;

    public enum GameSceneStatus { none = 0, ShowUpgradeInside = 1, ShowDonationForm, ShowDepositForm, ShowWithdrawalForm, ShowPassbook, };
    public GameSceneStatus currentGameStatus;
	
	internal static bool HaveUpgradeOutSide = false;
	
	// Use this for initialization
	void Start () {
        Mz_ResizeScale.ResizingScale(background_obj.transform);
		
		SheepBank.HaveUpgradeOutSide = false;

        StartCoroutine(this.InitializeAudio());
        StartCoroutine(base.InitializeIdentityGUI());
        StartCoroutine(this.InitializeBankOfficer());
        this.InitializeGameEffectGenerator();
		
		upgradeInsideManager = upgradeInside_window_Obj.GetComponent<UpgradeInsideManager>();
        donationManager = donationForm_group.GetComponent<DonationManager>();

        this.InitializeFields();
		
		if(MainMenu._HasNewGameEvent) {
			shadowPlane_Obj.gameObject.active = true;
			sheepBankTutor.Deposit_button_obj.transform.position += Vector3.back * 11;
			sheepBankTutor.Back_Button_obj.transform.position += Vector3.forward * 20;
			this.CreateTutorObjectAtRuntime();
		}
		else {
			shadowPlane_Obj.gameObject.active = false;
		}
	}

    private IEnumerator InitializeBankOfficer()
    {
        if (MainMenu._HasNewGameEvent)
        {
            audioDescribe.PlayOnecWithOutStop(long_introduce_clip);
            StartCoroutine(PlayWomanOfficerAnimation(string.Empty));
            StartCoroutine(PlayManOfficerAnimation(string.Empty));
        }
        else
        {
            audioDescribe.PlayOnecWithOutStop(short_introduce_clip);
            offecer.woman_animated.Play(BankOfficer.WOMAN_TALK_LOOP);
            StartCoroutine(PlayManOfficerAnimation(string.Empty));

            yield return new WaitForSeconds(short_introduce_clip.length);

            StartCoroutine(PlayWomanOfficerAnimation(string.Empty));
        }
    }

    #region <@-- Tutor system.

    void CreateTutorObjectAtRuntime ()
	{
//		sheepBankTutor = new SheepBankTutor();
		cameraTutor_Obj = GameObject.FindGameObjectWithTag("MainCamera");
		
		handTutor = Instantiate(Resources.Load("Tutor_Objs/Town/HandTutor", typeof(GameObject))) as GameObject;
		handTutor.transform.parent = cameraTutor_Obj.transform;
		handTutor.transform.localPosition = new Vector3(-75f, 17f, 8);
		handTutor.transform.localScale += Vector3.one * 100;
		
		GameObject tutorText_0 = Instantiate(Resources.Load("Tutor_Objs/SheepBank/Tutor_description", typeof(GameObject))) as GameObject;
		tutorText_0.transform.parent = cameraTutor_Obj.transform;
		tutorText_0.transform.localPosition = new Vector3(-36f, 30f, 8f);
		tutorText_0.transform.localScale += Vector3.one * 100;
		base.tutorDescriptions = new List<GameObject>();
		tutorDescriptions.Add(tutorText_0);
		//<@-- Animated hand with tweening.
		iTween.MoveTo(handTutor.gameObject, iTween.Hash("y", 25f, "Time", .5f, "easetype", iTween.EaseType.easeInSine, "looptype", iTween.LoopType.pingPong));
	}

	void CreateDepositMoneyTutorEvent ()
	{
		this.SetActivateTotorObject(true);

		handTutor.transform.localPosition = new Vector3(-27f, 1.9f, 8f);

		tutorDescriptions[0].transform.localPosition = new Vector3(15f, 5.5f, 8f);
		tutorDescriptions[0].GetComponent<tk2dTextMesh>().text = "PUT \"1300\"";
		tutorDescriptions[0].GetComponent<tk2dTextMesh>().Commit();
		//<@-- Animated hand with tweening.
		iTween.MoveTo(handTutor.gameObject, iTween.Hash("y", 15f, "Time", .5f, "easetype", iTween.EaseType.easeInSine, "looptype", iTween.LoopType.pingPong));
	}

	IEnumerator CreateUpgradeInsideTutorEvent ()
	{
		yield return new WaitForFixedUpdate();

		SetActivateTotorObject(true);

		shadowPlane_Obj.gameObject.active = true;
		sheepBankTutor.UpgradeInside_button_obj.transform.position += Vector3.back * 11;
		sheepBankTutor.Back_Button_obj.transform.position += Vector3.forward * 20;

		handTutor.transform.localPosition = new Vector3(48f, 17f, 8f);
		tutorDescriptions[0].transform.localPosition = new Vector3(1f, 32f, 8f);
		tutorDescriptions[0].GetComponent<tk2dTextMesh>().text = "UPGRADE SHOP";
		tutorDescriptions[0].GetComponent<tk2dTextMesh>().Commit();
		
		//<@-- Animated hand with tweening.
		iTween.MoveTo(handTutor.gameObject, iTween.Hash("y", 25f, "Time", .5f, "easetype", iTween.EaseType.easeInSine, "looptype", iTween.LoopType.pingPong));
	}

    void CreateBuyUpgradeShopTutorEvent()
    {
        this.SetActivateTotorObject(true);

        handTutor.transform.localPosition = new Vector3(-65f, 22f, 8f);
        tutorDescriptions[0].transform.localPosition = new Vector3(-98f, 30f, 8f);
        tutorDescriptions[0].GetComponent<tk2dTextMesh>().text = "BUY ITEM";
        tutorDescriptions[0].GetComponent<tk2dTextMesh>().Commit();

        sheepBankTutor.blueberry_sprite_icon.transform.position += Vector3.back * 10;
        sheepBankTutor.upgrade_blueberryJam_button.transform.position += Vector3.back * 10;

        //<@-- Animated hand with tweening.
        iTween.MoveTo(handTutor.gameObject, iTween.Hash("y", 25f, "Time", .5f, "easetype", iTween.EaseType.easeInSine, "looptype", iTween.LoopType.pingPong));
    }

    void CreateUpgradeOutsideTutorEvent()
    {
        SetActivateTotorObject(true);

        shadowPlane_Obj.gameObject.active = true;
        sheepBankTutor.UpgradeOutside_button_obj.transform.position += Vector3.back * 11;
        sheepBankTutor.Back_Button_obj.transform.localPosition = new Vector3(-108f, -85f, -5);

        handTutor.transform.localPosition = new Vector3(93f, 17f, 8f);
        tutorDescriptions[0].transform.localPosition = new Vector3(35f, 20f, 8f);
        tutorDescriptions[0].GetComponent<tk2dTextMesh>().text = "BUY DECORATION";
        tutorDescriptions[0].GetComponent<tk2dTextMesh>().Commit();

        //<@-- Animated hand with tweening.
        iTween.MoveTo(handTutor.gameObject, iTween.Hash("y", 25f, "Time", .5f, "easetype", iTween.EaseType.easeInSine, "looptype", iTween.LoopType.pingPong));
    }

    #endregion

    protected new IEnumerator InitializeAudio()
    {
        base.InitializeAudio();

        audioBackground_Obj.audio.clip = base.background_clip;
        audioBackground_Obj.audio.loop = true;
        audioBackground_Obj.audio.Play();
		
		yield return 0;
    }
	protected override void InitializeGameEffectGenerator ()
	{
//		base.InitializeGameEffectGenerator ();

		this.gameObject.AddComponent<GameEffectManager>();
		base.gameEffectManager = this.gameObject.GetComponent<GameEffectManager>();
	}

	private IEnumerator PlayWomanOfficerAnimation (string onCompleteFuctionName)
	{
		offecer.woman_animated.Play(BankOfficer.WOMAN_TALK);
		offecer.woman_animated.animationCompleteDelegate = delegate(tk2dAnimatedSprite sprite, int clipId) {
			offecer.woman_animated.Play(BankOfficer.WOMAN_IDLE);
		};

		do{
			yield return 0;
		}
		while(offecer.woman_animated.CurrentClip.name == BankOfficer.WOMAN_TALK);

        if (onCompleteFuctionName != "")
            this.SendMessage(onCompleteFuctionName, null, SendMessageOptions.RequireReceiver);
        else
            Debug.LogWarning("PlayWomanOfficerAnimation : " + onCompleteFuctionName);
	}
    private IEnumerator PlayManOfficerAnimation(string oncompleteFunctionName)
    {
		offecer.man_animated.Play(BankOfficer.MAN_TALK);
		offecer.man_animated.animationCompleteDelegate = delegate(tk2dAnimatedSprite sprite, int clipId) {
			offecer.man_animated.Play(BankOfficer.MAN_IDLE);
		};

		do{
			yield return 0;
		}
		while(offecer.man_animated.CurrentClip.name == BankOfficer.MAN_TALK);

        if (oncompleteFunctionName != "")
            this.SendMessage(oncompleteFunctionName, null, SendMessageOptions.RequireReceiver);
        else
            Debug.LogWarning("PlayManOfficerAnimation : " + oncompleteFunctionName);
    }

    private void InitializeFields()
    {
        moveDownUpgradeInside.Add("position", new Vector3(0, -10f, -20f));
        moveDownUpgradeInside.Add("islocal", true);
        moveDownUpgradeInside.Add("time", .5f);
        moveDownUpgradeInside.Add("easetype", iTween.EaseType.spring);
		moveDownUpgradeInside.Add("oncomplete", "OnMoveDownComplete_event");
		moveDownUpgradeInside.Add("oncompletetarget", this.gameObject);

        moveUp_hashdata.Add("position", new Vector3(0, 200, -20f));
        moveUp_hashdata.Add("islocal", true);
        moveUp_hashdata.Add("time", .5f);
        moveUp_hashdata.Add("easetype", iTween.EaseType.easeOutSine);
        moveUp_hashdata.Add("oncomplete", "OnMoveUpComplete_event");
        moveUp_hashdata.Add("oncompletetarget", this.gameObject);
		
		moveDown_Transaction_Hash = new Hashtable(moveDownUpgradeInside);
		moveDown_Transaction_Hash["position"] = new Vector3(0, 0, -20f);
		moveDown_Transaction_Hash["oncomplete"] = "OnTransactionMoveDownComplete";
		moveDown_Transaction_Hash["oncompletetarget"] = this.gameObject;
		
		for (int i = 0; i < 2; i++) {
			for (int j = 0; j < 4; j++) {
				string temp = i.ToString() + j.ToString();
				upgradeInsideManager.upgradeInsideObj2D[i,j] = upgradeInside_window_Obj.transform.Find(temp).gameObject;
				upgradeInsideManager.upgradeButton_Objs[i,j] = upgradeInside_window_Obj.transform.Find("upgrade_" + i+j).gameObject;
			}
		}
		upgradeInside_window_Obj.SetActiveRecursively(false);
		depositForm_Obj.gameObject.SetActiveRecursively(false);
		withdrawalForm_Obj.gameObject.SetActiveRecursively(false);
		transactionForm_Obj.gameObject.SetActiveRecursively(false);
        donationForm_group.SetActiveRecursively(false);
		passbook_group.SetActiveRecursively(false);

		this.AvailableMoneyManager(Mz_StorageManage.AvailableMoney);
        this.AccountBalanceManager(Mz_StorageManage.AccountBalance);
    }

    void AvailableMoneyManager(int r_value) {
        availableMoney_Textmesh.text = r_value.ToString("N");
        availableMoney_Textmesh.Commit();

		Mz_StorageManage.AvailableMoney = r_value;
		this.ReDrawGUIIdentity();
    }
    void AccountBalanceManager(int r_value) {
		accountBalance_Textmesh.text = r_value.ToString("N");
        accountBalance_Textmesh.Commit();

		Mz_StorageManage.AccountBalance = r_value;
    }

	void ReDrawGUIIdentity ()
	{
		base.availableMoney.text = Mz_StorageManage.AvailableMoney.ToString();
		base.availableMoney.Commit();
	}
	
	private void OnMoveDownComplete_event() {
        shadowPlane_Obj.active = true;
		if(upgradeInside_window_Obj.active) {        
            currentGameStatus = GameSceneStatus.ShowUpgradeInside;

			availabelMoneyBillboard_Obj.gameObject.SetActiveRecursively(true);
			this.ManageAvailabelMoneyBillBoard();

			upgradeInsideManager.ReInitializeData();
        }
		
		if(MainMenu._HasNewGameEvent) {
			upgradeInside_window_Obj.transform.position += Vector3.forward * 15;
            this.CreateBuyUpgradeShopTutorEvent();
		}
	}

	internal void ManageAvailabelMoneyBillBoard ()
	{
		availableMoneyBillboard_Textmesh.text = Mz_StorageManage.AccountBalance.ToString();
		availableMoneyBillboard_Textmesh.Commit();
	}

    private void OnMoveUpComplete_event() {
        upgradeInside_window_Obj.SetActiveRecursively(false);
		depositForm_Obj.gameObject.SetActiveRecursively(false);
		withdrawalForm_Obj.gameObject.SetActiveRecursively(false);
		transactionForm_Obj.gameObject.SetActiveRecursively(false);
        donationForm_group.SetActiveRecursively(false);
		passbook_group.SetActiveRecursively(false);
		availabelMoneyBillboard_Obj.SetActiveRecursively(false);

        currentGameStatus = GameSceneStatus.none;   
		
		if(MainMenu._HasNewGameEvent == false)
			shadowPlane_Obj.gameObject.active = false;
    }

	void OnTransactionMoveDownComplete() {        
        this.AccountBalanceManager(Mz_StorageManage.AccountBalance);
		
		if(donationForm_group.active) {			
			availabelMoneyBillboard_Obj.gameObject.SetActiveRecursively(true);
			this.ManageAvailabelMoneyBillBoard();
		}
	}
        
	protected override void Update ()
	{
		base.Update ();
	}
	
	private new void OnGUI ()
	{
		
	}
	
    public override void OnInput (string nameInput)	
	{
        if (nameInput == UpgradeInside_BUTTON_NAME)
        {
			if(MainMenu._HasNewGameEvent) {
				this.SetActivateTotorObject(false);
				sheepBankTutor.UpgradeInside_button_obj.transform.position += Vector3.forward * 11;
			}
			
            StartCoroutine(this.PlayManOfficerAnimation("ActiveUpgradeInsideForm"));
            shadowPlane_Obj.gameObject.active = true;
            return;
        }
        else if (nameInput == UpgradeOutside_BUTTON_NAME)
        {
            StartCoroutine(PlayManOfficerAnimation("ActiveUpgradeOutside"));
            shadowPlane_Obj.gameObject.active = true;
            return;
        }
        else if (nameInput == DEPOSIT_BUTTON_NAME)
        {
			if(MainMenu._HasNewGameEvent) {
				this.SetActivateTotorObject(false);
				sheepBankTutor.Deposit_button_obj.transform.position += Vector3.forward * 11;
			}
			
			StartCoroutine(this.PlayWomanOfficerAnimation(ActiveDepositForm_function));
			shadowPlane_Obj.gameObject.active = true;
			return;
        }
        else if (nameInput == WITHDRAWAL_BUTTON_NAME)
        {
            StartCoroutine(this.PlayWomanOfficerAnimation(ActiveWithdrawalForm_function));
            shadowPlane_Obj.gameObject.active = true;
            return;
        }
        else if (nameInput == Donate_Button_Name)
        {
            StartCoroutine(this.PlayWomanOfficerAnimation(ActiveDonationForm_function));
            shadowPlane_Obj.gameObject.active = true;
            return;
        }
        else if (nameInput == PASSBOOKBUTTONNAME)
        {
            this.ActivePassbookForm();
            shadowPlane_Obj.gameObject.active = true;
            return;
        }
        else if (nameInput == BACK_BUTTON_NAME)
        {
            if (upgradeInside_window_Obj.active)
            {
                iTween.MoveTo(upgradeInside_window_Obj.gameObject, moveUp_hashdata);
                return;
            }
            if (depositForm_Obj.gameObject.active)
            {
                iTween.MoveTo(depositForm_Obj.gameObject, moveUp_hashdata);
                iTween.MoveTo(transactionForm_Obj.gameObject, moveUp_hashdata);
                return;
            }
            if (withdrawalForm_Obj.gameObject.active)
            {
                iTween.MoveTo(withdrawalForm_Obj.gameObject, moveUp_hashdata);
                iTween.MoveTo(transactionForm_Obj.gameObject, moveUp_hashdata);
                return;
            }
            else if (donationForm_group.active)
            {
                iTween.MoveTo(donationForm_group, moveUp_hashdata);
                return;
            }
            else if (currentGameStatus == GameSceneStatus.ShowPassbook)
            {
                iTween.MoveTo(passbook_group, moveUp_hashdata);
                return;
            }

            if (upgradeInside_window_Obj.active == false)
            {
                if (Application.isLoadingLevel == false && _onDestroyScene == false) {
					_onDestroyScene = true;
					base.extendsStorageManager.SaveDataToPermanentMemory();

                    Mz_LoadingScreen.LoadSceneName = Mz_BaseScene.SceneNames.Town.ToString();
                    Application.LoadLevel(Mz_BaseScene.SceneNames.LoadingScene.ToString());
                }

                return;
            }
        }

        switch (currentGameStatus)
        {
            case GameSceneStatus.ShowUpgradeInside:
                if (nameInput == NextButtonName) { upgradeInsideManager.GotoNextPage(); }
                else if (nameInput == PreviousButtonName) { upgradeInsideManager.BackToPreviousPage(); }
                else if (nameInput == YES_BUTTON_NAME) {
                    if (MainMenu._HasNewGameEvent)
                    {
                        this.SetActivateTotorObject(false);
                        upgradeInsideManager.UserComfirm(); 
                        this.OnInput(BACK_BUTTON_NAME);
                        this.CreateUpgradeOutsideTutorEvent();
                    }
                    else
                        upgradeInsideManager.UserComfirm(); 
			    }
                else if (nameInput == NO_BUTTON_NAME) {
					if(MainMenu._HasNewGameEvent)
						this.audioEffect.PlayOnecWithOutStop(this.audioEffect.wrong_Clip);
					else 
						upgradeInsideManager.UnActiveComfirmationWindow(); 
				}
                else
                {
                    for (int i = 0; i < upgradeButtons.Length; i++)
                    {
                        if (nameInput == upgradeButtons[i].name)
                        {
                            upgradeInsideManager.BuyingUpgradeMechanism(upgradeButtons[i].name);
                            break;
                        }
                    }
                }
                break;
            case GameSceneStatus.ShowDonationForm : {
	                if (nameInput == PreviousButtonName)
	                {
	                    donationManager.PreviousDonationPage();
	                }
	                else if (nameInput == NextButtonName)
	                {
	                    donationManager.NextDonationPage();
	                }
					else {
            			donationManager.GetInput(nameInput);
					}
				}
                break;
            case GameSceneStatus.ShowDepositForm:
                if (nameInput == OKButtonName) {
                    CompleteDepositSession();
				}
                break;
            case GameSceneStatus.ShowWithdrawalForm:
                if (nameInput == OKButtonName)
                    CompleteWithdrawalSesion();
                break;
            default:
                break;
        }

        if(calculatorBeh.gameObject.active) {
            calculatorBeh.GetInput(nameInput);
        }
    }

    private void ActiveUpgradeInsideForm()
    {
		upgradeInside_window_Obj.SetActiveRecursively (true);
		iTween.MoveTo (upgradeInside_window_Obj.gameObject, moveDownUpgradeInside);

        audioEffect.PlayOnecWithOutStop(audioEffect.calc_clip);
    }  
    private void ActiveUpgradeOutside()
    {
		SheepBank.HaveUpgradeOutSide = true;
		if (Application.isLoadingLevel == false) {
			Mz_LoadingScreen.LoadSceneName = Mz_BaseScene.SceneNames.Town.ToString ();
			Application.LoadLevel(Mz_BaseScene.SceneNames.LoadingScene.ToString ());
		}
    }

    private void ActiveDepositForm() {
        depositForm_Obj.gameObject.SetActiveRecursively(true);
        transactionForm_Obj.gameObject.SetActiveRecursively(true);
        iTween.MoveTo(depositForm_Obj.gameObject, moveDown_Transaction_Hash);
        iTween.MoveTo(transactionForm_Obj.gameObject, moveDown_Transaction_Hash);
        
        audioEffect.PlayOnecWithOutStop(audioEffect.calc_clip);

        currentGameStatus = GameSceneStatus.ShowDepositForm;
		
		if(MainMenu._HasNewGameEvent) {
			this.CreateDepositMoneyTutorEvent();			
		}
    }
	private void CompleteDepositSession ()
	{
		resultValue = calculatorBeh.GetDisplayResultTextToInt();

		if(MainMenu._HasNewGameEvent) {   //<@-- Has nawgame tutor event.
			if(resultValue != 1300) {
				calculatorBeh.ClearCalcMechanism();		
				audioEffect.PlayOnecWithOutStop(audioEffect.wrong_Clip);
				return;
			}
			else if(resultValue == 1300) {
					int sumOfAccountBalance =  Mz_StorageManage.AccountBalance + resultValue;
					int availableBalance = Mz_StorageManage.AvailableMoney - resultValue;
					AccountBalanceManager(sumOfAccountBalance);
					AvailableMoneyManager(availableBalance);
					calculatorBeh.ClearCalcMechanism();
					
					audioEffect.PlayOnecWithOutStop(audioEffect.longBring_clip);
					gameEffectManager.Create2DSpriteAnimationEffect(GameEffectManager.BLOOMSTAR_EFFECT_PATH, GameObject.Find(OKButtonName).transform);
					
					base.SetActivateTotorObject(false);
					this.OnInput(BACK_BUTTON_NAME);
					StartCoroutine(this.CreateUpgradeInsideTutorEvent());
			}
		}
		else {   //<@-- General game event.
	        if(resultValue > 0 && resultValue <= Mz_StorageManage.AvailableMoney) {
			    int sumOfAccountBalance =  Mz_StorageManage.AccountBalance + resultValue;
			    int availableBalance = Mz_StorageManage.AvailableMoney - resultValue;
				//<@-- Checking available money after deposit more than 200.
				if(availableBalance >= 200) {				
				    AccountBalanceManager(sumOfAccountBalance);
				    AvailableMoneyManager(availableBalance);
				    calculatorBeh.ClearCalcMechanism();
					
					audioEffect.PlayOnecWithOutStop(audioEffect.longBring_clip);
					gameEffectManager.Create2DSpriteAnimationEffect(GameEffectManager.BLOOMSTAR_EFFECT_PATH, GameObject.Find(OKButtonName).transform);
				}
				else {
					//<@-- warning sound user not enough available money.
					audioDescribe.PlayOnecWithOutStop(audioEffect.wrong_Clip);
	            	calculatorBeh.ClearCalcMechanism();		
				}
	        }
	        else {
	            calculatorBeh.ClearCalcMechanism();			
				audioEffect.PlayOnecWithOutStop(audioEffect.mutter_clip);

				Debug.LogWarning("result value more than available money");
	        }
		}
	}

    private void ActiveWithdrawalForm() { 
		withdrawalForm_Obj.gameObject.SetActiveRecursively(true);
		transactionForm_Obj.gameObject.SetActiveRecursively(true);
		iTween.MoveTo(withdrawalForm_Obj.gameObject, moveDown_Transaction_Hash);
		iTween.MoveTo(transactionForm_Obj.gameObject, moveDown_Transaction_Hash);
    
        audioEffect.PlayOnecWithOutStop(audioEffect.calc_clip);

        currentGameStatus = GameSceneStatus.ShowWithdrawalForm;
    }
    private void CompleteWithdrawalSesion()
    {
        resultValue = calculatorBeh.GetDisplayResultTextToInt();

        if (resultValue > 0 && resultValue <= Mz_StorageManage.AccountBalance)
        {
            int newAvailableBalance = Mz_StorageManage.AvailableMoney + resultValue;
            int newAccountBalance = Mz_StorageManage.AccountBalance - resultValue;
            AvailableMoneyManager(newAvailableBalance);
            AccountBalanceManager(newAccountBalance);
            calculatorBeh.ClearCalcMechanism();			
			
			audioEffect.PlayOnecWithOutStop(audioEffect.longBring_clip);
			gameEffectManager.Create2DSpriteAnimationEffect(GameEffectManager.BLOOMSTAR_EFFECT_PATH, GameObject.Find(OKButtonName).transform);
        }
        else {
            calculatorBeh.ClearCalcMechanism();
			audioEffect.PlayOnecWithOutStop(audioEffect.mutter_clip);
			Debug.LogWarning("result value more than account balance");
        }
    }

    private void ActiveDonationForm() {				
        donationForm_group.SetActiveRecursively(true);
		donationManager.ReInitializeData();
        iTween.MoveTo(donationForm_group, moveDown_Transaction_Hash);

        audioEffect.PlayOnecWithOutStop(audioEffect.calc_clip);
		
		currentGameStatus = GameSceneStatus.ShowDonationForm;
    }

	private void ActivePassbookForm() {
		passbook_group.SetActiveRecursively(true);
		iTween.MoveTo(passbook_group, moveDown_Transaction_Hash);

		audioEffect.PlayOnecWithOutStop(audioEffect.calc_clip);

		currentGameStatus = GameSceneStatus.ShowPassbook;

		passbookAccountBalance_textmesh.text = Mz_StorageManage.AccountBalance.ToString();
		passbookAccountBalance_textmesh.Commit();
	}
}

