using UnityEngine;
using System.Collections;

public class DetectActions : MonoBehaviour {
	LayerMask layerMask;
	Vector2 initialPos,finalPos;
	GameObject starParticles;
	RoomUIManager uiManager;
	public AnimationClip idleAnim,tickleAnim,hitAnim,fartAnim,slapAnimLeft,slapAnimRight,successAnim,hitCompleteAnim,listenAnim,towelAnim;
	public Vector3 myPoitionInLoo;
	public Vector3 myScaleInLoo;
	public GameObject myShadow;
	public Vector3 myPosAwakeInBed , myRotAwakeInBed;

	public GameObject myTowel;

	// Use this for initialization
	void Start () {
		layerMask = 1<<8;
		uiManager = GameObject.Find("UI Root").GetComponent<RoomUIManager>();
	}

	void StarsPos()
	{
		if(ScrollButtonAction.characterNo == 0)
			starParticles.transform.position = new Vector3(transform.position.x - 0.16f , transform.position.y + 4.32f,0);
		else if(ScrollButtonAction.characterNo == 1)
			starParticles.transform.position = new Vector3(transform.position.x + 1.5f , transform.position.y + 3.84f,0);
		else if(ScrollButtonAction.characterNo == 3)
			starParticles.transform.position = new Vector3(transform.position.x + 1.5f , transform.position.y + 3.5f,0);
		else if(ScrollButtonAction.characterNo == 2)
			starParticles.transform.position = new Vector3(transform.position.x + 0.42f , transform.position.y + 3.88f,0);

	}

	void ShowStars()
	{
		if(AnimationHandler._instance.myCharacter.GetComponent<Animation>().IsPlaying ("Hit"))
		{
            if (starParticles == null)
            {
                starParticles = (GameObject)Instantiate(Resources.Load("Headstars"));
            }
			StarsPos ();
			starParticles.GetComponent<ParticleSystem>().Play ();
			if(PlayerPrefs.GetInt("Sound") == 1)
			{
				AnimationHandler._instance.GetComponent<AudioSource>().clip = InGameSoundManager._instance.headhit;
				AnimationHandler._instance.GetComponent<AudioSource>().Play ();
			}
		}
	}

