using UnityEngine;
using System.Collections;

public class SoupTankBeh : ObjectsBeh {
    const string PATH_OF_SOUP_INSTANCE = "Goods/Miso_soup";

    internal GameObject soup_instance; 
    private tk2dAnimatedSprite animatedInstance;
    private GoodsBeh soup;
    private Vector3 instancePosition = new Vector3(-110f, 34f, 0f);

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
        if (soup_instance == null)
        {
            soup_instance = Instantiate(Resources.Load(PATH_OF_SOUP_INSTANCE, typeof(GameObject))) as GameObject;
            soup_instance.transform.position = instancePosition;
            soup_instance.gameObject.name = GoodDataStore.FoodMenuList.Miso_soup.ToString();
            
            animatedInstance = soup_instance.GetComponent<tk2dAnimatedSprite>();
            animatedInstance.Play();

            soup = soup_instance.GetComponent<GoodsBeh>();

            animatedInstance.animationCompleteDelegate = delegate(tk2dAnimatedSprite sprite, int clipId)
            {
                soup._canDragaable = true;
                soup.originalPosition = instancePosition;
                soup.putObjectOnTray_Event += Handle_putObjectOnTray_Event;
                soup.destroyObj_Event += Handle_destroyObj_Event;
            };
        }

        base.OnTouchDown();
    }

    void Handle_putObjectOnTray_Event(object sender, System.EventArgs e) {
        GoodsBeh obj = sender as GoodsBeh;
        if (sceneManager.foodTrayBeh.goodsOnTray_List.Contains(obj) == false && sceneManager.foodTrayBeh.goodsOnTray_List.Count < FoodTrayBeh.MaxGoodsCapacity)
        {
            sceneManager.foodTrayBeh.goodsOnTray_List.Add(obj);
            sceneManager.foodTrayBeh.ReCalculatatePositionOfGoods();

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
        sceneManager.foodTrayBeh.goodsOnTray_List.Remove(sender as GoodsBeh);
        sceneManager.foodTrayBeh.ReCalculatatePositionOfGoods();
    }
}
