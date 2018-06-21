using UnityEngine;
using System.Collections;

public class VirtualAnimationHandler : MonoBehaviour {
	GameObject starParticles;
//	public AnimationClip idleAnim,tickleAnim,hitStartAnim,slapAnimLeft,slapAnimRight,hitCompleteAnim,fartAnim;
	int consecutiveHitCount;
	bool isPlayingSlapAnim;
	bool suspendActions;
	public DinoMove myDino;
	public bool isJumping;
	int countOfJump;
	float characterPosInPlayground;
	public static VirtualAnimationHandler _instance;
	VirtualRealityPanel uiObject;
	public ParticleSystem sleepParticles;
	// Use this for initialization
	void Start () {
		_instance = this;
//		MoreApps._instance.moreAppsButton.SetActive (false);
//		CrossPromo._instance.promoImage.gameObject.SetActive (false);
		PlayerPrefs.SetInt ("Sound",1);
		uiObject = GetComponent<VirtualRealityPanel>();
	}
	
	// Update is called once per frame
	void Update () {
		if(DinoMove._instance.turtle)
		{
			if(isJumping)
			{
				if(DinoMove._instance.turtle.GetComponent<Animation>().IsPlaying ("Jump") )
				{
					if(Time.time - startTime <= 0)
						DinoMove._instance.turtle.transform.localPosition = new Vector3(DinoMove._instance.turtle.transform.localPosition.x,DinoMove._instance.turtle.transform.localPosition.y+Time.deltaTime,DinoMove._instance.turtle.transform.localPosition.z);
					else
						DinoMove._instance.turtle.transform.localPosition = new Vector3(DinoMove._instance.turtle.transform.localPosition.x,DinoMove._instance.turtle.transform.localPosition.y+(1/(20*(Time.time-startTime))),DinoMove._instance.turtle.transform.localPosition.z);
						
					if(DinoMove._instance.turtle.transform.localPosition.y > 22f || ((Time.time-startTime) > 1.5f))
					{
						countOfJump++;
						startTime = Time.time;
						DinoMove._instance.turtle.GetComponent<Animation>().Play ("Fall");
					}
		
				}
				else if(DinoMove._instance.turtle.GetComponent<Animation>().IsPlaying ("Fall") )
				{
					DinoMove._instance.turtle.transform.localPosition = new Vector3(DinoMove._instance.turtle.transform.localPosition.x,DinoMove._instance.turtle.transform.localPosition.y-(Time.time-startTime),DinoMove._instance.turtle.transform.localPosition.z);
					if(DinoMove._instance.turtle.transform.localPosition.y < 10.2f || ((Time.time-startTime) > 1.5f))
					{
						startTime = Time.time;
						if(countOfJump > 2)
							RevertToIdle ();
						else
							DinoMove._instance.turtle.GetComponent<Animation>().Play ("Jump");
					}

				}
				
	//			else if(countOfJump > 4)
	//			{
	//				if(DinoMove._instance.turtle.transform.position.y < (characterPosInPlayground+0.5f))
	//				{
	//					RevertToIdle ();
	//				}
	//			}
			}
		}
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
		if(starParticles == null)
			starParticles = (GameObject)Instantiate(Resources.Load("Headstars"));
		StarsPos ();
		starParticles.GetComponent<ParticleSystem>().Play ();
		if(PlayerPrefs.GetInt("Sound") == 1)
		{
			DinoMove._instance.turtle.GetComponent<AudioSource>().loop = false;
			DinoMove._instance.turtle.GetComponent<AudioSource>().clip = InGameSoundManager._instance.headhit;
			DinoMove._instance.turtle.GetComponent<AudioSource>().Play ();
		}
	}
	

	public void PlayAnim(string anim , float animLength , float animSpeed)
	{
		DinoMove._instance.turtle.GetComponent<Animation>().Play(anim);
		DinoMove._instance.turtle.GetComponent<Animation>()[anim].speed = animSpeed;
		if(!anim.Contains("Slap"))
		{
			CancelInvoke("RevertToIdle");
			CancelInvoke("RemoveSlap");
			isPlayingSlapAnim = false;
			Invoke("RevertToIdle",animLength);
		}
		else
		{
			isPlayingSlapAnim = true;
			Invoke("RemoveSlap",animLength);
		}
		
	}

