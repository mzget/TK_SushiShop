using UnityEngine;
using System.Collections;

public class Mz_StorageManage : MonoBehaviour
{
    public const string KEY_SYSTEM_LANGUAGE = "SYSTEM_LANGUAGE";
    public static int Language_id;


    /// <summary>
    /// Standard storage game data.
    /// </summary>
    //<!-- Save Game Slot.
    public static int SaveSlot = 0;
    //<!-- User Name.
    public static string Username = "";
    
    public static string ShopName;
    public static int AvailableMoney = 100000;
    public static int AccountBalance = 100000;
	public static int ShopLogo = 0;
	public static string ShopLogoColor = "Blue";

	public static int Roof_id = 255;
	public static int Awning_id = 255;
	public static int Table_id = 0;
	public static int Accessory_id = 0;

    public static int TK_clothe_id = 255;
    public static int TK_hat_id = 255;
    public static int Pet_id = 0;
	
	public static bool _IsNoticeUser = false;
	public static string KEY_NOTICE_USER_TO_UPGRADE = "NOTICE_USER_TO_UPGRADE";

	/// <summary>
	/// Storage data key.
	/// </summary>
	public const string KEY_USERNAME = "USERNAME";
	public const string KEY_SHOP_NAME = "SHOP_NAME";
	public const string KEY_MONEY = "MONEY";
	public const string KEY_ACCOUNTBALANCE = "ACCOUNTBALANCE";
	public const string KEY_SHOP_LOGO = "SHOP_LOGO";
	public const string KEY_SHOP_LOGO_COLOR = "SHOP_LOGO_COLOR";

	public const string KEY_ROOF_ID = "ROOF_ID";
	public const string KEY_AWNING_ID = "AWNING_ID";
	public const string KEY_TABLE_ID = "TABLE_ID";
	public const string KEY_ACCESSORY_ID = "ACCESSORY_ID";

    public const string KEY_TK_CLOTHE_ID = "CLOTHE_ID";
    public const string KEY_TK_HAT_ID = "HAT_ID";
    /// <summary>
    /// Donation key involve.
    /// </summary>
    public const string KEY_CONSERVATION_ANIMAL_LV = "CONSERVATION_ANIMAL_LV";
    public const string KEY_AIDSFOUNDATION_LV = "AIDSFOUNDATION_LV";
    public const string KEY_LOVEDOGFOUNDATION_LV = "LOVEDOGFOUNDATION_LV";
    public const string KEY_LOVEKIDFOUNDATION_LV = "LOVEKIDFOUNDATION_LV";
    public const string KEY_ECOFOUNDATION_LV = "ECOFOUNDATION_LV";
    public const string KEY_GLOBALWARMING_LV = "KEY_GLOBALWARMING_LV";
    //<@-- Can sell item list storage key.
    public const string KEY_CANSELLGOODSLIST = "KEY_CANSELLGOODSLIST";

    //<@-- Costume Storage Key Data.
    public const string KEY_CAN_EQUIP_CLOTHE_LIST = "KEY_CAN_EQUIP_CLOTHE_LIST";
	public const string KEY_CAN_EQUIP_HAT_LIST = "KEY_CAN_EQUIP_HAT_LIST";
	
	//<@-- Decoration storage key.
	public const string KEY_CAN_DECORATE_ROOF_LIST = "KEY_CAN_DECORATE_ROOF_LIST";
	public const string KEY_CAN_DECORATE_AWNING_LIST = "KEY_CAN_DECORATE_AWNING_LIST";
	public const string KEY_CAN_DECORATE_TABLE_LIST = "KEY_CAN_DECORATE_TABLE_LIST";
	public const string KEY_CAN_DECORATE_ACCESSORIES_LIST = "KEY_CAN_DECORATE_ACCESSORIES_LIST";
	
	
	

	public virtual void LoadSaveDataToGameStorage()
	{		
		Debug.Log("Load storage data to static variable complete.");
	}

    public virtual void SaveDataToPermanentMemory() {
        Debug.Log("SaveDataToPermanentMemory");
    }
}
