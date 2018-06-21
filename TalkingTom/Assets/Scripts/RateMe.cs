using UnityEngine;
using System.Collections;

public class RateMe : MonoBehaviour {

	public UISprite BTSprite;
	public UISprite[] StarSprites;
	public static bool rateMeOn;

	public enum SelectPlateform
	{
		Amazon,
		Android,
		iOS
	}

	public SelectPlateform SelectedPlateform = SelectPlateform.Amazon;


	// Use this for initialization
	void Start () {
		Starval = 4;
		BTSprite.spriteName = "rate-btn";
		BTSprite.GetComponent<UIButton>().normalSprite = "rate-btn";
		for(int i = 0; i < Starval; i++)
		{
			StarSprites[i].spriteName = "active-star-rate";
		}
		
		for(int j = Starval; j < StarSprites.Length;j++)
		{
			
			StarSprites[j].spriteName = "unactive-star-rate";
			
		}
	}


	void OnEnable()
	{
		rateMeOn = true;
	}
	void OnDisable()
	{
		rateMeOn = false;
	}
	// Update is called once per frame
	void Update () {
		#if UNITY_ANDROID
		if(Input.GetKeyDown (KeyCode.Escape))
		{
			OnDismiss();
		}
#endif
	}

	private int Starval;
	

	public void OnStarClicked()
	{
		Starval = int.Parse(UIEventTrigger.current.name);
		
		if(Starval <= 3)
		{
			BTSprite.spriteName = "contact-btn";
			BTSprite.GetComponent<UIButton>().normalSprite = "contact-btn";
		}
		else
		{
			BTSprite.spriteName = "rate-btn";
			BTSprite.GetComponent<UIButton>().normalSprite = "rate-btn";
		}


		for(int i = 0; i < Starval; i++)
		{
			StarSprites[i].spriteName = "active-star-rate";
		}
		
		for(int j = Starval; j < StarSprites.Length;j++)
		{
			StarSprites[j].spriteName = "unactive-star-rate";
		}
		

	}

	void SendEmail ()
	{
		string email = "INSERT EMAIL ID HERE";
		string subject = MyEscapeURL("INSERT THE NAME OF THE GAME HERE.");
		string body = MyEscapeURL("Please share your valuable feedback and suggestions so that we can improve the game.");
		
		Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
	}
	
	string MyEscapeURL (string url)
	{
		return WWW.EscapeURL(url).Replace("+","%20");
	}

	public void OnRate()
	{
		if(Starval <= 3)
		{
			SendEmail();

		}else{
#if UNITY_IPHONE
			Application.OpenURL("PASTE YOUR IOS URL HERE");
#else

			Application.OpenURL("PASTE YOUR GOOGLE PLAYSTORE URL HERE");
#endif
		}

	}

	public void OnDismiss()
	{
		this.gameObject.SetActive(false);
	}

}
