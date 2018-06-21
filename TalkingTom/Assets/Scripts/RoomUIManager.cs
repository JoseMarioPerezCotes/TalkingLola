using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Advertisements;

public class RoomUIManager : MonoBehaviour {
	public GameObject myCharacter;
	string roomClicked;

	public UISprite mainRoomSrpite,gymSprite,looSprite,bedRoomSprite,playGroundSprite, soundSprite , lightSprite , miniGames, freeCoins , recordingSprite;
	public GameObject playgroundLock , gymLock; 
	public GameObject popUpPanel , loaderPanel;
	public int gymCost, playgroundCost;
	public GameObject roomLight;
	public GameObject coinsGeneratedViaMiniGames;
	public static string previousLoadedLevel;
	public SpriteRenderer bedBack , bedfront;
	public GameObject nameOfGame;

    public static  RoomUIManager _instance;

    [SerializeField]
    public List<string> character2 = new List<string>()
    {
        "Cat",
        "Dog",
        "Owl",
        "Turtle"
    };
    // Use this for initialization
    void Start () {
//		GoogleMobileAdsDemoScript._instance.ShowBanner ();
		LoaderPanel.asyncLoad = false;
		ExtraInteractions.stopSoundRecordingDueToFlush = false;
		if(InitializePlugins.isRecording)
		{
			InitializePlugins.isRecording = false;
			Everyplay.StopRecording ();
		}
        //		PlayerPrefs.DeleteAll ();


        if (PlayerPrefs.HasKey ("Gym"))
		{
			gymLock.SetActive (false);
		}
		if(PlayerPrefs.HasKey ("PlayGround"))
		{
			playgroundLock.SetActive (false);
		}

		if(!PlayerPrefs.HasKey("Character"))
		{
            PlayerPrefs.SetInt("Character", character2.IndexOf(character2[0]));//PlayerPrefs.SetInt("Character",3);
            ScrollButtonAction.characterNo = character2.IndexOf(character2[0]);//ScrollButtonAction.characterNo = 3;
        }
		else
			ScrollButtonAction.characterNo = PlayerPrefs.GetInt("Character");

		switch(ScrollButtonAction.characterNo)
		{
		case 0:
                myCharacter = (GameObject)Instantiate(Resources.Load(character2[0]));//myCharacter = (GameObject)Instantiate(Resources.Load("Dog"));
                break;
		case 1:
                myCharacter = (GameObject)Instantiate(Resources.Load(character2[1]));//myCharacter = (GameObject)Instantiate(Resources.Load("Owl"));
                break;
		case 2:
                myCharacter = (GameObject)Instantiate(Resources.Load(character2[2]));// = (GameObject)Instantiate(Resources.Load("Turtle"));
                break;
		case 3:
                myCharacter = (GameObject)Instantiate(Resources.Load(character2[3]));//myCharacter = (GameObject)Instantiate(Resources.Load("Cat"));
                break;
		}
		if(Application.loadedLevelName == "BedRoom")
		{
			myCharacter.GetComponent<Animation>().Play ("Sleep");
			if(myCharacter.GetComponent<DetectActions>().myTowel)
				myCharacter.GetComponent<DetectActions>().myTowel.SetActive (false);
		}


		AnimationHandler._instance.myCharacter = myCharacter;
		AnimationHandler._instance.myCharacterDetectActions = myCharacter.GetComponent<DetectActions>();
		if(Application.loadedLevelName == "BathRoom")
		{
			myCharacter.transform.position = new Vector3(1.72f,-4.31f,1);
			AnimationHandler._instance.characterGeneralPosInBathRoom = myCharacter.transform.position;
			looSprite.spriteName = "bathroom-red-btn";
			looSprite.transform.GetComponent<UIButton>().disabledSprite = "bathroom-red-btn";
			looSprite.transform.GetComponent<UIButton>().normalSprite = "bathroom-red-btn";
			looSprite.transform.GetComponent<UIButton>().hoverSprite = "bathroom-red-btn";
			if(ScrollButtonAction.characterNo < 3)
			{
				GameObject bathroomDoor = AnimationHandler._instance.door;
				bathroomDoor.transform.position = new Vector3(bathroomDoor.transform.position.z,1.68f,bathroomDoor.transform.position.z);
				bathroomDoor.transform.localScale = new Vector3(bathroomDoor.transform.localScale.x,1.28f,bathroomDoor.transform.localScale.z);
				bathroomDoor.GetComponent<ObjectMotion>().myOriginalPos = bathroomDoor.transform.position;
			}
		}
		else if(Application.loadedLevelName == "BedRoom")
		{
			BedroomPosition();
			bedRoomSprite.spriteName = "bedroom-red-btn";
			bedRoomSprite.transform.GetComponent<UIButton>().disabledSprite = "bedroom-red-btn";
			bedRoomSprite.transform.GetComponent<UIButton>().hoverSprite = "bedroom-red-btn";
			bedRoomSprite.transform.GetComponent<UIButton>().normalSprite = "bedroom-red-btn";
			lightSprite.gameObject.SetActive (true);
		}
		else if(Application.loadedLevelName == "MainRoom")
		{
			mainRoomSrpite.spriteName = "mainroom-icon";
			mainRoomSrpite.transform.GetComponent<UIButton>().disabledSprite = "mainroom-icon";
			mainRoomSrpite.transform.GetComponent<UIButton>().normalSprite = "mainroom-icon";
			mainRoomSrpite.transform.GetComponent<UIButton>().hoverSprite = "mainroom-icon";
		}
		else if(Application.loadedLevelName == "PlayGround")
		{
			myCharacter.transform.position = new Vector3(0,-1.65f,0);
			AnimationHandler._instance.characterPosInPlayground = myCharacter.transform.position;
			playGroundSprite.spriteName = "playground-red-btn";
			playGroundSprite.transform.GetComponent<UIButton>().disabledSprite = "playground-red-btn";
			playGroundSprite.transform.GetComponent<UIButton>().normalSprite = "playground-red-btn";
			playGroundSprite.transform.GetComponent<UIButton>().hoverSprite = "playground-red-btn";
		}
		else if(Application.loadedLevelName == "Gym")
		{
			myCharacter.transform.position = new Vector3(0f,-2.1f,0.5f);
			gymSprite.spriteName = "gym-red-btn";
			gymSprite.transform.GetComponent<UIButton>().disabledSprite = "gym-red-btn";
			gymSprite.transform.GetComponent<UIButton>().normalSprite = "gym-red-btn";
			gymSprite.transform.GetComponent<UIButton>().hoverSprite = "gym-red-btn";
		}
		AnimationHandler._instance.RemoveExtraButtons ();
		if(GameManager.coinsGenerated > 0)
		{
			coinsGeneratedViaMiniGames.SetActive (true);
			coinsGeneratedViaMiniGames.GetComponent<TweenPosition>().from = miniGames.transform.localPosition;
			coinsGeneratedViaMiniGames.GetComponent<TweenPosition>().to = freeCoins.transform.localPosition;
			coinsGeneratedViaMiniGames.GetComponentInChildren<UILabel>().text = GameManager.coinsGenerated.ToString ();
			GameManager.coinsGenerated = 0;
			coinsGeneratedViaMiniGames.GetComponent<TweenPosition>().AddOnFinished (new EventDelegate(TweenCompleted));
		}
	}
	float timerOfAd;
	void Update()
	{
#if UNITY_ANDROID
		timerOfAd+=Time.deltaTime;
		if(timerOfAd > 300.0f)
		{
			if(Input.GetMouseButtonDown (0))
			{
//				GoogleMobileAdsDemoScript._instance.ShowInterstitial ();
				timerOfAd = 0;
			}
		}

		if(Input.GetKeyDown (KeyCode.Escape))
		{
			if(!LoaderPanel.loaderActive && !PopupPanel.popupPanelActive && !AnimationHandler._instance.coinsPanelActive )
			{
				Back ();
				Debug.Log("pressed here");
			}
		}
#endif
	}

