using UnityEngine;
using System.Collections;

public class CharacterCustomization : MonoBehaviour {

    public const int AvailableClothesNumber = 15;
    public string[] arr_clothesNameSpec = new string[AvailableClothesNumber] {
        "Clothe_0001", "Clothe_0002", "Clothe_0003", 
         "Clothe_0004", "Clothe_0005", "Clothe_0006",
          "Clothe_0007", "Clothe_0008", "Clothe_0009",
           "Clothe_0010", "Clothe_0011", "Clothe_0012",  
            "Clothe_0013", "Clothe_0014", "Clothe_0015",
    };
	public Vector3[] arr_clothesOffsetPos = new Vector3[AvailableClothesNumber] {
		Vector3.zero,		Vector3.zero,		Vector3.zero,
		Vector3.zero,		Vector3.zero,		Vector3.zero,
		Vector3.zero,		Vector3.right,		Vector3.zero,
		Vector3.zero,		Vector3.zero,		Vector3.zero,
		Vector3.zero,		Vector3.up,         Vector3.up * 0.5f,
	};
	
    public const int AvailableHatNumber = 23;
    public string[] arrHatNameSpec = new string[AvailableHatNumber] {
		"Hat_0001", "Hat_0002", "Hat_0003", 
        "Hat_0004", "Hat_0005", "Hat_0006", 
        "Hat_0007", "Hat_0008", "Hat_0009", 
        "Hat_0010", "Hat_0011", "Hat_0012", 
        "Hat_0013", "Hat_0014", "Hat_0015", 
        "Hat_0016", "Hat_0017", "Hat_0018", 
        "Hat_0019", "Hat_0020", "Hat_0021", 
        "Hat_0022", "Hat_0023", 
    };
	private Vector3[] arr_hatLocalPos = new Vector3[AvailableHatNumber] {
		Vector3.zero,
		Vector3.zero,
		Vector3.zero,	
		Vector3.zero,
		Vector3.zero,
		Vector3.zero, // 5
		Vector3.right,
		new Vector3(0, -.095f, -4.5f),	
		Vector3.down * 6,
		new Vector3(2.5f, -7f, 0f),	
		new Vector3(0.8f, -7f, 0),	// 10.
		new Vector3(0.85f, -6f, 0),
		new Vector3(1f, 0.8f, 0),	
		Vector3.zero,
		new Vector3(-2f, -5.5f, 0),
		Vector3.down * 22,	        // 15.
		Vector3.zero,	 
		Vector3.zero,
		Vector3.zero,
		Vector3.zero,	
		Vector3.right * 3.9f, // 20.
		Vector3.zero,
		Vector3.zero,
	};

    public tk2dSprite TK_clothe;
    public tk2dSprite TK_hat;
	public tk2dSprite TK_hair;

	// Use this for initialization
	void Start () {
	    if(Mz_StorageManage.TK_clothe_id == 255)
			TK_clothe.gameObject.active = false;
		else 
			this.ChangeClotheAtRuntime(Mz_StorageManage.TK_clothe_id);
		if(Mz_StorageManage.TK_hat_id == 255)
			TK_hat.gameObject.active = false;
		else
			this.ChangeHatAtRuntime(Mz_StorageManage.TK_hat_id);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	private Vector3 originalClothePosition = new Vector3(0, -30, -3);
    public void ChangeClotheAtRuntime(int arr_index) {
        if (arr_index >= AvailableClothesNumber)
        {
            TK_clothe.gameObject.active = false;
			Mz_StorageManage.TK_clothe_id = 255;
            return;
        }
        else {
            TK_clothe.gameObject.active = true;
        }

        TK_clothe.spriteId = TK_clothe.GetSpriteIdByName(arr_clothesNameSpec[arr_index]);
		TK_clothe.transform.localPosition = originalClothePosition + arr_clothesOffsetPos[arr_index];
		Mz_StorageManage.TK_clothe_id = arr_index;
    }

	private Vector3 originalHatPosition = new Vector3(0, 0, -4.5f);
    public void ChangeHatAtRuntime(int arr_index) {
        if (arr_index >= AvailableHatNumber)
        {
            TK_hat.gameObject.active = false;
			Mz_StorageManage.TK_hat_id = 255;
            return;
        }
        else {
            TK_hat.gameObject.active = true;
        }

        TK_hat.spriteId = TK_hat.GetSpriteIdByName(arrHatNameSpec[arr_index]);
		TK_hat.transform.localPosition = originalHatPosition + arr_hatLocalPos[arr_index];
		Mz_StorageManage.TK_hat_id = arr_index;

		if(arr_index == 8 || arr_index == 9 || arr_index == 10 || arr_index == 11) {
			TK_hair.gameObject.active = false;	
		}		
		else {
			TK_hair.gameObject.active = true;
		}
    }
}
