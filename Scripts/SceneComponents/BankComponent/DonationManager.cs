using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


public class ConservationAnimals {
    public static int Level = 0;
    public int[] DonationPrices = new int[5] { 
        500, 1000, 1500, 2000, 2500,
    }; 
};
public class EcoFoundation {
    public static int Level = 0;
    public int[] donationPrice = new int[5] {
        500, 1000, 1500, 2000, 2500,
    };
};
public class AIDSFoundation {
    public static int Level = 0;
    public int[] donationPrice = new int[5] {
        500, 1000, 1500, 2000, 2500,
    };
};
public class LoveDogConsortium { 
    public static int Level = 0;
    public int[] donationPrice = new int[5] {
        500, 1000, 1500, 2000, 2500,
    };
};
public class LoveKidsFoundation { 
    public static int Level = 0;
    public int[] donationPrice = new int[5] {
        500, 1000, 1500, 2000, 2500,
    };
};
public class GlobalWarmingOranization {
    public static int Level = 0;
    public int[] donationPrice = new int[5] {
        500, 1000, 1500, 2000, 2500,
    };
};

public class DonationManager : MonoBehaviour
{
    const string NOTICE_levelMoreThanLimit = "Cannot donation!. Your donation level is more than limit";
    const string NOTICE_accountBalanceLessThanRequire = "Cannot donation !, Your account balance are less than requirement.";


    public const string TOP_RED = "Top_red";
    public const string TOP_ORANGE = "Top_orange";
    public const string TOP_YELLOW = "Top_yellow";
    public const string TOP_LIGHTGREEN = "Top_lightGreen";
    public const string TOP_DARKGREEN = "Top_darkGreen";
    public const string DOWN_RED = "Down_red";
    public const string DOWN_ORANGE = "Down_orange";
    public const string DOWN_YELLOW = "Down_yellow";
    public const string DOWN_LIGHTGREEN = "Down_lightGreen";
    public const string DOWN_DARKGREEN = "Down_darkGreen";
    public const string TOP_DONATEBUTTONNAME = "TopDonate_button";
    public const string DOWN_DONATEBUTTONNAME = "DownDonate_button";

    private string[] arr_nameOfDonationTopic = new string[] {
        "ConservationAnimals_plate", "GlobalAIDFund_plate", "LoveDog_plate",
        "LoveKids_plate", "Eco_plate", "GlobalWarming_plate",
    };

    private SheepBank sceneController;
    ConservationAnimals conservationAnimal = new ConservationAnimals();
    EcoFoundation ecoDonation = new EcoFoundation();
    AIDSFoundation aidsFoundation = new AIDSFoundation();
    LoveDogConsortium loveDogFound = new LoveDogConsortium();
    LoveKidsFoundation loveKidsFound = new LoveKidsFoundation();
    GlobalWarmingOranization globalWarmingORG = new GlobalWarmingOranization();
    public tk2dSprite topicIcon_0;
    public tk2dSprite topicIcon_1;
    public tk2dTextMesh topDonationPrice;
    public tk2dTextMesh downDonationPrice;
    public GameObject[] arr_topBarColor = new GameObject[5];
    public GameObject[] arr_downBarColor = new GameObject[5];
	//@!-- Donation button concerned.
	private int activeDonationButton_id;
	private int unactiveDonationButton_id;
    public GameObject topDonateButton_Obj;
    public GameObject downDonateButton_Obj;
	private tk2dSprite topDonationButton_sprite;
	private tk2dSprite downDonationButton_sprite;
    public tk2dAnimatedSprite topAnimSprite;
    public tk2dAnimatedSprite downAnimSprite;
    public tk2dTextMesh displayPageId_textmesh;
	public tk2dAnimatedSprite congratulationEffect;
    public tk2dSprite medal_reward;

    const int MAX_PageNumber = 3;
    private int currentPageId = 0;
		
	void Awake() {				
        sceneController = GameObject.FindGameObjectWithTag("GameController").GetComponent<SheepBank>();

		topDonationButton_sprite = topDonateButton_Obj.GetComponent<tk2dSprite>();
		downDonationButton_sprite = downDonateButton_Obj.GetComponent<tk2dSprite>();
		//@!-- Find index of donation_button texture;
		activeDonationButton_id = topDonationButton_sprite.GetSpriteIdByName("Donate_button");
		unactiveDonationButton_id = topDonationButton_sprite.GetSpriteIdByName("Donate_button_unactive");
	}
	
