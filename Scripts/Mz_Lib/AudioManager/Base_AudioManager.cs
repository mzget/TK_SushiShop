using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Base_AudioManager : ScriptableObject {

    public List<AudioClip> appreciate_Clips = new List<AudioClip>();
    public List<AudioClip> warning_Clips = new List<AudioClip>();

    protected virtual void OnEnable() {}

    protected virtual void OnDestroy() { }
}
