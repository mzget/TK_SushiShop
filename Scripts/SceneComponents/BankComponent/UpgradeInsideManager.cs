using System;
using UnityEngine;

public class UpgradeInsideManager : MonoBehaviour {
	//<@-- Upgrade button.
	public GameObject[,] upgradeButton_Objs = new GameObject[2,4];
	private tk2dSprite[,] upgradeButton_Sprites = new tk2dSprite[2,4];
	//<@-- Upgrade item objs.
	public GameObject[,] upgradeInsideObj2D = new GameObject[2,4];
	private tk2dSprite[,] upgradeInsideSprite2D = new tk2dSprite[2, 4];
    private tk2dTextMesh[,] upgradeInside_PriceTextmesh = new tk2dTextMesh[2, 4];
    //<@-- Confirmation window obj.
    public GameObject confirmationWindow;
	
    private string[,] firstPage_spriteNames = new string[2, 4] {
       {"Eel_sushi", "Fatty_tuna_sushi", "Octopus_sushi", "Prawn_sushi"},
       {"Salmon_sushi", "Skipjack_tuna_sushi", "Spicy_shell_sushi", "Sweetened_egg_sushi"},
    };
    private string[,] secondPage_spriteNames = new string[2, 4] {
        {"Roe_maki", "Prawn_brown_maki", "Pickling_cucumber_filled_maki", "Ramen"},
        {"Zaru_soba", "Yaki_soba", "Tempura", "Curry_with_rice"},
    };
    private string[,] thirdPage_spriteNames = new string[2, 4] {
		{"Miso_soup", "Kimji", "Bean_ice_jam_on_crunching", "GreenTea_icecream"},
		{"", "", "", ""},
	};
    private int[,] firstPage_prices = new int[,] {
        {300, 300, 1500, 3000},
        {800, 900, 500, 1000},
    };
    private int[,] secondPage_prices = new int[,] {
        {100, 150, 300, 300},
        {1400, 1200, 500, 100},
    };
    private int[,] thirdPage_prices = new int[,] {
        {150, 200, 1600, 700},
        {500, 1300,0,0},
    };

	public const string Message_Warning_NotEnoughMoney = "Connot upgrade this item because available money is not enough.";
	private int currentPageIndex = 0;
    private const int MAX_PAGE_NUMBER = 3;
	private const int ActiveUpgradeButtonID = 25;
	private const int UnActiveUpgradeButtonID = 29;
    public tk2dTextMesh displayCurrentPageID_Textmesh;
	internal bool _isInitialize = false;

    private event EventHandler<OnUpdateEvenArgs> OnUpgrade_Event;
    private void OnUpgradeEvent_checkingDelegation(OnUpdateEvenArgs e) {
        if (OnUpgrade_Event != null)
            OnUpgrade_Event(this, e);
    }
    private class OnUpdateEvenArgs : EventArgs
    {
        public int I = 0;
        public int J = 0;
        public int Item_id;
		public string AdditionalParams = string.Empty;
		
        public OnUpdateEvenArgs() { }
    };
    private OnUpdateEvenArgs currentOnUpdateTarget;

	/// <summary>
	/// Calss references.
	/// </summary>
	private SheepBank sceneController;
	
	// Use this for initialization
	void Start () {
		currentPageIndex = 0;
        confirmationWindow.gameObject.SetActiveRecursively(false);

		var controller = GameObject.FindGameObjectWithTag("GameController");
		sceneController = controller.GetComponent<SheepBank>();

        OnUpgrade_Event += Handle_OnUpgrade_Event;
	}
	
	internal void ReInitializeData() {
		currentPageIndex = 0;

		CalculateObjectsToDisplay();
	}
	
	// Update is called once per frame
	void Update () {
		if(_isInitialize == false)
			InitailizeDataFields();
	}

	private int eel_sushi_id;
	private int fattyTuna_sushi_id;
	private int octopus_sushi_id;
	private int prawn_sushi_id;
	private int salmon_sushi_id;
	private int skipjackTuna_sushi_id;
	private int spicyShell_sushi_id;
	private int sweetenedEgg_sushi_id;
	private int roe_maki_id;
	private int flyingFishRoe_maki_id;
	private int PicklesFilled_maki_id;
    private int miso_soup_id;
    private int kimji_id;
    private int Bean_ice_jam_on_crunching_id;
    private int GreenTea_icecream_id;
    private int ramen_id;
    private int zaru_soba_id;
    private int yaki_soba_id;
    private int tempura_id;
    private int curry_with_rice_id;

