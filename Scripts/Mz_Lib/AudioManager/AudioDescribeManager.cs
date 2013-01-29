using UnityEngine;
using System.Collections;

public class AudioDescribeManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    public void PlayOnecSound(AudioClip sound)
    {
        this.audio.Stop();
        this.audio.PlayOneShot(sound);
    }

    public void PlayOnecWithOutStop(AudioClip sound)
    {
        this.audio.PlayOneShot(sound);
    }
}