    private IEnumerator Start() {
		print("DonationManager.Start");

		congratulationEffect.gameObject.SetActiveRecursively(false);

        yield return new WaitForEndOfFrame();

        topAnimSprite.CurrentClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
        downAnimSprite.CurrentClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
		
        currentPageId = 0;

        this.ResetDatafields();
		this.ChangeTopicIcon();
    }

    internal void ReInitializeData() {     
		congratulationEffect.gameObject.SetActiveRecursively(false);

        this.ResetDatafields();
        this.ReActiveColorBarPicker();
        this.RedrawPageIDTexmesh();
    }

    private void ResetDatafields()
    {			
		for (int i = 0; i < arr_topBarColor.Length; i++) {
			arr_topBarColor[i].active = false;
		}
        for (int i = 0; i < arr_downBarColor.Length; i++) {
            arr_downBarColor[i].active = false;
        }
    }

    private void ReActiveColorBarPicker()
    {
        //<@-- Active color bar button.
        int temp_topLv = 0;
        int temp_downLv = 0;
        if (currentPageId == 0) {
            temp_topLv = ConservationAnimals.Level;
            temp_downLv = AIDSFoundation.Level;

	        topDonationPrice.text = conservationAnimal.DonationPrices[ConservationAnimals.Level].ToString();
	        topDonationPrice.Commit();
            topDonationButton_sprite.spriteId = ConservationAnimals.Level < 4 ? activeDonationButton_id : unactiveDonationButton_id;

	        downDonationPrice.text = aidsFoundation.donationPrice[AIDSFoundation.Level].ToString();
	        downDonationPrice.Commit();
            downDonationButton_sprite.spriteId = AIDSFoundation.Level < 4 ? activeDonationButton_id : unactiveDonationButton_id;
        }
        else if (currentPageId == 1) {
            temp_topLv = LoveDogConsortium.Level;
            temp_downLv = LoveKidsFoundation.Level;
			
	        topDonationPrice.text = loveDogFound.donationPrice[LoveDogConsortium.Level].ToString();
	        topDonationPrice.Commit();
            topDonationButton_sprite.spriteId = LoveDogConsortium.Level < 4 ? activeDonationButton_id : unactiveDonationButton_id;

	        downDonationPrice.text = loveKidsFound.donationPrice[LoveKidsFoundation.Level].ToString();
	        downDonationPrice.Commit();
            downDonationButton_sprite.spriteId = LoveKidsFoundation.Level < 4 ? activeDonationButton_id : unactiveDonationButton_id;
        }
        else if (currentPageId == 2) {
            temp_topLv = EcoFoundation.Level;
            temp_downLv = GlobalWarmingOranization.Level;
			
	        topDonationPrice.text = ecoDonation.donationPrice[EcoFoundation.Level].ToString();
	        topDonationPrice.Commit();
            topDonationButton_sprite.spriteId = EcoFoundation.Level < 4 ? activeDonationButton_id : unactiveDonationButton_id;

            downDonationPrice.text =  globalWarmingORG.donationPrice[GlobalWarmingOranization.Level].ToString();
            downDonationPrice.Commit();
            downDonationButton_sprite.spriteId = GlobalWarmingOranization.Level < 4 ? activeDonationButton_id : unactiveDonationButton_id;
        }

        switch (temp_topLv) {
            case 0:
                arr_topBarColor[0].active = true;
                break;
                case 1:
                    arr_topBarColor[0].active = true;
                    arr_topBarColor[1].active = true;
                    break;
                case 2:
                    for (int i = 0; i <= 2; i++)
                        arr_topBarColor[i].active = true;
                    break;
                case 3:
                    for (int i = 0; i <= 3; i++)
                        arr_topBarColor[i].active = true;
                    break;
                case 4:
                    for (int i = 0; i <= 4; i++)
                        arr_topBarColor[i].active = true;
                    break;
                default:
                    break;
        }

        switch (temp_downLv)
        {
            case 0:
                arr_downBarColor[0].active = true;
                break;
            case 1:
                arr_downBarColor[0].active = true;
                arr_downBarColor[1].active = true;
                break;
            case 2:
                for (int i = 0; i <= 2; i++)
                    arr_downBarColor[i].active = true;
                break;
            case 3:
                for (int i = 0; i <= 3; i++)
                    arr_downBarColor[i].active = true;
                break;
            case 4:
                for (int i = 0; i <= 4; i++)
                    arr_downBarColor[i].active = true;
                break;
            default:
                break;
        }
    }

