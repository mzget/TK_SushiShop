using UnityEngine;
using System.Collections;


public class Mz_PlayLogoMovie : MonoBehaviour {
	
#if UNITY_STANDALONE_WIN
	
    public MovieTexture logoMovie;
	public MovieTexture gameMovie;
	public AudioSource logoAudioObj;
    public AudioSource gameAudioObj;

    private float time;
    /// <summary>
    /// Static Prop.
    /// </summary>
    private static bool _playLogo = true;
    public static bool _PlayLogo {
        get { return _playLogo; }
        set { _playLogo = value; }
    }
	
#endif

	public GameObject introScene_Prefab;
	private GameObject introScene_Instance;
	
	
	// Use this for initialization
	void Awake() 
	{
#if UNITY_STANDALONE_WIN
        if (_playLogo) 
			PlayLogo(); 
        else 
			PlayMovie();
#endif
		
#if UNITY_IPHONE || UNITY_ANDROID
		
		iPhoneUtils.PlayMovie("TK_Logo.mov", Color.clear, iPhoneMovieControlMode.CancelOnTouch, iPhoneMovieScalingMode.Fill);

#endif
	}
	
	private IEnumerator Start() {
		yield return new WaitForSeconds(0.5f);
	}
	
	// Update is called once per frame
    void Update() { 
#if UNITY_STANDALONE_WIN
		
        time += Time.deltaTime;
        if (time >= logoMovie.duration && _playLogo) {
			_playLogo  = false;
			time = 0f;
			PlayMovie();
        }
		
		if(time >= gameMovie.duration && !_playLogo) {
            time = 0;
			this.LoadGame();
		}

        if (!_playLogo) {
            if (Input.GetKeyDown(KeyCode.Escape) || 
				Input.GetMouseButtonDown(0))
				LoadGame();
        }
		else if(_playLogo) {
			if(Input.GetKeyDown(KeyCode.Escape)) {
				_playLogo = false;
				logoAudioObj.Stop();
				this.PlayMovie();
			}
		}
		
#endif
		
#if UNITY_IPHONE || UNITY_ANDROID
//		Mz_SmartDeviceInput.ImplementTouchInput();
		
		if(!introScene_Instance) {
			introScene_Instance = Instantiate(introScene_Prefab) as GameObject;
		}
#endif
	}

	/// <summary>
	/// Waits for GUI event.
	/// </summary>
	/// <param name='nameEvent'>
	/// Name event.
	/// </param>
/*	public void WaitForGUIEvent(string nameEvent) {
		if(nameEvent == "th") {
			Manager.gameLanguage = Manager.GameLanguage.Th;
			LoadGame();
		}
		else if(nameEvent == "en") {
			Manager.gameLanguage = Manager.GameLanguage.Eng;
			LoadGame();
		}
	}*/

    void PlayMovie() {
#if UNITY_STANDALONE_WIN
        gameMovie.Play();
        gameAudioObj.Play();
        this.guiTexture.texture = gameMovie;
#endif
	}

    void PlayLogo() {	
#if UNITY_STANDALONE_WIN
        logoMovie.Play();
        logoAudioObj.Play();
#endif
	}

    void LoadGame() {
#if UNITY_STANDALONE_WIN
		
        logoMovie.Stop();
        gameMovie.Stop();	

        if (!Application.isLoadingLevel) {
            Mz_LoadingScreen.LoadSceneName = Mz_BaseScene.SceneNames.MainMenu.ToString();
            Application.LoadLevelAsync(Mz_BaseScene.SceneNames.LoadingScene.ToString());
        }

        Destroy(this.gameObject);	
		
#endif	
		
#if UNITY_IPHONE || UNITY_ANDROID
		if (!Application.isLoadingLevel) {
            Mz_LoadingScreen.LoadSceneName = Mz_BaseScene.SceneNames.MainMenu.ToString();
            Application.LoadLevelAsync(Mz_BaseScene.SceneNames.LoadingScene.ToString());
        }
#endif
    }
}
