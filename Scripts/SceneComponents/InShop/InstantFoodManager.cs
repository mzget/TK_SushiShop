using UnityEngine;
using System.Collections;

public class InstantFoodManager : MonoBehaviour {

    public SushiShop stageManager;
    private GameObject kimjiInstance;
    public GoodsBeh kimji;

	// Use this for initialization
	void Start () {
        Debug.Log("InstantFoodManager : Start");

        stageManager = this.GetComponent<SushiShop>();
	}

    internal IEnumerator Create_InstantFoodObject()
    {
        yield return new WaitForFixedUpdate();

        if (SushiShop.NumberOfCansellItem.Contains((int)GoodDataStore.FoodMenuList.Kimji))
        {
            if (kimjiInstance == null && kimji == null)
            {
                kimjiInstance = Instantiate(Resources.Load("Goods/Kimji", typeof(GameObject))) as GameObject;
                kimjiInstance.transform.position = new Vector3(-59.8f, 11, 7);
                kimjiInstance.name = GoodDataStore.FoodMenuList.Kimji.ToString();

                kimji = kimjiInstance.GetComponent<GoodsBeh>();
                kimji.costs = stageManager.goodDataStore.FoodDatabase_list[(int)GoodDataStore.FoodMenuList.Kimji].costs;
                kimji._canDragaable = true;
                kimji.GoodsBeh_putObjectOnTray_Event = InstantFood_putObjectOnTray_Event;
                kimji.ObjectsBeh_destroyObj_Event = InstantFood_destroyObj_Event;
            }
        }
    }

    private void InstantFood_putObjectOnTray_Event(object sender, GoodsBeh.PutGoodsToTrayEventArgs e)
    {
        GoodsBeh obj = sender as GoodsBeh;
        if (stageManager.foodTrayBeh.goodsOnTray_List.Contains(obj) == false && stageManager.foodTrayBeh.goodsOnTray_List.Count < FoodTrayBeh.MaxGoodsCapacity)
        {
            stageManager.foodTrayBeh.goodsOnTray_List.Add(obj);
            stageManager.foodTrayBeh.ReCalculatatePositionOfGoods();
			
			if(e.foodInstance.name == kimjiInstance.name)
			{
				kimji = null;
				kimjiInstance = null;
			}

            StartCoroutine_Auto(this.Create_InstantFoodObject());
        }
        else
        {
            Debug.LogWarning("Goods on tray have to max capacity.");

            obj.transform.position = obj.originalPosition;
        }
    }

    private void InstantFood_destroyObj_Event(object sender, System.EventArgs e)
    {
		GoodsBeh goods = sender as GoodsBeh;
		Mz_StorageManage.AvailableMoney -= goods.costs;
        stageManager.ReFreshAvailableMoney();

		stageManager.foodTrayBeh.goodsOnTray_List.Remove(goods);
		stageManager.foodTrayBeh.ReCalculatatePositionOfGoods();

        StartCoroutine_Auto(this.Create_InstantFoodObject());
    }
}
