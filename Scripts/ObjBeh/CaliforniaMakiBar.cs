using UnityEngine;
using System.Collections;

public class CaliforniaMakiBar : ObjectsBeh {
	
	internal GameObject instance;
    internal GoodsBeh food;
    private Vector3 instancePosition = new Vector3(52, 45, 0);
	private SushiShop stageManager;
	
	// Use this for initialization
	protected override void Start ()
	{
        base.Start();

        stageManager = baseScene.GetComponent<SushiShop>();
	}
	
	// Update is called once per frame
	protected override void Update ()
	{
		base.Update ();
	}
	
	protected override void OnTouchDown ()
	{
		base.OnTouchDown ();
		
		if(instance == null) {
            base.animatedSprite.Play();
            base.animatedSprite.animationCompleteDelegate = delegate(tk2dAnimatedSprite sprite, int clipId)
            {
                animatedSprite.spriteId = 0;
                instance = Instantiate(Resources.Load("Goods/CaliforniaMaki", typeof(GameObject))) as GameObject;
                instance.transform.position = instancePosition;
                instance.name = GoodDataStore.FoodMenuList.California_maki.ToString();

                food = instance.GetComponent<GoodsBeh>();
				food.costs = stageManager.goodDataStore.FoodDatabase_list[(int)GoodDataStore.FoodMenuList.California_maki].costs;
                food.originalPosition = instancePosition;
				food.offsetPos = Vector3.up * 4;
                food._canDragaable = true;
                food.GoodsBeh_putObjectOnTray_Event = Handle_putObjectOnTray_Event;
                food.ObjectsBeh_destroyObj_Event = Handle_destroyObj_Event;
            };
            // Play sound effect.
            baseScene.audioEffect.PlayOnecWithOutStop(baseScene.soundEffect_clips[3]);
		}
	}

    private void Handle_putObjectOnTray_Event(object sender, GoodsBeh.PutGoodsToTrayEventArgs e) {
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

	private void Handle_destroyObj_Event(object sender, System.EventArgs e) {
		GoodsBeh goods = sender as GoodsBeh;
		Mz_StorageManage.AvailableMoney -= goods.costs;
		stageManager.CreateDeductionsCoin (goods.costs);
		stageManager.ReFreshAvailableMoney();		
		stageManager.foodTrayBeh.goodsOnTray_List.Remove(goods);
		stageManager.foodTrayBeh.ReCalculatatePositionOfGoods();
    }
}