	void TweenCompleted()
	{
		Destroy (coinsGeneratedViaMiniGames);
	}

	void SoundButtonSprites()
	{
		if(PlayerPrefs.GetInt ("Sound") == 0)
		{
			soundSprite.spriteName = "mute-btn";
			soundSprite.GetComponent<UIButton>().normalSprite = "mute-btn";
			soundSprite.GetComponent<UIButton>().hoverSprite = "mute-btn";
			soundSprite.GetComponent<UIButton>().pressedSprite = "volume-btn";
			soundSprite.GetComponent<UIButton>().disabledSprite = "mute-btn";
		}
		else
		{
			soundSprite.spriteName = "volume-btn";
			soundSprite.GetComponent<UIButton>().normalSprite = "volume-btn";
			soundSprite.GetComponent<UIButton>().hoverSprite = "volume-btn";
			soundSprite.GetComponent<UIButton>().pressedSprite = "mute-btn";
			soundSprite.GetComponent<UIButton>().disabledSprite = "volume-btn";
		}
	}

	public void BedroomPosition()
	{
		switch(ScrollButtonAction.characterNo)
		{
		case 0:
			myCharacter.transform.position = new Vector3(-0.86f,1.2f,2.350002f);
                myCharacter.transform.rotation = Quaternion.Euler(-62.00067f,152.1f,38.81095f);
                break;
		case 1:
			myCharacter.transform.position = new Vector3(-0.3f,0.1800001f,2.74f);
                myCharacter.transform.rotation = Quaternion.Euler(-63.0f, 0f, 0f);//myCharacter.transform.rotation = Quaternion.Euler(0f,110f,0f);
                break;
		case 2://buo
                myCharacter.transform.position = new Vector3(-0.05f, -0.02f, 1.21f);//myCharacter.transform.position = new Vector3(1.3f,1.02f,3.029746f);
                myCharacter.transform.rotation = Quaternion.Euler(-42.19f, -161.3001f, -1.728469e-06f);//myCharacter.transform.rotation = Quaternion.Euler(23.15633f,133.62f,-56.47394f);
                break;
		case 3://morrocoyo
			myCharacter.transform.position = new Vector3(-0.856347f,0.730195f,2.74f);
                myCharacter.transform.rotation = Quaternion.Euler(90.0f, 150.1f, 0f); //myCharacter.transform.rotation = Quaternion.Euler(-33.49133f,105.4364f,-12.82684f);
                break;
		}
		if(PlayerPrefs.GetInt ("Sound") == 1)
		{
			Camera.main.GetComponent<AudioSource>().Play ();
		}
		Invoke ("SnoreSound",0.5f);
	}
	

