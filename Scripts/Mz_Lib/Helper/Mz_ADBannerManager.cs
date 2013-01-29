using UnityEngine;
using System.Collections;

public class Mz_ADBannerManager : MonoBehaviour {

#if UNITY_IPHONE

	private ADBannerView banner = null;
	string bannerStatus = "";
		
		
	// Use this for initialization
	void Start () {
		this.CreateBanner();
	}
	
	void CreateBanner() {
		banner = new ADBannerView();
        banner.autoSize = true;
        banner.autoPosition = ADPosition.Bottom;
        StartCoroutine(ShowBanner());
	}

	// Update is called once per frame
	void Update () {

	}

	private IEnumerator ShowBanner() {
		while (!banner.loaded && banner.error == null) {
			yield return bannerStatus = "Loading... iAd banner";
		}

		if (banner.error == null)
			banner.Show();
		else {
			bannerStatus = banner.error.ToString();
			banner = null;
		}
	}
	
	void OnGUI() {
		GUI.depth = 0;
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(Screen.width/Main.FixedGameWidth, Screen.height/Main.FixedGameHeight, 1));
		
		if(banner == null || !banner.loaded || !banner.visible) {    
		    if (GUI.Button(new Rect(Main.FixedGameWidth /2 - 60, 0, 120, 50), "Show Banner")) {
				if(banner == null)
					this.CreateBanner();
				else
					banner.Show();
		    }
		}
		else if(banner != null || banner.visible) {
			if (GUI.Button(new Rect(Main.FixedGameWidth /2 - 60, 0, 120, 50), "Close Banner")) {
				banner.Hide();
			}
		}
		
		if(banner == null) 
			GUI.Box(new Rect(Main.FixedGameWidth /2 - 200, Main.FixedGameHeight - 40, 400, 40), bannerStatus);
	}

#endif

}