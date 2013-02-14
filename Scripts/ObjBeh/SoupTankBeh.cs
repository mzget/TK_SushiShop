using UnityEngine;
using System.Collections;

public class SoupTankBeh : ObjectsBeh {
    const string PATH_OF_SOUP_INSTANCE = "Goods/Miso_soup";

    internal GameObject soup_instance; 
    private tk2dAnimatedSprite animatedInstance;
    private GoodsBeh soup;
    private Vector3 instancePosition = new Vector3(-110f, 34f, 0f);
    private FoodTrayBeh foodTrayBeh;

	// Use this for initialization
    protected override void Start()
    {
        base.Start();

        foodTrayBeh = baseScene.GetComponent<SushiShop>().foodTrayBeh;
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
			soup.offsetPos = Vector3.up * 4.5f;
			soup.GoodsBeh_putObjectOnTray_Event = Handle_putObjectOnTray_Event;
			soup.ObjectsBeh_destroyObj_Event = Handle_destroyObj_Event;

            animatedInstance.animationCompleteDelegate = delegate(tk2dAnimatedSprite sprite, int clipId)
            {
                soup._canDragaable = true;
            };
        }

        base.OnTouchDown();
    }

    void Handle_putObjectOnTray_Event(object sender, GoodsBeh.PutGoodsToTrayEventArgs e) {
        GoodsBeh obj = sender as GoodsBeh;
        if (foodTrayBeh.goodsOnTray_List.Contains(obj) == false && foodTrayBeh.goodsOnTray_List.Count < FoodTrayBeh.MaxGoodsCapacity)
        {
            foodTrayBeh.goodsOnTray_List.Add(obj);
            foodTrayBeh.ReCalculatatePositionOfGoods();

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
        foodTrayBeh.goodsOnTray_List.Remove(sender as GoodsBeh);
        foodTrayBeh.ReCalculatatePositionOfGoods();
    }
}
