using UnityEngine;
using System.Collections;

public class ExtraInteractions : MonoBehaviour {

	/// <summary>
	/// Mainroom cupboard
	/// </summary>
	bool isCupboardOut;
	public bool isCupboard;
	public ParticleSystem myParticles;
	public GameObject myToy;

	/// <summary>
	/// Playground Football
	/// </summary>
	public TweenPosition myTween;

	/// <summary>
	/// Bedroom
	/// </summary>
	public GameObject roomLight;
	public GameObject myStar;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void MakeItFalse()
	{
		Debug.Log("false");
		stopSoundRecordingDueToFlush = false;
	}
	public static bool stopSoundRecordingDueToFlush;
	void OnMouseDown()
	{
		if(!AnimationHandler._instance.coinsPanelActive  && !LoaderPanel.loaderActive  && !PopupPanel.popupPanelActive)
		{
			Debug.Log(" sound");
			if(GetComponent<AudioSource>() != null && PlayerPrefs.GetInt ("Sound") == 1)
			{
				GetComponent<AudioSource>().Play ();
				if(transform.name == "Flush")
				{
					stopSoundRecordingDueToFlush = true;
					Invoke ("MakeItFalse",2.0f);
				}
			}
			if(isCupboard)
			{
				Debug.Log("here");
				if(isCupboardOut)
				{
					transform.localPosition = new Vector3(transform.localPosition.x,transform.localPosition.y+0.12f,transform.localPosition.z);
					isCupboardOut = false;
					if(myToy)
						myToy.SetActive (false);
				}
				else
				{
					isCupboardOut = true;
					transform.localPosition = new Vector3(transform.localPosition.x,transform.localPosition.y-0.12f,transform.localPosition.z);
					if(myParticles)
						myParticles.Play ();
					if(myToy)
					{
						myToy.SetActive (true);
						if(PlayerPrefs.GetInt ("Sound") == 1)
							myToy.GetComponent<AudioSource>().Play ();
					}
				}
			}
			else
			{
				if(myTween != null && !myTween.enabled)
				{
					myTween.enabled = true;
					Invoke("StopBounce",1.99f);
				}
				if(roomLight != null)
				{
                    if (AnimationHandler._instance.myCharacter.GetComponent<Animation>().IsPlaying("Sleep"))
                    {
                        if (ScrollButtonAction.characterNo == 0) { 
                            AnimationHandler._instance.myCharacter.GetComponent<Animation>().Play("Idle2");
                        }
                        else
                        {
                            AnimationHandler._instance.myCharacter.GetComponent<Animation>().Play("Idle");
                        }
						AnimationHandler._instance.myCharacter.transform.position = AnimationHandler._instance.myCharacter.GetComponent<DetectActions>().myPosAwakeInBed;
						AnimationHandler._instance.myCharacter.transform.localEulerAngles = AnimationHandler._instance.myCharacter.GetComponent<DetectActions>().myRotAwakeInBed;
						AnimationHandler._instance.sleepParticles.Stop ();
						AnimationHandler._instance.sleepParticles.gameObject.SetActive (false);
						AnimationHandler._instance.GetComponent<AudioSource>().Stop ();
						AnimationHandler._instance.myCharacter.GetComponent<DetectActions>().CallMakeItSleep ();
					}
                    if (roomLight.GetComponent<Light>().intensity < 0.1f)//if(roomLight.GetComponent<Light>().intensity == 0.0f)
                    {
						roomLight.GetComponent<Light>().intensity = 0.5f;
						GameObject.Find("UI Root").GetComponent<RoomUIManager>().ChangeLightSprites ();
					}

					Destroy (gameObject);
					myStar.SetActive (true);
					if(PlayerPrefs.GetInt ("Sound") == 1)
						myStar.GetComponent<AudioSource>().Play();
				}
			}
		}
	}

	void StopBounce()
	{
		myTween.enabled = false;
	}

}
