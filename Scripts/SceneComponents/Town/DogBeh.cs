using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class DogBeh : PetBeh {
		
	public AudioClip chaseBite_clip;
    public AudioClip yelp_clip;

    public new enum NameAnimationList { None = 0, WalkRight, WalkLeft, Glad, Glad2, Eat, Crap, ChaseBite, };
    public static NameAnimationList nameAnimationList;
	
		
	// Use this for initialization
    protected override void Initializing()
    {
        base.Initializing();

        this.audio.clip = chaseBite_clip;
        this.audio.volume = 1;
        this.audio.loop = false;
    }

    protected new void RandomBeh()
    {
        int r = Random.Range(3, 7);
        NameAnimationList nameAnimated = (NameAnimationList)r;
        animatedSprite.Play(nameAnimated.ToString());
        animatedSprite.animationCompleteDelegate = delegate(tk2dAnimatedSprite sprite, int clipId) {
            WalkRight();
        };
    }

    internal static void ChaseBite() {
        nameAnimationList = NameAnimationList.ChaseBite;
    }

    void ChaseBakeryTruck() {
		iTween.Stop(this.gameObject);
		
        animatedSprite.Play(NameAnimationList.WalkRight.ToString());
		audio.Play();
		
        this.transform.position = new Vector3(-300f, -110f, -7f);
        iTween.MoveTo(this.gameObject, iTween.Hash("position", new Vector3(300f, -110f, -7f), "islocal", false, "time", 12f, "easetype", iTween.EaseType.linear, "looptype", iTween.LoopType.none,
            "oncomplete", "ONChaseBakeryTruckComplete", "oncompletetarget", this.gameObject));
    }
	
	void ONChaseBakeryTruckComplete() {
		audio.PlayOneShot(yelp_clip);
		WalkLeft();
	}
	
	// Update is called once per frame
	void Update () {
        if (nameAnimationList == NameAnimationList.ChaseBite) {
            nameAnimationList = NameAnimationList.None;

            this.ChaseBakeryTruck();
        }
	}
}
