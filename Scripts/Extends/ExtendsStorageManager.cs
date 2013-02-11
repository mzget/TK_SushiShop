using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtendsStorageManager : Mz_StorageManage
{	
    public const String KEY_CAN_ALIMENT_PET_LIST = "KEY_CAN_ALIMENT_PET_LIST";
    public const string KEY_PET_ID = "PET_ID";
	
	#region <@-- Load secsion.

	public void LoadCanSellGoodsListData ()
	{		
		int[] array = PlayerPrefsX.GetIntArray(Mz_StorageManage.SaveSlot + KEY_CANSELLGOODSLIST);
        SushiShop.NumberOfCansellItem.Clear();
		foreach (var item in array) {
			SushiShop.NumberOfCansellItem.Add(item);
		}
	}

    public void LoadCostumeData() {
		/// Clothes data.
		int[] load_arr = PlayerPrefsX.GetIntArray(Mz_StorageManage.SaveSlot + Mz_StorageManage.KEY_CAN_EQUIP_CLOTHE_LIST);
		if(load_arr.Length != 0) {
			this.AddCanEquipClotheTempToStaticVar(ref load_arr);
		}
		else if(load_arr.Length == 0) {
			load_arr = new int[] { 0, 1, 2 };
			this.AddCanEquipClotheTempToStaticVar(ref load_arr);
		}

		/// Hats data.
		int[] load_temp_hats = PlayerPrefsX.GetIntArray(Mz_StorageManage.SaveSlot + Mz_StorageManage.KEY_CAN_EQUIP_HAT_LIST);
		if(load_temp_hats.Length != 0) {
			this.AddCanEquipHatTempToStaticVar(ref load_temp_hats);
		}
		else if(load_temp_hats.Length == 0) {
			load_temp_hats = new int[] { 0, 1, 2 };
			this.AddCanEquipHatTempToStaticVar(ref load_temp_hats);
		}
    }

	void AddCanEquipClotheTempToStaticVar (ref int[] temp_arr)
	{
		Dressing.CanEquipClothe_list.Clear();
		foreach (int item in temp_arr)
		{
			Dressing.CanEquipClothe_list.Add(item);
		}
	}

	void AddCanEquipHatTempToStaticVar (ref int[] temp_hats)
	{
		Dressing.CanEquipHat_list.Clear();
		foreach (int item in temp_hats) {
			Dressing.CanEquipHat_list.Add(item);
		}
	}

    private void LoadDecorationShopOutside()
    {
		int[] roof_temp_array = PlayerPrefsX.GetIntArray(Mz_StorageManage.SaveSlot + Mz_StorageManage.KEY_CAN_DECORATE_ROOF_LIST, 0, 0);
		if(roof_temp_array.Length != 0) {
			this.AddCanDecorationRoofList(ref roof_temp_array);
		}
		else return;

		int[] awning_temp_array = PlayerPrefsX.GetIntArray(SaveSlot + KEY_CAN_DECORATE_AWNING_LIST, 0, 0);
		if(awning_temp_array.Length != 0) {
			this.AddCanDecorateAwningList(ref awning_temp_array);
		}
		else return;

		int[] table_temp_array = PlayerPrefsX.GetIntArray(SaveSlot + KEY_CAN_DECORATE_TABLE_LIST, 0, 0);
		if(table_temp_array.Length != 0) {
			this.AddCanDecorationTableList(ref table_temp_array);
		}
		else return;

		int[] accessories_temp_array = PlayerPrefsX.GetIntArray(SaveSlot + KEY_CAN_DECORATE_ACCESSORIES_LIST, 0, 0);
		if(accessories_temp_array.Length != 0) {
			this.AddCanDecorationAccessoriesList(ref accessories_temp_array);
		}
		else return;
    }
	void AddCanDecorationRoofList (ref int[] roof_temp_array)
	{
		UpgradeOutsideManager.CanDecorateRoof_list.Clear();
		foreach (int item in roof_temp_array) {
			UpgradeOutsideManager.CanDecorateRoof_list.Add(item);
		}
	}
	void AddCanDecorateAwningList (ref int[] awning_temp_array)
	{
		UpgradeOutsideManager.CanDecorateAwning_list.Clear();
		foreach (int item in awning_temp_array) {
			UpgradeOutsideManager.CanDecorateAwning_list.Add(item);
		}
	}
	void AddCanDecorationTableList (ref int[] table_temp_array)
	{
		UpgradeOutsideManager.CanDecoration_Table_list.Clear();
		foreach (int item in table_temp_array) {
			UpgradeOutsideManager.CanDecoration_Table_list.Add(item);
		}
	}
	void AddCanDecorationAccessoriesList (ref int[] accessories_temp_array)
	{
		UpgradeOutsideManager.CanDecoration_Accessories_list.Clear();
		foreach (var item in accessories_temp_array) {
			UpgradeOutsideManager.CanDecoration_Accessories_list.Add(item);
		}
	}

	public override void LoadSaveDataToGameStorage()
	{
		base.LoadSaveDataToGameStorage ();
				
		Mz_StorageManage.Username = PlayerPrefs.GetString(Mz_StorageManage.SaveSlot + Mz_StorageManage.KEY_USERNAME);
		Mz_StorageManage.ShopName = PlayerPrefs.GetString(Mz_StorageManage.SaveSlot + Mz_StorageManage.KEY_SHOP_NAME);
		Mz_StorageManage.AvailableMoney = PlayerPrefs.GetInt(Mz_StorageManage.SaveSlot + Mz_StorageManage.KEY_MONEY);
		Mz_StorageManage.AccountBalance = PlayerPrefs.GetInt(Mz_StorageManage.SaveSlot + Mz_StorageManage.KEY_ACCOUNTBALANCE);
		Mz_StorageManage.ShopLogo = PlayerPrefs.GetInt(Mz_StorageManage.SaveSlot + Mz_StorageManage.KEY_SHOP_LOGO);
		Mz_StorageManage.ShopLogoColor = PlayerPrefs.GetString(Mz_StorageManage.SaveSlot + Mz_StorageManage.KEY_SHOP_LOGO_COLOR);
				
		Mz_StorageManage.Roof_id = PlayerPrefs.GetInt(SaveSlot + KEY_ROOF_ID);
		Mz_StorageManage.Awning_id = PlayerPrefs.GetInt(SaveSlot + KEY_AWNING_ID);
		Mz_StorageManage.Table_id = PlayerPrefs.GetInt(SaveSlot + KEY_TABLE_ID);
		Mz_StorageManage.Accessory_id = PlayerPrefs.GetInt(SaveSlot + KEY_ACCESSORY_ID);
		
		Mz_StorageManage.TK_clothe_id = PlayerPrefs.GetInt(SaveSlot + KEY_TK_CLOTHE_ID);
		Mz_StorageManage.TK_hat_id = PlayerPrefs.GetInt(SaveSlot + KEY_TK_HAT_ID);
        // Pet.
        Mz_StorageManage.Pet_id = PlayerPrefs.GetInt(SaveSlot + KEY_PET_ID);
		
		//@!-- Load Donation data.
		ConservationAnimals.Level = PlayerPrefs.GetInt(SaveSlot + KEY_CONSERVATION_ANIMAL_LV, 0);
		AIDSFoundation.Level = PlayerPrefs.GetInt(SaveSlot + KEY_AIDSFOUNDATION_LV, 0);
		LoveDogConsortium.Level = PlayerPrefs.GetInt(SaveSlot + KEY_LOVEDOGFOUNDATION_LV, 0);
		LoveKidsFoundation.Level = PlayerPrefs.GetInt(SaveSlot + KEY_LOVEKIDFOUNDATION_LV, 0);
		EcoFoundation.Level = PlayerPrefs.GetInt(SaveSlot + KEY_ECOFOUNDATION_LV, 0);
        GlobalWarmingOranization.Level = PlayerPrefs.GetInt(SaveSlot + KEY_GLOBALWARMING_LV, 0);

        this.LoadCanSellGoodsListData();
        this.LoadCostumeData();
        this.LoadDecorationShopOutside();
        this.LoadCanAlimentPetList();
	}
	
    private void LoadCanAlimentPetList()
    {
        int[] array_temp = PlayerPrefsX.GetIntArray(SaveSlot + KEY_CAN_ALIMENT_PET_LIST);
        UpgradeOutsideManager.CanAlimentPet_id_list.Clear();
        foreach (int item in array_temp)
        {
            UpgradeOutsideManager.CanAlimentPet_id_list.Add(item);
        }
    }

	#endregion

	#region <@-- Save section.

    public override void SaveDataToPermanentMemory()
    {
        base.SaveDataToPermanentMemory();

        PlayerPrefs.SetString(Mz_StorageManage.SaveSlot + Mz_StorageManage.KEY_USERNAME, Mz_StorageManage.Username);
        PlayerPrefs.SetString(Mz_StorageManage.SaveSlot + Mz_StorageManage.KEY_SHOP_NAME, Mz_StorageManage.ShopName);
        PlayerPrefs.SetInt(Mz_StorageManage.SaveSlot + Mz_StorageManage.KEY_MONEY, Mz_StorageManage.AvailableMoney);
		PlayerPrefs.SetInt(Mz_StorageManage.SaveSlot + Mz_StorageManage.KEY_ACCOUNTBALANCE, Mz_StorageManage.AccountBalance);
		PlayerPrefs.SetInt(Mz_StorageManage.SaveSlot + Mz_StorageManage.KEY_SHOP_LOGO, Mz_StorageManage.ShopLogo);
		PlayerPrefs.SetString(Mz_StorageManage.SaveSlot + Mz_StorageManage.KEY_SHOP_LOGO_COLOR, Mz_StorageManage.ShopLogoColor);

		PlayerPrefs.SetInt(Mz_StorageManage.SaveSlot + Mz_StorageManage.KEY_ROOF_ID, Mz_StorageManage.Roof_id);
		PlayerPrefs.SetInt(Mz_StorageManage.SaveSlot + Mz_StorageManage.KEY_AWNING_ID, Mz_StorageManage.Awning_id);
		PlayerPrefs.SetInt(Mz_StorageManage.SaveSlot + Mz_StorageManage.KEY_TABLE_ID, Mz_StorageManage.Table_id);
		PlayerPrefs.SetInt(Mz_StorageManage.SaveSlot + Mz_StorageManage.KEY_ACCESSORY_ID, Mz_StorageManage.Accessory_id);

        PlayerPrefs.SetInt(Mz_StorageManage.SaveSlot + Mz_StorageManage.KEY_TK_CLOTHE_ID, Mz_StorageManage.TK_clothe_id);
        PlayerPrefs.SetInt(Mz_StorageManage.SaveSlot + Mz_StorageManage.KEY_TK_HAT_ID, Mz_StorageManage.TK_hat_id);
        // pet.
        PlayerPrefs.SetInt(Mz_StorageManage.SaveSlot + KEY_PET_ID, Pet_id);

        //@!-- Donation data.
        PlayerPrefs.SetInt(Mz_StorageManage.SaveSlot + Mz_StorageManage.KEY_CONSERVATION_ANIMAL_LV, ConservationAnimals.Level);
        PlayerPrefs.SetInt(Mz_StorageManage.SaveSlot + Mz_StorageManage.KEY_AIDSFOUNDATION_LV, AIDSFoundation.Level);
        PlayerPrefs.SetInt(Mz_StorageManage.SaveSlot + Mz_StorageManage.KEY_LOVEDOGFOUNDATION_LV, LoveDogConsortium.Level);
        PlayerPrefs.SetInt(Mz_StorageManage.SaveSlot + Mz_StorageManage.KEY_LOVEKIDFOUNDATION_LV, LoveKidsFoundation.Level);
        PlayerPrefs.SetInt(Mz_StorageManage.SaveSlot + Mz_StorageManage.KEY_ECOFOUNDATION_LV, EcoFoundation.Level);
        PlayerPrefs.SetInt(Mz_StorageManage.SaveSlot + Mz_StorageManage.KEY_GLOBALWARMING_LV, GlobalWarmingOranization.Level);

		if(SushiShop.NumberOfCansellItem.Count != 0)
			this.SaveCanSellGoodListData();
        if (Dressing.CanEquipClothe_list.Count != 0)
            this.SaveCanEquipClothesData();
		if(Dressing.CanEquipHat_list.Count != 0) 
			this.SaveCanEquipHatData();

		if(UpgradeOutsideManager.CanAlimentPet_id_list.Count != 0)
			this.SaveCanAlimentPetList();

		this.SaveCanDecorateShopOutside();
		
		PlayerPrefs.Save();
    }

    public void SaveCanSellGoodListData() {
        int[] array_temp = SushiShop.NumberOfCansellItem.ToArray();
        PlayerPrefsX.SetIntArray(Mz_StorageManage.SaveSlot + KEY_CANSELLGOODSLIST, array_temp);
    }

    private void SaveCanEquipClothesData() {
        int[] arr_clothe = Dressing.CanEquipClothe_list.ToArray();
        PlayerPrefsX.SetIntArray(Mz_StorageManage.SaveSlot + Mz_StorageManage.KEY_CAN_EQUIP_CLOTHE_LIST, arr_clothe); 
    }

	private void SaveCanEquipHatData ()
	{
		int[] arr_hat = Dressing.CanEquipHat_list.ToArray();
		PlayerPrefsX.SetIntArray(Mz_StorageManage.SaveSlot + Mz_StorageManage.KEY_CAN_EQUIP_HAT_LIST, arr_hat);
	}

	private void SaveCanDecorateShopOutside ()
	{
		if(UpgradeOutsideManager.CanDecorateRoof_list.Count != 0) {
			int[] roof_temp_arr = UpgradeOutsideManager.CanDecorateRoof_list.ToArray();
			PlayerPrefsX.SetIntArray(SaveSlot + KEY_CAN_DECORATE_ROOF_LIST, roof_temp_arr); 
		}

		if(UpgradeOutsideManager.CanDecorateAwning_list.Count != 0) {
			int[] awning_temp_array = UpgradeOutsideManager.CanDecorateAwning_list.ToArray();
			PlayerPrefsX.SetIntArray(SaveSlot + KEY_CAN_DECORATE_AWNING_LIST, awning_temp_array);
		}
		
		if(UpgradeOutsideManager.CanDecoration_Table_list.Count != 0) {
			int[] table_temp_array = UpgradeOutsideManager.CanDecoration_Table_list.ToArray();
			PlayerPrefsX.SetIntArray(SaveSlot + KEY_CAN_DECORATE_TABLE_LIST, table_temp_array);
		}

		if(UpgradeOutsideManager.CanDecoration_Accessories_list.Count != 0) {
			int[] accessories_temp_array = UpgradeOutsideManager.CanDecoration_Accessories_list.ToArray();
			PlayerPrefsX.SetIntArray(SaveSlot + KEY_CAN_DECORATE_ACCESSORIES_LIST, accessories_temp_array);
		}
	}
	
    private void SaveCanAlimentPetList()
    {
        int[] array_temp = UpgradeOutsideManager.CanAlimentPet_id_list.ToArray();
        PlayerPrefsX.SetIntArray(SaveSlot + KEY_CAN_ALIMENT_PET_LIST, array_temp);
    }
 
	#endregion
}

