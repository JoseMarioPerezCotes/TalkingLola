using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VirtualRealityPanel : MonoBehaviour {
	bool treesGrown , waterfallOn , butterfliesOn;
	public GameObject characterSelectionPanel , uiPanel;
	public GameObject otherAnimations,catAnimations;
	public GameObject waterFall;
	public List<TweenScale> myTress = new List<TweenScale>();
	public List<ParticleSystem> butterflies = new List<ParticleSystem>();
	public List<TweenPosition> clouds = new List<TweenPosition>();
	public List<ParticleSystem> lightning = new List<ParticleSystem>();
	public ParticleSystem rain;
	public ParticleSystem snow;
	public static VirtualRealityPanel _instance;
	public GameObject loaderPanel;
	public GameObject backButton;
	public UISprite recordingSprite;
	public GameObject popUpPanel;
	public GameObject nameSprite;
	// Use this for initialization
	void Awake () {
		if(InitializePlugins.isRecording)
		{
			InitializePlugins.isRecording = false;
			Everyplay.StopRecording ();
		}
		if(PlayerPrefs.GetInt("Sound") == 1)
		{
			AudioListener.volume = 1;
		}
		else
		{
			AudioListener.volume = 0;
		}
		_instance = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void CharacterSelection()
	{
		if(!PopupPanel.popupPanelActive)
		{
			VirtualAnimationHandler._instance.sleepParticles.transform.parent = null;
			VirtualAnimationHandler._instance.sleepParticles.Stop ();
			characterSelectionPanel.SetActive (true);
			DeactivateUI();
			backButton.SetActive (false);
		}
	}

	public void Waterfall()
	{
		if(!PopupPanel.popupPanelActive)
		{
			if(!waterfallOn)
			{
				waterFall.SetActive (true);
				waterFall.GetComponent<AudioSource>().Play ();
				waterFall.GetComponent<ParticleSystem>().Play ();
				foreach(ParticleSystem a in waterFall.GetComponentsInChildren<ParticleSystem>())
				{
					a.Play ();
				}
				waterfallOn = true;
			}
			else
			{
				waterFall.GetComponent<AudioSource>().Stop ();
				waterfallOn = false;
				waterFall.SetActive (false);
			}
		}
	}
	int weather;
	public static bool allowCloudMotion;
	public void Rain()
	{
		if(!PopupPanel.popupPanelActive)
		{
			if(weather == 0)
			{
				foreach(TweenPosition a in clouds)
				{
					a.ResetToBeginning ();
					a.gameObject.SetActive (true);
					a.Play ();
				}
				StartCoroutine(AllowCloudMotion());
				weather++;
			}
			else if(weather == 1)
			{
				rain.gameObject.SetActive (true);
				rain.Play ();
				rain.GetComponent<AudioSource>().Play ();
				foreach(ParticleSystem a in rain.GetComponentsInChildren<ParticleSystem>())
				{
					a.Play ();
				}
				weather++;
				StartCoroutine ("Thunder");
			}
			else if(weather == 2)
			{
				rain.GetComponent<AudioSource>().Stop ();

				StopCoroutine("Thunder");
				rain.gameObject.SetActive (false);
				rain.Stop ();
				foreach(ParticleSystem a in rain.GetComponentsInChildren<ParticleSystem>())
				{
					a.Stop ();
				}
				foreach(TweenPosition a in clouds)
				{
					a.gameObject.SetActive (false);
				}
				snow.gameObject.SetActive (true);
				snow.Play ();
				snow.GetComponent<AudioSource>().Play ();
				foreach(ParticleSystem a in snow.GetComponentsInChildren<ParticleSystem>())
				{
					a.Play ();
				}
				weather++;
			}
			else
			{
				weather = 0;
				snow.gameObject.SetActive (false);
				snow.Stop ();
				foreach(ParticleSystem a in snow.GetComponentsInChildren<ParticleSystem>())
				{
					a.Stop ();
				}
				allowCloudMotion = false;
			}
		}
	}

	IEnumerator AllowCloudMotion()
	{
		yield return new WaitForSeconds(1.0f);
		allowCloudMotion = true;
	}

	IEnumerator Thunder()
	{

		Debug.Log ("weather = "+weather);
		float timerOfLight = 1.5f;
		while(weather == 2)
		{

			yield return new WaitForSeconds(timerOfLight);
			timerOfLight+=0.3f;
			if(timerOfLight > 5.0f)
			{
				timerOfLight = 1.5f;
			}
			int lightningInt = Random.Range (0,lightning.Count);
			Debug.Log("lightningInt = "+lightningInt);
			lightning[lightningInt].gameObject.SetActive (true);
			lightning[lightningInt].Play ();
			lightning[lightningInt].GetComponent<AudioSource>().Play ();
			lightning[lightningInt].GetComponentInChildren<ParticleSystem>().Play ();
		}
	}

	public void Trees()
	{
		if(!PopupPanel.popupPanelActive)
		{
			if(!treesGrown)
			{
				myTress[0].transform.parent.GetComponent<AudioSource>().Play();
				foreach(TweenScale a in myTress)
				{
					StopCoroutine("TreeSound");
					a.Play ();
	//				StartCoroutine(TreeSound (a.delay , a.gameObject));
				}
				treesGrown = true;

			}
			else
			{
				treesGrown = false;
				myTress[0].transform.parent.GetComponent<AudioSource>().Play();
				foreach(TweenScale a in myTress)
				{
					a.PlayReverse ();
					StopCoroutine("TreeSound");
	//				StartCoroutine(TreeSound (a.delay , a.gameObject));
				}
			}
		}
	}

	IEnumerator TreeSound(float timer , GameObject treeObject)
	{
		yield return new WaitForSeconds(timer);
		treeObject.GetComponent<AudioSource>().Play ();
	}

	public void Butterflies()
	{
		if(!PopupPanel.popupPanelActive)
		{
			if(!butterfliesOn)
			{
				foreach(ParticleSystem a in butterflies)
				{
					a.gameObject.SetActive (true);
					a.Play ();
					foreach(ParticleSystem b in base.GetComponentsInChildren<ParticleSystem>())
					{
						a.Play ();
					}
				}
				butterfliesOn = true;
			}
			else
			{
				foreach(ParticleSystem a in butterflies)
				{
					a.gameObject.SetActive (false);
					a.Stop ();
				}
				butterfliesOn = false;
			}
		}
	}

	public void DeactivateUI()
	{
		if(uiPanel != null)
			uiPanel.SetActive (false);
		if(characterSelectionPanel.activeInHierarchy!=null)
			backButton.SetActive (true);
	}

	public void ActivateUI()
	{
		if(!characterSelectionPanel.activeInHierarchy)
		{
			uiPanel.SetActive (true);
			backButton.SetActive (true);
		}
	}

	public void Back()
	{
		Application.LoadLevel (0);
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

	public void RecordVideo()
	{
		if(InitializePlugins._instance.IsRecordSupported())
		{
			if(!InitializePlugins.isRecording)
			{
				Everyplay.StartRecording();
				recordingSprite.spriteName = "video-icon-hover";
				recordingSprite.GetComponent<UIButton>().normalSprite =  "video-icon-hover";
				nameSprite.SetActive (true);
			}
			else
			{
				recordingSprite.spriteName = "video-icon";
				recordingSprite.GetComponent<UIButton>().normalSprite =  "video-icon";
				Everyplay.SetMetadata ("Level" , Application.loadedLevelName);
				Everyplay.StopRecording();
				Everyplay.ShowSharingModal ();
				nameSprite.SetActive (false);
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

}