	public void StopStar()
	{
		if(starParticles != null)
		{
			Debug.Log("stop star");
			starParticles.GetComponent<ParticleSystem>().Stop ();
		}
	}
	// Update is called once per frame
	void Update () {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if(!ObjectMotion.bathroomObjectsDragged && !AnimationHandler._instance.isSoapOnBody && !AnimationHandler._instance.coinsPanelActive  && !LoaderPanel.loaderActive  && !PopupPanel.popupPanelActive)
		{
			bool allow = (!AnimationHandler._instance.suspendActions || Application.loadedLevelName == "BedRoom");
			if(Input.GetMouseButtonDown(0) && (!AnimationHandler._instance.suspendActions || Application.loadedLevelName == "BedRoom"))
			{
				if (Physics.Raycast(ray, out hit, 100,layerMask))
				{
					if(hit.collider.name == "Bip01 Head")
					{
						if(Application.loadedLevelName == "BedRoom")
						{
							if(transform.GetComponent<Animation>().IsPlaying ("Sleep"))
							{
								if(ScrollButtonAction.characterNo == 0) { 
									transform.GetComponent<Animation>().Play("Idle2");
                                }
                                else
                                { 
									transform.GetComponent<Animation>().Play("Idle");
                                }
                                AnimationHandler._instance.sleepParticles.Stop ();
								AnimationHandler._instance.sleepParticles.gameObject.SetActive (false);
								transform.position = myPosAwakeInBed;
								transform.localEulerAngles = myRotAwakeInBed;
								Invoke ("MakeItSleep",3.0f);
								AnimationHandler._instance.GetComponent<AudioSource>().Stop ();
//								if(PlayerPrefs.GetInt("Sound") == 1)
//								{
//									AnimationHandler._instance.audio.loop = false;
//									AnimationHandler._instance.audio.clip = InGameSoundManager._instance.wakeUp;
//									AnimationHandler._instance.audio.Play ();
//								}
							}

						}
						else
						{
							AnimationHandler._instance.consecutiveHitCount++;
							if(AnimationHandler._instance.consecutiveHitCount > 2)
							{
								AnimationHandler._instance.PlayAnim(hitCompleteAnim.name,hitCompleteAnim.length,1);
							
								Invoke ("ShowStars",hitCompleteAnim.length/4);
								AnimationHandler._instance.consecutiveHitCount = 0;
							}
							else
								AnimationHandler._instance.PlayAnim(hitAnim.name,hitAnim.length/2.5f, 2.5f);
							if(PlayerPrefs.GetInt ("Sound") == 1)
							{
								AnimationHandler._instance.GetComponent<AudioSource>().clip = InGameSoundManager._instance.punch;
								AnimationHandler._instance.GetComponent<AudioSource>().Play ();
							}
						}
					}
					else if(hit.collider.name == "Bip01 Spine1")
					{
						AnimationHandler._instance.PlayAnim(tickleAnim.name,1.3f,1);
						if(PlayerPrefs.GetInt ("Sound") == 1)
						{
							AnimationHandler._instance.GetComponent<AudioSource>().clip = InGameSoundManager._instance.tickling;
							AnimationHandler._instance.GetComponent<AudioSource>().Play ();
						}
					}
				}
				else
				{
					initialPos = Input.mousePosition;
				}
			}
			else if(Input.GetMouseButton(0))
			{
				finalPos = Input.mousePosition;
				if (Physics.Raycast(ray, out hit, 100,layerMask) && Vector2.Distance(initialPos,finalPos) > 70.0f && !AnimationHandler._instance.suspendActions)
				{

					if(hit.collider.name == "Bip01 L Ear 1" || hit.collider.name == "Eye Left")
					{
						Debug.Log("moving -- "+Vector2.Distance(initialPos,finalPos));
						AnimationHandler._instance.PlayAnim(slapAnimRight.name,slapAnimRight.length/5.5f,5.5f);
						initialPos = Vector2.zero;
						if(PlayerPrefs.GetInt ("Sound") == 1)
						{
							AnimationHandler._instance.GetComponent<AudioSource>().clip = InGameSoundManager._instance.slap;
							AnimationHandler._instance.GetComponent<AudioSource>().Play ();
						}
					}
					else if(hit.collider.name == "Bip01 R Ear 1" || hit.collider.name == "Eye Right")
					{
						Debug.Log("moving -- "+Vector2.Distance(initialPos,finalPos));
						AnimationHandler._instance.PlayAnim(slapAnimLeft.name,slapAnimLeft.length/5.5f,5.5f);
						initialPos = Vector2.zero;
						if(PlayerPrefs.GetInt ("Sound") == 1)
						{
							AnimationHandler._instance.GetComponent<AudioSource>().clip = InGameSoundManager._instance.slap;
							AnimationHandler._instance.GetComponent<AudioSource>().Play ();
						}
					}
				}
			}
			else if(Input.GetMouseButtonUp(0))
			{
				finalPos = Input.mousePosition;
				if (Physics.Raycast(ray, out hit, 100,layerMask) && Vector2.Distance(initialPos,finalPos) > 70.0f && !AnimationHandler._instance.suspendActions )
				{
					if(hit.collider.name == "Bip01 L Ear 1" )
					{
						Debug.Log("moving -- "+Vector2.Distance(initialPos,finalPos));
						AnimationHandler._instance.PlayAnim(slapAnimRight.name,slapAnimRight.length/5.5f,5.5f);
						if(PlayerPrefs.GetInt ("Sound") == 1)
						{
							AnimationHandler._instance.GetComponent<AudioSource>().clip = InGameSoundManager._instance.slap;
							AnimationHandler._instance.GetComponent<AudioSource>().Play ();
						}
						initialPos = Vector2.zero;
					}
					else if(hit.collider.name == "Bip01 R Ear 1")
					{
						Debug.Log("moving -- "+Vector2.Distance(initialPos,finalPos));
						AnimationHandler._instance.PlayAnim(slapAnimLeft.name,slapAnimLeft.length/5.5f,5.5f);
						if(PlayerPrefs.GetInt ("Sound") == 1)
						{
							AnimationHandler._instance.GetComponent<AudioSource>().clip = InGameSoundManager._instance.slap;
							AnimationHandler._instance.GetComponent<AudioSource>().Play ();
						}
						initialPos = Vector2.zero;
					}
				}
			}
		}
	}

	public void CallMakeItSleep()
	{
		Invoke ("MakeItSleep",3.0f);
	}

	public void CancelSleepInvoke()
	{
		CancelInvoke("MakeItSleep");
	}

	public void MakeItSleep()
	{
		transform.GetComponent<Animation>().Play ("Sleep");
		AnimationHandler._instance.BedroomPosition ();
        AnimationHandler._instance.sleepParticles.gameObject.SetActive (true);
		AnimationHandler._instance.sleepParticles.Play ();
		uiManager.roomLight.GetComponent<Light>().intensity = 0.1f;
		uiManager.ChangeLightSprites();
		if(PlayerPrefs.GetInt ("Sound") == 1)
		{
			AnimationHandler._instance.GetComponent<AudioSource>().loop = true;
			AnimationHandler._instance.GetComponent<AudioSource>().clip = InGameSoundManager._instance.snoring;
			AnimationHandler._instance.GetComponent<AudioSource>().Play ();
		}
	}

}
