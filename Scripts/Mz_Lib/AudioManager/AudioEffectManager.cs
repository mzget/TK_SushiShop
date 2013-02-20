using UnityEngine;
using System.Collections;

public class AudioEffectManager : MonoBehaviour {
	
    public AudioSource alternativeEffect_source;
	
    public AudioClip buttonDown_Clip;
	public AudioClip buttonUp_Clip;
	public AudioClip buttonHover_Clip;
	public AudioClip correct_Clip;
	public AudioClip wrong_Clip;
    public AudioClip pop_clip;
    //<!-- Bakery scene.
    public AudioClip dingdong_clip;
	public AudioClip calc_clip;
    public AudioClip receiptCash_clip;
	public AudioClip giveTheChange_clip;
	public AudioClip longBring_clip;
	public AudioClip mutter_clip;
	public AudioClip correctBring_clip;
	
	
	void Awake() {
		GameObject source = new GameObject("alternativeEffect", typeof(AudioSource));
		source.transform.parent = this.gameObject.transform;
		
		if(alternativeEffect_source == null)
			alternativeEffect_source = source.gameObject.GetComponent<AudioSource>();
	}
	
	// Use this for initialization
	void Start () {
        audio.volume = 1;
		alternativeEffect_source.volume = 1;
	}
	
	public void PlayOnecSound(AudioClip sound) {
        this.audio.Stop();
		this.audio.PlayOneShot(sound);
	}
	
	public void PlayOnecWithOutStop(AudioClip sound) {
        this.alternativeEffect_source.PlayOneShot(sound);
	}
}
