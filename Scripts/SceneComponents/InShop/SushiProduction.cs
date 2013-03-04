using UnityEngine;
using System;
using System.Collections;

public class SushiProduction : ObjectsBeh {
		
	public const string SushiIngredientTray = "SushiIngredientTray";
	public const string ClosePopup = "ClosePopup";
	public const string BucketOfRice = "BucketOfRice";
	public const string Alga = "Alga";
	public const string Pickles = "Pickles";
	public const string FlyingFishRoe = "FlyingFishRoe";
	public const string Roe = "Roe";

	public const string Crab_sushi_face = "Crab_sushi_face";
	public const string Eel_sushi_face = "Eel_sushi_face";
	public const string Fatty_tuna_sushi_face = "Fatty_tuna_sushi_face";
	public const string Octopus_sushi_face = "Octopus_sushi_face";
	public const string Prawn_sushi_face = "Prawn_sushi_face";
	public const string Salmon_sushi_face = "Salmon_sushi_face";
	public const string Skipjack_tuna_sushi_face = "Skipjack_tuna_sushi_face";
	public const string Spicy_shell_sushi_face = "Spicy_shell_sushi_face";
	public const string Sweetened_egg_sushi_face = "Sweetened_egg_sushi_face";

    public const string Sushi_rice_anim = "Sushi_rice_anim";
	public const string CrabSushi_anim = "CrabSushi_anim";
	public const string Eel_sushi_anim = "Eel_sushi_anim";
    public const string Fatty_tuna_anim = "Fatty_tuna_anim";
    public const string Octopus_sushi_anim = "Octopus_sushi_anim";
    public const string Prawn_sushi_anim = "Prawn_sushi_anim";
    public const string Salmon_sushi_anim = "Salmon_sushi_anim";
    public const string Skipjack_tuna_sushi_anim = "Skipjack_tuna_sushi_anim";
    public const string Spicy_shell_sushi_anim = "Spicy_shell_sushi_anim";
    public const string Sweetened_egg_anim = "Sweetened_egg_anim";
    
    public const string PATH_OF_Sushi_product = "Goods/Sushi_product";
	
	public enum ProductionState {
		None = 0,
		CreateSushiRice,
		WaitForSushiIngredient,
		WaitForMakiIngredient,
        CompleteProduction,
	};
	public ProductionState currentProductionState;
	public tk2dSprite[] sushiFaces_sprite = new tk2dSprite[9];
	private string[] arr_THsushiFaceSpriteNames = new string[9] {
		"TH_Crab_sushi", "TH_Eel_sushi", "TH_Fatty_tuna_sushi",
		"TH_Octopus_sushi", "TH_Prawn_sushi", "TH_Salmon_sushi",
		"TH_Skipjack_tuna_sushi", "TH_Spicy_shell_sushi", "TH_Sweetened_egg_sushi",
	} ;
	private string[] arr_ENsushiFaceSpriteNames = new string[9] {		
		"EN_Crab_sushi", "EN_Eel_sushi", "EN_Fatty_tuna_sushi",
		"EN_Octopus_sushi", "EN_Prawn_sushi", "EN_Salmon_sushi",
		"EN_Skipjack_tuna_sushi", "EN_Spicy_shell_sushi", "EN_Sweetened_egg_sushi",
	} ;
	
	public GameObject sushiPopup;
    private ObjectsBeh sushiRice;
    private Vector3 sushiRice_Pos = new Vector3(0, -44f, -2f);
	internal GameObject sushi_rice_solution;
    internal GoodsBeh sushi;
    private SushiShop stageManager;

	// Use this for initialization
    protected override void Start()
    {
        base.Start();

        stageManager = baseScene.GetComponent<SushiShop>();

        this.sushiPopup.gameObject.SetActiveRecursively(false);
    }

