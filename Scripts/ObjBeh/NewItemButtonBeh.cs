using UnityEngine;
using System.Collections;

public class NewItemButtonBeh : Base_ObjectBeh {

	private SushiShop stageManager;

	// Use this for initialization
	void Start () {
		GameObject stage = GameObject.FindGameObjectWithTag ("GameController");
		stageManager = stage.GetComponent<SushiShop> ();
	}
	
	// Update is called once per frame
	protected override void Update ()
	{
		base.Update ();
	}

	protected override void OnTouchDown ()
	{
		base.OnTouchDown ();

		StartCoroutine_Auto (this.CreateEffectAndDestroy ());
	}

	IEnumerator CreateEffectAndDestroy ()
	{
		stageManager.gameEffectManager.Create2DSpriteAnimationEffect (GameEffectManager.BLOOMSTAR_EFFECT_PATH, this.transform);
		stageManager.PlaySoundRejoice ();

		yield return new WaitForSeconds (0.5f);

		Destroy (this.gameObject);
	}
}