	private void InitailizeDataFields() {
		if(upgradeInsideSprite2D[0,0] == null) 
		{
			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					upgradeInsideSprite2D[i, j] = upgradeInsideObj2D[i, j].GetComponent<tk2dSprite>();
					upgradeInside_PriceTextmesh[i, j] = upgradeInsideObj2D[i, j].transform.GetComponentInChildren<tk2dTextMesh>();
					
					upgradeButton_Sprites[i,j] = upgradeButton_Objs[i,j].GetComponent<tk2dSprite>();
				}
			}
		}	

		eel_sushi_id = (int)GoodDataStore.FoodMenuList.Eel_sushi;
		fattyTuna_sushi_id = (int)GoodDataStore.FoodMenuList.Fatty_tuna_sushi;
		octopus_sushi_id = (int)GoodDataStore.FoodMenuList.Octopus_sushi;
        prawn_sushi_id = (int)GoodDataStore.FoodMenuList.Prawn_sushi;
        salmon_sushi_id = (int)GoodDataStore.FoodMenuList.Salmon_sushi;
        skipjackTuna_sushi_id = (int)GoodDataStore.FoodMenuList.Skipjack_tuna_sushi;
        spicyShell_sushi_id = (int)GoodDataStore.FoodMenuList.Spicy_shell_sushi;
        sweetenedEgg_sushi_id = (int)GoodDataStore.FoodMenuList.Sweetened_egg_sushi;

		roe_maki_id = (int)GoodDataStore.FoodMenuList.Roe_maki;
		flyingFishRoe_maki_id = (int)GoodDataStore.FoodMenuList.Prawn_brown_maki;
		PicklesFilled_maki_id = (int)GoodDataStore.FoodMenuList.Pickling_cucumber_filled_maki;

        ramen_id = (int)GoodDataStore.FoodMenuList.Ramen;
        zaru_soba_id = (int)GoodDataStore.FoodMenuList.Zaru_soba;
        yaki_soba_id = (int)GoodDataStore.FoodMenuList.Yaki_soba;
        tempura_id = (int)GoodDataStore.FoodMenuList.Tempura;
        curry_with_rice_id = (int)GoodDataStore.FoodMenuList.Curry_with_rice;

        miso_soup_id = (int)GoodDataStore.FoodMenuList.Miso_soup;
        kimji_id = (int)GoodDataStore.FoodMenuList.Kimji;
        Bean_ice_jam_on_crunching_id = (int)GoodDataStore.FoodMenuList.Bean_ice_jam_on_crunching;
        GreenTea_icecream_id = (int)GoodDataStore.FoodMenuList.GreenTea_icecream;
		
		_isInitialize  = true;

        Debug.Log("UpgradeInsideManager._isInitialize == " + _isInitialize);
	}
	
	public void GotoNextPage() {
	    foreach(GameObject obj in upgradeInsideObj2D) {
            obj.animation.Play();
        }
		
		if(currentPageIndex < MAX_PAGE_NUMBER - 1)	currentPageIndex += 1;
		else currentPageIndex = 0;
		
        CalculateObjectsToDisplay();
	}
	
	public void BackToPreviousPage() {		
	    foreach(GameObject obj in upgradeInsideObj2D) {
            obj.animation.Play();
        }

        if(currentPageIndex > 0)             
			currentPageIndex -= 1;
		else
			currentPageIndex = MAX_PAGE_NUMBER - 1;
		
		CalculateObjectsToDisplay();
	}

    private void CalculateObjectsToDisplay()
    {
        if(currentPageIndex == 0)
        {
            #region <@-- Page index == 0

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    string nameSpecify = firstPage_spriteNames[i, j];
                    upgradeInsideSprite2D[i, j].spriteId = upgradeInsideSprite2D[i, j].GetSpriteIdByName(nameSpecify);
                    upgradeInsideSprite2D[i, j].color = Color.white;

                    upgradeInside_PriceTextmesh[i, j].text = firstPage_prices[i, j].ToString();
                    upgradeInside_PriceTextmesh[i, j].Commit();
					
					upgradeButton_Objs[i,j].active = true;
					upgradeButton_Sprites[i,j].spriteId = ActiveUpgradeButtonID;
                }
            }			
			
			if(SushiShop.NumberOfCansellItem.Contains(eel_sushi_id) || Mz_StorageManage.AccountBalance < firstPage_prices[0,0]) {
				upgradeButton_Sprites[0,0].spriteId = UnActiveUpgradeButtonID;

                if(SushiShop.NumberOfCansellItem.Contains(eel_sushi_id)) {
                    upgradeInsideSprite2D[0, 0].color = Color.grey;
					upgradeButton_Objs[0,0].SetActiveRecursively(false);
				}
			}
			if(SushiShop.NumberOfCansellItem.Contains(fattyTuna_sushi_id) || Mz_StorageManage.AccountBalance < firstPage_prices[0,1]) {
				upgradeButton_Sprites[0,1].spriteId = UnActiveUpgradeButtonID;

                if(SushiShop.NumberOfCansellItem.Contains(fattyTuna_sushi_id)) {
                    upgradeInsideSprite2D[0, 1].color = Color.grey;
                    upgradeButton_Objs[0, 1].SetActiveRecursively(false);
				}
			}
			if(SushiShop.NumberOfCansellItem.Contains(octopus_sushi_id) || Mz_StorageManage.AccountBalance < firstPage_prices[0,2]) {
				upgradeButton_Sprites[0,2].spriteId = UnActiveUpgradeButtonID;

                if(SushiShop.NumberOfCansellItem.Contains(octopus_sushi_id)) {
                    upgradeInsideSprite2D[0, 2].color = Color.grey;
					upgradeButton_Objs[0,2].SetActiveRecursively(false);
				}
			}
			if(SushiShop.NumberOfCansellItem.Contains(prawn_sushi_id) || Mz_StorageManage.AccountBalance < firstPage_prices[0,3]) {
				upgradeButton_Sprites[0,3].spriteId = UnActiveUpgradeButtonID;

                if (SushiShop.NumberOfCansellItem.Contains(prawn_sushi_id)) {
                    upgradeInsideSprite2D[0, 3].color = Color.grey;
					upgradeButton_Objs[0,3].SetActiveRecursively(false);
				}
			}
			if(SushiShop.NumberOfCansellItem.Contains(salmon_sushi_id) || Mz_StorageManage.AccountBalance < firstPage_prices[1,0]) {
				upgradeButton_Sprites[1,0].spriteId = UnActiveUpgradeButtonID;

                if (SushiShop.NumberOfCansellItem.Contains(salmon_sushi_id)) {
                    upgradeInsideSprite2D[1, 0].color = Color.grey;
					upgradeButton_Objs[1,0].SetActiveRecursively(false);
				}
			}
			if(SushiShop.NumberOfCansellItem.Contains(skipjackTuna_sushi_id) || Mz_StorageManage.AccountBalance < firstPage_prices[1,1]) {
				upgradeButton_Sprites[1,1].spriteId = UnActiveUpgradeButtonID;

                if (SushiShop.NumberOfCansellItem.Contains(skipjackTuna_sushi_id)) {
                    upgradeInsideSprite2D[1, 1].color = Color.grey;
					upgradeButton_Objs[1,1].SetActiveRecursively(false);
				}
			}
			if(SushiShop.NumberOfCansellItem.Contains(spicyShell_sushi_id) || Mz_StorageManage.AccountBalance < firstPage_prices[1,2]) {
				upgradeButton_Sprites[1,2].spriteId = UnActiveUpgradeButtonID;

                if (SushiShop.NumberOfCansellItem.Contains(spicyShell_sushi_id)) {
                    upgradeInsideSprite2D[1, 2].color = Color.grey;
					upgradeButton_Objs[1,2].SetActiveRecursively(false);
				}
			}
			if(SushiShop.NumberOfCansellItem.Contains(sweetenedEgg_sushi_id) || Mz_StorageManage.AccountBalance < firstPage_prices[1,3])	{
				upgradeButton_Sprites[1,3].spriteId = UnActiveUpgradeButtonID;

                if (SushiShop.NumberOfCansellItem.Contains(sweetenedEgg_sushi_id)) {
                    upgradeInsideSprite2D[1, 3].color = Color.grey;
					upgradeButton_Objs[1,3].SetActiveRecursively(false);
				}
            }

            #endregion
        }
        else if(currentPageIndex == 1)
        {
            #region <@-- Page index = 1;

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    string nameSpecify = secondPage_spriteNames[i, j];
                    upgradeInsideSprite2D[i, j].spriteId = upgradeInsideSprite2D[i, j].GetSpriteIdByName(nameSpecify);
                    upgradeInsideSprite2D[i, j].color = Color.white;

                    upgradeInside_PriceTextmesh[i, j].text = secondPage_prices[i, j].ToString();
                    upgradeInside_PriceTextmesh[i, j].Commit();
					
					upgradeButton_Objs[i,j].active = true;
					upgradeButton_Sprites[i,j].spriteId = ActiveUpgradeButtonID;
                }
            }
			
			if(SushiShop.NumberOfCansellItem.Contains(roe_maki_id) || Mz_StorageManage.AccountBalance < secondPage_prices[0,0]) {
				upgradeButton_Sprites[0,0].spriteId = UnActiveUpgradeButtonID;

                if (SushiShop.NumberOfCansellItem.Contains(roe_maki_id)) {
                    upgradeInsideSprite2D[0, 0].color = Color.grey;
					upgradeButton_Objs[0,0].SetActiveRecursively(false);
				}
			}
			if(SushiShop.NumberOfCansellItem.Contains(flyingFishRoe_maki_id) || Mz_StorageManage.AccountBalance < secondPage_prices[0,1]) {
				upgradeButton_Sprites[0,1].spriteId = UnActiveUpgradeButtonID;

                if (SushiShop.NumberOfCansellItem.Contains(flyingFishRoe_maki_id)) {
                    upgradeInsideSprite2D[0, 1].color = Color.grey;
					upgradeButton_Objs[0,1].SetActiveRecursively(false);
				}
			}
			if(SushiShop.NumberOfCansellItem.Contains(PicklesFilled_maki_id) || Mz_StorageManage.AccountBalance < secondPage_prices[0,2]) {
				upgradeButton_Sprites[0,2].spriteId = UnActiveUpgradeButtonID;

                if (SushiShop.NumberOfCansellItem.Contains(PicklesFilled_maki_id)) {
                    upgradeInsideSprite2D[0, 2].color = Color.grey;
					upgradeButton_Objs[0,2].SetActiveRecursively(false);
				}
			}
			if(SushiShop.NumberOfCansellItem.Contains(13) || Mz_StorageManage.AccountBalance < secondPage_prices[0,3]) {
				upgradeButton_Sprites[0,3].spriteId = UnActiveUpgradeButtonID;
				
				if (SushiShop.NumberOfCansellItem.Contains(13)) {
					upgradeInsideSprite2D[0,3].color = Color.grey;
					upgradeButton_Objs[0,3].SetActiveRecursively(false);
				}
			}
			if(SushiShop.NumberOfCansellItem.Contains(14) || Mz_StorageManage.AccountBalance < secondPage_prices[1,0]) {
				upgradeButton_Sprites[1,0].spriteId = UnActiveUpgradeButtonID;

                if (SushiShop.NumberOfCansellItem.Contains(14)) {
                    upgradeInsideSprite2D[1, 0].color = Color.grey;
					upgradeButton_Objs[1,0].SetActiveRecursively(false);
				}
			}
			if(SushiShop.NumberOfCansellItem.Contains(15) || Mz_StorageManage.AccountBalance < secondPage_prices[1,1])	{
				upgradeButton_Sprites[1,1].spriteId = UnActiveUpgradeButtonID;

                if (SushiShop.NumberOfCansellItem.Contains(15)) {
                    upgradeInsideSprite2D[1, 1].color = Color.grey;
					upgradeButton_Objs[1,1].SetActiveRecursively(false);
				}
			}
			if(SushiShop.NumberOfCansellItem.Contains(16) || Mz_StorageManage.AccountBalance < secondPage_prices[1,2]) {
				upgradeButton_Sprites[1,2].spriteId = UnActiveUpgradeButtonID;

                if (SushiShop.NumberOfCansellItem.Contains(16)) {
                    upgradeInsideSprite2D[1, 2].color = Color.grey;
					upgradeButton_Objs[1,2].SetActiveRecursively(false);
				}
			}
			if(SushiShop.NumberOfCansellItem.Contains(17) || Mz_StorageManage.AccountBalance < secondPage_prices[1,3]) {
				upgradeButton_Sprites[1,3].spriteId = UnActiveUpgradeButtonID;

                if (SushiShop.NumberOfCansellItem.Contains(17)) {
                    upgradeInsideSprite2D[1, 3].color = Color.grey;
					upgradeButton_Objs[1,3].SetActiveRecursively(false);
				}
            }

            #endregion
        }
        else if(currentPageIndex == 2)
        {
            #region <@-- Page index == 2

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    string nameSpecify = thirdPage_spriteNames[i, j];
					if(nameSpecify != "") {
                    	upgradeInsideSprite2D[i, j].spriteId = upgradeInsideSprite2D[i, j].GetSpriteIdByName(nameSpecify);
                        upgradeInsideSprite2D[i, j].color = Color.white;

                    	upgradeInside_PriceTextmesh[i, j].text = thirdPage_prices[i, j].ToString();
                    	upgradeInside_PriceTextmesh[i, j].Commit();
						
						upgradeButton_Objs[i,j].active = true;
						upgradeButton_Sprites[i,j].spriteId = ActiveUpgradeButtonID;
					}
					else {
						tk2dSprite sprite = upgradeInsideSprite2D[i,j];
						tk2dTextMesh textmesh = upgradeInside_PriceTextmesh[i, j];
						sprite.spriteId = 27;
						textmesh.text = "none";
						textmesh.transform.localPosition = new Vector3(1, textmesh.transform.localPosition.y, textmesh.transform.localPosition.z);
						textmesh.Commit();
						
						upgradeButton_Objs[i,j].active = false;
					}
                }
            }
			
			if(SushiShop.NumberOfCansellItem.Contains(miso_soup_id) || Mz_StorageManage.AccountBalance < thirdPage_prices[0,0]) {
				upgradeButton_Sprites[0,0].spriteId = UnActiveUpgradeButtonID;

                if (SushiShop.NumberOfCansellItem.Contains(miso_soup_id)) {
                    upgradeInsideSprite2D[0, 0].color = Color.grey;
					upgradeButton_Objs[0,0].SetActiveRecursively(false);
				}
			}
			if(SushiShop.NumberOfCansellItem.Contains(kimji_id) || Mz_StorageManage.AccountBalance < thirdPage_prices[0,1]) {
				upgradeButton_Sprites[0,1].spriteId = UnActiveUpgradeButtonID;

                if (SushiShop.NumberOfCansellItem.Contains(kimji_id)) {
                    upgradeInsideSprite2D[0, 1].color = Color.grey;
					upgradeButton_Objs[0,1].SetActiveRecursively(false);
				}
			}
			if(SushiShop.NumberOfCansellItem.Contains(Bean_ice_jam_on_crunching_id) || Mz_StorageManage.AccountBalance < thirdPage_prices[0,2]) {
				upgradeButton_Sprites[0,2].spriteId = UnActiveUpgradeButtonID;

                if (SushiShop.NumberOfCansellItem.Contains(Bean_ice_jam_on_crunching_id)) {
                    upgradeInsideSprite2D[0, 2].color = Color.grey;
					upgradeButton_Objs[0,2].SetActiveRecursively(false);
				}
			}
			if(SushiShop.NumberOfCansellItem.Contains(GreenTea_icecream_id) || Mz_StorageManage.AccountBalance < thirdPage_prices[0,3]) {
				upgradeButton_Sprites[0,3].spriteId = UnActiveUpgradeButtonID;

                if (SushiShop.NumberOfCansellItem.Contains(GreenTea_icecream_id)) {
                    upgradeInsideSprite2D[0, 3].color = Color.grey;
					upgradeButton_Objs[0,3].SetActiveRecursively(false);
				}
			}
            //if(SushiShop.NumberOfCansellItem.Contains(27) || Mz_StorageManage.AccountBalance < thirdPage_prices[1,0]) {
            //    upgradeButton_Sprites[1,0].spriteId = UnActiveUpgradeButtonID;

            //    if (SushiShop.NumberOfCansellItem.Contains(27)) {
            //        upgradeInsideSprite2D[1, 0].color = Color.grey;
            //        upgradeButton_Objs[1,0].SetActiveRecursively(false);
            //    }
            //}
            //if(SushiShop.NumberOfCansellItem.Contains(29) || Mz_StorageManage.AccountBalance < thirdPage_prices[1,1]) {
            //    upgradeButton_Sprites[1,1].spriteId = UnActiveUpgradeButtonID;

            //    if (SushiShop.NumberOfCansellItem.Contains(29)) {
            //        upgradeInsideSprite2D[1, 1].color = Color.grey;
            //        upgradeButton_Objs[1,1].SetActiveRecursively(false);
            //    }
            //}

            #endregion
        }

        int temp_pageID = currentPageIndex + 1;
        displayCurrentPageID_Textmesh.text = temp_pageID + "/" + MAX_PAGE_NUMBER;
        displayCurrentPageID_Textmesh.Commit();
    }

    internal void BuyingUpgradeMechanism(string upgradeName) {
        if(currentPageIndex == 0) 
		{
			#region <!-- page 0, low 0.
			
            if(upgradeName == "upgrade_00")
            {
                #region <@-- "buying : Eel_sushi".

                if (Mz_StorageManage.AccountBalance >= firstPage_prices[0,0]) 
				{
					if(SushiShop.NumberOfCansellItem.Contains(eel_sushi_id) == false) {
                        Debug.Log("buying : Eel_sushi");
                        currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 0, J = 0, Item_id = eel_sushi_id, };
                        this.ActiveComfirmationWindow();
						
						//<@-- Handle buying upgrade tutor.
                        if (MainMenu._HasNewGameEvent) {
                            sceneController.SetActivateTotorObject(false);
                        }
                    }
					else{
						this.PlaySoundWarning();
					}
                }
				else {
					print(Message_Warning_NotEnoughMoney);
					sceneController.audioEffect.PlayOnecWithOutStop(sceneController.audioEffect.wrong_Clip);
                }

                #endregion
            }
            else if(upgradeName == "upgrade_01")
            {
                #region <@-- "buying : Fatty_tuna_sushi"

                if (Mz_StorageManage.AccountBalance >= firstPage_prices[0,1])
                {								
					if(SushiShop.NumberOfCansellItem.Contains(fattyTuna_sushi_id) == false) {
                        Debug.Log("buying : Fatty_tuna_sushi");
                        currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 0, J = 0, Item_id = fattyTuna_sushi_id, };
                        this.ActiveComfirmationWindow();
                    }
					else{
						this.PlaySoundWarning();
					}
                }
				else {
					print(Message_Warning_NotEnoughMoney);
					sceneController.audioEffect.PlayOnecWithOutStop(sceneController.audioEffect.wrong_Clip);
                }

                #endregion
            }
			else if(upgradeName == "upgrade_02")
            {
				#region <@-- buying : Octopus_sushi.

                if (Mz_StorageManage.AccountBalance >= firstPage_prices[0,2])
				{							
					if(SushiShop.NumberOfCansellItem.Contains(octopus_sushi_id) == false) {
						Debug.Log("buying : Octopus_sushi");
						currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 0, J = 0, Item_id = octopus_sushi_id, };
						this.ActiveComfirmationWindow();
					}
					else{
						this.PlaySoundWarning();
					}
				}
				else {
					print(Message_Warning_NotEnoughMoney);
					sceneController.audioEffect.PlayOnecWithOutStop(sceneController.audioEffect.wrong_Clip);
                }

                #endregion
            }
			else if(upgradeName == "upgrade_03")
            {
				#region <@-- "buying : Prawn_sushi".

                if (Mz_StorageManage.AccountBalance >= firstPage_prices[0, 3]) 
                {					
					if(SushiShop.NumberOfCansellItem.Contains(prawn_sushi_id) == false) {
						Debug.Log("buying : Prawn_sushi");
						currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 0, J = 0, Item_id = prawn_sushi_id, };
						this.ActiveComfirmationWindow();
					}
					else{
						this.PlaySoundWarning();
					}
				}
				else {
					print (Message_Warning_NotEnoughMoney);
					sceneController.audioEffect.PlayOnecWithOutStop(sceneController.audioEffect.wrong_Clip);
                }

                #endregion
            }
			
			#endregion 

			#region <!-- page 0, Low 1.
			
			else if(upgradeName == "upgrade_10") 
			{	
				#region <@-- "buying : Salmon_sushi".

                if (Mz_StorageManage.AccountBalance >= firstPage_prices[1, 0]) {					
					if(SushiShop.NumberOfCansellItem.Contains(salmon_sushi_id) == false) {
						Debug.Log("buying : Salmon_sushi");
						currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 0, J = 0, Item_id = salmon_sushi_id, };
						this.ActiveComfirmationWindow();
					}
					else{
						this.PlaySoundWarning();
					}
                }
                else {
					print(Message_Warning_NotEnoughMoney);
                    sceneController.audioEffect.PlayOnecWithOutStop(sceneController.audioEffect.wrong_Clip);
                }

				#endregion
			}
			else if(upgradeName == "upgrade_11") 
			{		
				#region <@-- "buying : Skipjack_tuna_sushi".
				
				if(Mz_StorageManage.AccountBalance >= firstPage_prices[1, 1]) 
                {
                    if (SushiShop.NumberOfCansellItem.Contains(skipjackTuna_sushi_id) == false)
                    {
						Debug.Log("buying : Skipjack_tuna_sushi");
                        this.currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 1, J = 1, Item_id = skipjackTuna_sushi_id, };
                        this.ActiveComfirmationWindow();
					}
					else{
						this.PlaySoundWarning();
					}
				}
				else {
					print(Message_Warning_NotEnoughMoney);
					sceneController.audioEffect.PlayOnecWithOutStop(sceneController.audioEffect.wrong_Clip);
				}
				
				#endregion
			}
			else if(upgradeName == "upgrade_12")
			{				
				#region <@-- "buying : Spicy_shell_sushi".

				if(Mz_StorageManage.AccountBalance >= firstPage_prices[1, 2])
                {
					if(SushiShop.NumberOfCansellItem.Contains(spicyShell_sushi_id) == false) {
						Debug.Log("buying : Spicy_shell_sushi");
                        this.currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 1, J = 2, Item_id = spicyShell_sushi_id };
                        this.ActiveComfirmationWindow();
					}
					else {
						this.PlaySoundWarning();
					}
				}
				else {
					print(Message_Warning_NotEnoughMoney);
					sceneController.audioEffect.PlayOnecWithOutStop(sceneController.audioEffect.wrong_Clip);
				}

				#endregion
			}
			else if(upgradeName == "upgrade_13") 
			{
				#region <@-- "buying : Sweetened_egg_sushi".

				if(Mz_StorageManage.AccountBalance >= firstPage_prices[1, 3]) 
                {
					if(SushiShop.NumberOfCansellItem.Contains(sweetenedEgg_sushi_id) == false) {
						Debug.Log("buying : Sweetened_egg_sushi");
                        this.currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 1, J = 3, Item_id = sweetenedEgg_sushi_id };
                        this.ActiveComfirmationWindow();
					}
					else {
						this.PlaySoundWarning();
					}
				}
				else {
					print(Message_Warning_NotEnoughMoney);
					sceneController.audioEffect.PlayOnecWithOutStop(sceneController.audioEffect.wrong_Clip);
				}

				#endregion
			}
			
			#endregion
        }
		else if(currentPageIndex == 1)
		{
			#region <!-- Page 1 low 0.
			
			if(upgradeName == "upgrade_00")
			{	
				#region <@-- "buying : Roe_maki".

                if(Mz_StorageManage.AccountBalance >= secondPage_prices[0,0]) 
                {
					if(SushiShop.NumberOfCansellItem.Contains(roe_maki_id) == false) {
						Debug.Log("buying : Roe_maki");
						this.currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 0, J = 0, Item_id = roe_maki_id };
						this.ActiveComfirmationWindow();
					}
					else
						PlaySoundWarning();
                }
				else {
					print(Message_Warning_NotEnoughMoney);
					sceneController.audioEffect.PlayOnecWithOutStop(sceneController.audioEffect.wrong_Clip);
				}

				#endregion
			}
			else if(upgradeName == "upgrade_01") 
			{
				#region <@-- "buying : Prawn_brown_maki".

                if(Mz_StorageManage.AccountBalance >= secondPage_prices[0,1])
                {
					if(SushiShop.NumberOfCansellItem.Contains(flyingFishRoe_maki_id) == false) {
						Debug.Log("buying : Prawn_brown_maki");
                        this.currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 0, J = 1, Item_id = flyingFishRoe_maki_id };
                        this.ActiveComfirmationWindow();
					}
					else
						PlaySoundWarning();
                }
				else {
					print(Message_Warning_NotEnoughMoney);
					sceneController.audioEffect.PlayOnecWithOutStop(sceneController.audioEffect.wrong_Clip);
				}

				#endregion
			}
			else if(upgradeName == "upgrade_02") 
			{
				#region <@-- "buying : Pickling_cucumber_filled_maki".

				if(Mz_StorageManage.AccountBalance >= secondPage_prices[0,2]) 
                {
					if(SushiShop.NumberOfCansellItem.Contains(PicklesFilled_maki_id) == false) {
						Debug.Log("buying : Pickling_cucumber_filled_maki");
                        this.currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 0, J = 2, Item_id = PicklesFilled_maki_id };
                        this.ActiveComfirmationWindow();
					}
					else
						PlaySoundWarning();
				}
				else {
					print(Message_Warning_NotEnoughMoney);
					sceneController.audioEffect.PlayOnecWithOutStop(sceneController.audioEffect.wrong_Clip);
				}

				#endregion
			}
			else if(upgradeName == "upgrade_03") 
			{
				#region <@-- "buying : Ramen".

                if (Mz_StorageManage.AccountBalance >= secondPage_prices[0, 3])
                {
					if(SushiShop.NumberOfCansellItem.Contains(ramen_id) == false) {
						Debug.Log("buying : Ramen");
                        this.currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 0, J = 2, Item_id = ramen_id };
                        this.ActiveComfirmationWindow();
					}
					else
						PlaySoundWarning();
                }
                else
                {
					print(Message_Warning_NotEnoughMoney);
                    sceneController.audioEffect.PlayOnecWithOutStop(sceneController.audioEffect.wrong_Clip);
                }

				#endregion
			}
			
			#endregion

			#region <!-- page1 low 1.

			if(upgradeName == "upgrade_10")
			{				
				#region <@-- "buying : Zaru_soba".

				if(Mz_StorageManage.AccountBalance >= secondPage_prices[1,0]) 
                {
					if(SushiShop.NumberOfCansellItem.Contains(zaru_soba_id) == false) {
						Debug.Log("buying : Zaru_soba");
                        this.currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 1, J = 0, Item_id = zaru_soba_id };
                        this.ActiveComfirmationWindow();
					}
					else 
						PlaySoundWarning();
				}
				else {
                    print(Message_Warning_NotEnoughMoney);
					sceneController.audioEffect.PlayOnecWithOutStop(sceneController.audioEffect.wrong_Clip);
				}

				#endregion
			}
			else if(upgradeName == "upgrade_11")
			{				
				#region <@-- "buying : Yaki_soba".

				if(Mz_StorageManage.AccountBalance >= secondPage_prices[1,1])
                {		
					if(SushiShop.NumberOfCansellItem.Contains(yaki_soba_id) == false) {
						Debug.Log("buying : Yaki_soba");
                        currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 1, J = 1, Item_id = yaki_soba_id };
                        ActiveComfirmationWindow();
					}
					else 
						PlaySoundWarning();
				}
				else {
                    print(Message_Warning_NotEnoughMoney);
					sceneController.audioEffect.PlayOnecWithOutStop(sceneController.audioEffect.wrong_Clip);
				}

				#endregion
			}
			else if(upgradeName == "upgrade_12")
			{				
				#region <@-- "buying : Tempura".

				if(Mz_StorageManage.AccountBalance >= secondPage_prices[1,2]) 
                {
					if(SushiShop.NumberOfCansellItem.Contains(tempura_id) == false) {
						Debug.Log("buying : Tempura");
                        currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 1, J = 2, Item_id = tempura_id };
                        ActiveComfirmationWindow();
                    }
					else
						PlaySoundWarning();
				}
				else {
                    print(Message_Warning_NotEnoughMoney);
					sceneController.audioEffect.PlayOnecWithOutStop(sceneController.audioEffect.wrong_Clip);
				}

				#endregion
			}
			else if(upgradeName == "upgrade_13")
			{				
				#region <@-- "buying : Curry_with_rice".

				if(Mz_StorageManage.AccountBalance >= secondPage_prices[1,3]) 
				{
					if(SushiShop.NumberOfCansellItem.Contains(curry_with_rice_id) == false) {
						Debug.Log("buying : Curry_with_rice");
                        currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 1, J = 3, Item_id = curry_with_rice_id };
                        ActiveComfirmationWindow();
					}
					else
						PlaySoundWarning();
                }
				else {
                    print(Message_Warning_NotEnoughMoney);
					sceneController.audioEffect.PlayOnecWithOutStop(sceneController.audioEffect.wrong_Clip);
				}

				#endregion
			}

			#endregion
		}
		else if(currentPageIndex == 2) 
		{
			#region <!-- page2 low 0.
			
			if(upgradeName == "upgrade_00")
			{				
				#region <@-- "buying : Miso_soup".

				if(Mz_StorageManage.AccountBalance >= thirdPage_prices[0,0]) {
					if(SushiShop.NumberOfCansellItem.Contains(miso_soup_id) == false) {
						Debug.Log("buying : Miso_soup");
                        currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 0, J = 0, Item_id = miso_soup_id };
                        ActiveComfirmationWindow();
					}
					else 
						PlaySoundWarning();
				}
				else {
                    print(Message_Warning_NotEnoughMoney);
					sceneController.audioEffect.PlayOnecWithOutStop(sceneController.audioEffect.wrong_Clip);
				}

				#endregion
			}
			else if(upgradeName == "upgrade_01")
			{		
				#region <@-- "buying : Kimji".

				if(Mz_StorageManage.AccountBalance >= thirdPage_prices[0,1]) {
					if(SushiShop.NumberOfCansellItem.Contains(kimji_id) == false) {
						Debug.Log("buying : Kimji");
                        currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 0, J = 1, Item_id = kimji_id };
                        ActiveComfirmationWindow();
					}
					else 
						PlaySoundWarning();
				}
				else {
                    print(Message_Warning_NotEnoughMoney);
					sceneController.audioEffect.PlayOnecWithOutStop(sceneController.audioEffect.wrong_Clip);
				}

				#endregion
			}
			else if(upgradeName == "upgrade_02") 
			{
				#region <@-- "buying : Bean_ice_jam_on_crunching".

				if(Mz_StorageManage.AccountBalance >= thirdPage_prices[0,2]) {
					if(SushiShop.NumberOfCansellItem.Contains(Bean_ice_jam_on_crunching_id) == false) {
						Debug.Log("buying : Bean_ice_jam_on_crunching");
                        currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 0, J = 2, Item_id = Bean_ice_jam_on_crunching_id };
                        ActiveComfirmationWindow();
					}
					else
						PlaySoundWarning();
				}
				else {
                    print(Message_Warning_NotEnoughMoney);
					sceneController.audioEffect.PlayOnecWithOutStop(sceneController.audioEffect.wrong_Clip);
				}

				#endregion
			}
			else if(upgradeName == "upgrade_03") 
			{
				#region <@-- "buying : GreenTea_icecream".

				if(Mz_StorageManage.AccountBalance >= thirdPage_prices[0,3]) 
				{
					if(SushiShop.NumberOfCansellItem.Contains(GreenTea_icecream_id) == false)  {
						Debug.Log("buying : GreenTea_icecream");
                        currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 0, J = 3, Item_id = GreenTea_icecream_id };
                        ActiveComfirmationWindow();
					}
					else
						PlaySoundWarning();
				}
				else {
                    print(Message_Warning_NotEnoughMoney);
					sceneController.audioEffect.PlayOnecWithOutStop(sceneController.audioEffect.wrong_Clip);
				}

				#endregion
			}

			#endregion

			#region <!-- page2 low 1.
