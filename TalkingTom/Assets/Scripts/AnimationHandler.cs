using UnityEngine;
using System.Collections;

public class AnimationHandler : MonoBehaviour {


	/// <summary>
	/// BathRoom Items
	/// </summary>
	bool isPlayingTowelAnim;
	Vector3 characterPositionInLoo;
	public Vector3 characterGeneralPosInBathRoom;
	public GameObject brush , brushButton , door , shower , bucket , soap;
	public bool isSoapOnBody,isInLoo ;


	/// <summary>
	/// Gym Items
	/// </summary>
 	IEnumerator gymCoroutine;
	public GameObject walkButton,runButton,treadmill;
	public GameObject treadmillCounter;
	public UISprite runFillSprite;
	/// <summary>
	/// PlayGroundItems
	/// </summary>
	int countOfJump;
	IEnumerator playgroundCoroutine;
	public GameObject trampoline;
	public GameObject jumpButton;
	public bool isJumping , trampolineActive;
	public Vector3 characterPosInPlayground;

	/// <summary>
	/// Bedroom Items
	/// </summary>
	public ParticleSystem sleepParticles;
	public GameObject bedroomLight;

	public int consecutiveHitCount;
	public GameObject myCharacter;
	public DetectActions myCharacterDetectActions;
	public bool suspendActions, isPlayingSlapAnim , coinsPanelActive;
	public GameObject fartButton,danceButton , characterSelectionButton;
	public GameObject mainScreenPanel;
	public AudioSource audioPlayingSource; 
	public GameObject charSelection, charSelectionPrefab;
	public GameObject fadeInOutSprite;

	public static AnimationHandler _instance;



	// Use this for initialization
	void Awake () {
		isInLoo = false;
		Screen.orientation = ScreenOrientation.Portrait;
		_instance = this;
		TvAlphabets.tvOn = false;
		mainScreenPanel = GameObject.Find("MainScenePanel");
		if(Application.loadedLevelName == "BathRoom")
		{
			brushButton.SetActive(true);
		}
		else if(Application.loadedLevelName == "Gym")
		{
			walkButton.SetActive(true);
			runButton.SetActive(true);
		}
		else if(Application.loadedLevelName == "BedRoom")
		{
			suspendActions = true;
			fartButton.SetActive (false);
			danceButton.SetActive (false);
		}
		else if(Application.loadedLevelName == "PlayGround")
		{
			jumpButton.SetActive (true);
		}
		runFillSprite.fillAmount = 0.0f;
	}
    //REMOVER BOTON PEDO
	public void RemoveExtraButtons()
	{
        if (ScrollButtonAction.characterNo > 1)//if(ScrollButtonAction.characterNo < 3)
        {
			fartButton.SetActive (false);
			danceButton.SetActive (false);
			if(Application.loadedLevelName == "BathRoom")
			{
                brushButton.SetActive(false);
            }
		}
		else
		{
			if(Application.loadedLevelName != "BedRoom")
			{
				fartButton.SetActive (true);
				danceButton.SetActive (true);
			}
			if(Application.loadedLevelName == "BathRoom")
			{
                brushButton.SetActive(true);
            }
        }
	}

	void Start()
	{
		Screen.orientation = ScreenOrientation.Portrait;
		if(Application.loadedLevelName == "BedRoom")
		{
			Debug.LogError("Aaaaa");
            bedroomLight.GetComponent<Light>().intensity = 0.1f;
            myCharacter.GetComponent<Animation>().Play ("Sleep");
			Debug.LogError("ccccccccccccaaa");
			if(myCharacterDetectActions.myTowel != null) { 
				myCharacterDetectActions.myTowel.SetActive (false);
            }
            StopRecording();
			Debug.LogError("bbaaa");
		}
		else
			audioPlayingSource.GetComponent<AudioSource>().pitch = 1.4f;
	}
	// Update is called once per frame
	void Update () {
		if(myCharacter)
		{
			if(myCharacter.GetComponent<Animation>().IsPlaying ("Jump") && myCharacter.GetComponent<Rigidbody>().velocity.y < 0 && isJumping)
			{
               // Debug.Log("Salto" + countOfJump);
				countOfJump++;
              //  myCharacter.GetComponent<Rigidbody>().isKinematic = false;
               
                myCharacter.GetComponent<Animation>().Play ("Fall");
                
                isJumping = false;
				if(countOfJump > 4)
				{
					RemoveTrampoline();
				}
			}
			else if(countOfJump > 4)
			{
				if(myCharacter.transform.position.y < (characterPosInPlayground.y+0.5f))
				{
					BringBackCharacterToNormalAfterJump();
                   // myCharacter.GetComponent<Rigidbody>().isKinematic = true;
                }
			}
		}
	}

