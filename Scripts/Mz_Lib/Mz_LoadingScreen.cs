using UnityEngine;
using System.Collections;

public class Mz_LoadingScreen : MonoBehaviour
{
    //private GameObject backGroundIns, loadingAnimatedIns = null;

    public static string LoadSceneName { get; set; }

    //private int countTex = 0;
	private bool _haveDisk = true;
	private AsyncOperation async;

    public GameObject loading_background_sprite;
    private GUIStyle loadingGUISkin;
    public Font loadingFont;

	
	/// <summary>
	/// Start this instance.
	/// </summary>
    IEnumerator Start()
	{
        Time.timeScale = 1f;
		Resources.UnloadUnusedAssets();

//        Mz_ResizeScale.ResizingScale(loading_background_sprite.transform);
		
#if UNITY_STANDALONE_WIN 
            //async = DatVistaData.LoadLevelAsync(sceneName);

            //if(DatVistaData.IsLoadLevelCheckVista) {
            //    while (!async.isDone) {
            //        yield return 0;
            //    }
            //}
            //else {
            //    /// Show Insert Disk.
            //    _haveDisk = false;
            //}

            //<!!!!! WARNING...
            //<!-- FOR DEBUG ONLY.
            async = Application.LoadLevelAsync(LoadSceneName);
            if (Application.isLoadingLevel) {
                while (async.isDone == false) {
                    yield return 0;
                }
            }

#elif UNITY_IPHONE || UNITY_ANDROID || UNITY_FLASH || UNITY_WEBPLAYER
	
        async = Application.LoadLevelAsync(LoadSceneName);
		if(Application.isLoadingLevel) {
			while(async.isDone == false) {
				yield return 0;
			}
		}
		
#endif	
    }

    void OnGUI()
    {
        GUI.depth = 0;
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(Screen.width / Main.GAMEWIDTH, Screen.height / Main.GAMEHEIGHT, 1));

        loadingGUISkin = GUI.skin.label;
        loadingGUISkin.font = loadingFont;
        loadingGUISkin.alignment = TextAnchor.MiddleCenter;
        loadingGUISkin.fontSize = 24;
        loadingGUISkin.normal.textColor = Color.white;
		
        if (_haveDisk)
        {
            //<<!--- Have Disk is True, Show Loading Animation. 
            float process = 0;
            if(async != null)
               process = async.progress * 100f;

            GUI.Box(new Rect(Main.GAMEWIDTH - 320, Main.GAMEHEIGHT - 64, 300, 50), "Loading... " + process.ToString("F1") + " %", loadingGUISkin);
        }
        else
        {
            /// Have Disk is false.
            GUI.BeginGroup(new Rect(Main.GAMEWIDTH / 2 - 150, Main.GAMEHEIGHT / 2 - 75, 300, 150));
            {
                GUI.Box(new Rect(0, 0, 300, 150), "Please insert disk ! \n \n กรุณาใส่แผ่นดิกส์", loadingGUISkin);

                if (GUI.Button(new Rect(245, 5, 50, 30), "Exit", GUI.skin.button)) {
                    Application.Quit();                
				}
            }
            GUI.EndGroup();
        }
    }
}