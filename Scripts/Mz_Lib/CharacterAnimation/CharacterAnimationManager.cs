using UnityEngine;
using System.Collections;

public class CharacterAnimationManager : MonoBehaviour {

    public tk2dAnimatedSprite hair_anim;
	public tk2dAnimatedSprite eye_anim;
	public tk2dAnimatedSprite lefthand_anim;
	public tk2dAnimatedSprite righthand_anim;

	internal bool _isPlayingAnimation = false;

	public enum NameAnimationsList {
		idle = 0,
		talk = 1,
		good1,
		good2,
		good3,
		sad1,
		sad2,
		sad3,
        agape = 8,
        agape2 = 9,

        //<!-- Left hand --->
        lefthand_active,
        lefthand_good1,
        //<!-- right hand.
        righthand,
        //<!-- Hair animations list.
        hair,
	};

    double timer;

	
	// Use this for initialization
    void Start()
    {

    }
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if (timer >= 4) {
            timer = 0;

            PlayEyeAnimation(NameAnimationsList.idle);
        }
	}

    private void PlayLeftHandAnimation(NameAnimationsList nameAnimated) {
        lefthand_anim.Play(nameAnimated.ToString());
    }

	private void PlayRightHandAnimation(CharacterAnimationManager.NameAnimationsList nameAnimated) {
		righthand_anim.Play(nameAnimated.ToString());
	}

	internal void PlayEyeAnimation(CharacterAnimationManager.NameAnimationsList nameAnimated) {
		timer = 0;
		eye_anim.Play(nameAnimated.ToString());
		_isPlayingAnimation = true;
		
		eye_anim.animationCompleteDelegate = delegate(tk2dAnimatedSprite sprite, int clipId) {
			_isPlayingAnimation = false;
		};
	}

	public void PlayAnimationByName(NameAnimationsList nameAnimation) {
//		_CanUpdatable = true;
		eye_anim.Play(nameAnimation.ToString());
		lefthand_anim.Play(nameAnimation.ToString());
		righthand_anim.Play(nameAnimation.ToString());
	}
	
    public void PlayGoodAnimation() {
//		_CanUpdatable = true;
        eye_anim.Play(NameAnimationsList.good1.ToString());
        lefthand_anim.Play(NameAnimationsList.lefthand_good1.ToString());
    }
	
	public void RandomPlayGoodAnimation() {
//		_CanUpdatable = true;
		int i = UnityEngine.Random.Range(2, 5);		/// Random TK_good animation.
		this.PlayEyeAnimation((CharacterAnimationManager.NameAnimationsList)i);
		this.PlayLeftHandAnimation(CharacterAnimationManager.NameAnimationsList.lefthand_active);
		this.PlayRightHandAnimation(CharacterAnimationManager.NameAnimationsList.righthand);
	} 

	public void PlaySad2Animation() {
//		_CanUpdatable = true;
        eye_anim.Play(NameAnimationsList.sad2.ToString());
	}

    internal void PlayTalkingAnimation()
    {
//		_CanUpdatable = false;
		timer = 0;
		eye_anim.Stop();
        this.PlayEyeAnimation(CharacterAnimationManager.NameAnimationsList.talk);
        this.PlayLeftHandAnimation(CharacterAnimationManager.NameAnimationsList.lefthand_active);
    }
}