	public 	void BedroomPosition()
	{
		switch(ScrollButtonAction.characterNo)
		{
            case 0:
                myCharacter.transform.position = new Vector3(-0.86f, 1.2f, 2.350002f);
                myCharacter.transform.rotation = Quaternion.Euler(-62.00067f, 152.1f, 38.81095f);
                break;
            case 1:
                myCharacter.transform.position = new Vector3(-0.3f, 0.1800001f, 2.74f);
                myCharacter.transform.rotation = Quaternion.Euler(-63.0f, 0f, 0f);//myCharacter.transform.rotation = Quaternion.Euler(0f,110f,0f);
                break;
            case 2://buo
                myCharacter.transform.position = new Vector3(-0.05f, -0.02f, 1.21f);//myCharacter.transform.position = new Vector3(1.3f,1.02f,3.029746f);
                myCharacter.transform.rotation = Quaternion.Euler(-42.19f, -161.3001f, -1.728469e-06f);//myCharacter.transform.rotation = Quaternion.Euler(23.15633f,133.62f,-56.47394f);
                break;
            case 3://morrocoyo
                myCharacter.transform.position = new Vector3(-0.856347f, 0.730195f, 2.74f);
                myCharacter.transform.rotation = Quaternion.Euler(90.0f, 150.1f, 0f); //myCharacter.transform.rotation = Quaternion.Euler(-33.49133f,105.4364f,-12.82684f);
                break;
        }
	}



	/// <summary>
	/// Stops the recording of voice while playing other animations
	/// </summary>
	public void StopRecording()
	{
		suspendActions = true;
		CancelInvoke("RevertToIdle");
		CancelInvoke("RemoveSlap");
		myCharacter.GetComponent<VoiceRecognitionRepeating>().StopIt();
	}

	public void TvStopit()
	{
		myCharacter.GetComponent<VoiceRecognitionRepeating>().StopIt();
	}

	void OnGUI() 
	{
//		if (GUI.Button(new Rect(10,220,60,50),"Idle"))
//		{ 
//			RevertToIdle ();
//		} 

	}

	public void CharacterSelection()
	{
		
		if( !PopupPanel.popupPanelActive)
		{
			
//			GoogleMobileAdsDemoScript._instance.HideBanner ();
			PlayerPrefs.SetInt ("CharSelectAd",PlayerPrefs.GetInt("CharSelectAd")+1);
			if(PlayerPrefs.GetInt("CharSelectAd") > 1)
			{
				PlayerPrefs.SetInt("CharSelectAd", 0);
	//			GoogleMobileAdsDemoScript._instance.ShowInterstitial ();
			}
			if(charSelection == null)
			{
				charSelection = (GameObject)Instantiate(charSelectionPrefab);
			
				Debug.LogError("done");

				charSelection.transform.parent = transform;
				charSelection.transform.localScale = new Vector3(1.4f,1.4f,1.4f);
			}
			else
				charSelection.SetActive (true);
			if(isInLoo)
			{
				isInLoo = false;
				GameObject.Find("door").transform.position = new Vector3(4.72f,GameObject.Find("door").transform.position.y,GameObject.Find("door").transform.position.z);
				CancelInvoke ("NowLoo");
			}
			RevertToIdle ();
			myCharacter.SetActive(false);
			mainScreenPanel.SetActive(false);
			RemoveTrampoline();
			trampolineActive = false;
			if(treadmill)
			{
				RemoveTreadmill();
			}
			if(Application.loadedLevelName == "BathRoom")
			{
				GameObject.Find("sponge").GetComponent<PaintBrush>().bubble.GetComponent<ParticleSystem>().Stop ();
			}
			StopRecording();
			Destroy (myCharacter);
			coinsPanelActive = true;
			if(sleepParticles != null)
			{
				bedroomLight.GetComponent<Light>().intensity = 0.5f;
				sleepParticles.Stop ();
			}
			GetComponent<AudioSource>().loop = false;
			GetComponent<AudioSource>().Stop();
		}
	}