	public void StopRevert()
	{
		CancelInvoke("RevertToIdle");
		CancelInvoke("RemoveSlap");
	}

	/// <summary>
	/// Removes the slap Animation
	/// </summary>
	void RemoveSlap()
	{
		if(DinoMove._instance.turtle.GetComponent<Animation>().IsPlaying ("SlapR") || DinoMove._instance.turtle.GetComponent<Animation>().IsPlaying ("SlapL"))
		{
			DinoMove._instance.turtle.GetComponent<Animation>().Play ("Idle");
		}
		isPlayingSlapAnim = false;
		DinoMove._instance.turtle.GetComponent<AudioSource>().loop = false;
		DinoMove._instance.turtle.GetComponent<AudioSource>().Stop();
		
	}
	
	public void RevertToIdle()
	{
		sleepParticles.Stop ();
		isJumping = false;
		if(DinoMove._instance.turtle != null)
		{
			DinoMove._instance.turtle.GetComponent<AudioSource>().loop = false;
			DinoMove._instance.turtle.GetComponent<AudioSource>().Stop();
			DinoMove._instance.turtle.GetComponent<Animation>().Play ("Idle");
		}
	}

	void ResumePosition()
	{
		isJumping = false;
		DinoMove._instance.turtle.transform.localPosition = new Vector3(-12.3f,10f,-15);
	}

	public void Dance()
	{
		if(!PopupPanel.popupPanelActive)
		{
			sleepParticles.Stop ();
			ResumePosition();
			PlayAnim("Dance" ,10 , 1);
			if(PlayerPrefs.GetInt ("Sound") == 1)
			{
				DinoMove._instance.turtle.GetComponent<AudioSource>().clip = InGameSoundManager._instance.dance;
				DinoMove._instance.turtle.GetComponent<AudioSource>().loop = true;
				DinoMove._instance.turtle.GetComponent<AudioSource>().Play ();
			}
		}
	}

	/// <summary>
	/// Fart the character.
	/// </summary>
	public void Fart()
	{
		if(!PopupPanel.popupPanelActive)
		{
			DinoMove._instance.turtle.GetComponent<AudioSource>().Stop ();
			sleepParticles.Stop ();
			ResumePosition();
			PlayAnim("Fart" ,myDino.turtle.GetComponent<MyAnimations>().fartAnim.length , 1);
			CancelInvoke("FartParticles");
			Invoke("FartParticles",myDino.turtle.GetComponent<MyAnimations>().fartAnim.length/2);
		}

	}
	

	void FartParticles()
	{
		if(DinoMove._instance.turtle.GetComponent<Animation>().IsPlaying("Fart"))
		{
			if(PlayerPrefs.GetInt ("Sound") == 1)
			{
				DinoMove._instance.turtle.GetComponent<AudioSource>().loop = false;
				DinoMove._instance.turtle.GetComponent<AudioSource>().clip = InGameSoundManager._instance.fart;
				DinoMove._instance.turtle.GetComponent<AudioSource>().Play ();
			}
			GameObject	fartParticles = (GameObject)Instantiate(Resources.Load("poofV"));
			fartParticles.transform.parent = myDino.turtle.transform;
			fartParticles.transform.localPosition = new Vector3(0 , 0.93f,-0.56f);
//			fartParticles.transform.position = new Vector3(myDino.turtle.transform.position.x , myDino.turtle.transform.position.y + 1.82f,myDino.turtle.transform.position.z+1);
			fartParticles.GetComponent<ParticleSystem>().Play ();
			Destroy (fartParticles,1.0f);
		}
	}