    private void RedrawPageIDTexmesh()
    {
        int pageId = currentPageId + 1;
        displayPageId_textmesh.text = pageId + "/" + MAX_PageNumber;
        displayPageId_textmesh.Commit();
    }

    internal void PreviousDonationPage() {
        if (currentPageId > 0)
            currentPageId--;
        else
            currentPageId = MAX_PageNumber - 1;

        // Do something.
        this.ChangeTopicIcon();
    }
    public void NextDonationPage() {
        if (currentPageId < MAX_PageNumber - 1)
            currentPageId++;
        else
            currentPageId = 0;

        // Do something.
        this.ChangeTopicIcon();
    }
    private void ChangeTopicIcon()
    {
        if (currentPageId == 0) {
            topicIcon_0.spriteId = topicIcon_0.GetSpriteIdByName(arr_nameOfDonationTopic[0]);
            topicIcon_1.spriteId = topicIcon_1.GetSpriteIdByName(arr_nameOfDonationTopic[1]);
        }
        else if (currentPageId == 1) {
            topicIcon_0.spriteId = topicIcon_0.GetSpriteIdByName(arr_nameOfDonationTopic[2]);
            topicIcon_1.spriteId = topicIcon_1.GetSpriteIdByName(arr_nameOfDonationTopic[3]);
        }
        else if (currentPageId == 2) {
            topicIcon_0.spriteId = topicIcon_0.GetSpriteIdByName(arr_nameOfDonationTopic[4]);
            topicIcon_1.spriteId = topicIcon_1.GetSpriteIdByName(arr_nameOfDonationTopic[5]);
        }

        this.ReInitializeData();
        print("DonationManager.ChangeTopicIcon");
    }