	/// <summary>
	/// Reverts to idle.
	/// </summary>
	public void RevertToIdle()
	{
		GetComponent<AudioSource>().loop = false;
		GetComponent<AudioSource>().Stop();
		if(isInLoo && myCharacter)
		{
			fadeInOutSprite.SetActive (true);
			fadeInOutSprite.GetComponent<TweenScale>().Play ();
			Invoke ("FadeOutStop" , 1.6f);
			characterSelectionButton.SetActive (false);
			Invoke ("GeneralPosFromLoo" , 0.8f);

		}
		if(Application.loadedLevelName == "BathRoom")
		{
            //ACTIVAR BOTON CEPILLO DE DIENTES
            if (ScrollButtonAction.characterNo == 0)//if (ScrollButtonAction.characterNo == 3) { 
            {
                
				brushButton.SetActive (true);
            }
            bucket.SetActive (true);
			shower.SetActive (true);
			soap.SetActive (true);
		}
		if(!isInLoo && myCharacter)
		{
			myCharacter.GetComponent<Animation>().Play ("Idle");
			if(!TvAlphabets.tvOn)
				myCharacter.GetComponent<VoiceRecognitionRepeating>().SubsequentRecord ();
		}
		suspendActions = false;
		if(myCharacter)
		{
			if(myCharacterDetectActions.myTowel)
				myCharacterDetectActions.myTowel.SetActive (false);
		}
		if(brush)
			brush.SetActive (false);
		if(treadmill)
		{
			RemoveTreadmill();
		}
		if(runFillSprite)
			runFillSprite.fillAmount = 0f;
	}

	void GeneralPosFromLoo()
	{

		myCharacter.transform.position = characterGeneralPosInBathRoom;
		if(ScrollButtonAction.characterNo == 3)
			myCharacter.transform.localScale = new Vector3(1.2f,1.2f,1.2f);
		else
			myCharacter.transform.localScale = Vector3.one;
		
		GameObject.Find("door").transform.position = new Vector3(4.72f,GameObject.Find("door").transform.position.y,GameObject.Find("door").transform.position.z);

		myCharacter.GetComponent<Animation>().Play ("Idle");
		myCharacter.GetComponent<VoiceRecognitionRepeating>().SubsequentRecord ();
		GetComponent<AudioSource>().clip = InGameSoundManager._instance.flush;

		GetComponent<AudioSource>().Play ();
        //TIRAR PEDOS EN NORMAL
		if(ScrollButtonAction.characterNo <= 1)
		{
			fartButton.SetActive (true);
			danceButton.SetActive (true);
		}
		isInLoo = false;
	}
	/// <summary>
	/// Dance the character.
	/// </summary>
	public void Dance()
	{
		if(!AnimationHandler._instance.isInLoo && !PopupPanel.popupPanelActive)
		{
			bool play = false;
			if(brush)
			{
				if(!brush.activeInHierarchy)
				{
					play = true;
				}
			}else
			{
				play = true;
			}
			if(play)
			{
				PlayAnim("Dance" ,10 , 1);
				Debug.Log("sound = "+PlayerPrefs.GetInt ("Sound"));
				if(PlayerPrefs.GetInt ("Sound") == 1)
				{
					GetComponent<AudioSource>().clip = InGameSoundManager._instance.dance;
					GetComponent<AudioSource>().loop = true;
					GetComponent<AudioSource>().Play ();
				}
			}
			runFillSprite.fillAmount = 0;
		}
	}

	public void Loo()
	{
		myCharacterDetectActions.StopStar();
		RevertToIdle ();
		isInLoo = true;
		CancelInvoke("RemoveSlap");
		StopRecording();
		Invoke("RevertToIdle",10.0f);
		fartButton.SetActive (false);
		danceButton.SetActive (false);

		fadeInOutSprite.SetActive (true);
		fadeInOutSprite.GetComponent<TweenScale>().Play ();
		Invoke ("FadeOutStop" , 1.6f);
		characterSelectionButton.SetActive (false);
		Invoke("NowLoo" , 1.0f);

	}


