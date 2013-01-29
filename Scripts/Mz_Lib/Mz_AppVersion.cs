using UnityEngine;
using System.Collections;

public class Mz_AppVersion : MonoBehaviour {

	public enum AppVersion {Free = 0, Pro = 1};
	public AppVersion appVersion;
	public static AppVersion getAppVersion;
	
	// Use this for initialization
	void Start () {
		getAppVersion = appVersion;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