/*
			if(upgradeName == "upgrade_10")
			{	
				#region <@-- "buying : butter_cookie".

				if(Mz_StorageManage.AccountBalance >= thirdPage_prices[1,0]) {
                    int id = (int)GoodDataStore.FoodMenuList.Butter_cookie;
					if(BakeryShop.NumberOfCansellItem.Contains(id) == false) {
						Debug.Log("buying : butter_cookie");
                        currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 1, J = 0, Item_id = id };
                        ActiveComfirmationWindow();
					}
					else
						PlaySoundWarning();
				}
				else {
					sceneController.audioEffect.PlayOnecWithOutStop(sceneController.audioEffect.wrong_Clip);
				}

				#endregion
			}
			else if(upgradeName == "upgrade_11") 
			{		
				#region <@-- "buying : hotdog_cheese".

				if(Mz_StorageManage.AccountBalance >= thirdPage_prices[1,1]) 
				{
                    int id = (int)GoodDataStore.FoodMenuList.HotdogWithCheese;
					if(BakeryShop.NumberOfCansellItem.Contains(id) == false) {
						Debug.Log("buying : hotdog_cheese");
                        currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 1, J = 1, Item_id = id };
                        ActiveComfirmationWindow();
					}
					else
						PlaySoundWarning();
				}
				else {
					sceneController.audioEffect.PlayOnecWithOutStop(sceneController.audioEffect.wrong_Clip);
				}

				#endregion
			}
*/
			#endregion
		}
    }

	void BuyingUpgradeComplete (OnUpdateEvenArgs e) 
    {
        if (currentPageIndex == 0)
            Mz_StorageManage.AccountBalance -= firstPage_prices[e.I, e.J];
        else if (currentPageIndex == 1)
            Mz_StorageManage.AccountBalance -= secondPage_prices[e.I, e.J];
        else if (currentPageIndex == 2)
            Mz_StorageManage.AccountBalance -= thirdPage_prices[e.I, e.J];
		
        if (e.AdditionalParams != string.Empty)
        {
            switch (e.AdditionalParams)
            {
                default:
                    break;
            }
        }

        upgradeButton_Sprites[e.I, e.J].spriteId = UnActiveUpgradeButtonID;
        SushiShop.NumberOfCansellItem.Add(e.Item_id);
		CalculateObjectsToDisplay();

		sceneController.ManageAvailabelMoneyBillBoard();
        sceneController.gameEffectManager.Create2DSpriteAnimationEffect(GameEffectManager.BLOOMSTAR_EFFECT_PATH, upgradeButton_Objs[e.I, e.J].transform);
        sceneController.audioEffect.PlayOnecWithOutStop(sceneController.audioEffect.longBring_clip);
	}

	void PlaySoundWarning ()
	{
		sceneController.audioEffect.PlayOnecWithOutStop(sceneController.audioEffect.wrong_Clip);
		Debug.LogWarning("This Item has be upgraded");
	}

    private void ActiveComfirmationWindow()
    {
        confirmationWindow.SetActiveRecursively(true);
    }

	internal void UnActiveComfirmationWindow ()
	{
    	confirmationWindow.SetActiveRecursively(false);
		currentOnUpdateTarget = null;
	}

    internal void UserComfirm()
    {
        this.OnUpgradeEvent_checkingDelegation(currentOnUpdateTarget);
    }

    private void Handle_OnUpgrade_Event(object sender, OnUpdateEvenArgs e)
    {
        this.BuyingUpgradeComplete(e);
        this.UnActiveComfirmationWindow();
    }
}
