using UnityEngine;
using System.Collections;

public class TK_news : MonoBehaviour {
	private const int AMOUNT_OF_NEWS_TAG = 3;
    private readonly int AmountOfPage = 2; 
    private int currentPage = 0;


//	public Transform scrollingObjects_transform;
	public tk2dSprite[] news_tags = new tk2dSprite[AMOUNT_OF_NEWS_TAG];
	private readonly string[] arr_NameOfNewsSprite = new string[6] {
		"Bakery Shop", "Sushi Shop", "Icecream Shop",
        "Pizza Shop", "None", "None",
	};


    public GameObject facebook_button;

	// Use this for initialization
    void Start()
    {
        currentPage = 0;
        this.SynchronizeNewsTag();
	}

    private void SynchronizeNewsTag()
    {
        for (int i = 0; i < AMOUNT_OF_NEWS_TAG; i++) {
            news_tags[i].spriteId = news_tags[i].GetSpriteIdByName(arr_NameOfNewsSprite[i + (currentPage * AMOUNT_OF_NEWS_TAG)]);
        }
    }
	
	// Update is called once per frame
	void Update () {

	}

    internal void ShakeFacebookButton() {
        iTween.ShakePosition(facebook_button.gameObject, iTween.Hash("name", "ShakePosFacebook","amount", new Vector3(1f, 1f, 0f), "islocal", false, "time", 1f, "looptype", iTween.LoopType.pingPong));
    }

    internal void StopShakeFacebookButton() {
        iTween.StopByName(facebook_button.gameObject, "ShakePosFacebook");
    }

    internal void MoveUpPage()
    {
        if (currentPage > 0)
            currentPage--;
        else
            currentPage = AmountOfPage - 1;

        SynchronizeNewsTag();
    }

    internal void MoveDownPage()
    {
        if (currentPage < AmountOfPage-1)
            currentPage++;
        else
            currentPage = 0;

        SynchronizeNewsTag();
    }
}
