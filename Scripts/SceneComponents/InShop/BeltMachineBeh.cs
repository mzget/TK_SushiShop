using UnityEngine;
using System.Collections;

public class BeltMachineBeh : ObjectsBeh
{
    public const string BeltMachineObjectName = "FoodBeltMachine";
    public const string CloseButtonName = "Close_button";

    public const string PATH_OF_INSTANT_FOOD = "Goods/InstantFood";
    public const string Ramen_UI = "Ramen_UI";
    public const string CurryWithRice_UI = "CurryWithRice_UI";
    public const string Tempura_UI = "Tempura_UI";
    public const string YakiSoba_UI = "YakiSoba_UI";
    public const string ZaruSoba_UI = "ZaruSoba_UI";
    
    private SushiShop sceneManager;
    public GameObject beltMachinePopup_obj;
    public Transform ramen_transform;
    public Transform curryWithRice_transform;
    public Transform tempura_transform;
    public Transform yakisoba_transform;
    public Transform zarusoba_transform;

    private GameObject foodInstance;
    private GoodsBeh food;

    private Hashtable scaleUp_hash = new Hashtable();
    private Hashtable scaleDown_hash = new Hashtable();
    private const string OnScaleDownComplete = "OnPopupClosedComplete";
	

	// Use this for initialization
	protected override void Start ()
	{
		base.Start ();

        sceneManager = baseScene.GetComponent<SushiShop>();
		
        beltMachinePopup_obj.transform.localScale = Vector3.one * 0.4f;
        beltMachinePopup_obj.SetActiveRecursively(false);

        scaleUp_hash.Add("scale", Vector3.one);
        scaleUp_hash.Add("time", 1f);

        scaleDown_hash.Add("scale", Vector3.one * 0.4f);
        scaleDown_hash.Add("time", 1f);
        scaleDown_hash.Add("easetype", iTween.EaseType.easeInSine);
        scaleDown_hash.Add("oncompletetarget", this.gameObject);
        scaleDown_hash.Add("oncomplete", OnScaleDownComplete);
	}
	
	// Update is called once per frame
//	protected override void Update ()
//	{
//		base.Update ();
//	}

    internal void ActiveBeltMachinePopup()
    {
        beltMachinePopup_obj.SetActiveRecursively(true);
		base.animatedSprite.Pause();
        iTween.ScaleTo(beltMachinePopup_obj, scaleUp_hash);
    }

    internal void DeActiveBeltMachinePopup() {
        iTween.ScaleTo(beltMachinePopup_obj, scaleDown_hash);
        sceneManager.currentGamePlayState = SushiShop.GamePlayState.Ordering;
    }

    private void OnPopupClosedComplete() {
        beltMachinePopup_obj.SetActiveRecursively(false);
		base.animatedSprite.Resume();
    }
	
