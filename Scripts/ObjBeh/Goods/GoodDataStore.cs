using UnityEngine;
using System.Collections;

public class GoodDataStore : ScriptableObject {

	public const int FoodDatabaseCapacity = 24;

    public enum FoodMenuList
    {
        //<!-- Sushi.
        Prawn_sushi = 0, // �٪ԡ��
        Octopus_sushi = 1,
        Sweetened_egg_sushi = 2, // �٪�����ҹ
        Crab_sushi = 3,
        Spicy_shell_sushi = 4,
        Salmon_sushi = 5,
        Skipjack_tuna_sushi = 6, // �٪Ի����
        Eel_sushi = 7,
        Fatty_tuna_sushi = 8, // �٪Ի�����
		//<!--- Maki.
        Roe_maki = 9, // ������������������
        Prawn_brown_maki = 10, // ������������������
        Pickling_cucumber_filled_maki = 11, // ��������������ᵧ��Ҵͧ
        California_maki = 12,   // ����������������Կ�������ҡ�

        Ramen = 13,
        Zaru_soba = 14, // ���������
        Yaki_soba = 15,
		Tempura = 16,
        Curry_with_rice = 17,
        
        Miso_soup = 18, // �ػ��������
        Kimji = 19,
        
        Bean_ice_jam_on_crunching = 20, // ����ᴧ�������
        GreenTea_icecream = 21,

        Hot_greenTea = 22,
        Iced_greenTea = 23,
    };
	
   	public Food[] FoodDatabase_list = new Food[FoodDatabaseCapacity] {                               
	    new Food(FoodMenuList.Prawn_sushi.ToString(), 10),
        new Food(FoodMenuList.Octopus_sushi.ToString(), 10),
        new Food(FoodMenuList.Sweetened_egg_sushi.ToString(), 10),
        new Food(FoodMenuList.Crab_sushi.ToString(), 10),
        new Food(FoodMenuList.Spicy_shell_sushi.ToString(), 10),
	    new Food(FoodMenuList.Salmon_sushi.ToString(), 10),
        new Food(FoodMenuList.Skipjack_tuna_sushi.ToString(), 10),
        new Food(FoodMenuList.Eel_sushi.ToString(), 10),
        new Food(FoodMenuList.Fatty_tuna_sushi.ToString(), 10), 
        
	    new Food(FoodMenuList.Roe_maki.ToString(), 12), 
        new Food(FoodMenuList.Prawn_brown_maki.ToString(), 12),
        new Food(FoodMenuList.Pickling_cucumber_filled_maki.ToString(), 12),
        new Food(FoodMenuList.California_maki.ToString(), 15),
        
	    new Food(FoodMenuList.Ramen.ToString(), 30),
        new Food(FoodMenuList.Zaru_soba.ToString(), 25),
        new Food(FoodMenuList.Yaki_soba.ToString(), 25),
        new Food(FoodMenuList.Tempura.ToString(), 30),
        new Food(FoodMenuList.Curry_with_rice.ToString(), 25),
		
        new Food(FoodMenuList.Miso_soup.ToString(), 5),		
        new Food(FoodMenuList.Kimji.ToString(), 5),

        new Food(FoodMenuList.Bean_ice_jam_on_crunching.ToString(), 13),
        new Food(FoodMenuList.GreenTea_icecream.ToString(), 17),

        new Food(FoodMenuList.Hot_greenTea.ToString(), 21),
        new Food(FoodMenuList.Iced_greenTea.ToString(), 9),
    };
	
    public GoodDataStore() {
        Debug.Log("Starting GoodDataStore");
    }
}
