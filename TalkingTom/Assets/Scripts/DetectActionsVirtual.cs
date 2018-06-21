using UnityEngine;
using System.Collections;

public class DetectActionsVirtual : MonoBehaviour {
	LayerMask layerMask;
	Vector2 initialPos,finalPos;
	GameObject starParticles;
	int consecutiveHitCount;
	bool isPlayingSlapAnim;
	public AnimationClip idleAnim,tickleAnim,hitAnim,slapAnimLeft,slapAnimRight,hitCompleteAnim,fartAnim;
	// Use this for initialization
	void Start () {
		layerMask = 1<<8;
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
			GetComponent<AudioSource>().clip = InGameSoundManager._instance.headhit;
			GetComponent<AudioSource>().Play ();
		}
	}


	// Update is called once per frame
	void Update () {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if(!LoaderPanel.loaderActive  && !PopupPanel.popupPanelActive)
		{
			if(Input.GetMouseButtonDown(0))
			{
				if (Physics.Raycast(ray, out hit, 100,layerMask))
				{
					if(hit.collider.name == "Bip01 Head")
					{
						consecutiveHitCount++;
						if(consecutiveHitCount > 2)
						{
							PlayAnim(hitCompleteAnim.name,hitCompleteAnim.length,1);
						
							Invoke ("ShowStars",hitCompleteAnim.length/4);
							consecutiveHitCount = 0;
						}
						else
							PlayAnim(hitAnim.name,hitAnim.length/2.5f, 2.5f);
						if(PlayerPrefs.GetInt ("Sound") == 1)
						{
							GetComponent<AudioSource>().clip = InGameSoundManager._instance.punch;
							GetComponent<AudioSource>().Play ();
						}
					}
					else if(hit.collider.name == "Bip01 Spine1")
					{
						PlayAnim(tickleAnim.name,1.3f,1);
						if(PlayerPrefs.GetInt ("Sound") == 1)
						{
							GetComponent<AudioSource>().clip = InGameSoundManager._instance.tickling;
							GetComponent<AudioSource>().Play ();
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
				if (Physics.Raycast(ray, out hit, 100,layerMask) && Vector2.Distance(initialPos,finalPos) > 70.0f )
				{

					if(hit.collider.name == "Bip01 L Ear 1" || hit.collider.name == "Eye Left")
					{
						Debug.Log("moving -- "+Vector2.Distance(initialPos,finalPos));
						PlayAnim(slapAnimRight.name,slapAnimRight.length/5.5f,5.5f);
						initialPos = Vector2.zero;
						if(PlayerPrefs.GetInt ("Sound") == 1)
						{
							GetComponent<AudioSource>().clip = InGameSoundManager._instance.slap;
							GetComponent<AudioSource>().Play ();
						}
					}
					else if(hit.collider.name == "Bip01 R Ear 1" || hit.collider.name == "Eye Right")
					{
						Debug.Log("moving -- "+Vector2.Distance(initialPos,finalPos));
						PlayAnim(slapAnimLeft.name,slapAnimLeft.length/5.5f,5.5f);
						initialPos = Vector2.zero;
						if(PlayerPrefs.GetInt ("Sound") == 1)
						{
							GetComponent<AudioSource>().clip = InGameSoundManager._instance.slap;
							GetComponent<AudioSource>().Play ();
						}
					}
				}
			}
			else if(Input.GetMouseButtonUp(0))
			{
				finalPos = Input.mousePosition;
				if (Physics.Raycast(ray, out hit, 100,layerMask) && Vector2.Distance(initialPos,finalPos) > 70.0f  )
				{
					if(hit.collider.name == "Bip01 L Ear 1" )
					{
						Debug.Log("moving -- "+Vector2.Distance(initialPos,finalPos));
						PlayAnim(slapAnimRight.name,slapAnimRight.length/5.5f,5.5f);
						if(PlayerPrefs.GetInt ("Sound") == 1)
						{
							GetComponent<AudioSource>().clip = InGameSoundManager._instance.slap;
							GetComponent<AudioSource>().Play ();
						}
						initialPos = Vector2.zero;
					}
					else if(hit.collider.name == "Bip01 R Ear 1")
					{
						Debug.Log("moving -- "+Vector2.Distance(initialPos,finalPos));
						PlayAnim(slapAnimLeft.name,slapAnimLeft.length/5.5f,5.5f);
						if(PlayerPrefs.GetInt ("Sound") == 1)
						{
							GetComponent<AudioSource>().clip = InGameSoundManager._instance.slap;
							GetComponent<AudioSource>().Play ();
						}
						initialPos = Vector2.zero;
					}
				}
			}
		}
	}

	public void PlayAnim(string anim , float animLength , float animSpeed)
	{
		GetComponent<Animation>().Play(anim);
		GetComponent<Animation>()[anim].speed = animSpeed;
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

	/// <summary>
	/// Removes the slap Animation
	/// </summary>
	void RemoveSlap()
	{
		if(GetComponent<Animation>().IsPlaying ("SlapR") || GetComponent<Animation>().IsPlaying ("SlapL"))
		{
			GetComponent<Animation>().Play ("Idle");
		}
		isPlayingSlapAnim = false;
		GetComponent<AudioSource>().loop = false;
		GetComponent<AudioSource>().Stop();
		
	}

	public void RevertToIdle()
	{
		GetComponent<AudioSource>().loop = false;
		GetComponent<AudioSource>().Stop();
		GetComponent<Animation>().Play ("Idle");
	}
}