	void NowLoo()
	{

		if(PlayerPrefs.GetInt ("Sound") == 1)
		{
			GetComponent<AudioSource>().clip = InGameSoundManager._instance.loo;
			GetComponent<AudioSource>().Play ();
		}
		brush.SetActive (false);
		myCharacter.GetComponent<Animation>().Play ("Loo");
		myCharacter.transform.position = myCharacterDetectActions.myPoitionInLoo;
		myCharacter.transform.localScale = myCharacterDetectActions.myScaleInLoo;
		if(myCharacterDetectActions.myTowel)
			myCharacterDetectActions.myTowel.SetActive (false);

		if(ScrollButtonAction.characterNo == 3)
			brushButton.SetActive (false);
		bucket.SetActive (false);
		shower.SetActive (false);
		soap.SetActive (false);
	}
	void FadeOutStop()
	{
		fadeInOutSprite.SetActive (false);
		characterSelectionButton.SetActive (true);
	}
	/// <summary>
	/// Fart the character.
	/// </summary>
	public void Fart()
	{
		if(!AnimationHandler._instance.isInLoo && !PopupPanel.popupPanelActive)
		{
			bool play = false;
			if(brush)
			{
				if(!brush.activeInHierarchy)
				{
					play = true;
				}
			}else
			{
				play = true;
			}
			if(play)
			{
				myCharacterDetectActions.StopStar();
				GetComponent<AudioSource>().loop = false;
				GetComponent<AudioSource>().Stop ();
				PlayAnim("Fart" ,myCharacterDetectActions.fartAnim.length , 1);
				CancelInvoke("FartParticles");
				Invoke("FartParticles",myCharacterDetectActions.fartAnim.length/2);
			}
			runFillSprite.fillAmount = 0;
		}
	}

	void FartParticles()
	{
//		if(fartParticles == null)
		if(myCharacter.GetComponent<Animation>().IsPlaying("Fart"))
		{
			if(PlayerPrefs.GetInt ("Sound") == 1)
			{
				GetComponent<AudioSource>().clip = InGameSoundManager._instance.fart;
				GetComponent<AudioSource>().Play ();
			}
			GameObject	fartParticles = (GameObject)Instantiate(Resources.Load("poof"));
			fartParticles.transform.position = new Vector3(myCharacter.transform.position.x , myCharacter.transform.position.y + 1.82f,myCharacter.transform.position.z+1);
			fartParticles.GetComponent<ParticleSystem>().Play ();
			Destroy (fartParticles,1.0f);
		}
	}

	#region GYM

	/// <summary>
	/// Bring the Treadmill to start the Gym Animation
	/// </summary>
	/// <param name="anim">Animation name.</param>
	/// <param name="animLength">Animation length.</param>
	/// <param name="animSpeed">Animation speed.</param>
	void GymAnim(string anim , float animLength , float animSpeed)
	{
		if(!myCharacter.GetComponent<Animation>().IsPlaying ("Walk") && !myCharacter.GetComponent<Animation>().IsPlaying ("Run"))
		{
			if(!myCharacter.GetComponent<Animation>().IsPlaying ("Success"))
			{
				myCharacter.GetComponent<Animation>().Play ("Success");
				if(PlayerPrefs.GetInt ("Sound") == 1)
				{
					GetComponent<AudioSource>().clip = InGameSoundManager._instance.yippee;
					GetComponent<AudioSource>().loop = false;
					GetComponent<AudioSource>().Play ();
				}
				gymCoroutine = GymAnimPlay(anim,animLength,animSpeed,1);
				TweenPosition.Begin(treadmill,1.0f,new Vector3(0,treadmill.transform.position.y,treadmill.transform.position.z));
			}
		}
		else
		{
			gymCoroutine = GymAnimPlay(anim,animLength,animSpeed,0.001f);

		}
		myCharacterDetectActions.StopStar();
		StartCoroutine(gymCoroutine);
		StopRecording();

	}