	public void InitializeSushiPopupWindows ()
	{
		this.sushiPopup.SetActiveRecursively(true);

		for (int i = 0; i < sushiFaces_sprite.Length; i++) {
			if(Main.Mz_AppLanguage.appLanguage == Main.Mz_AppLanguage.SupportLanguage.EN) 
				sushiFaces_sprite[i].spriteId = sushiFaces_sprite[i].GetSpriteIdByName(arr_ENsushiFaceSpriteNames[i]);
			else if(Main.Mz_AppLanguage.appLanguage == Main.Mz_AppLanguage.SupportLanguage.TH) 
				sushiFaces_sprite[i].spriteId = sushiFaces_sprite[i].GetSpriteIdByName(arr_THsushiFaceSpriteNames[i]);
		}
		
		if(SushiShop.NumberOfCansellItem.Contains((int)GoodDataStore.FoodMenuList.Eel_sushi)) {
			var key = sushiFaces_sprite[1].transform.Find("Key");
			key.gameObject.active = false;
		}
		else {
			sushiFaces_sprite[1].gameObject.collider.enabled = false;
		}
		
		if(SushiShop.NumberOfCansellItem.Contains((int)GoodDataStore.FoodMenuList.Fatty_tuna_sushi)) {
			var key = sushiFaces_sprite[2].transform.Find("Key");
			key.gameObject.active = false;
		}
		else {
			sushiFaces_sprite[2].gameObject.collider.enabled = false;
		}
		
		if(SushiShop.NumberOfCansellItem.Contains((int)GoodDataStore.FoodMenuList.Octopus_sushi)) {
			var key = sushiFaces_sprite[3].transform.Find("Key");
			key.gameObject.active = false;
		}
		else {
			sushiFaces_sprite[3].gameObject.collider.enabled = false;
		}
		
		if(SushiShop.NumberOfCansellItem.Contains((int)GoodDataStore.FoodMenuList.Prawn_sushi)) {
			var key = sushiFaces_sprite[4].transform.Find("Key");
			key.gameObject.active = false;
		}
		else {
			sushiFaces_sprite[4].gameObject.collider.enabled = false;
		}
		
		if(SushiShop.NumberOfCansellItem.Contains((int)GoodDataStore.FoodMenuList.Salmon_sushi)) {
			var key = sushiFaces_sprite[5].transform.Find("Key");
			key.gameObject.active = false;
		}
		else {
			sushiFaces_sprite[5].gameObject.collider.enabled = false;
		}
		
		if(SushiShop.NumberOfCansellItem.Contains((int)GoodDataStore.FoodMenuList.Skipjack_tuna_sushi)) {
			var key = sushiFaces_sprite[6].transform.Find("Key");
			key.gameObject.active = false;
		}
		else {
			sushiFaces_sprite[6].gameObject.collider.enabled = false;
		}
		
		if(SushiShop.NumberOfCansellItem.Contains((int)GoodDataStore.FoodMenuList.Spicy_shell_sushi)) {
			var key = sushiFaces_sprite[7].transform.Find("Key");
			key.gameObject.active = false;
		}
		else {
			sushiFaces_sprite[7].gameObject.collider.enabled = false;
		}
		
		if(SushiShop.NumberOfCansellItem.Contains((int)GoodDataStore.FoodMenuList.Sweetened_egg_sushi)) {
			var key = sushiFaces_sprite[8].transform.Find("Key");
			key.gameObject.active = false;
		}
		else {
			sushiFaces_sprite[8].gameObject.collider.enabled = false;
		}
	}
	
	// Update is called once per frame
//	new void Update () { }

    private void Handle_SushiBeh_putObjectOnTray_Event(object sender, EventArgs e)
    {
        GoodsBeh obj = sender as GoodsBeh;
        if (stageManager.foodTrayBeh.goodsOnTray_List.Contains(obj) == false && stageManager.foodTrayBeh.goodsOnTray_List.Count < FoodTrayBeh.MaxGoodsCapacity)
        {
            stageManager.foodTrayBeh.goodsOnTray_List.Add(obj);
            stageManager.foodTrayBeh.ReCalculatatePositionOfGoods();

            sushi = null;
			this.currentProductionState = ProductionState.None;
        }
        else
        {
            Debug.LogWarning("Goods on tray have to max capacity.");

            obj.transform.position = obj.originalPosition;
        }
    }

    private void Handle_SushiBeh_destroyObj_Event(object sender, EventArgs e)
    {	
		GoodsBeh goods = sender as GoodsBeh;
		Mz_StorageManage.AvailableMoney -= goods.costs;
		stageManager.CreateDeductionsCoin (goods.costs);
		stageManager.ReFreshAvailableMoney();		
		stageManager.foodTrayBeh.goodsOnTray_List.Remove(goods);
		stageManager.foodTrayBeh.ReCalculatatePositionOfGoods();

		this.currentProductionState = ProductionState.None;
    }

