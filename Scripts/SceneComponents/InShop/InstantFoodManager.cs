using UnityEngine;
using System.Collections;

public class InstantFoodManager : MonoBehaviour {

    public FoodTrayBeh foodtrayBeh;

    private GameObject kimjiInstance;
    public GoodsBeh kimji;
//    public InstantFood hotTea;

	// Use this for initialization
	void Start () {
        Debug.Log("InstantFoodManager : Start");

        foodtrayBeh = this.GetComponent<SushiShop>().foodTrayBeh;
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
                kimji._canDragaable = true;
                kimji.GoodsBeh_putObjectOnTray_Event = InstantFood_putObjectOnTray_Event;
                kimji.ObjectsBeh_destroyObj_Event = InstantFood_destroyObj_Event;
            }
        }
    }

    private void InstantFood_putObjectOnTray_Event(object sender, GoodsBeh.PutGoodsToTrayEventArgs e)
    {
        GoodsBeh obj = sender as GoodsBeh;
        if (foodtrayBeh.goodsOnTray_List.Contains(obj) == false && foodtrayBeh.goodsOnTray_List.Count < FoodTrayBeh.MaxGoodsCapacity)
        {
            foodtrayBeh.goodsOnTray_List.Add(obj);
            foodtrayBeh.ReCalculatatePositionOfGoods();
			
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
        foodtrayBeh.goodsOnTray_List.Remove(sender as GoodsBeh);
        foodtrayBeh.ReCalculatatePositionOfGoods();

        StartCoroutine_Auto(this.Create_InstantFoodObject());
    }
}
