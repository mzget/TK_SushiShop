using UnityEngine;
using System.Collections;

public class IceBreakerBeh : ObjectsBeh
{
    const string PATH_OF_ICEDJAM_INSTANCE = "Goods/IceJam";

    private GameObject instance;
    private tk2dAnimatedSprite animatedInstance;
    private GoodsBeh food;
    private Vector3 instancePosition = new Vector3(-60f, 30f, 0f);


	// Use this for initialization
    protected override void Start()
    {
        base.Start();
    }
	
	// Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void OnTouchDown()
    {
        if (instance == null)
        {
            instance = Instantiate(Resources.Load(PATH_OF_ICEDJAM_INSTANCE, typeof(GameObject))) as GameObject;
            instance.transform.position = instancePosition;
            instance.gameObject.name = GoodDataStore.FoodMenuList.Bean_ice_jam_on_crunching.ToString();

            animatedInstance = instance.GetComponent<tk2dAnimatedSprite>();
            animatedInstance.Play();

            food = instance.GetComponent<GoodsBeh>();

            animatedInstance.animationCompleteDelegate = delegate(tk2dAnimatedSprite sprite, int clipId)
            {
                food._canDragaable = true;
                food.originalPosition = instancePosition;
                food.putObjectOnTray_Event += Handle_putObjectOnTray_Event;
                food.destroyObj_Event += Handle_destroyObj_Event;
            };
        }
     
        base.OnTouchDown();
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