    internal void GetInput (string inputName)
	{
		switch (inputName) {
		case TOP_RED:
			#region <@!-- Top red button.
			if (currentPageId == 0) {
				topDonationPrice.text = conservationAnimal.DonationPrices [0].ToString ();
				topDonationPrice.Commit ();

				if (ConservationAnimals.Level > 0)
						topDonationButton_sprite.spriteId = unactiveDonationButton_id;
				else 
						topDonationButton_sprite.spriteId = activeDonationButton_id;
			} 
			else if (currentPageId == 1) {
				topDonationPrice.text = loveDogFound.donationPrice [0].ToString ();
				topDonationPrice.Commit ();

				if (LoveDogConsortium.Level > 0)
						topDonationButton_sprite.spriteId = unactiveDonationButton_id;
				else 
						topDonationButton_sprite.spriteId = activeDonationButton_id;
			}
			else if (currentPageId == 2) {
				topDonationPrice.text = ecoDonation.donationPrice [0].ToString ();
				topDonationPrice.Commit ();
			
				if (EcoFoundation.Level > 0)
					topDonationButton_sprite.spriteId = unactiveDonationButton_id;
				else 
					topDonationButton_sprite.spriteId = activeDonationButton_id;
			}
			#endregion
			break;
		case TOP_ORANGE:
			#region <@!-- TOP_ORANGE.
				if (currentPageId == 0) {
						topDonationPrice.text = conservationAnimal.DonationPrices [1].ToString ();
						topDonationPrice.Commit ();
				
					if (ConservationAnimals.Level > 1)
						topDonationButton_sprite.spriteId = unactiveDonationButton_id;
					else 
						topDonationButton_sprite.spriteId = activeDonationButton_id;
				} else if (currentPageId == 1) {
						topDonationPrice.text = loveDogFound.donationPrice [1].ToString ();
						topDonationPrice.Commit ();
				
					if (LoveDogConsortium.Level > 1)
						topDonationButton_sprite.spriteId = unactiveDonationButton_id;
					else 
						topDonationButton_sprite.spriteId = activeDonationButton_id;
				} else if (currentPageId == 2) {
						topDonationPrice.text = ecoDonation.donationPrice [1].ToString ();
						topDonationPrice.Commit ();
				
					if (EcoFoundation.Level > 1)
						topDonationButton_sprite.spriteId = unactiveDonationButton_id;
					else 
						topDonationButton_sprite.spriteId = activeDonationButton_id;
				}
			#endregion
				break;
		case TOP_YELLOW:
			#region <@!-- TOP_YELLOW.
			if (currentPageId == 0) {
					topDonationPrice.text = conservationAnimal.DonationPrices [2].ToString ();
					topDonationPrice.Commit ();
			
				if (ConservationAnimals.Level > 2)
					topDonationButton_sprite.spriteId = unactiveDonationButton_id;
				else 
					topDonationButton_sprite.spriteId = activeDonationButton_id;
			}
			else if (currentPageId == 1) {
					topDonationPrice.text = loveDogFound.donationPrice [2].ToString ();
					topDonationPrice.Commit ();
			
				if (LoveDogConsortium.Level > 2)
					topDonationButton_sprite.spriteId = unactiveDonationButton_id;
				else 
					topDonationButton_sprite.spriteId = activeDonationButton_id;
			} 
			else if (currentPageId == 2) {
					topDonationPrice.text = ecoDonation.donationPrice [2].ToString ();
					topDonationPrice.Commit ();
			
				if (EcoFoundation.Level > 2)
					topDonationButton_sprite.spriteId = unactiveDonationButton_id;
				else 
					topDonationButton_sprite.spriteId = activeDonationButton_id;
			}
			#endregion
			break;
		case TOP_LIGHTGREEN:
			#region <@!-- TOP_LIGHTGREEN.
			if (currentPageId == 0) {
				topDonationPrice.text = conservationAnimal.DonationPrices [3].ToString ();
				topDonationPrice.Commit ();
			
				if (ConservationAnimals.Level > 3)
					topDonationButton_sprite.spriteId = unactiveDonationButton_id;
				else 
					topDonationButton_sprite.spriteId = activeDonationButton_id;
			} 
			else if (currentPageId == 1) {
				topDonationPrice.text = loveDogFound.donationPrice [3].ToString ();
				topDonationPrice.Commit ();
			
				if (LoveDogConsortium.Level > 3)
					topDonationButton_sprite.spriteId = unactiveDonationButton_id;
				else 
					topDonationButton_sprite.spriteId = activeDonationButton_id;
			}
			else if (currentPageId == 2) {
				topDonationPrice.text = ecoDonation.donationPrice [3].ToString ();
				topDonationPrice.Commit ();
			
				if (EcoFoundation.Level > 3)
					topDonationButton_sprite.spriteId = unactiveDonationButton_id;
				else 
					topDonationButton_sprite.spriteId = activeDonationButton_id;
			}
			#endregion
			break;
		case TOP_DARKGREEN:
			#region <@-- TOP_DARKGREEN.

			if (currentPageId == 0) {
					topDonationPrice.text = conservationAnimal.DonationPrices [4].ToString ();
					topDonationPrice.Commit ();
			
				if (ConservationAnimals.Level >= 4)
					topDonationButton_sprite.spriteId = unactiveDonationButton_id;
				else 
					topDonationButton_sprite.spriteId = activeDonationButton_id;
			}
			else if (currentPageId == 1) {
				topDonationPrice.text = loveDogFound.donationPrice [4].ToString ();
				topDonationPrice.Commit ();
			
				if (LoveDogConsortium.Level >= 4)
					topDonationButton_sprite.spriteId = unactiveDonationButton_id;
				else 
					topDonationButton_sprite.spriteId = activeDonationButton_id;
			} 
			else if (currentPageId == 2) {
				topDonationPrice.text = ecoDonation.donationPrice [4].ToString ();
				topDonationPrice.Commit ();
			
				if (EcoFoundation.Level >= 4)
					topDonationButton_sprite.spriteId = unactiveDonationButton_id;
				else 
					topDonationButton_sprite.spriteId = activeDonationButton_id;
			}

			#endregion
			break;
		case DOWN_RED:
			#region <!-- DOWN_RED.

			if (currentPageId == 0) {
				downDonationPrice.text = aidsFoundation.donationPrice [0].ToString ();
				downDonationPrice.Commit ();
			
				if (AIDSFoundation.Level > 0)
					downDonationButton_sprite.spriteId = unactiveDonationButton_id;
				else 
					downDonationButton_sprite.spriteId = activeDonationButton_id;
			}
			else if (currentPageId == 1) {
				downDonationPrice.text = loveKidsFound.donationPrice [0].ToString ();
				downDonationPrice.Commit ();
			
				if (LoveKidsFoundation.Level > 0)
					downDonationButton_sprite.spriteId = unactiveDonationButton_id;
				else 
					downDonationButton_sprite.spriteId = activeDonationButton_id;
			} 
			else if (currentPageId == 2) {
                downDonationPrice.text = globalWarmingORG.donationPrice[0].ToString();
                downDonationPrice.Commit();

                if (GlobalWarmingOranization.Level > 0)
                    downDonationButton_sprite.spriteId = unactiveDonationButton_id;
                else
                    downDonationButton_sprite.spriteId = activeDonationButton_id;
			}

			#endregion
				break;
		case DOWN_ORANGE:
			#region <!-- DOWN_ORANGE.

			if (currentPageId == 0) {
				downDonationPrice.text = aidsFoundation.donationPrice [1].ToString ();
				downDonationPrice.Commit ();
			
				if (AIDSFoundation.Level > 1)
					downDonationButton_sprite.spriteId = unactiveDonationButton_id;
				else 
					downDonationButton_sprite.spriteId = activeDonationButton_id;
			} 
			else if (currentPageId == 1) {
				downDonationPrice.text = loveKidsFound.donationPrice [1].ToString ();
				downDonationPrice.Commit ();
			
				if (LoveKidsFoundation.Level > 1)
					downDonationButton_sprite.spriteId = unactiveDonationButton_id;
				else 
					downDonationButton_sprite.spriteId = activeDonationButton_id;
				}
            else if (currentPageId == 2)
            {
                downDonationPrice.text = globalWarmingORG.donationPrice[1].ToString();
                downDonationPrice.Commit();

                if (GlobalWarmingOranization.Level > 1)
                    downDonationButton_sprite.spriteId = unactiveDonationButton_id;
                else
                    downDonationButton_sprite.spriteId = activeDonationButton_id;
			}

			#endregion
				break;
		case DOWN_YELLOW:
			#region <!-- DOWN_YELLOW.

			if (currentPageId == 0) {
				downDonationPrice.text = aidsFoundation.donationPrice [2].ToString ();
				downDonationPrice.Commit ();
			
				if (AIDSFoundation.Level > 2)
					downDonationButton_sprite.spriteId = unactiveDonationButton_id;
				else 
					downDonationButton_sprite.spriteId = activeDonationButton_id;
			} 
			else if (currentPageId == 1) {
				downDonationPrice.text = loveKidsFound.donationPrice [2].ToString ();
				downDonationPrice.Commit ();
			
				if (LoveKidsFoundation.Level > 2)
					downDonationButton_sprite.spriteId = unactiveDonationButton_id;
				else 
					downDonationButton_sprite.spriteId = activeDonationButton_id;
			}
            else if (currentPageId == 2)
            {
                downDonationPrice.text = globalWarmingORG.donationPrice[2].ToString();
                downDonationPrice.Commit();

                if (GlobalWarmingOranization.Level > 2)
                    downDonationButton_sprite.spriteId = unactiveDonationButton_id;
                else
                    downDonationButton_sprite.spriteId = activeDonationButton_id;
			}

			#endregion
			break;
		case DOWN_LIGHTGREEN:
			#region <!-- DOWN_LIGHTGREEN.

			if (currentPageId == 0) {
				downDonationPrice.text = aidsFoundation.donationPrice [3].ToString ();
				downDonationPrice.Commit ();
			
				if (AIDSFoundation.Level > 3)
					downDonationButton_sprite.spriteId = unactiveDonationButton_id;
				else 
					downDonationButton_sprite.spriteId = activeDonationButton_id;
			}
			else if (currentPageId == 1) {
				downDonationPrice.text = loveKidsFound.donationPrice [3].ToString ();
				downDonationPrice.Commit ();
			
				if (LoveKidsFoundation.Level > 3)
					downDonationButton_sprite.spriteId = unactiveDonationButton_id;
				else 
					downDonationButton_sprite.spriteId = activeDonationButton_id;
			}
            else if (currentPageId == 2)
            {
                downDonationPrice.text = globalWarmingORG.donationPrice[3].ToString();
                downDonationPrice.Commit();

                if (GlobalWarmingOranization.Level > 3)
                    downDonationButton_sprite.spriteId = unactiveDonationButton_id;
                else
                    downDonationButton_sprite.spriteId = activeDonationButton_id;
			}

			#endregion
			break;
		case DOWN_DARKGREEN:
            #region <@!-- DOWN_DARKGREEN.

            if (currentPageId == 0) {
				downDonationPrice.text = aidsFoundation.donationPrice [4].ToString ();
                downDonationPrice.Commit();

                if (AIDSFoundation.Level >= 4)
                    downDonationButton_sprite.spriteId = unactiveDonationButton_id;
                else
                    downDonationButton_sprite.spriteId = activeDonationButton_id;
			}
            else if (currentPageId == 1) {
				downDonationPrice.text = loveKidsFound.donationPrice [4].ToString ();
                downDonationPrice.Commit();

                if (LoveKidsFoundation.Level >= 4)
                    downDonationButton_sprite.spriteId = unactiveDonationButton_id;
                else
                    downDonationButton_sprite.spriteId = activeDonationButton_id;
			}
            else if (currentPageId == 2)
            {
                downDonationPrice.text = globalWarmingORG.donationPrice[4].ToString();
                downDonationPrice.Commit();

                if (GlobalWarmingOranization.Level > 4)
                    downDonationButton_sprite.spriteId = unactiveDonationButton_id;
                else
                    downDonationButton_sprite.spriteId = activeDonationButton_id;
			}

            #endregion
            break;
		case TOP_DONATEBUTTONNAME:
            if (currentPageId == 0) {
                //<@!-- ConservationAnimals.
                if (ConservationAnimals.Level < conservationAnimal.DonationPrices.Length - 1)
                    this.DonationProcessing(arr_nameOfDonationTopic[0]);
                else
					this.WarningCannotDonation(NOTICE_levelMoreThanLimit);
            }
            else if (currentPageId == 1) {
                //<@!-- LoveDogConsortium.
                if (LoveDogConsortium.Level < loveDogFound.donationPrice.Length - 1)
                    this.DonationProcessing(arr_nameOfDonationTopic[2]);
                else
                    this.WarningCannotDonation(NOTICE_levelMoreThanLimit);
            }
            else if (currentPageId == 2) {
                //<@!-- EcoFoundation.
                if(EcoFoundation.Level < ecoDonation.donationPrice.Length -1)
                    this.DonationProcessing(arr_nameOfDonationTopic[4]);
                else
                    this.WarningCannotDonation(NOTICE_levelMoreThanLimit);
            }
            break;
		case DOWN_DONATEBUTTONNAME:
				if (currentPageId == 0) {
                    //<@!-- AIDSFoundation.
                    if (AIDSFoundation.Level < aidsFoundation.donationPrice.Length - 1)
                        this.DonationProcessing(arr_nameOfDonationTopic[1]);
                    else
                        this.WarningCannotDonation(NOTICE_levelMoreThanLimit);
                }
				else if (currentPageId == 1) {
                    //<@!-- LoveKidsFoundation.
                    if(LoveKidsFoundation.Level < loveKidsFound.donationPrice.Length -1)
                        this.DonationProcessing (arr_nameOfDonationTopic [3]);
                    else
                        this.WarningCannotDonation(NOTICE_levelMoreThanLimit);
                }
                else if (currentPageId == 2)
                {
                    //<@-- GlobalWarmingOranization.
                    if (GlobalWarmingOranization.Level < globalWarmingORG.donationPrice.Length - 1)
                        this.DonationProcessing(arr_nameOfDonationTopic[5]);
                    else
                        this.WarningCannotDonation(NOTICE_levelMoreThanLimit);
                }
				break;
		default:
				break;
		}
	}

