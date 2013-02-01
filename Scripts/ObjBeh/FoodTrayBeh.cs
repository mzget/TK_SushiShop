using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FoodTrayBeh : ScriptableObject {
	
	public const int MaxGoodsCapacity = 3;
	
	public List<GoodsBeh> goodsOnTray_List = new List<GoodsBeh>(MaxGoodsCapacity);
    private Vector3[] arr_GoodsPositionOnTray = new Vector3[MaxGoodsCapacity] {
        new Vector3(0f, -70f, -2.5f), new Vector3(30f, -70f, -2.5f),  new Vector3(-30f, -70f, -2.5f),
    };
	public GameObject[] emptyObjSlot = new GameObject[MaxGoodsCapacity];
	
	// Use this for initialization
	public void OnEnable() {		
		for (int i = 0; i < MaxGoodsCapacity; i++) {
			emptyObjSlot[i] = new GameObject("Slot_" + i);
			emptyObjSlot[i].transform.position = arr_GoodsPositionOnTray[i];
		}
	}

    internal void ReCalculatatePositionOfGoods() {
        for (int i = 0; i < goodsOnTray_List.Count; i++)
        {
            goodsOnTray_List[i].transform.parent = emptyObjSlot[i].transform;
            goodsOnTray_List[i].transform.localPosition = Vector3.zero + goodsOnTray_List[i].offsetPos;
            //@-- Finally.
            goodsOnTray_List[i].originalPosition = goodsOnTray_List[i].transform.position;
        }
    }
}