	void SnoreSound()
	{
		if(PlayerPrefs.GetInt ("Sound") == 1)
		{
			GetComponent<AudioSource>().clip = InGameSoundManager._instance.snoring;
			GetComponent<AudioSource>().loop = true;
			GetComponent<AudioSource>().Play ();
		}
	}


	#region Light
	public void LightOnOff()
	{
        if (roomLight.GetComponent<Light>().intensity <= 0.2f)//if(roomLight.GetComponent<Light>().intensity == 0.0f)
        {
			roomLight.GetComponent<Light>().intensity = 0.5f;
			lightSprite.GetComponent<UIButton>().normalSprite = "light-on";
			lightSprite.GetComponent<UIButton>().hoverSprite = "light-on";
			lightSprite.GetComponent<UIButton>().pressedSprite = "light-off";
			lightSprite.GetComponent<UIButton>().disabledSprite = "light-on";
			if(AnimationHandler._instance.myCharacter.GetComponent<Animation>().IsPlaying ("Sleep"))
			{
				/*if(PlayerPrefs.GetInt("Sound") == 1)
				{
//					AnimationHandler._instance.audio.clip = InGameSoundManager._instance.wakeUp;
//					AnimationHandler._instance.audio.Play ();
				}*/
				if(ScrollButtonAction.characterNo == 0) { 
					AnimationHandler._instance.myCharacter.GetComponent<Animation>().Play("Idle2");
                }
                else {
					AnimationHandler._instance.myCharacter.GetComponent<Animation>().Play("Idle");
                }
                AnimationHandler._instance.myCharacter.transform.position = AnimationHandler._instance.myCharacter.GetComponent<DetectActions>().myPosAwakeInBed;
				AnimationHandler._instance.myCharacter.transform.localEulerAngles = AnimationHandler._instance.myCharacter.GetComponent<DetectActions>().myRotAwakeInBed;
				AnimationHandler._instance.sleepParticles.Stop ();
				AnimationHandler._instance.sleepParticles.gameObject.SetActive (false);
				AnimationHandler._instance.GetComponent<AudioSource>().Stop ();
			}
			AnimationHandler._instance.myCharacter.GetComponent<DetectActions>().CancelSleepInvoke();
			AnimationHandler._instance.myCharacter.GetComponent<DetectActions>().CallMakeItSleep();
		}
		else
		{
			roomLight.GetComponent<Light>().intensity = 0.1f;
			AnimationHandler._instance.myCharacter.GetComponent<DetectActions>().MakeItSleep();			
		}
		ChangeLightSprites();
	}

