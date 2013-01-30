using UnityEngine;
using System.Collections;

public class IcecreamTankBeh : ObjectsBeh {
	const string Icecream_ResourcePath = "Goods/GreenTea_icecream";

	
	private GameObject icecream_Instance;
	private IcecreamBeh icecreamBeh;
	private Vector3 icecreamPos = new Vector3(-20f, 55f, 0f);
	
	
	// Use this for initialization
	protected override void Start () {
		
		base.Start();
	
		base._canDragaable = false;
	}

    protected override void OnTouchDown()
    {
        if(icecream_Instance == null) {
            this.animatedSprite.Play();
			animatedSprite.animationCompleteDelegate = delegate(tk2dAnimatedSprite sprite, int clipId)
			{
				icecream_Instance = Instantiate(Resources.Load(Icecream_ResourcePath, typeof(GameObject))) as GameObject;
//				icecream_Instance.transform.parent = this.transform;
				icecream_Instance.transform.position = icecreamPos;
				icecream_Instance.gameObject.name = GoodDataStore.FoodMenuList.GreenTea_icecream.ToString();
				
				icecreamBeh = icecream_Instance.GetComponent<IcecreamBeh>();
                icecreamBeh.originalPosition = icecreamPos;
				icecreamBeh.putObjectOnTray_Event += new System.EventHandler(icecreamBeh_putObjectOnTray_Event);
				icecreamBeh.destroyObj_Event += new System.EventHandler(icecreamBeh_destroyObj_Event);
			};
		}

		base.OnTouchDown();
    }

	void icecreamBeh_putObjectOnTray_Event (object sender, System.EventArgs e)
	{		
		GoodsBeh obj = sender as GoodsBeh;
		if (sceneManager.foodTrayBeh.goodsOnTray_List.Contains (obj) == false && sceneManager.foodTrayBeh.goodsOnTray_List.Count<FoodTrayBeh.MaxGoodsCapacity) {
			sceneManager.foodTrayBeh.goodsOnTray_List.Add (obj);			
			sceneManager.foodTrayBeh.ReCalculatatePositionOfGoods();

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
        sceneManager.foodTrayBeh.goodsOnTray_List.Remove(sender as GoodsBeh);
		sceneManager.foodTrayBeh.ReCalculatatePositionOfGoods();
    }
}
