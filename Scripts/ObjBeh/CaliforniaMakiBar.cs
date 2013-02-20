using UnityEngine;
using System.Collections;

public class CaliforniaMakiBar : ObjectsBeh {
	
	internal GameObject instance;
    internal GoodsBeh food;
    private Vector3 instancePosition = new Vector3(52, 45, 0);
    private FoodTrayBeh foodTrayBeh;
	
	// Use this for initialization
	protected override void Start ()
	{
        base.Start();

        foodTrayBeh = baseScene.GetComponent<SushiShop>().foodTrayBeh;
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
        if (foodTrayBeh.goodsOnTray_List.Contains(obj) == false && foodTrayBeh.goodsOnTray_List.Count < FoodTrayBeh.MaxGoodsCapacity)
        {
            foodTrayBeh.goodsOnTray_List.Add(obj);
            foodTrayBeh.ReCalculatatePositionOfGoods();

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
        foodTrayBeh.goodsOnTray_List.Remove(sender as GoodsBeh);
        foodTrayBeh.ReCalculatatePositionOfGoods();
    }
}
