using UnityEngine;
using System.Collections;

public class ObjectMotion : MonoBehaviour {
	/// <summary>
	/// My original position - the position at which the object will return
	/// </summary>
	public Vector3 myOriginalPos;
	/// <summary>
	/// Is the dragged object a scrub?
	/// </summary>
	public bool isScrub;
	
	/// <summary>
	/// Is the dragged object a brush?
	/// </summary>
	public bool isBrush;
	
	/// <summary>
	///Is the dragged object a shower?
	/// </summary>
	public bool isShower;
	public GameObject showerAlpha,pipeAlpha;
	/// <summary>
	/// Is the dragged object a door
	/// </summary>
	public bool isDoor;

	
	/// <summary>
	/// Endpoint coordinates for door and brush
	/// </summary>
	public float maxX,minX;
	/// <summary>
	/// Particle Effect of shower
	/// </summary>
	public GameObject waterDroplets,starsAfterWash;
	public ParticleSystem toothpasteParticles;
	public static bool bathroomObjectsDragged;
	bool isSoapPresent;
	DetectActions characterActions;
	bool startedBrushing;
	bool once;

	// Use this for initialization
	void Start () {
		myOriginalPos = transform.position;
	}



	bool canMove;
	/// <summary>
	/// Raises the mouse down event.
	/// </summary>
	void OnMouseDown()
	{
		if(!AnimationHandler._instance.isInLoo && !AnimationHandler._instance.coinsPanelActive  && !LoaderPanel.loaderActive && !PopupPanel.popupPanelActive)
		{

			canMove = false;
			if(transform.name == "brush")
			{
				canMove = true;
			}
			else if(!(AnimationHandler._instance.brush.activeInHierarchy))
			{
				canMove = true;
			}
			if(canMove)
			{

				if(!once)
				{
					once = true;
					if(transform.GetComponent<TweenPosition>())
						Destroy(transform.GetComponent<TweenPosition>());
					if(transform.GetComponent<TweenScale>())
						Destroy(transform.GetComponent<TweenScale>());
					if(transform.name == "shower-pipe")
					{
						Destroy(showerAlpha);
						Destroy(pipeAlpha);
					}
				}
				Debug.Log("mouse down");
				bathroomObjectsDragged = true;
				Vector3 myPos = Camera.main.WorldToScreenPoint (transform.position);
				if(isShower)
				{
					if(Input.mousePosition.y < (Screen.height*0.7f))
					{
						myPos = new Vector3(Input.mousePosition.x, (Screen.height*0.7f),myPos.z);
						transform.position =  Camera.main.ScreenToWorldPoint (new Vector3(Input.mousePosition.x, myPos.y , myPos.z));
					}
					else
						transform.position =  Camera.main.ScreenToWorldPoint (new Vector3(Input.mousePosition.x, Input.mousePosition.y , myPos.z));
				}
				else
					transform.position =  Camera.main.ScreenToWorldPoint (new Vector3(Input.mousePosition.x, Input.mousePosition.y , myPos.z));
				if(isBrush || isDoor)
				{

					if(PlayerPrefs.GetInt ("Sound") == 1)
						GetComponent<AudioSource>().Play ();
					if(isBrush && !startedBrushing)
					{
						startedBrushing = true;
						toothpasteParticles.Play ();
					}
					BrushingConstraints ();
				}
				else if(isShower )
				{

					AnimationHandler._instance.RevertToIdle ();
					if(PlayerPrefs.GetInt ("Sound") == 1)
						GetComponent<AudioSource>().Play ();
					if(CharacterSoap._instance != null)
					{
						foreach(SpriteRenderer a in CharacterSoap._instance.mySoap)
						{
							if(a.enabled)
							{
								isSoapPresent = true;
									break;
							}
						}
					}

					waterDroplets.GetComponent<ParticleEmitter>().emit = true;
				}
				else if(isScrub)
				{

					AnimationHandler._instance.RevertToIdle ();
					if(PlayerPrefs.GetInt ("Sound") == 1)
						GetComponent<AudioSource>().Play ();
				}
				bathroomObjectsDragged = true;
			}
		}
	}

	int noOfTimesCleaned;

