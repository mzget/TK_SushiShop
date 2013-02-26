using UnityEngine;
using System.Collections;

public class SoupTankBeh : ObjectsBeh {
    const string PATH_OF_SOUP_INSTANCE = "Goods/Miso_soup";

    internal GameObject soup_instance; 
    private tk2dAnimatedSprite animatedInstance;
    private GoodsBeh soup;
    private Vector3 instancePosition = new Vector3(-110f, 34f, 0f);
    private SushiShop stageManager;

	// Use this for initialization
    protected override void Start()
    {
        base.Start();

        stageManager = baseScene.GetComponent<SushiShop>();
    }
	
	// Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void OnTouchDown()
    {
        if (soup_instance == null && soup == null)
        {
            soup_instance = Instantiate(Resources.Load(PATH_OF_SOUP_INSTANCE, typeof(GameObject))) as GameObject;
            soup_instance.transform.position = instancePosition;
            soup_instance.gameObject.name = GoodDataStore.FoodMenuList.Miso_soup.ToString();
            
            animatedInstance = soup_instance.GetComponent<tk2dAnimatedSprite>();
            animatedInstance.Play();

            soup = soup_instance.GetComponent<GoodsBeh>();
			soup.costs = stageManager.goodDataStore.FoodDatabase_list[(int)GoodDataStore.FoodMenuList.Miso_soup].costs;
			soup.offsetPos = Vector3.up * 4.5f;
			soup.GoodsBeh_putObjectOnTray_Event = Handle_putObjectOnTray_Event;
			soup.ObjectsBeh_destroyObj_Event = Handle_destroyObj_Event;

            animatedInstance.animationCompleteDelegate = delegate(tk2dAnimatedSprite sprite, int clipId)
            {
                soup._canDragaable = true;
            };
            // Play sound effect.
            baseScene.audioEffect.PlayOnecWithOutStop(baseScene.soundEffect_clips[4]);
        }

        base.OnTouchDown();
    }

    void Handle_putObjectOnTray_Event(object sender, GoodsBeh.PutGoodsToTrayEventArgs e) {
        GoodsBeh obj = sender as GoodsBeh;
        if (stageManager.foodTrayBeh.goodsOnTray_List.Contains(obj) == false && stageManager.foodTrayBeh.goodsOnTray_List.Count < FoodTrayBeh.MaxGoodsCapacity)
        {
            stageManager.foodTrayBeh.goodsOnTray_List.Add(obj);
            stageManager.foodTrayBeh.ReCalculatatePositionOfGoods();

            //<!-- Setting original position.
            obj.originalPosition = obj.transform.position;

            soup = null;
            soup_instance = null;
        }
        else
        {
            Debug.LogWarning("Goods on tray have to max capacity.");

            obj.transform.position = obj.originalPosition;
        }
    }

    void Handle_destroyObj_Event(object sender, System.EventArgs e) {		
		GoodsBeh goods = sender as GoodsBeh;
		Mz_StorageManage.AvailableMoney -= goods.costs;
		stageManager.CreateDeductionsCoin (goods.costs);
		baseScene.ReFreshAvailableMoney();
		
		stageManager.foodTrayBeh.goodsOnTray_List.Remove(goods);
		stageManager.foodTrayBeh.ReCalculatatePositionOfGoods();
    }
}