    private void DonationProcessing(string targetDonation)
    {
        int currentPriceToDonate = 0;
        if (targetDonation == arr_nameOfDonationTopic[0])
        {
            currentPriceToDonate = conservationAnimal.DonationPrices[ConservationAnimals.Level];
            if (currentPriceToDonate <= Mz_StorageManage.AccountBalance)
            {
                //@! Can donation.
                Mz_StorageManage.AccountBalance -= currentPriceToDonate;
                sceneController.gameEffectManager.Create2DSpriteAnimationEffect(GameEffectManager.BLOOMSTAR_EFFECT_PATH, topDonateButton_Obj.transform);
                sceneController.audioEffect.PlayOnecWithOutStop(sceneController.audioEffect.longBring_clip);

                ConservationAnimals.Level++;
                StartCoroutine(this.ActiveCongratulationEffect(ConservationAnimals.Level));
                this.ReActiveColorBarPicker();
				sceneController.ManageAvailabelMoneyBillBoard();

				UpgradeOutsideManager.CanAlimentPet_id_list.Add(ConservationAnimals.Level + 1);

                print("DonationProcessing... complete !");
            }
            else
                WarningCannotDonation(NOTICE_accountBalanceLessThanRequire);
        }
        else if (targetDonation == arr_nameOfDonationTopic[1])
        {
            currentPriceToDonate = aidsFoundation.donationPrice[AIDSFoundation.Level];
            if (currentPriceToDonate <= Mz_StorageManage.AccountBalance)
            {
                Mz_StorageManage.AccountBalance -= currentPriceToDonate;
                sceneController.gameEffectManager.Create2DSpriteAnimationEffect(GameEffectManager.BLOOMSTAR_EFFECT_PATH, downDonateButton_Obj.transform);
                sceneController.audioEffect.PlayOnecWithOutStop(sceneController.audioEffect.longBring_clip);

                AIDSFoundation.Level++;
                StartCoroutine(this.ActiveCongratulationEffect(AIDSFoundation.Level));
                this.ReActiveColorBarPicker();
				sceneController.ManageAvailabelMoneyBillBoard();

                print("DonationProcessing... complete !");
            }
            else
                WarningCannotDonation(NOTICE_accountBalanceLessThanRequire);
        }
        else if (targetDonation == arr_nameOfDonationTopic[2])
        {
            currentPriceToDonate = loveDogFound.donationPrice[LoveDogConsortium.Level];
            if (currentPriceToDonate <= Mz_StorageManage.AccountBalance)
            {
                Mz_StorageManage.AccountBalance -= currentPriceToDonate;
                sceneController.gameEffectManager.Create2DSpriteAnimationEffect(GameEffectManager.BLOOMSTAR_EFFECT_PATH, topDonateButton_Obj.transform);
                sceneController.audioEffect.PlayOnecWithOutStop(sceneController.audioEffect.longBring_clip);

				LoveDogConsortium.Level++;
                StartCoroutine(this.ActiveCongratulationEffect(LoveDogConsortium.Level));
				this.ReActiveColorBarPicker();
				sceneController.ManageAvailabelMoneyBillBoard();

				if(UpgradeOutsideManager.CanAlimentPet_id_list.Contains(1) == false)
					UpgradeOutsideManager.CanAlimentPet_id_list.Add(1);

                print("DonationProcessing... complete !");
            }
            else
                WarningCannotDonation(NOTICE_accountBalanceLessThanRequire);
        }
        else if (targetDonation == arr_nameOfDonationTopic[3]) {
            currentPriceToDonate = loveKidsFound.donationPrice[LoveKidsFoundation.Level];
            if (currentPriceToDonate <= Mz_StorageManage.AccountBalance)
            {
                Mz_StorageManage.AccountBalance -= currentPriceToDonate;
                sceneController.gameEffectManager.Create2DSpriteAnimationEffect(GameEffectManager.BLOOMSTAR_EFFECT_PATH, downDonateButton_Obj.transform);
                sceneController.audioEffect.PlayOnecWithOutStop(sceneController.audioEffect.longBring_clip);

				LoveKidsFoundation.Level++;
                StartCoroutine(this.ActiveCongratulationEffect(LoveKidsFoundation.Level));
				this.ReActiveColorBarPicker();
				sceneController.ManageAvailabelMoneyBillBoard();

				if(UpgradeOutsideManager.CanAlimentPet_id_list.Contains(6) == false)
					UpgradeOutsideManager.CanAlimentPet_id_list.Add(6);

                print("DonationProcessing... complete !");
            }
            else
                WarningCannotDonation(NOTICE_accountBalanceLessThanRequire);
        }
        else if (targetDonation == arr_nameOfDonationTopic[4])
        {
            currentPriceToDonate = ecoDonation.donationPrice[EcoFoundation.Level];
            if (currentPriceToDonate <= Mz_StorageManage.AccountBalance)
            {
                Mz_StorageManage.AccountBalance -= currentPriceToDonate;
                sceneController.gameEffectManager.Create2DSpriteAnimationEffect(GameEffectManager.BLOOMSTAR_EFFECT_PATH, topDonateButton_Obj.transform);
                sceneController.audioEffect.PlayOnecWithOutStop(sceneController.audioEffect.longBring_clip);

				EcoFoundation.Level ++;
                StartCoroutine(this.ActiveCongratulationEffect(EcoFoundation.Level));
				this.ReActiveColorBarPicker();
				sceneController.ManageAvailabelMoneyBillBoard();

                print("DonationProcessing... complete !");
            }
            else
                WarningCannotDonation(NOTICE_accountBalanceLessThanRequire);
        }
        else if (targetDonation == arr_nameOfDonationTopic[5]) {
            currentPriceToDonate = globalWarmingORG.donationPrice[GlobalWarmingOranization.Level];
            if (currentPriceToDonate <= Mz_StorageManage.AccountBalance)
            {
                Mz_StorageManage.AccountBalance -= currentPriceToDonate;
                sceneController.gameEffectManager.Create2DSpriteAnimationEffect(GameEffectManager.BLOOMSTAR_EFFECT_PATH, downDonateButton_Obj.transform);
                sceneController.audioEffect.PlayOnecWithOutStop(sceneController.audioEffect.longBring_clip);

                GlobalWarmingOranization.Level++;
                StartCoroutine(this.ActiveCongratulationEffect(GlobalWarmingOranization.Level));
                this.ReActiveColorBarPicker();
				sceneController.ManageAvailabelMoneyBillBoard();

                print("DonationProcessing... complete !");
            }
            else
                WarningCannotDonation(NOTICE_accountBalanceLessThanRequire);
        }
    }

