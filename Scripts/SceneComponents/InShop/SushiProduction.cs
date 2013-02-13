using UnityEngine;
using System;
using System.Collections;

public class SushiProduction : ObjectsBeh {
		
	public const string SushiIngredientTray = "SushiIngredientTray";
	public const string ClosePopup = "ClosePopup";
	public const string BucketOfRice = "BucketOfRice";
	public const string Alga = "Alga";
	public const string PicklingCucumber = "PicklingCucumber";
	public const string OrangeSpawn = "OrangeSpawn";
	public const string RedSpawn = "RedSpawn";

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

	public GameObject sushiPopup;
    private ObjectsBeh sushiRice;
    private Vector3 sushiRice_Pos = new Vector3(0, -44f, -1f);
	internal GameObject sushi_rice_solution;
    internal GoodsBeh sushi;
    private SushiShop sceneManager;

	// Use this for initialization
    protected override void Start()
    {
        base.Start();

        sceneManager = baseScene.GetComponent<SushiShop>();

        this.sushiPopup.gameObject.SetActiveRecursively(false);
    }
	
	// Update is called once per frame
	new void Update () {

    }

    private void Handle_SushiBeh_putObjectOnTray_Event(object sender, EventArgs e)
    {
        GoodsBeh obj = sender as GoodsBeh;
        if (sceneManager.foodTrayBeh.goodsOnTray_List.Contains(obj) == false && sceneManager.foodTrayBeh.goodsOnTray_List.Count < FoodTrayBeh.MaxGoodsCapacity)
        {
            sceneManager.foodTrayBeh.goodsOnTray_List.Add(obj);
            sceneManager.foodTrayBeh.ReCalculatatePositionOfGoods();

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
        sceneManager.foodTrayBeh.goodsOnTray_List.Remove(sender as GoodsBeh);
        sceneManager.foodTrayBeh.ReCalculatatePositionOfGoods();

		this.currentProductionState = ProductionState.None;
    }

	public void OnInput (ref string nameInput)
	{
		if(nameInput == SushiIngredientTray) {			
			this.sushiPopup.gameObject.SetActiveRecursively(true);
		}
		else if(nameInput == ClosePopup) {
			this.sushiPopup.gameObject.SetActiveRecursively(false);
		}
		else if(nameInput == BucketOfRice) {
			if(sushiRice == null && sushi_rice_solution == null && sushi == null) {
				sceneManager.choppingBlock_sprite.spriteId = sceneManager.choppingBlock_sprite.GetSpriteIdByName("choppingBlock");
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

					this.currentProductionState = ProductionState.WaitForSushiIngredient;
				};
			}
		}
		else if(nameInput == Alga) {
			if(this.currentProductionState == ProductionState.None) {
				this.currentProductionState = ProductionState.WaitForMakiIngredient;

				sceneManager.choppingBlock_sprite.spriteId = sceneManager.choppingBlock_sprite.GetSpriteIdByName("choppingBlock_maki");
			}
		}
		else if(nameInput == PicklingCucumber) {
			if(this.currentProductionState == ProductionState.WaitForMakiIngredient) {
				if(sushi_rice_solution == null && sushi == null) {
					sushi_rice_solution = Instantiate(Resources.Load("FoodSolution/" + "PicklingCucumberFilledMaki_anim", typeof(GameObject))) as GameObject;
					sushi_rice_solution.transform.position = sceneManager.choppingBlock_sprite.transform.position;
					sceneManager.choppingBlock_sprite.gameObject.active = false;

					tk2dAnimatedSprite new_maki_anim = sushi_rice_solution.GetComponent<tk2dAnimatedSprite>();
					new_maki_anim.animationCompleteDelegate = delegate(tk2dAnimatedSprite sprite, int clipId) {
						Destroy(sushi_rice_solution);
						sceneManager.choppingBlock_sprite.gameObject.active = true;
						sceneManager.choppingBlock_sprite.spriteId = sceneManager.choppingBlock_sprite.GetSpriteIdByName("choppingBlock");

						GameObject maki_product_obj = Instantiate(Resources.Load(PATH_OF_Sushi_product, typeof(GameObject))) as GameObject;
						maki_product_obj.transform.position = new Vector3(0, -44, -2);
						maki_product_obj.name = GoodDataStore.FoodMenuList.Pickling_cucumber_filled_maki.ToString();
						
						sushi = maki_product_obj.GetComponent<GoodsBeh>();
						sushi._canDragaable = true;
						sushi.sprite.spriteId = sushi.sprite.GetSpriteIdByName(GoodDataStore.FoodMenuList.Pickling_cucumber_filled_maki.ToString());
						sushi.GoodsBeh_putObjectOnTray_Event = Handle_SushiBeh_putObjectOnTray_Event;
                        sushi.ObjectsBeh_destroyObj_Event += Handle_SushiBeh_destroyObj_Event;

						this.currentProductionState = ProductionState.CompleteProduction;
					};
				}
			}
			else {
				Debug.Log(SushiShop.WarningMessageToSeeManual);
			}
		}
		else if(nameInput == OrangeSpawn) {
            if(this.currentProductionState == ProductionState.WaitForMakiIngredient) {
                if (sushi_rice_solution == null && sushi == null)
                {
                    sushi_rice_solution = Instantiate(Resources.Load("FoodSolution/" + "PrawnBrownMaki_anim", typeof(GameObject))) as GameObject;
                    sushi_rice_solution.transform.position = sceneManager.choppingBlock_sprite.transform.position;
                    sceneManager.choppingBlock_sprite.gameObject.active = false;

                    tk2dAnimatedSprite new_maki_anim = sushi_rice_solution.GetComponent<tk2dAnimatedSprite>();
                    new_maki_anim.animationCompleteDelegate = delegate(tk2dAnimatedSprite sprite, int clipId)
                    {
                        Destroy(sushi_rice_solution);
                        sceneManager.choppingBlock_sprite.gameObject.active = true;
                        sceneManager.choppingBlock_sprite.spriteId = sceneManager.choppingBlock_sprite.GetSpriteIdByName("choppingBlock");

                        GameObject maki_product_obj = Instantiate(Resources.Load(PATH_OF_Sushi_product, typeof(GameObject))) as GameObject;
                        maki_product_obj.transform.position = new Vector3(0, -44, -2);
                        maki_product_obj.name = GoodDataStore.FoodMenuList.Prawn_brown_maki.ToString();

						sushi = maki_product_obj.GetComponent<GoodsBeh>();
						sushi._canDragaable = true;
                        sushi.sprite.spriteId = sushi.sprite.GetSpriteIdByName(GoodDataStore.FoodMenuList.Prawn_brown_maki.ToString());
                        sushi.GoodsBeh_putObjectOnTray_Event = Handle_SushiBeh_putObjectOnTray_Event;
                        sushi.ObjectsBeh_destroyObj_Event += Handle_SushiBeh_destroyObj_Event;

                        this.currentProductionState = ProductionState.CompleteProduction;
                    };
                }
            }
		} 
		else if(nameInput == RedSpawn) {
            if (this.currentProductionState == ProductionState.WaitForMakiIngredient)
            {
                if (sushi_rice_solution == null && sushi == null)
                {
                    sushi_rice_solution = Instantiate(Resources.Load("FoodSolution/" + "RoeMaki_anim", typeof(GameObject))) as GameObject;
                    sushi_rice_solution.transform.position = sceneManager.choppingBlock_sprite.transform.position;
                    sceneManager.choppingBlock_sprite.gameObject.active = false;

                    tk2dAnimatedSprite new_maki_anim = sushi_rice_solution.GetComponent<tk2dAnimatedSprite>();
                    new_maki_anim.animationCompleteDelegate = delegate(tk2dAnimatedSprite sprite, int clipId)
                    {
                        Destroy(sushi_rice_solution);
                        sceneManager.choppingBlock_sprite.gameObject.active = true;
                        sceneManager.choppingBlock_sprite.spriteId = sceneManager.choppingBlock_sprite.GetSpriteIdByName("choppingBlock");

                        GameObject maki_product_obj = Instantiate(Resources.Load(PATH_OF_Sushi_product, typeof(GameObject))) as GameObject;
                        maki_product_obj.transform.position = new Vector3(0, -44, -2);
                        maki_product_obj.name = GoodDataStore.FoodMenuList.Roe_maki.ToString();

						sushi = maki_product_obj.GetComponent<GoodsBeh>();
						sushi._canDragaable = true;
                        sushi.sprite.spriteId = sushi.sprite.GetSpriteIdByName(GoodDataStore.FoodMenuList.Roe_maki.ToString());
                        sushi.GoodsBeh_putObjectOnTray_Event = Handle_SushiBeh_putObjectOnTray_Event;
                        sushi.ObjectsBeh_destroyObj_Event += Handle_SushiBeh_destroyObj_Event;

                        this.currentProductionState = ProductionState.CompleteProduction;
                    };
                }
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
						sushi._canDragaable = true;
						sushi.sprite.spriteId = sushi.sprite.GetSpriteIdByName(GoodDataStore.FoodMenuList.Crab_sushi.ToString());
		                sushi.GoodsBeh_putObjectOnTray_Event = Handle_SushiBeh_putObjectOnTray_Event;
                        sushi.ObjectsBeh_destroyObj_Event += Handle_SushiBeh_destroyObj_Event;
					};
				}
			}
            else
            {
                sceneManager.WarningPlayerToSeeManual();
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
						sushi._canDragaable = true;
						sushi.sprite.spriteId = sushi.sprite.GetSpriteIdByName(GoodDataStore.FoodMenuList.Eel_sushi.ToString());
		                sushi.GoodsBeh_putObjectOnTray_Event = Handle_SushiBeh_putObjectOnTray_Event;
                        sushi.ObjectsBeh_destroyObj_Event += Handle_SushiBeh_destroyObj_Event;
					};
				}
			}
            else
            {
                sceneManager.WarningPlayerToSeeManual();
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
						sushi._canDragaable = true;
                        sushi.sprite.spriteId = sushi.sprite.GetSpriteIdByName(GoodDataStore.FoodMenuList.Fatty_tuna_sushi.ToString());
                        sushi.GoodsBeh_putObjectOnTray_Event = Handle_SushiBeh_putObjectOnTray_Event;
                        sushi.ObjectsBeh_destroyObj_Event += Handle_SushiBeh_destroyObj_Event;
                    };
                }
            }
            else
            {
                sceneManager.WarningPlayerToSeeManual();
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
						sushi._canDragaable = true;
                        sushi.sprite.spriteId = sushi.sprite.GetSpriteIdByName(GoodDataStore.FoodMenuList.Octopus_sushi.ToString());
                        sushi.GoodsBeh_putObjectOnTray_Event = Handle_SushiBeh_putObjectOnTray_Event;
                        sushi.ObjectsBeh_destroyObj_Event += Handle_SushiBeh_destroyObj_Event;
                    };
                }
            }
            else
            {
                sceneManager.WarningPlayerToSeeManual();
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
						sushi._canDragaable = true;
                        sushi.sprite.spriteId = sushi.sprite.GetSpriteIdByName(GoodDataStore.FoodMenuList.Prawn_sushi.ToString());
                        sushi.GoodsBeh_putObjectOnTray_Event = Handle_SushiBeh_putObjectOnTray_Event;
                        sushi.ObjectsBeh_destroyObj_Event += Handle_SushiBeh_destroyObj_Event;
                    };
                }
            }
            else
            {
                sceneManager.WarningPlayerToSeeManual();
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
						sushi._canDragaable = true;
                        sushi.sprite.spriteId = sushi.sprite.GetSpriteIdByName(GoodDataStore.FoodMenuList.Salmon_sushi.ToString());
                        sushi.GoodsBeh_putObjectOnTray_Event = Handle_SushiBeh_putObjectOnTray_Event;
                        sushi.ObjectsBeh_destroyObj_Event += Handle_SushiBeh_destroyObj_Event;
                    };
                }
            }
            else
            {
                sceneManager.WarningPlayerToSeeManual();
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
						sushi._canDragaable = true;
                        sushi.sprite.spriteId = sushi.sprite.GetSpriteIdByName(GoodDataStore.FoodMenuList.Skipjack_tuna_sushi.ToString());
                        sushi.GoodsBeh_putObjectOnTray_Event = Handle_SushiBeh_putObjectOnTray_Event;
                        sushi.ObjectsBeh_destroyObj_Event += Handle_SushiBeh_destroyObj_Event;
                    };
                }
            }
            else
            {
                sceneManager.WarningPlayerToSeeManual();
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
						sushi._canDragaable = true;
                        sushi.sprite.spriteId = sushi.sprite.GetSpriteIdByName(GoodDataStore.FoodMenuList.Spicy_shell_sushi.ToString());
                        sushi.GoodsBeh_putObjectOnTray_Event = Handle_SushiBeh_putObjectOnTray_Event;
                        sushi.ObjectsBeh_destroyObj_Event += Handle_SushiBeh_destroyObj_Event;
                    };
                }
            }
            else
            {
                sceneManager.WarningPlayerToSeeManual();
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
						sushi._canDragaable = true;
                        sushi.sprite.spriteId = sushi.sprite.GetSpriteIdByName(GoodDataStore.FoodMenuList.Sweetened_egg_sushi.ToString());
                        sushi.GoodsBeh_putObjectOnTray_Event = Handle_SushiBeh_putObjectOnTray_Event;
                        sushi.ObjectsBeh_destroyObj_Event += Handle_SushiBeh_destroyObj_Event;
                    };
                }
            }
            else
            {
                sceneManager.WarningPlayerToSeeManual();
                this.sushiPopup.SetActiveRecursively(false);
            }

            #endregion
        }
	}
}
