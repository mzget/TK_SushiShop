using UnityEngine;
using System.Collections;

public class IcecreamTankBeh : ObjectsBeh {
	const string Icecream_ResourcePath = "Goods/GreenTea_icecream";
    	
	private GameObject icecream_Instance;
	private GoodsBeh icecreamBeh;
	private Vector3 icecreamPos = new Vector3(-20f, 55f, 0f);

    private FoodTrayBeh foodTrayBeh;
	
	// Use this for initialization
	protected override void Start () {
		
		base.Start();

        foodTrayBeh = baseScene.GetComponent<SushiShop>().foodTrayBeh;
	}

    protected override void OnTouchDown()
    {
        if(icecream_Instance == null) {
            this.animatedSprite.Play();
			animatedSprite.animationCompleteDelegate = delegate(tk2dAnimatedSprite sprite, int clipId)
			{
				icecream_Instance = Instantiate(Resources.Load(Icecream_ResourcePath, typeof(GameObject))) as GameObject;
				icecream_Instance.transform.position = icecreamPos;
				icecream_Instance.gameObject.name = GoodDataStore.FoodMenuList.GreenTea_icecream.ToString();
				
				icecreamBeh = icecream_Instance.GetComponent<GoodsBeh>();
				icecreamBeh._canDragaable = true;
				icecreamBeh.GoodsBeh_putObjectOnTray_Event = icecreamBeh_putObjectOnTray_Event;
				icecreamBeh.ObjectsBeh_destroyObj_Event = icecreamBeh_destroyObj_Event;
			};
		}

		base.OnTouchDown();
    }

	void icecreamBeh_putObjectOnTray_Event (object sender, GoodsBeh.PutGoodsToTrayEventArgs e)
	{		
		GoodsBeh obj = sender as GoodsBeh;
		if (foodTrayBeh.goodsOnTray_List.Contains (obj) == false && foodTrayBeh.goodsOnTray_List.Count<FoodTrayBeh.MaxGoodsCapacity) {
			foodTrayBeh.goodsOnTray_List.Add (obj);			
			foodTrayBeh.ReCalculatatePositionOfGoods();

			//<!-- Setting original position.
			obj.originalPosition = obj.transform.position;
			
			icecreamBeh = null;
			icecream_Instance = null;
		} else {
			Debug.LogWarning("Goods on tray have to max capacity.");

			obj.transform.position = obj.originalPosition;
		}
	}

    private void icecreamBeh_destroyObj_Event(object sender, System.EventArgs e) {
        foodTrayBeh.goodsOnTray_List.Remove(sender as GoodsBeh);
		foodTrayBeh.ReCalculatatePositionOfGoods();
    }
}
