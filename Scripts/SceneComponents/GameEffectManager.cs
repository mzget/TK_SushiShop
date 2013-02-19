using UnityEngine;
using System.Collections;

public class GameEffectManager : MonoBehaviour {
	
	public const string BLOOMSTAR_EFFECT_PATH = "GameEffects/BloomStar";
	public const string IRIDESCENT_EFFECT_PATH = "GameEffects/Iridescent";
	public const string STARINCIDENT_EFFECT_PATH = "GameEffects/StarIncident";
	
	// Use this for initialization
//	void Start () {
//	
//	}
	
	public void Create2DSpriteAnimationEffect(string targetName, Transform transform) {
        GameObject effect = Instantiate(Resources.Load(targetName, typeof(GameObject)), transform.position, Quaternion.identity) as GameObject;
        effect.transform.parent = transform;
        effect.transform.localScale = Vector3.one;
        effect.transform.position += Vector3.back;
		

        tk2dAnimatedSprite animatedSprite = effect.GetComponent<tk2dAnimatedSprite>();
        animatedSprite.animationCompleteDelegate = delegate(tk2dAnimatedSprite anim, int id) {
            Destroy(effect);
            animatedSprite = null;
        };
	}
 }
