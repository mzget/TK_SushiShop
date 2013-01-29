using UnityEngine;
using System.Collections;

public class RewardManager : MonoBehaviour
{
	public tk2dSprite titleIcon_0;
	public tk2dSprite titleIcon_1;
	public tk2dSprite titleIcon_2;
    public tk2dAnimatedSprite[] spriteEffects = new tk2dAnimatedSprite[3];
	
	public GameObject[] arr_medals_Low0 = new GameObject[5];
	public GameObject[] arr_medals_Low1 = new GameObject[5];
	public GameObject[] arr_medals_Low2 = new GameObject[5];

	public tk2dTextMesh displayPageId_textmesh;
 	
	private const int MAX_PAGENUMBER = 2;
	private int currentPageID = 0;
	private string[] arr_nameOfPlates = new string[6] {
		"ConservationAnimals_plate", "GlobalAIDFund_plate", "LoveDog_plate",
        "LoveKids_plate", "Eco_plate", "GlobalWarming_plate",
	};
	
	// Use this for initialization
	private IEnumerator Start ()
	{
		this.CheckAvailabelDataFields();

		yield return new WaitForEndOfFrame ();

		currentPageID = 0;

		foreach (tk2dAnimatedSprite item in spriteEffects) {
            item.CurrentClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
		}

		this.ResetActiveAvailableMedal();
		this.SetActiveAvailableMedal();
	}

	private void CheckAvailabelDataFields ()
	{
		if(displayPageId_textmesh == null)
			Debug.LogError("displayPageId_textmesh == null");
	}

	private void ResetActiveAvailableMedal ()
	{		
		foreach (var item in arr_medals_Low0) {
			item.active = false;
		}
		foreach (var item in arr_medals_Low1) {
			item.active = false;
		}
		foreach (var item in arr_medals_Low2) {
			item.active = false;
		}
	}
	
	/// <summary>
	/// Sets the active available plate.
	/// </summary>
	/// Call when 
	/// 1. initailizetion all medals and,
	/// 2. when user have change page display.
	private void SetActiveAvailableMedal ()	{
		if (currentPageID == 0) {
			for (int i = 0; i < ConservationAnimals.Level; i++) {
				arr_medals_Low0[i].active = true;
			}
			for (int i = 0; i < AIDSFoundation.Level; i++) {
				arr_medals_Low1[i].active = true;
			}
			for (int i = 0; i < LoveDogConsortium.Level; i++) {
				arr_medals_Low2[i].active = true;
			}
		}
		else if(currentPageID == 1) {
			for (int i = 0; i < LoveKidsFoundation.Level; i++) {
				arr_medals_Low0[i].active = true;
			}
			for (int i = 0; i < EcoFoundation.Level; i++) {
				arr_medals_Low1[i].active = true;
			}
            for (int i = 0; i < GlobalWarmingOranization.Level; i++)
            {
                arr_medals_Low2[i].active = true;
            }
		}

		this.ChangeDisplayPageIdText();
	}

	private void ChangeDisplayPageIdText ()
	{
        int temp_CurrentId = currentPageID + 1;
		displayPageId_textmesh.text = temp_CurrentId + "/" + MAX_PAGENUMBER;
		displayPageId_textmesh.Commit();
	}
	
	// Update is called once per frame
//	void Update ()
//	{
//	
//	}
		
	internal void HaveNextPageCommand() {
		if(currentPageID < MAX_PAGENUMBER - 1)
			currentPageID++;
		else 
			currentPageID = 0;

		this.ChangePageProcessing();
	}
	
	internal void HavePreviousCommand() {
		if(currentPageID > 0)
			currentPageID--;
		else
			currentPageID = MAX_PAGENUMBER - 1;
		
		this.ChangePageProcessing();
	}

	void ChangePageProcessing ()
	{
		if (currentPageID == 0) {
            titleIcon_0.spriteId = titleIcon_0.GetSpriteIdByName(arr_nameOfPlates[0]);
            titleIcon_1.spriteId = titleIcon_1.GetSpriteIdByName(arr_nameOfPlates[1]);
            titleIcon_2.spriteId = titleIcon_2.GetSpriteIdByName(arr_nameOfPlates[2]);
		} else if (currentPageID == 1) {
            titleIcon_0.spriteId = titleIcon_0.GetSpriteIdByName(arr_nameOfPlates[3]);
            titleIcon_1.spriteId = titleIcon_1.GetSpriteIdByName(arr_nameOfPlates[4]);
            titleIcon_2.spriteId = titleIcon_2.GetSpriteIdByName(arr_nameOfPlates[5]);
		}

		this.ResetActiveAvailableMedal();
		this.SetActiveAvailableMedal();
	}
}