	public void ChangeLightSprites()
	{
		if(roomLight.GetComponent<Light>().intensity < 0.2f)
		{    

            lightSprite.GetComponent<UIButton>().normalSprite = "light-off";
			lightSprite.GetComponent<UIButton>().hoverSprite = "light-off";
			lightSprite.GetComponent<UIButton>().pressedSprite = "light-on";
			lightSprite.GetComponent<UIButton>().disabledSprite = "light-off";
			bedBack.color = new Color(112/255f,112/255f,112/255f,1);
			bedfront.color = new Color(112/255f,112/255f,112/255f,1);
		}
		else
		{
           
            lightSprite.GetComponent<UIButton>().normalSprite = "light-on";
			lightSprite.GetComponent<UIButton>().hoverSprite = "light-on";
			lightSprite.GetComponent<UIButton>().pressedSprite = "light-off";
			lightSprite.GetComponent<UIButton>().disabledSprite = "light-on";
			//Debug.Log("bedback color = one");
			bedBack.color = new Color(1,1,1,1);
			bedfront.color = new Color(1,1,1,1);
		}
	}
	#endregion
	public void Sound()
	{
		if(PlayerPrefs.GetInt ("Sound") == 1)
		{
			PlayerPrefs.SetInt ("Sound" , 0);
			AnimationHandler._instance.GetComponent<AudioSource>().volume = 0;
			if(Application.loadedLevelName == "BedRoom")
			{
				Camera.main.GetComponent<AudioSource>().volume = 0;
			}
			else if(Application.loadedLevelName == "MainRoom")
			{
				GameObject.FindObjectOfType<TvAlphabets>().myMesh.GetComponent<AudioSource>().volume = 0;
			}
		}
		else
		{
			PlayerPrefs.SetInt ("Sound" , 1);
			AnimationHandler._instance.GetComponent<AudioSource>().volume = 1;
			if(Application.loadedLevelName == "BedRoom")
			{
                
                if ( AnimationHandler._instance.myCharacter.GetComponent<Animation>().IsPlaying ("Sleep"))
					SnoreSound();
				Camera.main.GetComponent<AudioSource>().volume = 1;
				Camera.main.GetComponent<AudioSource>().Play ();
			}
			else if(Application.loadedLevelName == "MainRoom")
			{
				GameObject.FindObjectOfType<TvAlphabets>().myMesh.GetComponent<AudioSource>().volume = 1;
			}
		}
		Debug.Log("function called");
		SoundButtonSprites();
	}

	public void MiniGames()
	{
		previousLoadedLevel = Application.loadedLevelName;
		StartCoroutine(LoadALevel("FairyGame"));
	}

	AsyncOperation asyncOp;

