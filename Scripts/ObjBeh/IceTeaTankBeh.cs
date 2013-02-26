using UnityEngine;
using System.Collections;

public class IceTeaTankBeh : ObjectsBeh {

    const string PATH_OF_INSTANCE_PREFAB = "Goods/IceTea";
    
    private SushiShop stageManager;
    internal GameObject instance;
    private tk2dAnimatedSprite animatedInstance;
    private GoodsBeh food;
    private Vector3 instancePosition = new Vector3(30.5f, 50f, 5f);


	// Use this for initialization
    protected override void Start()
    {
        base.Start();
    }
	
	// Update is called once per frame
    protected override void Update()
    {
        base.Update();
		
		if(stageManager == null)
			stageManager = baseScene.GetComponent<SushiShop>();
    }

    protected override void OnTouchDown()
    {
        if (instance == null && food == null)
        {
            instance = Instantiate(Resources.Load(PATH_OF_INSTANCE_PREFAB, typeof(GameObject))) as GameObject;
            instance.transform.position = instancePosition;
            instance.gameObject.name = GoodDataStore.FoodMenuList.Iced_greenTea.ToString();
        
			baseScene.audioEffect.PlayOnecWithOutStop(baseScene.soundEffect_clips[2]);

            animatedInstance = instance.GetComponent<tk2dAnimatedSprite>();
            animatedInstance.Play();

            food = instance.GetComponent<GoodsBeh>();
			food.offsetPos = Vector3.up * 4;
            food.costs = stageManager.goodDataStore.FoodDatabase_list[(int)GoodDataStore.FoodMenuList.Iced_greenTea].costs;
			food.GoodsBeh_putObjectOnTray_Event = Handle_putObjectOnTray_Event;
			food.ObjectsBeh_destroyObj_Event = Handle_destroyObj_Event;

            animatedInstance.animationCompleteDelegate = delegate(tk2dAnimatedSprite sprite, int clipId)
            {
                food._canDragaable = true;
            };
        }

        base.OnTouchDown();
    }

    void Handle_putObjectOnTray_Event(object sender, System.EventArgs e)
    {
        GoodsBeh obj = sender as GoodsBeh;
        if (stageManager.foodTrayBeh.goodsOnTray_List.Contains(obj) == false && stageManager.foodTrayBeh.goodsOnTray_List.Count < FoodTrayBeh.MaxGoodsCapacity)
        {
            stageManager.foodTrayBeh.goodsOnTray_List.Add(obj);
            stageManager.foodTrayBeh.ReCalculatatePositionOfGoods();

            //<!-- Setting original position.
            obj.originalPosition = obj.transform.position;

            food = null;
            instance = null;
        }
        else
        {
            Debug.LogWarning("Goods on tray have to max capacity.");

            obj.transform.position = obj.originalPosition;
        }
    }

    void Handle_destroyObj_Event(object sender, System.EventArgs e)
    {		
		GoodsBeh goods = sender as GoodsBeh;
		Mz_StorageManage.AvailableMoney -= goods.costs;
		stageManager.CreateDeductionsCoin (goods.costs);
        baseScene.ReFreshAvailableMoney();
		stageManager.foodTrayBeh.goodsOnTray_List.Remove(goods);
		stageManager.foodTrayBeh.ReCalculatatePositionOfGoods();
    }
}