	/// <summary>
	///Play the Gym Animation
	/// </summary>
	/// <returns>The animation play.</returns>
	/// <param name="anim">Animation Name.</param>
	/// <param name="animLength">Animation length.</param>
	/// <param name="animSpeed">Animation speed.</param>
	/// <param name="waitTime">Wait time.</param>
	IEnumerator GymAnimPlay(string anim , float animLength , float animSpeed,float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		treadmillCounter.GetComponent<MyCounter>().StartCounter ();
		treadmillCounter.SetActive (true);
		myCharacter.GetComponent<Animation>().Play(anim);
		myCharacter.GetComponent<Animation>()[anim].speed = animSpeed;
		CancelInvoke("RevertToIdle");
		CancelInvoke("RemoveSlap");
		Invoke("RevertToIdle",animLength);
		if(waitTime > 0.5f)
		{
			if(PlayerPrefs.GetInt ("Sound") == 1)
			{
				GetComponent<AudioSource>().Stop ();
				GetComponent<AudioSource>().clip = InGameSoundManager._instance.gym;
				GetComponent<AudioSource>().loop = true;
				GetComponent<AudioSource>().Play ();
			}
		}
	}

	
	/// <summary>
	/// Removes the treadmill.
	/// </summary>
	void RemoveTreadmill()
	{
//		if(treadmill.GetComponent<TweenPosition>())
//		{
//
//			Destroy(treadmill.GetComponent<TweenPosition>());
//		}
		TweenPosition.Begin(treadmill,0.3f,new Vector3(7,treadmill.transform.position.y,treadmill.transform.position.z));
		if(gymCoroutine != null)
			StopCoroutine(gymCoroutine);
//		treadmill.transform.position = new Vector3(7,treadmill.transform.position.y,treadmill.transform.position.z);
		treadmillCounter.SetActive (false);
		treadmillCounter.GetComponent<MyCounter>().StopCounter ();
	}

	#endregion
	/// <summary>
	/// Walk 
	/// </summary>
	public void Walk()
	{
		if( !PopupPanel.popupPanelActive)
		{
			runSpeed = 1;
			GymAnim("Walk" ,11,1);
			runFillSprite.fillAmount = 0.0f;
		}

	}

	public float runSpeed = 1;

	/// <summary>
	/// Run 
	/// </summary>
	public void Run()
	{
		if( !PopupPanel.popupPanelActive)
		{
			if(myCharacter.GetComponent<Animation>().IsPlaying ("Run") )  //Increasing run speed Successively
			{
				runSpeed+=0.2f;
				Debug.Log("runSpeed = "+runSpeed);
				if(runSpeed == 1.2f)
				{
					runFillSprite.fillAmount = 0.5f;
				}
				else if(runSpeed > 1.3f && runSpeed < 1.5f)
				{
					runFillSprite.fillAmount = 0.75f;
				}
				else
				{
					runFillSprite.fillAmount = 1.0f;
				}

				if(runSpeed > 1.7f)
				{
					runSpeed = 1.0f;
					runFillSprite.fillAmount = 0.25f;
				}
			}
			else
			{
				runSpeed = 1;
				runFillSprite.fillAmount = 0.25f;
			}
			myCharacterDetectActions.StopStar();
			GymAnim("Run" ,11,runSpeed);
		}
	}

	/// <summary>
	/// Brush Animation
	/// </summary>
	public void Brush()
	{
		if(!AnimationHandler._instance.isInLoo && !AnimationHandler._instance.isSoapOnBody && !PopupPanel.popupPanelActive)
		{
			GetComponent<AudioSource>().loop = false;
			GetComponent<AudioSource>().Stop ();
			if(!brush.activeInHierarchy)
			{
				brush.SetActive(true);
				myCharacter.GetComponent<Animation>().Play ("Brushing");
				StopRecording();
                //			Invoke("RevertToIdle",5.0f);
                if (myCharacterDetectActions.myTowel) {
                   // Debug.Log("my Towel");
					myCharacterDetectActions.myTowel.SetActive (false);
                }
            }
			else
			{
				RevertToIdle();
			}
			myCharacterDetectActions.StopStar();
		}
	}