	public void OnInput (ref string nameInput)
	{
		if(nameInput == SushiIngredientTray) {			
			this.InitializeSushiPopupWindows();
		}
		else if(nameInput == ClosePopup) {
			this.sushiPopup.gameObject.SetActiveRecursively(false);
		}
		else if(nameInput == BucketOfRice) {
			if(sushiRice == null && sushi_rice_solution == null && sushi == null) {
				stageManager.choppingBlock_sprite.spriteId = stageManager.choppingBlock_sprite.GetSpriteIdByName("choppingBlock");
				this.currentProductionState = ProductionState.CreateSushiRice;

				sushi_rice_solution = Instantiate(Resources.Load("FoodSolution/" + Sushi_rice_anim, typeof(GameObject))) as GameObject;
				sushi_rice_solution.transform.position = new Vector3(0, -25, -2);

				tk2dAnimatedSprite sushi_rice_anim = sushi_rice_solution.GetComponent<tk2dAnimatedSprite>();
				sushi_rice_anim.animationCompleteDelegate = delegate(tk2dAnimatedSprite sprite, int clipId) {
					Destroy(sushi_rice_solution);

                    GameObject sushiRiceInstance = Instantiate(Resources.Load("FoodSolution/Sushi_rice", typeof(GameObject))) as GameObject;
                    sushiRiceInstance.transform.position = sushiRice_Pos;

                    sushiRice = sushiRiceInstance.GetComponent<ObjectsBeh>();
                    sushiRice._canDragaable = true;
                    sushiRice.originalPosition = sushiRice_Pos;
					sushiRice.ObjectsBeh_destroyObj_Event = delegate(object sender, System.EventArgs e) {
						Destroy(sushiRice.gameObject);						
						this.currentProductionState = ProductionState.None;

						Mz_StorageManage.AvailableMoney -= 1;
						stageManager.CreateDeductionsCoin(1);
						stageManager.ReFreshAvailableMoney();
					};

					this.currentProductionState = ProductionState.WaitForSushiIngredient;
				};
				// Play sound effect.
				baseScene.audioEffect.PlayOnecSound(baseScene.soundEffect_clips[3]);
			}
		}
		else if(nameInput == Alga) {
			if(this.currentProductionState == ProductionState.None) {
				this.currentProductionState = ProductionState.WaitForMakiIngredient;

				stageManager.choppingBlock_sprite.spriteId = stageManager.choppingBlock_sprite.GetSpriteIdByName("choppingBlock_maki");
			}
		}
		else if(nameInput == Pickles) {
			if(this.currentProductionState == ProductionState.WaitForMakiIngredient) {
				if(sushi_rice_solution == null && sushi == null) {
					sushi_rice_solution = Instantiate(Resources.Load("FoodSolution/" + "PicklingCucumberFilledMaki_anim", typeof(GameObject))) as GameObject;
					sushi_rice_solution.transform.position = stageManager.choppingBlock_sprite.transform.position;
					stageManager.choppingBlock_sprite.gameObject.active = false;

					tk2dAnimatedSprite new_maki_anim = sushi_rice_solution.GetComponent<tk2dAnimatedSprite>();
					new_maki_anim.animationCompleteDelegate = delegate(tk2dAnimatedSprite sprite, int clipId) {
						Destroy(sushi_rice_solution);
						stageManager.choppingBlock_sprite.gameObject.active = true;
						stageManager.choppingBlock_sprite.spriteId = stageManager.choppingBlock_sprite.GetSpriteIdByName("choppingBlock");

						GameObject maki_product_obj = Instantiate(Resources.Load(PATH_OF_Sushi_product, typeof(GameObject))) as GameObject;
						maki_product_obj.transform.position = new Vector3(0, -44, -2);
						maki_product_obj.name = GoodDataStore.FoodMenuList.Pickling_cucumber_filled_maki.ToString();
						
						sushi = maki_product_obj.GetComponent<GoodsBeh>();
						sushi.costs = stageManager.goodDataStore.FoodDatabase_list[(int)GoodDataStore.FoodMenuList.Pickling_cucumber_filled_maki].costs;
						sushi._canDragaable = true;
						sushi.sprite.spriteId = sushi.sprite.GetSpriteIdByName(GoodDataStore.FoodMenuList.Pickling_cucumber_filled_maki.ToString());
						sushi.GoodsBeh_putObjectOnTray_Event = Handle_SushiBeh_putObjectOnTray_Event;
                        sushi.ObjectsBeh_destroyObj_Event = Handle_SushiBeh_destroyObj_Event;

						this.currentProductionState = ProductionState.CompleteProduction;
					};
					// Play sound effect.
					baseScene.audioEffect.PlayOnecSound(baseScene.soundEffect_clips[6]);
				}
			}
			else {
				stageManager.WarningPlayerToSeeManual();
			}
		}
		else if(nameInput == FlyingFishRoe) {
            if(this.currentProductionState == ProductionState.WaitForMakiIngredient) {
                if (sushi_rice_solution == null && sushi == null)
                {
                    sushi_rice_solution = Instantiate(Resources.Load("FoodSolution/" + "PrawnBrownMaki_anim", typeof(GameObject))) as GameObject;
                    sushi_rice_solution.transform.position = stageManager.choppingBlock_sprite.transform.position;
                    stageManager.choppingBlock_sprite.gameObject.active = false;

                    tk2dAnimatedSprite new_maki_anim = sushi_rice_solution.GetComponent<tk2dAnimatedSprite>();
                    new_maki_anim.animationCompleteDelegate = delegate(tk2dAnimatedSprite sprite, int clipId)
                    {
                        Destroy(sushi_rice_solution);
                        stageManager.choppingBlock_sprite.gameObject.active = true;
                        stageManager.choppingBlock_sprite.spriteId = stageManager.choppingBlock_sprite.GetSpriteIdByName("choppingBlock");

                        GameObject maki_product_obj = Instantiate(Resources.Load(PATH_OF_Sushi_product, typeof(GameObject))) as GameObject;
                        maki_product_obj.transform.position = new Vector3(0, -44, -2);
                        maki_product_obj.name = GoodDataStore.FoodMenuList.Prawn_brown_maki.ToString();

						sushi = maki_product_obj.GetComponent<GoodsBeh>();
						sushi.costs = stageManager.goodDataStore.FoodDatabase_list[(int)GoodDataStore.FoodMenuList.Prawn_brown_maki].costs;
						sushi._canDragaable = true;
                        sushi.sprite.spriteId = sushi.sprite.GetSpriteIdByName(GoodDataStore.FoodMenuList.Prawn_brown_maki.ToString());
                        sushi.GoodsBeh_putObjectOnTray_Event = Handle_SushiBeh_putObjectOnTray_Event;
                        sushi.ObjectsBeh_destroyObj_Event = Handle_SushiBeh_destroyObj_Event;

                        this.currentProductionState = ProductionState.CompleteProduction;
                    };
					// Play sound effect.
					baseScene.audioEffect.PlayOnecSound(baseScene.soundEffect_clips[6]);
                }
            }
			else {
				stageManager.WarningPlayerToSeeManual();
			}
		} 
		else if(nameInput == Roe) {
            if (this.currentProductionState == ProductionState.WaitForMakiIngredient)
            {
                if (sushi_rice_solution == null && sushi == null)
                {
                    sushi_rice_solution = Instantiate(Resources.Load("FoodSolution/" + "RoeMaki_anim", typeof(GameObject))) as GameObject;
                    sushi_rice_solution.transform.position = stageManager.choppingBlock_sprite.transform.position;
                    stageManager.choppingBlock_sprite.gameObject.active = false;

                    tk2dAnimatedSprite new_maki_anim = sushi_rice_solution.GetComponent<tk2dAnimatedSprite>();
                    new_maki_anim.animationCompleteDelegate = delegate(tk2dAnimatedSprite sprite, int clipId)
                    {
                        Destroy(sushi_rice_solution);
                        stageManager.choppingBlock_sprite.gameObject.active = true;
                        stageManager.choppingBlock_sprite.spriteId = stageManager.choppingBlock_sprite.GetSpriteIdByName("choppingBlock");

                        GameObject maki_product_obj = Instantiate(Resources.Load(PATH_OF_Sushi_product, typeof(GameObject))) as GameObject;
                        maki_product_obj.transform.position = new Vector3(0, -44, -2);
                        maki_product_obj.name = GoodDataStore.FoodMenuList.Roe_maki.ToString();

						sushi = maki_product_obj.GetComponent<GoodsBeh>();
						sushi.costs = stageManager.goodDataStore.FoodDatabase_list[(int)GoodDataStore.FoodMenuList.Roe_maki].costs;
						sushi._canDragaable = true;
                        sushi.sprite.spriteId = sushi.sprite.GetSpriteIdByName(GoodDataStore.FoodMenuList.Roe_maki.ToString());
                        sushi.GoodsBeh_putObjectOnTray_Event = Handle_SushiBeh_putObjectOnTray_Event;
                        sushi.ObjectsBeh_destroyObj_Event = Handle_SushiBeh_destroyObj_Event;

                        this.currentProductionState = ProductionState.CompleteProduction;
                    };
					// Play sound effect.
					baseScene.audioEffect.PlayOnecSound(baseScene.soundEffect_clips[6]);
                }
            }
			else {
				stageManager.WarningPlayerToSeeManual();
			}
		}

		if(nameInput == Crab_sushi_face)
        {
            #region <!-- Crab_sushi_face.

            if (this.currentProductionState == ProductionState.WaitForSushiIngredient) {
				if (sushi_rice_solution == null && sushi == null)
				{
					this.currentProductionState = ProductionState.CompleteProduction;

		            this.sushiPopup.gameObject.SetActiveRecursively(false);
					Destroy(sushiRice.gameObject);

					sushi_rice_solution = Instantiate(Resources.Load("FoodSolution/" + CrabSushi_anim, typeof(GameObject))) as GameObject;
					sushi_rice_solution.transform.position = new Vector3(0, -25, -2);

					tk2dAnimatedSprite sushi_rice_anim = sushi_rice_solution.GetComponent<tk2dAnimatedSprite>();
					sushi_rice_anim.animationCompleteDelegate = delegate(tk2dAnimatedSprite sprite, int clipId) {
						Destroy(sushi_rice_solution);

		                GameObject sushi_product_obj = Instantiate(Resources.Load(PATH_OF_Sushi_product, typeof(GameObject))) as GameObject;
		                sushi_product_obj.transform.position = new Vector3(0, -44, -2);
						sushi_product_obj.name = GoodDataStore.FoodMenuList.Crab_sushi.ToString();

						sushi = sushi_product_obj.GetComponent<GoodsBeh>();
						sushi.costs = stageManager.goodDataStore.FoodDatabase_list[(int)GoodDataStore.FoodMenuList.Crab_sushi].costs;
						sushi._canDragaable = true;
						sushi.sprite.spriteId = sushi.sprite.GetSpriteIdByName(GoodDataStore.FoodMenuList.Crab_sushi.ToString());
		                sushi.GoodsBeh_putObjectOnTray_Event = Handle_SushiBeh_putObjectOnTray_Event;
                        sushi.ObjectsBeh_destroyObj_Event = Handle_SushiBeh_destroyObj_Event;
					};
					// Play sound effect.
					baseScene.audioEffect.PlayOnecSound(baseScene.soundEffect_clips[3]);
				}
			}
            else
            {
                stageManager.WarningPlayerToSeeManual();
                this.sushiPopup.SetActiveRecursively(false);
            }

            #endregion
        }
		else if(nameInput == Eel_sushi_face)
        {
            #region <!-- Eel_sushi_face.

            if (this.currentProductionState == ProductionState.WaitForSushiIngredient) {
				if(sushi_rice_solution == null && sushi == null) {	
					this.currentProductionState = ProductionState.CompleteProduction;

                    this.sushiPopup.gameObject.SetActiveRecursively(false);
					Destroy(sushiRice.gameObject);

					sushi_rice_solution = Instantiate(Resources.Load("FoodSolution/" + Eel_sushi_anim, typeof(GameObject))) as GameObject;
					sushi_rice_solution.transform.position = new Vector3(0, -25, -2);

					tk2dAnimatedSprite sushi_rice_anim = sushi_rice_solution.GetComponent<tk2dAnimatedSprite>();
					sushi_rice_anim.animationCompleteDelegate = delegate(tk2dAnimatedSprite sprite, int clipId) {
						Destroy(sushi_rice_solution);

		                GameObject sushi_product_obj = Instantiate(Resources.Load(PATH_OF_Sushi_product, typeof(GameObject))) as GameObject;
		                sushi_product_obj.transform.position = new Vector3(0, -44, -2);
						sushi_product_obj.name = GoodDataStore.FoodMenuList.Eel_sushi.ToString();

						sushi = sushi_product_obj.GetComponent<GoodsBeh>();
						sushi.costs = stageManager.goodDataStore.FoodDatabase_list[(int)GoodDataStore.FoodMenuList.Eel_sushi].costs;
						sushi._canDragaable = true;
						sushi.sprite.spriteId = sushi.sprite.GetSpriteIdByName(GoodDataStore.FoodMenuList.Eel_sushi.ToString());
		                sushi.GoodsBeh_putObjectOnTray_Event = Handle_SushiBeh_putObjectOnTray_Event;
                        sushi.ObjectsBeh_destroyObj_Event = Handle_SushiBeh_destroyObj_Event;
					};
					// Play sound effect.
					baseScene.audioEffect.PlayOnecSound(baseScene.soundEffect_clips[3]);
				}
			}
            else
            {
                stageManager.WarningPlayerToSeeManual();
                this.sushiPopup.SetActiveRecursively(false);
            }

            #endregion
        }
        else if (nameInput == Fatty_tuna_sushi_face)
        {
            #region <!-- Fatty_tuna_sushi_face.

            if (this.currentProductionState == ProductionState.WaitForSushiIngredient)
            {
                if (sushi_rice_solution == null && sushi == null)
                {
                    this.currentProductionState = ProductionState.CompleteProduction;

                    this.sushiPopup.gameObject.SetActiveRecursively(false);
					Destroy(sushiRice.gameObject);

                    sushi_rice_solution = Instantiate(Resources.Load("FoodSolution/" + Fatty_tuna_anim, typeof(GameObject))) as GameObject;
                    sushi_rice_solution.transform.position = new Vector3(0, -25, -2);

                    tk2dAnimatedSprite sushi_rice_anim = sushi_rice_solution.GetComponent<tk2dAnimatedSprite>();
                    sushi_rice_anim.animationCompleteDelegate = delegate(tk2dAnimatedSprite sprite, int clipId)
                    {
                        Destroy(sushi_rice_solution);

                        GameObject sushi_product_obj = Instantiate(Resources.Load(PATH_OF_Sushi_product, typeof(GameObject))) as GameObject;
                        sushi_product_obj.transform.position = new Vector3(0, -44, -2);
                        sushi_product_obj.name = GoodDataStore.FoodMenuList.Fatty_tuna_sushi.ToString();

						sushi = sushi_product_obj.GetComponent<GoodsBeh>();
						sushi.costs = stageManager.goodDataStore.FoodDatabase_list[(int)GoodDataStore.FoodMenuList.Fatty_tuna_sushi].costs;
						sushi._canDragaable = true;
                        sushi.sprite.spriteId = sushi.sprite.GetSpriteIdByName(GoodDataStore.FoodMenuList.Fatty_tuna_sushi.ToString());
                        sushi.GoodsBeh_putObjectOnTray_Event = Handle_SushiBeh_putObjectOnTray_Event;
                        sushi.ObjectsBeh_destroyObj_Event = Handle_SushiBeh_destroyObj_Event;
                    };
					// Play sound effect.
					baseScene.audioEffect.PlayOnecSound(baseScene.soundEffect_clips[3]);
                }
            }
            else
            {
                stageManager.WarningPlayerToSeeManual();
                this.sushiPopup.SetActiveRecursively(false);
            }

            #endregion
        }
        else if (nameInput == Octopus_sushi_face)
        {
            #region <!-- Octopus_sushi_face.

            if (this.currentProductionState == ProductionState.WaitForSushiIngredient)
            {
                if (sushi_rice_solution == null && sushi == null)
                {
                    this.currentProductionState = ProductionState.CompleteProduction;

                    this.sushiPopup.gameObject.SetActiveRecursively(false);
					Destroy(sushiRice.gameObject);

                    sushi_rice_solution = Instantiate(Resources.Load("FoodSolution/" + Octopus_sushi_anim, typeof(GameObject))) as GameObject;
                    sushi_rice_solution.transform.position = new Vector3(0, -25, -2);

                    tk2dAnimatedSprite sushi_rice_anim = sushi_rice_solution.GetComponent<tk2dAnimatedSprite>();
                    sushi_rice_anim.animationCompleteDelegate = delegate(tk2dAnimatedSprite sprite, int clipId)
                    {
                        Destroy(sushi_rice_solution);

                        GameObject sushi_product_obj = Instantiate(Resources.Load(PATH_OF_Sushi_product, typeof(GameObject))) as GameObject;
                        sushi_product_obj.transform.position = new Vector3(0, -44, -2);
                        sushi_product_obj.name = GoodDataStore.FoodMenuList.Octopus_sushi.ToString();

						sushi = sushi_product_obj.GetComponent<GoodsBeh>();
						sushi.costs = stageManager.goodDataStore.FoodDatabase_list[(int)GoodDataStore.FoodMenuList.Octopus_sushi].costs;
						sushi._canDragaable = true;
                        sushi.sprite.spriteId = sushi.sprite.GetSpriteIdByName(GoodDataStore.FoodMenuList.Octopus_sushi.ToString());
                        sushi.GoodsBeh_putObjectOnTray_Event = Handle_SushiBeh_putObjectOnTray_Event;
                        sushi.ObjectsBeh_destroyObj_Event = Handle_SushiBeh_destroyObj_Event;
                    };
					// Play sound effect.
					baseScene.audioEffect.PlayOnecSound(baseScene.soundEffect_clips[3]);
                }
            }
            else
            {
                stageManager.WarningPlayerToSeeManual();
                this.sushiPopup.SetActiveRecursively(false);
            }

            #endregion
        }
        else if (nameInput == Prawn_sushi_face)
        {
            #region <!-- Prawn_sushi_face.

            if (this.currentProductionState == ProductionState.WaitForSushiIngredient)
            {
                if (sushi_rice_solution == null && sushi == null)
                {
                    this.currentProductionState = ProductionState.CompleteProduction;

                    this.sushiPopup.gameObject.SetActiveRecursively(false);
					Destroy(sushiRice.gameObject);

                    sushi_rice_solution = Instantiate(Resources.Load("FoodSolution/" + Prawn_sushi_anim, typeof(GameObject))) as GameObject;
                    sushi_rice_solution.transform.position = new Vector3(0, -25, -2);

                    tk2dAnimatedSprite sushi_rice_anim = sushi_rice_solution.GetComponent<tk2dAnimatedSprite>();
                    sushi_rice_anim.animationCompleteDelegate = delegate(tk2dAnimatedSprite sprite, int clipId)
                    {
                        Destroy(sushi_rice_solution);

                        GameObject sushi_product_obj = Instantiate(Resources.Load(PATH_OF_Sushi_product, typeof(GameObject))) as GameObject;
                        sushi_product_obj.transform.position = new Vector3(0, -44, -2);
                        sushi_product_obj.name = GoodDataStore.FoodMenuList.Prawn_sushi.ToString();

						sushi = sushi_product_obj.GetComponent<GoodsBeh>();
						sushi.costs = stageManager.goodDataStore.FoodDatabase_list[(int)GoodDataStore.FoodMenuList.Prawn_sushi].costs;
						sushi._canDragaable = true;
                        sushi.sprite.spriteId = sushi.sprite.GetSpriteIdByName(GoodDataStore.FoodMenuList.Prawn_sushi.ToString());
                        sushi.GoodsBeh_putObjectOnTray_Event = Handle_SushiBeh_putObjectOnTray_Event;
                        sushi.ObjectsBeh_destroyObj_Event = Handle_SushiBeh_destroyObj_Event;
                    };
					// Play sound effect.
					baseScene.audioEffect.PlayOnecSound(baseScene.soundEffect_clips[3]);
                }
            }
            else
            {
                stageManager.WarningPlayerToSeeManual();
                this.sushiPopup.SetActiveRecursively(false);
            }

            #endregion
        }
        else if (nameInput == Salmon_sushi_face)
        {
            #region <!-- Salmon_sushi_face.

            if (this.currentProductionState == ProductionState.WaitForSushiIngredient)
            {
                if (sushi_rice_solution == null && sushi == null)
                {
                    this.currentProductionState = ProductionState.CompleteProduction;

                    this.sushiPopup.gameObject.SetActiveRecursively(false);
					Destroy(sushiRice.gameObject);

                    sushi_rice_solution = Instantiate(Resources.Load("FoodSolution/" + Salmon_sushi_anim, typeof(GameObject))) as GameObject;
                    sushi_rice_solution.transform.position = new Vector3(0, -25, -2);

                    tk2dAnimatedSprite sushi_rice_anim = sushi_rice_solution.GetComponent<tk2dAnimatedSprite>();
                    sushi_rice_anim.animationCompleteDelegate = delegate(tk2dAnimatedSprite sprite, int clipId)
                    {
                        Destroy(sushi_rice_solution);

                        GameObject sushi_product_obj = Instantiate(Resources.Load(PATH_OF_Sushi_product, typeof(GameObject))) as GameObject;
                        sushi_product_obj.transform.position = new Vector3(0, -44, -2);
                        sushi_product_obj.name = GoodDataStore.FoodMenuList.Salmon_sushi.ToString();

						sushi = sushi_product_obj.GetComponent<GoodsBeh>();
						sushi.costs = stageManager.goodDataStore.FoodDatabase_list[(int)GoodDataStore.FoodMenuList.Salmon_sushi].costs;
						sushi._canDragaable = true;
                        sushi.sprite.spriteId = sushi.sprite.GetSpriteIdByName(GoodDataStore.FoodMenuList.Salmon_sushi.ToString());
                        sushi.GoodsBeh_putObjectOnTray_Event = Handle_SushiBeh_putObjectOnTray_Event;
                        sushi.ObjectsBeh_destroyObj_Event = Handle_SushiBeh_destroyObj_Event;
                    };
					// Play sound effect.
					baseScene.audioEffect.PlayOnecSound(baseScene.soundEffect_clips[3]);
                }
            }
            else
            {
                stageManager.WarningPlayerToSeeManual();
                this.sushiPopup.SetActiveRecursively(false);
            }

            #endregion
        }
        else if (nameInput == Skipjack_tuna_sushi_face)
        {
            #region <!-- Skipjack_tuna_sushi_face.

            if (this.currentProductionState == ProductionState.WaitForSushiIngredient)
            {
                if (sushi_rice_solution == null && sushi == null)
                {
                    this.currentProductionState = ProductionState.CompleteProduction;

                    this.sushiPopup.gameObject.SetActiveRecursively(false);
					Destroy(sushiRice.gameObject);

                    sushi_rice_solution = Instantiate(Resources.Load("FoodSolution/" + Skipjack_tuna_sushi_anim, typeof(GameObject))) as GameObject;
                    sushi_rice_solution.transform.position = new Vector3(0, -25, -2);

                    tk2dAnimatedSprite sushi_rice_anim = sushi_rice_solution.GetComponent<tk2dAnimatedSprite>();
                    sushi_rice_anim.animationCompleteDelegate = delegate(tk2dAnimatedSprite sprite, int clipId)
                    {
                        Destroy(sushi_rice_solution);

                        GameObject sushi_product_obj = Instantiate(Resources.Load(PATH_OF_Sushi_product, typeof(GameObject))) as GameObject;
                        sushi_product_obj.transform.position = new Vector3(0, -44, -2);
                        sushi_product_obj.name = GoodDataStore.FoodMenuList.Skipjack_tuna_sushi.ToString();

						sushi = sushi_product_obj.GetComponent<GoodsBeh>();
						sushi.costs = stageManager.goodDataStore.FoodDatabase_list[(int)GoodDataStore.FoodMenuList.Skipjack_tuna_sushi].costs;
						sushi._canDragaable = true;
                        sushi.sprite.spriteId = sushi.sprite.GetSpriteIdByName(GoodDataStore.FoodMenuList.Skipjack_tuna_sushi.ToString());
                        sushi.GoodsBeh_putObjectOnTray_Event = Handle_SushiBeh_putObjectOnTray_Event;
                        sushi.ObjectsBeh_destroyObj_Event = Handle_SushiBeh_destroyObj_Event;
                    };
					// Play sound effect.
					baseScene.audioEffect.PlayOnecSound(baseScene.soundEffect_clips[3]);
                }
            }
            else
            {
                stageManager.WarningPlayerToSeeManual();
                this.sushiPopup.SetActiveRecursively(false);
            }

            #endregion
        }
        else if (nameInput == Spicy_shell_sushi_face)
        {
            #region <!-- Spicy_shell_sushi_face.

            if (this.currentProductionState == ProductionState.WaitForSushiIngredient)
            {
                if (sushi_rice_solution == null && sushi == null)
                {
                    this.currentProductionState = ProductionState.CompleteProduction;

                    this.sushiPopup.gameObject.SetActiveRecursively(false);
					Destroy(sushiRice.gameObject);

                    sushi_rice_solution = Instantiate(Resources.Load("FoodSolution/" + Spicy_shell_sushi_anim, typeof(GameObject))) as GameObject;
                    sushi_rice_solution.transform.position = new Vector3(0, -25, -2);

                    tk2dAnimatedSprite sushi_rice_anim = sushi_rice_solution.GetComponent<tk2dAnimatedSprite>();
                    sushi_rice_anim.animationCompleteDelegate = delegate(tk2dAnimatedSprite sprite, int clipId)
                    {
                        Destroy(sushi_rice_solution);

                        GameObject sushi_product_obj = Instantiate(Resources.Load(PATH_OF_Sushi_product, typeof(GameObject))) as GameObject;
                        sushi_product_obj.transform.position = new Vector3(0, -44, -2);
                        sushi_product_obj.name = GoodDataStore.FoodMenuList.Spicy_shell_sushi.ToString();

						sushi = sushi_product_obj.GetComponent<GoodsBeh>();
						sushi.costs = stageManager.goodDataStore.FoodDatabase_list[(int)GoodDataStore.FoodMenuList.Spicy_shell_sushi].costs;
						sushi._canDragaable = true;
                        sushi.sprite.spriteId = sushi.sprite.GetSpriteIdByName(GoodDataStore.FoodMenuList.Spicy_shell_sushi.ToString());
                        sushi.GoodsBeh_putObjectOnTray_Event = Handle_SushiBeh_putObjectOnTray_Event;
                        sushi.ObjectsBeh_destroyObj_Event = Handle_SushiBeh_destroyObj_Event;
                    };
					// Play sound effect.
					baseScene.audioEffect.PlayOnecSound(baseScene.soundEffect_clips[3]);
                }
            }
            else
            {
                stageManager.WarningPlayerToSeeManual();
                this.sushiPopup.SetActiveRecursively(false);
            }

            #endregion
        }
        else if (nameInput == Sweetened_egg_sushi_face)
        {
            #region <!-- Sweetened_egg_sushi_face.

            if (this.currentProductionState == ProductionState.WaitForSushiIngredient)
            {
                if (sushi_rice_solution == null && sushi == null)
                {
                    this.currentProductionState = ProductionState.CompleteProduction;

                    this.sushiPopup.gameObject.SetActiveRecursively(false);
					Destroy(sushiRice.gameObject);

                    sushi_rice_solution = Instantiate(Resources.Load("FoodSolution/" + Sweetened_egg_anim, typeof(GameObject))) as GameObject;
                    sushi_rice_solution.transform.position = new Vector3(0, -25, -2);

                    tk2dAnimatedSprite sushi_rice_anim = sushi_rice_solution.GetComponent<tk2dAnimatedSprite>();
                    sushi_rice_anim.animationCompleteDelegate = delegate(tk2dAnimatedSprite sprite, int clipId)
                    {
                        Destroy(sushi_rice_solution);

                        GameObject sushi_product_obj = Instantiate(Resources.Load(PATH_OF_Sushi_product, typeof(GameObject))) as GameObject;
                        sushi_product_obj.transform.position = new Vector3(0, -44, -2);
                        sushi_product_obj.name = GoodDataStore.FoodMenuList.Sweetened_egg_sushi.ToString();

						sushi = sushi_product_obj.GetComponent<GoodsBeh>();
						sushi.costs = stageManager.goodDataStore.FoodDatabase_list[(int)GoodDataStore.FoodMenuList.Sweetened_egg_sushi].costs;
						sushi._canDragaable = true;
                        sushi.sprite.spriteId = sushi.sprite.GetSpriteIdByName(GoodDataStore.FoodMenuList.Sweetened_egg_sushi.ToString());
                        sushi.GoodsBeh_putObjectOnTray_Event = Handle_SushiBeh_putObjectOnTray_Event;
                        sushi.ObjectsBeh_destroyObj_Event = Handle_SushiBeh_destroyObj_Event;
                    };
					// Play sound effect.
					baseScene.audioEffect.PlayOnecSound(baseScene.soundEffect_clips[3]);
                }
            }
            else
            {
                stageManager.WarningPlayerToSeeManual();
                this.sushiPopup.SetActiveRecursively(false);
            }

            #endregion
        }
	}
}
