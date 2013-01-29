using UnityEngine;
using System.Collections;

public class PetBeh : MonoBehaviour {

    protected const string WALKLEFT_FUNC = "WalkLeft";
    protected const string FUNC_WALKRIGHT = "WalkRight";
    protected const string FUNC_RANDOM_BEH = "RandomBeh";

    public enum NameAnimationList { None = 0, WalkRight, WalkLeft, Glad, Glad2, Eat, };
    protected tk2dAnimatedSprite animatedSprite;

	void Awake() {
		iTween.Init(this.gameObject);
		
		animatedSprite = this.gameObject.GetComponent<tk2dAnimatedSprite>();
	}
	
	// Use this for initialization
    void Start()
	{		
        this.WalkRight();
        this.Initializing();
	}

    protected virtual void Initializing() { }
    
    protected virtual void WalkLeft()
    {
        animatedSprite.Play(NameAnimationList.WalkLeft.ToString());
        iTween.MoveTo(this.gameObject, iTween.Hash("position", new Vector3(120f, -78f, -2f), "islocal", true, "time", 5f, "easetype", iTween.EaseType.easeInOutSine, "looptype", iTween.LoopType.none,
            "oncomplete", FUNC_RANDOM_BEH, "oncompletetarget", this.gameObject));
    }

    protected virtual void WalkRight()
    {
        animatedSprite.Play(NameAnimationList.WalkRight.ToString());
        iTween.MoveTo(this.gameObject, iTween.Hash("position", new Vector3(170f, -78f, -2f), "islocal", true, "time", 3f, "easetype", iTween.EaseType.easeInOutSine, "looptype", iTween.LoopType.none,
            "oncomplete", WALKLEFT_FUNC, "oncompletetarget", this.gameObject));
    }

    protected virtual void RandomBeh()
    {
        int r = Random.Range(3, 6);
        NameAnimationList nameAnimated = (NameAnimationList)r;
        animatedSprite.Play(nameAnimated.ToString());
        animatedSprite.animationCompleteDelegate = delegate(tk2dAnimatedSprite sprite, int clipId) {
            WalkRight();
        };
    }
	
	// Update is called once per frame
//	void Update () { }
}