	/// <summary>
	/// Play the different animations.
	/// </summary>
	/// <param name="anim">Animation Name.</param>
	/// <param name="animLength">Animation length.</param>
	/// <param name="animSpeed">Animation speed.</param>
	public void PlayAnim(string anim , float animLength , float animSpeed)
	{
		myCharacterDetectActions.StopStar();
		if(gymCoroutine != null)
			StopCoroutine(gymCoroutine);
		if(playgroundCoroutine != null)
			StopCoroutine(playgroundCoroutine);
		if(treadmill)
		{
			RemoveTreadmill();
		}
		if(trampoline)
		{
			RemoveTrampoline();
			BringBackCharacterToNormalAfterJump();
		}
		myCharacter.GetComponent<Animation>().Play(anim);
		myCharacter.GetComponent<Animation>()[anim].speed = animSpeed;
        if (!anim.Contains("Slap"))//if(!anim.Contains("Slap"))
        {
           // Debug.Log("Secarse con Toalla");
			CancelInvoke("RevertToIdle");
			CancelInvoke("RemoveSlap");
			if(myCharacterDetectActions.myTowel)
			{
				if(anim.Contains("Towel"))
				{
					myCharacterDetectActions.myTowel.SetActive (true);
				}
                else { 
					myCharacterDetectActions.myTowel.SetActive (false);
                }
            }
			StopRecording();
			isPlayingSlapAnim = false;
			Invoke("RevertToIdle",animLength);
		}
		else
		{
			if(myCharacterDetectActions.myTowel)
				myCharacterDetectActions.myTowel.SetActive (false);
			isPlayingSlapAnim = true;
			myCharacter.GetComponent<VoiceRecognitionRepeating>().StopIt();
			Invoke("RemoveSlap",animLength);
		}

	}

	/// <summary>
	/// Removes the slap Animation
	/// </summary>
	void RemoveSlap()
	{
		if(myCharacter.GetComponent<Animation>().IsPlaying ("SlapR") || myCharacter.GetComponent<Animation>().IsPlaying ("SlapL"))
		{
			myCharacter.GetComponent<Animation>().Play ("Idle");
			if(!TvAlphabets.tvOn)
				myCharacter.GetComponent<VoiceRecognitionRepeating>().SubsequentRecord ();
		}
		isPlayingSlapAnim = false;
		GetComponent<AudioSource>().loop = false;
		GetComponent<AudioSource>().Stop();

	}


	#region PLAYGROUND

	public void Trampoline()
	{
		if(!trampolineActive && !PopupPanel.popupPanelActive)
		{
			myCharacterDetectActions.StopStar();
//			myCharacterDetectActions.myShadow.SetActive (false);
			fartButton.SetActive (false);
			danceButton.SetActive (false);
			countOfJump = 0;
			TweenPosition.Begin(trampoline,1.0f,new Vector3(0,trampoline.transform.position.y,trampoline.transform.position.z));
			playgroundCoroutine = PlayGroundAnimPlay();
			GetComponent<AudioSource>().loop = false;
			GetComponent<AudioSource>().Stop ();
			StartCoroutine(playgroundCoroutine);
			myCharacter.GetComponent<Animation>().Play ("Idle");
			StopRecording();
			trampolineActive = true;
		}
	}

	/// <summary>
	///Play the Playground Jump Animation
	/// </summary>
	/// <returns>The animation playground.</returns>

	IEnumerator PlayGroundAnimPlay()
	{
		yield return new WaitForSeconds(1.0f);
		myCharacter.GetComponent<MyColliders>().body.enabled = true;
		myCharacter.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
		myCharacter.GetComponent<Rigidbody>().AddForce(new Vector3(0,600,0));
		if(PlayerPrefs.GetInt ("Sound") == 1)
		{
			GetComponent<AudioSource>().clip = InGameSoundManager._instance.jump;
			GetComponent<AudioSource>().Play ();
		}
		myCharacter.GetComponent<Animation>().Play("Jump");
		isJumping = true;
		trampoline.GetComponent<Animation>().Play ();
		CancelInvoke("RevertToIdle");
		CancelInvoke("RemoveSlap");
//		Invoke("RevertToIdle",10);
	}


	/// <summary>
	/// Removes the trampoline.
	/// </summary>
	public void RemoveTrampoline()
	{
		if(trampoline)
		{
			TweenPosition.Begin(trampoline,0.3f,new Vector3(8,trampoline.transform.position.y,trampoline.transform.position.z));
			if(playgroundCoroutine != null)
				StopCoroutine(playgroundCoroutine);
//			trampoline.transform.position = new Vector3(8,trampoline.transform.position.y,trampoline.transform.position.z);

		}
	}

	public void BringBackCharacterToNormalAfterJump()
	{
		countOfJump = 0;
		myCharacter.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
		myCharacter.transform.position = characterPosInPlayground;
		if(ScrollButtonAction.characterNo == 0)
		{
			fartButton.SetActive (true);
			danceButton.SetActive (true);
		}
		trampolineActive = false;
		RevertToIdle();
//		myCharacterDetectActions.myShadow.SetActive (true);
	}
	#endregion

}