	private IEnumerator LoadALevel(string index) {
		loaderPanel.SetActive(true);
		LoaderPanel.asyncLoad = true;
//		GoogleMobileAdsDemoScript._instance.HideBanner ();
		AsyncOperation asyncOp = Application.LoadLevelAsync(index);
		while(!asyncOp.isDone)
		{
			loaderPanel.GetComponent<LoaderPanel>().myFillingSprite.fillAmount = asyncOp.progress;
			yield return null;
		}
		LoaderPanel.asyncLoad = false;
		loaderPanel.SetActive(false);

	}


	public void MainRoom()
	{
		if( !PopupPanel.popupPanelActive)
		{
			StartCoroutine(LoadALevel("MainRoom"));
			AnimationHandler._instance.suspendActions = false;
		}
	}

	public void Gym()
	{
		if( !PopupPanel.popupPanelActive)
		{
			if(PlayerPrefs.HasKey("Gym"))
			{
				StartCoroutine(LoadALevel("Gym"));
				AnimationHandler._instance.suspendActions = false;
			}
			else
			{
				ActivatePopup("Do you want to buy the room for "+gymCost+" coins?",true);
				popUpPanel.GetComponent<PopupPanel>().okButton.GetComponent<UIButton>().onClick.Clear ();
				popUpPanel.GetComponent<PopupPanel>().okButton.GetComponent<UIButton>().onClick.Add(new EventDelegate(BuyRoom));
				roomClicked = "Gym";

			}
		}
	}


	public void RecordVideo()
	{
		if(InitializePlugins._instance.IsRecordSupported())
		{
			if(!InitializePlugins.isRecording)
			{
				Everyplay.StartRecording();
				recordingSprite.spriteName = "video-icon-hover";
				nameOfGame.SetActive (true);
			}
			else
			{
				recordingSprite.spriteName = "video-icon";
				Everyplay.SetMetadata ("Level" , Application.loadedLevelName);
				Everyplay.StopRecording();
				Everyplay.ShowSharingModal ();
				nameOfGame.SetActive (false);
			}
		}
		else
		{
			ActivatePopup("Recording not supported on this device!",false);
		}
	}


