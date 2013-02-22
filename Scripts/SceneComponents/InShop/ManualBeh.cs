using UnityEngine;
using System.Collections;

public class ManualBeh : ObjectsBeh {
	private readonly string[] arr_titleImgName = new string[12] {
		GoodDataStore.FoodMenuList.Salmon_sushi.ToString(),
		GoodDataStore.FoodMenuList.Skipjack_tuna_sushi.ToString(),
		GoodDataStore.FoodMenuList.Sweetened_egg_sushi.ToString(),
		GoodDataStore.FoodMenuList.Eel_sushi.ToString(),
		GoodDataStore.FoodMenuList.Fatty_tuna_sushi.ToString(),
		GoodDataStore.FoodMenuList.Spicy_shell_sushi.ToString(),
		GoodDataStore.FoodMenuList.Crab_sushi.ToString(),
		GoodDataStore.FoodMenuList.Octopus_sushi.ToString(),
		GoodDataStore.FoodMenuList.Prawn_sushi.ToString(),
		GoodDataStore.FoodMenuList.Pickling_cucumber_filled_maki.ToString(),
		GoodDataStore.FoodMenuList.Prawn_brown_maki.ToString(),
		GoodDataStore.FoodMenuList.Roe_maki.ToString(),
	};

    private readonly string[] arr_foodOrderName = new string[12] {
        "Salmon_form","SkipjackTuna_form","SweetenedEgg_form","Eel_form","FattyTuna_form","SpicyShell_form",
        "Crab_form","Octopus_form","Prawn_form","Cucumber_maki_form","PrawnBrownMaki_form","RoeMaki_form",
    };

    public GameObject manualCookbook;
	private tk2dAnimatedSprite cookbook_animatedSprite;
	public tk2dSprite[] titles_sprite = new tk2dSprite[3];
    public tk2dSprite form_0;
    public tk2dSprite form_1;
    public tk2dSprite form_2;

    public tk2dTextMesh cookbookPage_textmesh;

    private int currentPage_id = 0;
    private const int MaxPageNumber = 4;


	// Use this for initialization
    protected override void Start()
    {
        base.Start();

        cookbook_animatedSprite = manualCookbook.GetComponent<tk2dAnimatedSprite>();
    }
	
	// Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    internal void OnActiveCookbook()
    {
        this.animation.Play("ManualAnim");
        StartCoroutine_Auto(CheckingUnityAnimationComplete.ICheckAnimationComplete(this.animation, "ManualAnim", this.gameObject, "ActiveCookbookObjectGroup"));
    }

    private void ActiveCookbookObjectGroup() {
        manualCookbook.SetActiveRecursively(true);
        baseScene.plane_darkShadow.active = true;
		this.Setting_CookbookOrder ();
    }

    private void UnActiveCookbookObjectGroup()
    {
        manualCookbook.SetActiveRecursively(false);
        baseScene.plane_darkShadow.active = false;
    }

    internal void Setting_CookbookOrder() {
		for (int i = 0; i < titles_sprite.Length; i++) {
			titles_sprite [i].spriteId = titles_sprite [i].GetSpriteIdByName (arr_titleImgName [(currentPage_id * 3) + i]);
		}

        form_0.spriteId = form_0.GetSpriteIdByName(arr_foodOrderName[(currentPage_id * 3) + 0]);
        form_1.spriteId = form_1.GetSpriteIdByName(arr_foodOrderName[(currentPage_id * 3) + 1]);
        form_2.spriteId = form_2.GetSpriteIdByName(arr_foodOrderName[(currentPage_id * 3) + 2]);

        int displayId = currentPage_id + 1;
        cookbookPage_textmesh.text = displayId + "/" + MaxPageNumber;
        cookbookPage_textmesh.Commit();
    }
	
	private void ActivateCookbookFormOrder(bool p_active) {
		foreach (var item in titles_sprite) {
			item.gameObject.active = p_active;
		}

		form_0.gameObject.active = p_active;
		form_1.gameObject.active = p_active;
		form_2.gameObject.active = p_active;
	}

    internal void Handle_onInput(ref string nameInput)
    {
        if (nameInput == "Previous_button") {
            cookbook_animatedSprite.Play("Playback");
            this.ActivateCookbookFormOrder(false);
            cookbook_animatedSprite.animationCompleteDelegate = delegate(tk2dAnimatedSprite sprite, int clipId) {
                this.ActivateCookbookFormOrder(true);
                if (currentPage_id > 0)
                {
                    currentPage_id--;
                    Setting_CookbookOrder();
                }
                else
                {
                    currentPage_id = MaxPageNumber - 1;
                    Setting_CookbookOrder();
                }
            };
        }
        else if (nameInput == "Next_button") {
			cookbook_animatedSprite.Play("Play");
            this.ActivateCookbookFormOrder(false);
            cookbook_animatedSprite.animationCompleteDelegate = (sprite, clipId) => {
                this.ActivateCookbookFormOrder(true);
	            if (currentPage_id < MaxPageNumber - 1) {
	                currentPage_id++;
	                Setting_CookbookOrder();
	            }
	            else {
	                currentPage_id = 0;
	                Setting_CookbookOrder();
	            }
			};
        }
        else if (nameInput == "Close_button") {
            SushiShop shop = baseScene.GetComponent<SushiShop>();
            shop.currentGamePlayState = SushiShop.GamePlayState.Ordering;

            this.UnActiveCookbookObjectGroup();
        }
    }
}
