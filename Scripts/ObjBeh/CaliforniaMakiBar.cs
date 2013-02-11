using UnityEngine;
using System.Collections;

public class CaliforniaMakiBar : ObjectsBeh {
	
	internal GameObject instance;
    internal GoodsBeh food;
    private Vector3 instancePosition = new Vector3(52, 45, 0);
	
	// Use this for initialization
	protected override void Start ()
	{
		base.Start ();
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
                food._canDragaable = true;
                food.putObjectOnTray_Event += new System.EventHandler(Handle_putObjectOnTray_Event);
                food.destroyObj_Event += new System.EventHandler(Handle_destroyObj_Event);
            };
		}
	}

    private void Handle_putObjectOnTray_Event(object sender, System.EventArgs e) {
        GoodsBeh obj = sender as GoodsBeh;
        if (sceneManager.foodTrayBeh.goodsOnTray_List.Contains(obj) == false && sceneManager.foodTrayBeh.goodsOnTray_List.Count < FoodTrayBeh.MaxGoodsCapacity)
        {
            sceneManager.foodTrayBeh.goodsOnTray_List.Add(obj);
            sceneManager.foodTrayBeh.ReCalculatatePositionOfGoods();

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
        sceneManager.foodTrayBeh.goodsOnTray_List.Remove(sender as GoodsBeh);
        sceneManager.foodTrayBeh.ReCalculatatePositionOfGoods();
    }
}
