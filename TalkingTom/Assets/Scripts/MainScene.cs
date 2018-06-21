using UnityEngine;
using System.Collections;
using MiniJSON;

public class MainScene : MonoBehaviour {
	public GameObject loaderPanel;
	public GameObject rateMePanel ;
	public PopupPanel panelScript;
	public GameObject popUpPanel ;
	bool augmentedRealityClicked;
	public UISprite soundSprite;
	public GameObject instructionsPanel;
	 
	public static MainScene _instance;
	// Use this for initialization
	void Start () {
		_instance = this;
		AudioListener.volume = 1;
		Screen.orientation = ScreenOrientation.Portrait;
		if(!PlayerPrefs.HasKey("Sound"))
			PlayerPrefs.SetInt("Sound" , 1);
		if(PlayerPrefs.GetInt ("Sound") == 1)
		{
			GetComponent<AudioSource>().GetComponent<AudioSource>().Play ();
		}
		SoundButtonSprites ();

	}


	public void InstructionsOFAugmented()
	{
		if(!LoaderPanel.loaderActive && !PopupPanel.popupPanelActive && !RateMe.rateMeOn  && !InstructionsPanel.instructionPanelPresent)
		{

			instructionsPanel.SetActive (true);
		}
	}

	public void Sound()
	{
		if( !InstructionsPanel.instructionPanelPresent)
		{
			if(PlayerPrefs.GetInt ("Sound") == 1)
			{
				Debug.Log("set sound 0");
				PlayerPrefs.SetInt ("Sound" , 0);
				GetComponent<AudioSource>().Stop();
			}
			else
			{
				Debug.Log("set sound 1");
				PlayerPrefs.SetInt ("Sound" , 1);
				GetComponent<AudioSource>().Play ();
			}
			Debug.Log("function called");
			SoundButtonSprites();
		}
	}

	void SoundButtonSprites()
	{
		if(PlayerPrefs.GetInt ("Sound") == 0)
		{
			soundSprite.spriteName = "mute-btn";
			soundSprite.GetComponent<UIButton>().normalSprite = "mute-btn";
			soundSprite.GetComponent<UIButton>().hoverSprite = "mute-btn";
			soundSprite.GetComponent<UIButton>().pressedSprite = "mute-btn";
			soundSprite.GetComponent<UIButton>().disabledSprite = "mute-btn";
		}
		else
		{
			soundSprite.spriteName = "volume-btn";
			soundSprite.GetComponent<UIButton>().normalSprite = "volume-btn";
			soundSprite.GetComponent<UIButton>().hoverSprite = "volume-btn";
			soundSprite.GetComponent<UIButton>().pressedSprite = "volume-btn";
			soundSprite.GetComponent<UIButton>().disabledSprite = "volume-btn";
		}
	}


	// Update is called once per frame
	void Update () {
#if UNITY_ANDROID
		if(Input.GetKeyDown (KeyCode.Escape))
		{
			if(!LoaderPanel.loaderActive && !PopupPanel.popupPanelActive && !RateMe.rateMeOn  && !InstructionsPanel.instructionPanelPresent)
			{
				Application.Quit ();
			}
		}
#endif
	}

	public void AugmentedReality()
	{
		if(!LoaderPanel.loaderActive && !PopupPanel.popupPanelActive && !RateMe.rateMeOn  && !InstructionsPanel.instructionPanelPresent)
		{
			StartCoroutine(LoadALevel("VirtualReality"));
//			GoogleMobileAdsDemoScript._instance.HideBanner ();
		}
				
	}

	public void Play()
	{
		if(!LoaderPanel.loaderActive && !PopupPanel.popupPanelActive && !RateMe.rateMeOn && !InstructionsPanel.instructionPanelPresent)
		{
			StartCoroutine(LoadALevel("MainRoom"));
		}
	}

	AsyncOperation asyncOp;
	
	private IEnumerator LoadALevel(string index) {
		loaderPanel.SetActive(true);
		LoaderPanel.asyncLoad = true;
		AsyncOperation asyncOp = Application.LoadLevelAsync(index);
		
		while(!asyncOp.isDone)
		{
			loaderPanel.GetComponent<LoaderPanel>().myFillingSprite.fillAmount = asyncOp.progress;
			yield return null;
		}
		LoaderPanel.asyncLoad = false;
		loaderPanel.SetActive(false);

	}


	public void Rate()
	{
		if(!LoaderPanel.loaderActive && !PopupPanel.popupPanelActive && !RateMe.rateMeOn  && !InstructionsPanel.instructionPanelPresent)
		{
		//	rateMePanel.SetActive (true);
		}
	}

	public void ActivatePopup(string myText , bool okCancelTrue )
	{
		popUpPanel.SetActive (true);
		if(okCancelTrue)
		{
			panelScript.individualOk.SetActive (false);
			panelScript.okCancelParent.SetActive (true);
		}
		else
		{
			panelScript.individualOk.SetActive (true);
			panelScript.okCancelParent.SetActive (false);
		}
		panelScript.poupLabel.text = myText;
		
	}

}
