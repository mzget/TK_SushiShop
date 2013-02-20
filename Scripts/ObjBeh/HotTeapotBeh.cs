using UnityEngine;
using System.Collections;

public class HotTeapotBeh : ObjectsBeh {

	public GameObject foodInstance;
	private GoodsBeh food;
	private Vector3 instancePosition = new Vector3(-85.5f, -45f, -2f);
	private FoodTrayBeh foodTrayBeh;

	// Use this for initialization
	protected override void Start ()
	{
		base.Start ();
	}
	
	// Update is called once per frame
	protected override void Update ()
	{
		base.Update ();
		
		if(foodTrayBeh == null)
			foodTrayBeh = baseScene.GetComponent<SushiShop>().foodTrayBeh;
	}

	protected override void OnTouchDown ()
	{
        if (foodInstance == null && food == null)
        {
            animatedSprite.Play();
            animatedSprite.animationCompleteDelegate = delegate(tk2dAnimatedSprite sprite, int clipId)
            {
                foodInstance = Instantiate(Resources.Load("Goods/Hot_greenTea", typeof(GameObject))) as GameObject;
                foodInstance.transform.position = instancePosition;
                foodInstance.name = GoodDataStore.FoodMenuList.Hot_greenTea.ToString();

                food = foodInstance.GetComponent<GoodsBeh>();
                food._canDragaable = true;
                food.GoodsBeh_putObjectOnTray_Event = Handle_putObjectOnTrayEvent;
                food.ObjectsBeh_destroyObj_Event = Handle_destroyObjectEvent;
            };
            // Play sound effect.
            baseScene.audioEffect.PlayOnecSound(baseScene.soundEffect_clips[4]);
        }

		base.OnTouchDown ();
	}

	void Handle_putObjectOnTrayEvent (object sender, GoodsBeh.PutGoodsToTrayEventArgs e)
	{
		GoodsBeh obj = sender as GoodsBeh;
		if (foodTrayBeh.goodsOnTray_List.Contains(obj) == false && foodTrayBeh.goodsOnTray_List.Count < FoodTrayBeh.MaxGoodsCapacity)
		{
			foodTrayBeh.goodsOnTray_List.Add(obj);
			foodTrayBeh.ReCalculatatePositionOfGoods();
			
			//<!-- Setting original position.
			obj.originalPosition = obj.transform.position;
			
			food = null;
			foodInstance = null;
		}
		else
		{
			Debug.LogWarning("Goods on tray have to max capacity.");
			
			obj.transform.position = obj.originalPosition;
		}
	}

	void Handle_destroyObjectEvent (object sender, System.EventArgs e)
	{
		foodTrayBeh.goodsOnTray_List.Remove(sender as GoodsBeh);
		foodTrayBeh.ReCalculatatePositionOfGoods();
	}
}