	internal void HandleOnInput(ref string nameInput) {
		if(nameInput == string.Empty)
			return;


        if (nameInput == BeltMachineObjectName) {
            if(foodInstance == null)
                this.ActiveBeltMachinePopup();
			else
				return;
        }
        else if (nameInput == Ramen_UI)
        {
			this.DeActiveBeltMachinePopup();
			
			string foodName = GoodDataStore.FoodMenuList.Ramen.ToString();

            foodInstance = Instantiate(Resources.Load(PATH_OF_INSTANT_FOOD, typeof(GameObject))) as GameObject;
            foodInstance.transform.position = ramen_transform.position + Vector3.back;
			foodInstance.name = foodName;

            food = foodInstance.GetComponent<GoodsBeh>();
			food.sprite.spriteId = food.sprite.GetSpriteIdByName(foodName);
            food._canDragaable = true;
            food.GoodsBeh_putObjectOnTray_Event = food_putObjectOnTray_Event;
            food.ObjectsBeh_destroyObj_Event = new System.EventHandler(food_destroyObj_Event);
        }
        else if (nameInput == CurryWithRice_UI)
        {
            this.DeActiveBeltMachinePopup();

			string foodName = GoodDataStore.FoodMenuList.Curry_with_rice.ToString();

            foodInstance = Instantiate(Resources.Load(PATH_OF_INSTANT_FOOD, typeof(GameObject))) as GameObject;
            foodInstance.transform.position = curryWithRice_transform.position + Vector3.back;
			foodInstance.name = foodName;

            food = foodInstance.GetComponent<GoodsBeh>();
			food.sprite.spriteId = food.sprite.GetSpriteIdByName(foodName);
            food._canDragaable = true;
            food.GoodsBeh_putObjectOnTray_Event = food_putObjectOnTray_Event;
            food.ObjectsBeh_destroyObj_Event += new System.EventHandler(food_destroyObj_Event);
        }
        else if (nameInput == Tempura_UI)
        {
            this.DeActiveBeltMachinePopup();

			string foodName = GoodDataStore.FoodMenuList.Tempura.ToString();

            foodInstance = Instantiate(Resources.Load(PATH_OF_INSTANT_FOOD, typeof(GameObject))) as GameObject;
            foodInstance.transform.position = tempura_transform.position + Vector3.back;
			foodInstance.name = foodName;

            food = foodInstance.GetComponent<GoodsBeh>();
			food.sprite.spriteId = food.sprite.GetSpriteIdByName(foodName);
            food._canDragaable = true;
            food.GoodsBeh_putObjectOnTray_Event = food_putObjectOnTray_Event;
            food.ObjectsBeh_destroyObj_Event += new System.EventHandler(food_destroyObj_Event);
        }
        else if (nameInput == YakiSoba_UI)
        {
            this.DeActiveBeltMachinePopup();

			string foodName = GoodDataStore.FoodMenuList.Yaki_soba.ToString();

            foodInstance = Instantiate(Resources.Load(PATH_OF_INSTANT_FOOD, typeof(GameObject))) as GameObject;
            foodInstance.transform.position = yakisoba_transform.position + Vector3.back;
			foodInstance.name = foodName;

            food = foodInstance.GetComponent<GoodsBeh>();
			food.sprite.spriteId = food.sprite.GetSpriteIdByName(foodName);
            food._canDragaable = true;
            food.GoodsBeh_putObjectOnTray_Event = food_putObjectOnTray_Event;
            food.ObjectsBeh_destroyObj_Event += new System.EventHandler(food_destroyObj_Event);
        }
        else if (nameInput == ZaruSoba_UI)
        {
            this.DeActiveBeltMachinePopup();

			string foodName = GoodDataStore.FoodMenuList.Zaru_soba.ToString();

            foodInstance = Instantiate(Resources.Load(PATH_OF_INSTANT_FOOD, typeof(GameObject))) as GameObject;
            foodInstance.transform.position = zarusoba_transform.position + Vector3.back;
			foodInstance.name = foodName;

            food = foodInstance.GetComponent<GoodsBeh>();
			food.sprite.spriteId = food.sprite.GetSpriteIdByName(foodName);
            food._canDragaable = true;
            food.GoodsBeh_putObjectOnTray_Event = food_putObjectOnTray_Event;
            food.ObjectsBeh_destroyObj_Event += new System.EventHandler(food_destroyObj_Event);
        }
	}

    private void food_destroyObj_Event(object sender, System.EventArgs e) {
        sceneManager.foodTrayBeh.goodsOnTray_List.Remove(sender as GoodsBeh);
        sceneManager.foodTrayBeh.ReCalculatatePositionOfGoods();
    }

    private void food_putObjectOnTray_Event(object sender, GoodsBeh.PutGoodsToTrayEventArgs e) {
        GoodsBeh obj = sender as GoodsBeh;
        if (sceneManager.foodTrayBeh.goodsOnTray_List.Contains(obj) == false && sceneManager.foodTrayBeh.goodsOnTray_List.Count < FoodTrayBeh.MaxGoodsCapacity)
        {
            sceneManager.foodTrayBeh.goodsOnTray_List.Add(obj);
            sceneManager.foodTrayBeh.ReCalculatatePositionOfGoods();

            food = null;
            foodInstance = null;
        }
        else
        {
            Debug.LogWarning("Goods on tray have to max capacity.");

            obj.transform.position = obj.originalPosition;
        }
    }
}

