using UnityEngine;
using System.Collections;

public class GoodDataStore : ScriptableObject {

	public const int FoodDatabaseCapacity = 24;

    public enum FoodMenuList
    {
        //<!-- Sushi.
        Prawn_sushi = 0, // ซูชิกุ้ง
        Octopus_sushi = 1,
        Sweetened_egg_sushi = 2,
        Crab_sushi = 3,
        Spicy_shell_sushi = 4,
        Salmon_sushi = 5,
        Tuna_sushi = 6,
        Eel_sushi = 7,
        Fatty_tuna_sushi = 8, // ซูชิปลาโทโร
		//<!--- Maki.
        Prawn_maki = 9, // ข้าวห่อสาหร่ายไข่ปลา
        Prawn_brown_maki = 10, // ข้าวห่อสาหร่ายไข่กุ้ง
        California_maki = 11,   // ข้าวห่อสาหร่ายแคลิฟอร์เนียมากิ
        Pickling_cucumber_filled_maki = 12, // ข้าวห่อสาหร่ายแตงกวาดอง
        //<@-- ZXC.
		Miso_soup = 13,
        Kimji = 14, // ซุปเต้าเจี้ยว
		Tempura = 15,
        Zaru_soba = 16, // บะหมี่เย็น
		Ramen = 17,
		Curry_with_rice = 18,
		Yaki_soba = 19,
        //<!--- Sandwich.
        Bean_ice_jam_on_crunching = 20, // ถั่วแดงน้ำแข็งใส
        GreenTea_icecream = 21,
        Hot_greenTea = 22,
        Iced_greenTea = 23,
    };
	
   	public Food[] FoodDatabase_list = new Food[FoodDatabaseCapacity] {                               
	    new Food(FoodMenuList.Prawn_sushi.ToString(), 3),
        new Food(FoodMenuList.Octopus_sushi.ToString(), 3),
        new Food(FoodMenuList.Sweetened_egg_sushi.ToString(), 5),
        new Food(FoodMenuList.Crab_sushi.ToString(), 3),
        new Food(FoodMenuList.Spicy_shell_sushi.ToString(), 5),

	    new Food(FoodMenuList.Salmon_sushi.ToString(), 10),
        new Food(FoodMenuList.Tuna_sushi.ToString(), 10),
        new Food(FoodMenuList.Eel_sushi.ToString(), 10),
        new Food(FoodMenuList.Fatty_tuna_sushi.ToString(), 10), 
        
	    new Food(FoodMenuList.Prawn_maki.ToString(), 13), 
        new Food(FoodMenuList.Prawn_brown_maki.ToString(), 13),
        new Food(FoodMenuList.California_maki.ToString(), 13),
		
        new Food(FoodMenuList.Pickling_cucumber_filled_maki.ToString(), 20),
        new Food(FoodMenuList.Miso_soup.ToString(), 20),
		
        new Food(FoodMenuList.Kimji.ToString(), 25),
        new Food(FoodMenuList.Tempura.ToString(), 25),
        new Food(FoodMenuList.Zaru_soba.ToString(), 25),
        
	    new Food(FoodMenuList.Ramen.ToString(), 8),
        new Food(FoodMenuList.Curry_with_rice.ToString(), 11),
        new Food(FoodMenuList.Yaki_soba.ToString(), 11),

        new Food(FoodMenuList.Bean_ice_jam_on_crunching.ToString(), 13),
        new Food(FoodMenuList.GreenTea_icecream.ToString(), 17),
        new Food(FoodMenuList.Hot_greenTea.ToString(), 21),
        new Food(FoodMenuList.Iced_greenTea.ToString(), 9),
    };
	
    public GoodDataStore() {
        Debug.Log("Starting GoodDataStore");
    }
}