	/// <summary>
	/// Raises the mouse drag event.
	/// </summary>
	void OnMouseDrag()
	{
		if(!AnimationHandler._instance.isInLoo && !AnimationHandler._instance.coinsPanelActive && canMove)
		{
			Vector3 myPos = Camera.main.WorldToScreenPoint (transform.position);
			if(isShower)
			{
				if(Input.mousePosition.y < (Screen.height*0.7f))
				{
					myPos = new Vector3(Input.mousePosition.x, (Screen.height*0.7f),myPos.z);
					transform.position =  Camera.main.ScreenToWorldPoint (new Vector3(Input.mousePosition.x, myPos.y , myPos.z));
				}
				else
					transform.position =  Camera.main.ScreenToWorldPoint (new Vector3(Input.mousePosition.x, Input.mousePosition.y , myPos.z));
			}
			else
				transform.position =  Camera.main.ScreenToWorldPoint (new Vector3(Input.mousePosition.x, Input.mousePosition.y , myPos.z));
			if(isBrush || isDoor)
			{
				BrushingConstraints ();
			}
			if(isShower)
			{
				if(transform.position.x > 0 && transform.position.x < 3.5f)
				{
					float myX = transform.position.x; 
					if(CharacterSoap._instance != null)
					{
						foreach(SpriteRenderer a in CharacterSoap._instance.mySoap)
						{
							if(Mathf.Abs(a.transform.position.x-myX ) < 0.5f)
							{
								if(a.enabled && noOfTimesCleaned == 2)
								{
									noOfTimesCleaned = 0;
									a.enabled = false;
									if(a.GetComponent<Paste>().myCollider)
										a.GetComponent<Paste>().myCollider.enabled = true;
									if(a.transform.childCount > 0)
									{
										SpriteRenderer []chileRenderer =  a.transform.GetComponentsInChildren<SpriteRenderer>();
										foreach(SpriteRenderer b in chileRenderer)
										{
											b.enabled = false;

										}
									}
									break;
								}
								else if(a.enabled && noOfTimesCleaned < 2)
								{
									noOfTimesCleaned++;
								}
							}
						}
					}
				}
			}
		}
	}
	
	/// <summary>
	/// Raises the mouse up event.
	/// </summary>
	void OnMouseUp()
	{
		if(!AnimationHandler._instance.isInLoo && !AnimationHandler._instance.coinsPanelActive && canMove)
		{
			Vector3 myPos = Camera.main.WorldToScreenPoint (transform.position);
			if(isShower)
			{
				if(Input.mousePosition.y < (Screen.height*0.7f))
				{
					myPos = new Vector3(Input.mousePosition.x, (Screen.height*0.7f),myPos.z);
					transform.position =  Camera.main.ScreenToWorldPoint (new Vector3(Input.mousePosition.x, myPos.y , myPos.z));
				}
				else
					transform.position =  Camera.main.ScreenToWorldPoint (new Vector3(Input.mousePosition.x, Input.mousePosition.y , myPos.z));
			}
			else
				transform.position =  Camera.main.ScreenToWorldPoint (new Vector3(Input.mousePosition.x, Input.mousePosition.y , myPos.z));
			if(isBrush)
			{
				BrushingConstraints ();
				if(startedBrushing)
				{
					startedBrushing = false;
					toothpasteParticles.Stop ();
				}
				GetComponent<AudioSource>().Stop ();
			}
			else
			{
				if(isDoor)
				{
					BrushingConstraints ();
				}
				StartCoroutine(MoveToPosition());
				if(isScrub) // set the istrigger off for brush to prevent collisions
				{
					GetComponent<AudioSource>().Stop ();
				}

				if(isShower)
				{

					waterDroplets.GetComponent<ParticleEmitter>().emit = false;
					bool anySoapLeft = false;
					if(CharacterSoap._instance != null)
					{
						foreach(SpriteRenderer a in CharacterSoap._instance.mySoap)
						{
							if(a.enabled)
							{
								anySoapLeft = true;
								break;
							}
						}
					}
					if(!anySoapLeft)
					{
						GameObject.Find("sponge").GetComponent<PaintBrush>().bubble.GetComponent<ParticleSystem>().Stop ();
						if(ScrollButtonAction.characterNo == 0)
						{
							characterActions = AnimationHandler._instance.myCharacter.GetComponent<DetectActions>();
							AnimationHandler._instance.PlayAnim(characterActions.towelAnim.name,characterActions.towelAnim.length*2,1);
						}
						GameObject starsPrefab = (GameObject)Instantiate (starsAfterWash);
						Destroy(starsPrefab,1.0f);
						AnimationHandler._instance.isSoapOnBody = false;
					}
					GetComponent<AudioSource>().Stop ();
				}
			}
			bathroomObjectsDragged = false;
		}
	}

	void BrushingConstraints()
	{
		transform.position = new Vector3(transform.position.x,myOriginalPos.y,myOriginalPos.z);
		if(isDoor && !AnimationHandler._instance.isSoapOnBody && !AnimationHandler._instance.isInLoo)
		{
			if(transform.position.x < (minX+2))
			{
				AnimationHandler._instance.Loo();
				bathroomObjectsDragged = false;
				transform.position = new Vector3(minX,myOriginalPos.y,myOriginalPos.z);
			}
		}
		if(transform.position.x < minX)
		{
			transform.position = new Vector3(minX,myOriginalPos.y,myOriginalPos.z);
		}
		else if(transform.position.x > maxX)
		{
			transform.position = new Vector3(maxX,myOriginalPos.y,myOriginalPos.z);
		}
	}

	/// <summary>
	/// Brings Back the object to its original position
	/// </summary>
	/// <returns>The to position.</returns>
	IEnumerator MoveToPosition()
	{
		float distance = Vector3.Distance (transform.position , myOriginalPos);
		float speed = 15;
		while(distance > 0.1f)
		{
			float step = speed * Time.deltaTime;
			transform.position = Vector3.MoveTowards(transform.position, myOriginalPos, step);
			distance = Vector3.Distance (transform.position , myOriginalPos);
			yield return 0;
		}
	}
	

	
}
