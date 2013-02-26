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
    
    private SushiShop stageManager;
    public GameObject beltMachinePopup_obj;
    public Transform ramen_transform;
    public Transform curryWithRice_transform;
    public Transform tempura_transform;
    public Transform yakisoba_transform;
    public Transform zarusoba_transform;
    public GameObject ramen_lockIcon;
    public GameObject curryWithRice_lockIcon;
    public GameObject tempura_lockIcon;
    public GameObject yakisoba_lockIcon;
    public GameObject zarusoba_lockIcon;

    private GameObject foodInstance;
    private GoodsBeh food;

    private Hashtable scaleUp_hash = new Hashtable();
    private Hashtable scaleDown_hash = new Hashtable();
    private const string OnScaleDownComplete = "OnPopupClosedComplete";
	

	// Use this for initialization
	protected override void Start ()
	{
		base.Start ();

        stageManager = baseScene.GetComponent<SushiShop>();
		
        beltMachinePopup_obj.transform.localScale = Vector3.one * 0.4f;
        beltMachinePopup_obj.SetActiveRecursively(false);

        scaleUp_hash.Add("scale", Vector3.one);
        scaleUp_hash.Add("time", 1f);

        scaleDown_hash.Add("scale", Vector3.one * 0.4f);
        scaleDown_hash.Add("time", 0.5f);
        scaleDown_hash.Add("easetype", iTween.EaseType.easeInSine);
        scaleDown_hash.Add("oncompletetarget", this.gameObject);
        scaleDown_hash.Add("oncomplete", OnScaleDownComplete);
	}
	
    private void Initializing()
    {
        if (SushiShop.NumberOfCansellItem.Contains((int)GoodDataStore.FoodMenuList.Ramen)) {
            ramen_lockIcon.active = false;
            ramen_transform.collider.enabled = true;
        }
        else {
            ramen_lockIcon.active = true;
            ramen_transform.collider.enabled = false;
        }

        if (SushiShop.NumberOfCansellItem.Contains((int)GoodDataStore.FoodMenuList.Curry_with_rice)) {
            curryWithRice_lockIcon.active = false;
            curryWithRice_transform.collider.enabled = true;
        }
        else {
            curryWithRice_lockIcon.active = true;
            curryWithRice_transform.collider.enabled = false;
        }

        if (SushiShop.NumberOfCansellItem.Contains((int)GoodDataStore.FoodMenuList.Yaki_soba)) {
            yakisoba_lockIcon.active = false;
            yakisoba_transform.collider.enabled = true;
        }
        else {
            yakisoba_lockIcon.active = true;
            yakisoba_transform.collider.enabled = false;
        }

        if (SushiShop.NumberOfCansellItem.Contains((int)GoodDataStore.FoodMenuList.Zaru_soba)) {
            zarusoba_lockIcon.active = false;
            zarusoba_transform.collider.enabled = true;
        }
        else {
            zarusoba_lockIcon.active = true;
            zarusoba_transform.collider.enabled = false;
        }

        if (SushiShop.NumberOfCansellItem.Contains((int)GoodDataStore.FoodMenuList.Tempura)) {
            tempura_lockIcon.active = false;
            tempura_transform.collider.enabled = true;
        }
        else {
            tempura_lockIcon.active = true;
            tempura_transform.collider.enabled = false;
        }
    }

    internal void ActiveBeltMachinePopup()
    {
        beltMachinePopup_obj.SetActiveRecursively(true);
        this.Initializing();
		base.animatedSprite.Pause();
        iTween.ScaleTo(beltMachinePopup_obj, scaleUp_hash);
    }

    internal void DeActiveBeltMachinePopup() {
        iTween.ScaleTo(beltMachinePopup_obj, scaleDown_hash);
        stageManager.currentGamePlayState = SushiShop.GamePlayState.Ordering;
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
			food.costs = stageManager.goodDataStore.FoodDatabase_list[(int)GoodDataStore.FoodMenuList.Ramen].costs;
			food.sprite.spriteId = food.sprite.GetSpriteIdByName(foodName);
            food._canDragaable = true;
            food.GoodsBeh_putObjectOnTray_Event = food_putObjectOnTray_Event;
            food.ObjectsBeh_destroyObj_Event = food_destroyObj_Event;
        }
        else if (nameInput == CurryWithRice_UI)
        {
            this.DeActiveBeltMachinePopup();

			string foodName = GoodDataStore.FoodMenuList.Curry_with_rice.ToString();

            foodInstance = Instantiate(Resources.Load(PATH_OF_INSTANT_FOOD, typeof(GameObject))) as GameObject;
            foodInstance.transform.position = curryWithRice_transform.position + Vector3.back;
			foodInstance.name = foodName;

            food = foodInstance.GetComponent<GoodsBeh>();
			food.costs = stageManager.goodDataStore.FoodDatabase_list[(int)GoodDataStore.FoodMenuList.Curry_with_rice].costs;
			food.sprite.spriteId = food.sprite.GetSpriteIdByName(foodName);
            food._canDragaable = true;
            food.GoodsBeh_putObjectOnTray_Event = food_putObjectOnTray_Event;
			food.ObjectsBeh_destroyObj_Event = food_destroyObj_Event;
        }
        else if (nameInput == Tempura_UI)
        {
            this.DeActiveBeltMachinePopup();

			string foodName = GoodDataStore.FoodMenuList.Tempura.ToString();

            foodInstance = Instantiate(Resources.Load(PATH_OF_INSTANT_FOOD, typeof(GameObject))) as GameObject;
            foodInstance.transform.position = tempura_transform.position + Vector3.back;
			foodInstance.name = foodName;

            food = foodInstance.GetComponent<GoodsBeh>();
			food.costs = stageManager.goodDataStore.FoodDatabase_list[(int)GoodDataStore.FoodMenuList.Tempura].costs;
			food.sprite.spriteId = food.sprite.GetSpriteIdByName(foodName);
            food._canDragaable = true;
            food.GoodsBeh_putObjectOnTray_Event = food_putObjectOnTray_Event;
            food.ObjectsBeh_destroyObj_Event = food_destroyObj_Event;
        }
        else if (nameInput == YakiSoba_UI)
        {
            this.DeActiveBeltMachinePopup();

			string foodName = GoodDataStore.FoodMenuList.Yaki_soba.ToString();

            foodInstance = Instantiate(Resources.Load(PATH_OF_INSTANT_FOOD, typeof(GameObject))) as GameObject;
            foodInstance.transform.position = yakisoba_transform.position + Vector3.back;
			foodInstance.name = foodName;

            food = foodInstance.GetComponent<GoodsBeh>();
			food.costs = stageManager.goodDataStore.FoodDatabase_list[(int)GoodDataStore.FoodMenuList.Yaki_soba].costs;
			food.sprite.spriteId = food.sprite.GetSpriteIdByName(foodName);
            food._canDragaable = true;
            food.GoodsBeh_putObjectOnTray_Event = food_putObjectOnTray_Event;
            food.ObjectsBeh_destroyObj_Event = food_destroyObj_Event;
        }
        else if (nameInput == ZaruSoba_UI)
        {
            this.DeActiveBeltMachinePopup();

			string foodName = GoodDataStore.FoodMenuList.Zaru_soba.ToString();

            foodInstance = Instantiate(Resources.Load(PATH_OF_INSTANT_FOOD, typeof(GameObject))) as GameObject;
            foodInstance.transform.position = zarusoba_transform.position + Vector3.back;
			foodInstance.name = foodName;

			food = foodInstance.GetComponent<GoodsBeh>();
			food.costs = stageManager.goodDataStore.FoodDatabase_list[(int)GoodDataStore.FoodMenuList.Zaru_soba].costs;
			food.sprite.spriteId = food.sprite.GetSpriteIdByName(foodName);
            food._canDragaable = true;
            food.GoodsBeh_putObjectOnTray_Event = food_putObjectOnTray_Event;
            food.ObjectsBeh_destroyObj_Event = food_destroyObj_Event;
        }
	}

	private void food_destroyObj_Event(object sender, System.EventArgs e) {
		GoodsBeh goods = sender as GoodsBeh;
		Mz_StorageManage.AvailableMoney -= goods.costs;
		stageManager.CreateDeductionsCoin (goods.costs);
		baseScene.ReFreshAvailableMoney();		
		stageManager.foodTrayBeh.goodsOnTray_List.Remove(goods);
		stageManager.foodTrayBeh.ReCalculatatePositionOfGoods();
    }

    private void food_putObjectOnTray_Event(object sender, GoodsBeh.PutGoodsToTrayEventArgs e) {
        GoodsBeh obj = sender as GoodsBeh;
        if (stageManager.foodTrayBeh.goodsOnTray_List.Contains(obj) == false && stageManager.foodTrayBeh.goodsOnTray_List.Count < FoodTrayBeh.MaxGoodsCapacity)
        {
            stageManager.foodTrayBeh.goodsOnTray_List.Add(obj);
            stageManager.foodTrayBeh.ReCalculatatePositionOfGoods();

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

