using UnityEngine;
using System.Collections;

public class ManualBeh : ObjectsBeh {

    private string[] arr_foodOrderName = new string[12] {
        "Salmon_form","SkipjackTuna_form","SweetenedEgg_form","Eel_form","FattyTuna_form","SpicyShell_form",
        "Crab_form","Octopus_form","Prawn_form","Cucumber_maki_form","PrawnBrownMaki_form","RoeMaki_form",
    };

    public GameObject manualCookbook;
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
    }

    private void UnActiveCookbookObjectGroup()
    {
        manualCookbook.SetActiveRecursively(false);
        baseScene.plane_darkShadow.active = false;
    }

    internal void Setting_CookbookOrder() {
        form_0.spriteId = form_0.GetSpriteIdByName(arr_foodOrderName[(currentPage_id * 3) + 0]);
        form_1.spriteId = form_1.GetSpriteIdByName(arr_foodOrderName[(currentPage_id * 3) + 1]);
        form_2.spriteId = form_2.GetSpriteIdByName(arr_foodOrderName[(currentPage_id * 3) + 2]);

        int displayId = currentPage_id + 1;
        cookbookPage_textmesh.text = displayId + "/" + MaxPageNumber;
        cookbookPage_textmesh.Commit();
    }

    internal void Handle_onInput(ref string nameInput)
    {
        if (nameInput == "Previous_button") {
            if (currentPage_id > 0)
            {
                currentPage_id--;
                Setting_CookbookOrder();
            }
            else {
                currentPage_id = MaxPageNumber - 1;
                Setting_CookbookOrder();
            }
        }
        else if (nameInput == "Next_button") {
            if (currentPage_id < MaxPageNumber - 1)
            {
                currentPage_id++;
                Setting_CookbookOrder();
            }
            else {
                currentPage_id = 0;
                Setting_CookbookOrder();
            }
        }
        else if (nameInput == "Close_button") {
            SushiShop shop = baseScene.GetComponent<SushiShop>();
            shop.currentGamePlayState = SushiShop.GamePlayState.Ordering;

            this.UnActiveCookbookObjectGroup();
        }
    }
}