	public void ActivatePopup(string myText , bool okCancelTrue )
	{
		popUpPanel.SetActive (true);
		PopupPanel panelScript = popUpPanel.GetComponent<PopupPanel>();
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

	void WatchVideo()
	{
		if(allowaftersec)
		{
//			InitializePlugins.completedRewardedVideo = false;
//			Chartboost.showRewardedVideo (CBLocation.IAPStore);
//			Chartboost.cacheRewardedVideo (CBLocation.IAPStore);

			if( Advertisement.isReady() ) {
				// Show with default zone, pause engine and print result to debug log
				Advertisement.Show(null, new ShowOptions {
					pause = true,
					resultCallback = result => {
						Debug.Log(result.ToString());
						if(result.ToString ().Contains("Finished"))
						{
							PlayerPrefs.SetInt ("MyCoins",(PlayerPrefs.GetInt ("MyCoins")+30) );
							ActivatePopup("30 Coins added..!" , false);

						}
					}
				});
			}





		}
	}

	bool allowaftersec;
	void AllowAfterASec()
	{
		allowaftersec = true;
		popUpPanel.GetComponent<PopupPanel>().okButton.GetComponent<UIButton>().onClick.Clear ();
		popUpPanel.GetComponent<PopupPanel>().okButton.GetComponent<UIButton>().onClick.Add(new EventDelegate(WatchVideo));
	}

	void BuyRoom()
	{
		popUpPanel.SetActive (false);
		if(roomClicked == "Gym")
		{
			if(PlayerPrefs.GetInt ("MyCoins") > gymCost)
			{
				PlayerPrefs.SetInt ("MyCoins", PlayerPrefs.GetInt ("MyCoins") - gymCost);
				PlayerPrefs.SetInt ("Gym",1);
				ActivatePopup("Room Purchased Successfully..!" , false);
				gymLock.SetActive (false);
			}
			else
//			{
//
//				if(Advertisement.isReady ())
//				{
//					allowaftersec = false;
//					Invoke("AllowAfterASec",1.0f);
//					ActivatePopup("Not Enough Coins. Do you want to add more coins by watching a video?",true);
//					
//				}
//				else
				{
					ActivatePopup("You do not have enough coins to buy the room!" , false);
				}




//				if(Chartboost.hasRewardedVideo (CBLocation.IAPStore))
//				{
//					allowaftersec = false;
//					Invoke("AllowAfterASec",1.0f);
//					ActivatePopup("Not Enough Coins. Do you want to add more coins by watching a video?",true);
//
//				}
//				else
//				{
//					ActivatePopup("You do not have enough coins to buy the room!" , false);
//				}

			}
		else if(roomClicked == "PlayGround")
		{
			if(PlayerPrefs.GetInt ("MyCoins") > playgroundCost)
			{
				PlayerPrefs.SetInt ("MyCoins", PlayerPrefs.GetInt ("MyCoins") - playgroundCost);
				PlayerPrefs.SetInt ("PlayGround",1);
				ActivatePopup("Room Purchased Successfully..!" , false);
				playgroundLock.SetActive (false);
			}
//			else
//			{
//
//				if(Advertisement.isReady ())
//				{
//					allowaftersec = false;
//					Invoke("AllowAfterASec",1.0f);
//					ActivatePopup("Not Enough Coins. Do you want to add more coins by watching a video?",true);
//					
//				}
				else
				{
					ActivatePopup("You do not have enough coins to buy the room!" , false);
				}
//				if(Chartboost.hasRewardedVideo (CBLocation.IAPStore))
//				{
//					allowaftersec = false;
//					Invoke("AllowAfterASec",1.0f);
//					ActivatePopup("Not Enough Coins. Do you want to add more coins by watching a video?",true);
//
//				}
//				else
//				{
//					ActivatePopup("You do not have enough coins to buy the room!" , false);
//				}
			}
		}

	public void PlayRoom()
	{
		if( !PopupPanel.popupPanelActive)
		{
			if(PlayerPrefs.HasKey("PlayGround"))
			{
				StartCoroutine(LoadALevel("PlayGround"));
				AnimationHandler._instance.suspendActions = false;
			}
			else
			{
				ActivatePopup("Do you want to buy the room for "+playgroundCost+" coins?",true);
				popUpPanel.GetComponent<PopupPanel>().okButton.GetComponent<UIButton>().onClick.Clear();
				popUpPanel.GetComponent<PopupPanel>().okButton.GetComponent<UIButton>().onClick.Add(new EventDelegate(BuyRoom));
				roomClicked = "PlayGround";
				
			}
		}
	}

	public void BathRoom()
	{
		if( !PopupPanel.popupPanelActive)
		{
			StartCoroutine(LoadALevel("BathRoom"));
			AnimationHandler._instance.suspendActions = false;
		}
	}

	public void BedRoom()
	{
		if( !PopupPanel.popupPanelActive)
		{
			StartCoroutine(LoadALevel("BedRoom"));           
            AnimationHandler._instance.suspendActions = false;
		}
	}

	public void GetCoins()
	{
		if( !PopupPanel.popupPanelActive)
		{
			GameObject getCoinsPanel = (GameObject)Instantiate(Resources.Load("GetCoins"));
			getCoinsPanel.transform.parent = AnimationHandler._instance.mainScreenPanel.transform.parent;
			getCoinsPanel.transform.localScale = Vector3.one;
			AnimationHandler._instance.coinsPanelActive = true;
			AnimationHandler._instance.mainScreenPanel.SetActive (false);
	//		GoogleMobileAdsDemoScript._instance.HideBanner ();
		}
	}

	public void Back()
	{
		if( !PopupPanel.popupPanelActive)
		{
			StartCoroutine(LoadALevel("MainScene"));
//			GoogleMobileAdsDemoScript._instance.HideBanner ();
		}
	}
}