	IEnumerator ActiveCongratulationEffect (int level)
	{
		Debug.Log("ActiveCongratulationEffect");

		sceneController.shadowPlane_Obj.transform.position += Vector3.back * 20; 
		congratulationEffect.gameObject.SetActiveRecursively(true);

        switch (level)
        {
            case 1: medal_reward.spriteId = medal_reward.GetSpriteIdByName(DisplayRewards_Scene.MEDAL_Bronze);
                break;
            case 2: medal_reward.spriteId = medal_reward.GetSpriteIdByName(DisplayRewards_Scene.MEDAL_Copper);
                break;
            case 3: medal_reward.spriteId = medal_reward.GetSpriteIdByName(DisplayRewards_Scene.MEDAL_Silver);
                break;
            case 4: medal_reward.spriteId = medal_reward.GetSpriteIdByName(DisplayRewards_Scene.MEDAL_Gold);
                break;
            case 5: medal_reward.spriteId = medal_reward.GetSpriteIdByName(DisplayRewards_Scene.MEDAL_Crystal);
                break;
            default:
                break;
        }

		yield return new WaitForSeconds(3);

		sceneController.shadowPlane_Obj.transform.position += Vector3.forward * 20;
        congratulationEffect.gameObject.SetActiveRecursively(false);
	}

	void WarningCannotDonation (string noticeMessage)
	{
		sceneController.audioEffect.PlayOnecWithOutStop(sceneController.audioEffect.wrong_Clip);
		Debug.LogWarning(noticeMessage);
	}
}
