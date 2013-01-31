using UnityEngine;
using System;
using System.Collections;

public class SushiProduction : ObjectsBeh {
		
	public const string SushiIngredientTray = "SushiIngredientTray";
	public const string ClosePopup = "ClosePopup";
	public const string BucketOfRice = "BucketOfRice";

	public const string Crab_sushi_face = "Crab_sushi_face";
	public const string Eel_sushi_face = "Eel_sushi_face";
	public const string Fatty_tuna_sushi_face = "Fatty_tuna_sushi_face";
	public const string Octopus_sushi_face = "Octopus_sushi_face";
	public const string Prawn_sushi_face = "Prawn_sushi_face";
	public const string Salmon_sushi_face = "Salmon_sushi_face";
	public const string Skipjack_tuna_sushi_face = "Skipjack_tuna_sushi_face";
	public const string Spicy_shell_sushi_face = "Spicy_shell_sushi_face";
	public const string Sweetened_egg_sushi = "Sweetened_egg_sushi";

    public const string Sushi_rice_anim = "Sushi_rice_anim";
	public const string CrabSushi_anim = "CrabSushi_anim";


    public const string PATH_OF_Sushi_product = "Goods/Sushi_product";

	public enum ProductionState {
		None = 0,
		CreateSushiRice,
		WaitForIngredient,
        CompleteProduction,
	};
	public ProductionState currentProductionState;

	public GameObject sushiPopup;
	public GameObject sushi_rice_obj_base;
	internal GameObject sushi_rice_solution;
    internal SushiBeh sushiBeh;

	// Use this for initialization
    protected override void Start()
    {
        base.Start();

        this.sushiPopup.gameObject.SetActiveRecursively(false);
        this.sushi_rice_obj_base.gameObject.SetActiveRecursively(false);
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

            //<!-- Setting original position.
            obj.originalPosition = obj.transform.position;

            sushiBeh = null;
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
    }

	public void OnInput (ref string nameInput)
	{
		if(nameInput == "SushiIngredientTray") {			
			this.sushiPopup.gameObject.SetActiveRecursively(true);
		}
		else if(nameInput == "ClosePopup") {
			this.sushiPopup.gameObject.SetActiveRecursively(false);
		}
		else if(nameInput == "BucketOfRice") {
			if(sushi_rice_solution == null) {
				this.currentProductionState = ProductionState.CreateSushiRice;

				sushi_rice_obj_base.gameObject.SetActiveRecursively(false);

				sushi_rice_solution = Instantiate(Resources.Load("FoodSolution/" + Sushi_rice_anim, typeof(GameObject))) as GameObject;
				sushi_rice_solution.transform.position = new Vector3(0, -25, -2);
				tk2dAnimatedSprite sushi_rice_anim = sushi_rice_solution.GetComponent<tk2dAnimatedSprite>();
				sushi_rice_anim.animationCompleteDelegate = delegate(tk2dAnimatedSprite sprite, int clipId) {
					Destroy(sushi_rice_solution);
					sushi_rice_obj_base.SetActiveRecursively(true);

					this.currentProductionState = ProductionState.WaitForIngredient;
				};
			}
		}

		if(this.currentProductionState == ProductionState.WaitForIngredient) {
			if(nameInput == Crab_sushi_face) {
                if (sushi_rice_solution == null && sushiBeh == null)
                {
                    this.sushiPopup.gameObject.SetActiveRecursively(false);
                    sushi_rice_obj_base.gameObject.SetActiveRecursively(false);

					sushi_rice_solution = Instantiate(Resources.Load("FoodSolution/" + CrabSushi_anim, typeof(GameObject))) as GameObject;
					sushi_rice_solution.transform.position = new Vector3(0, -25, -2);
					tk2dAnimatedSprite sushi_rice_anim = sushi_rice_solution.GetComponent<tk2dAnimatedSprite>();
					sushi_rice_anim.animationCompleteDelegate = delegate(tk2dAnimatedSprite sprite, int clipId) {
						Destroy(sushi_rice_solution);

                        GameObject sushi_product_obj = Instantiate(Resources.Load(PATH_OF_Sushi_product, typeof(GameObject))) as GameObject;
                        sushi_product_obj.transform.position = new Vector3(0, -44, -2);
                        sushiBeh = sushi_product_obj.GetComponent<SushiBeh>();
                        sushiBeh.putObjectOnTray_Event += Handle_SushiBeh_putObjectOnTray_Event;
                        sushiBeh.destroyObj_Event += Handle_SushiBeh_destroyObj_Event;
					};
				}
			}
		}
	}
}