	public void Tickling()
	{
		if(!PopupPanel.popupPanelActive)
		{
			sleepParticles.Stop ();
			ResumePosition();
			PlayAnim("Tickling",1.3f,1);
			if(PlayerPrefs.GetInt ("Sound") == 1)
			{
				DinoMove._instance.turtle.GetComponent<AudioSource>().loop = false;
				DinoMove._instance.turtle.GetComponent<AudioSource>().clip = InGameSoundManager._instance.tickling;
				DinoMove._instance.turtle.GetComponent<AudioSource>().Play ();
			}
		}
	}

	public void HeadHit()
	{
		sleepParticles.Stop ();
		ResumePosition();
		consecutiveHitCount++;
		if(consecutiveHitCount > 2)
		{
			PlayAnim("Hit",myDino.turtle.GetComponent<MyAnimations>().hitCompleteAnim.length,1);
			
			Invoke ("ShowStars",myDino.turtle.GetComponent<MyAnimations>().hitCompleteAnim.length/4);
			consecutiveHitCount = 0;
		}
		else
			PlayAnim("Hitstart",myDino.turtle.GetComponent<MyAnimations>().hitStartAnim.length/2.5f, 2.5f);

		if(PlayerPrefs.GetInt ("Sound") == 1)
		{
			DinoMove._instance.turtle.GetComponent<AudioSource>().loop = false;
			DinoMove._instance.turtle.GetComponent<AudioSource>().clip = InGameSoundManager._instance.punch;
			DinoMove._instance.turtle.GetComponent<AudioSource>().Play ();
		}
	}

	public void Slap()
	{
		sleepParticles.Stop ();
		ResumePosition();
		int a  = Random.Range (0,2);
		if(a == 0)
			PlayAnim("SlapR",myDino.turtle.GetComponent<MyAnimations>().slapAnimRight.length/5.5f,5.5f);
		else
			PlayAnim("SlapL",myDino.turtle.GetComponent<MyAnimations>().slapAnimLeft.length/5.5f,5.5f);
		if(PlayerPrefs.GetInt ("Sound") == 1)
		{
			DinoMove._instance.turtle.GetComponent<AudioSource>().loop = false;
			DinoMove._instance.turtle.GetComponent<AudioSource>().clip = InGameSoundManager._instance.slap;
			DinoMove._instance.turtle.GetComponent<AudioSource>().Play ();
		}
	}
	float startTime = 0.0f;
	public void Jump()
	{
		if(!PopupPanel.popupPanelActive)
		{
			if(!isJumping)
			{
				sleepParticles.Stop ();
				DinoMove._instance.turtle.GetComponent<AudioSource>().loop = false;
				DinoMove._instance.turtle.GetComponent<AudioSource>().clip = InGameSoundManager._instance.yippee;
				DinoMove._instance.turtle.GetComponent<AudioSource>().Play ();
				startTime = Time.time;
				countOfJump = 0;
				DinoMove._instance.turtle.GetComponent<Animation>().Play("Jump");
				isJumping = true;
				CancelInvoke("RevertToIdle");
				CancelInvoke("RemoveSlap");
			}
		}
	}

	public void Sleep()
	{
		if(!PopupPanel.popupPanelActive)
		{
			ResumePosition();
			PlayAnim("Sleep",10f,1);
			sleepParticles.transform.parent = myDino.turtle.transform;
			if(ScrollButtonAction.characterNo == 3 || ScrollButtonAction.characterNo == 0)
				sleepParticles.transform.localPosition = new Vector3(-1.2f,1,3);
			else if(ScrollButtonAction.characterNo == 2)
				sleepParticles.transform.localPosition = new Vector3(-2.51f,2.57f,0);
			else if(ScrollButtonAction.characterNo == 1)
				sleepParticles.transform.localPosition = new Vector3(-1f,3f,1.486676f);
			sleepParticles.Play ();
			if(PlayerPrefs.GetInt ("Sound") == 1)
			{
				DinoMove._instance.turtle.GetComponent<AudioSource>().clip = InGameSoundManager._instance.snoring;
				DinoMove._instance.turtle.GetComponent<AudioSource>().loop = true;
				DinoMove._instance.turtle.GetComponent<AudioSource>().Play ();
			}
		}
	}
}
