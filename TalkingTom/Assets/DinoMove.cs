using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DinoMove : MonoBehaviour {
	bool bringTurtleBack;

	public GameObject myDino;
	public GameObject turtle;
	public AnimationClip success;
	public AudioSource dinoRoar,dinoWalk , turtleScream;
	public GameObject characterParent;
	public static DinoMove _instance;


    [SerializeField]
    public List<string> character = new List<string>()
    {
        "Cat",
        "Dog",
        "Owl",
        "Turtle"
    };
    // Use this for initialization
    void Start () {
		Screen.orientation = ScreenOrientation.LandscapeLeft;
		_instance = this;
		if(!PlayerPrefs.HasKey("Character"))
		{
			PlayerPrefs.SetInt("Character",0);
			ScrollButtonAction.characterNo = 0;
		}
        else { 
			ScrollButtonAction.characterNo = PlayerPrefs.GetInt("Character");
        }



        switch (ScrollButtonAction.characterNo)
		{
		case 0:
			turtle = (GameObject)Instantiate(Resources.Load(character[0]));
                //(GameObject)Instantiate(Resources.Load("Dog"));
                // (GameObject)Instantiate(Resources.Load("DogVirtual"));
                break;
		case 1:
			turtle = (GameObject)Instantiate(Resources.Load(character[1]));////
			      //  (GameObject)Instantiate(Resources.Load("OwlVirtual"));
			break;
		case 2:
			turtle = (GameObject)Instantiate(Resources.Load(character[2]));
               // (GameObject)Instantiate(Resources.Load("TurtleVirtual"));
			
			break;
		case 3:
			turtle = (GameObject)Instantiate(Resources.Load(character[3]));
                //(GameObject)Instantiate(Resources.Load("CatVirtual"));
                break;
		}
        if (ScrollButtonAction.characterNo > 0)//if(ScrollButtonAction.characterNo < 3)
        {
			VirtualRealityPanel._instance.otherAnimations.SetActive (true);
			VirtualRealityPanel._instance.catAnimations.SetActive (false);
		}
		else
		{
			VirtualRealityPanel._instance.otherAnimations.SetActive (false);
			VirtualRealityPanel._instance.catAnimations.SetActive (true);
		}
		turtle.transform.parent = characterParent.transform;
		turtle.transform.localPosition = new Vector3(-12.3f,10f,-15f);
		turtle.transform.localRotation = Quaternion.Euler(0f,180f,0f);
		turtle.transform.localScale = new Vector3(5,5,5);
		if(!TrackableEventHandler.rendered)
		{
			Renderer[] rendererComponents = turtle.GetComponentsInChildren<Renderer>(true);
			Collider[] colliderComponents = turtle.GetComponentsInChildren<Collider>(true);
			
			// Disable rendering:
			foreach (Renderer component in rendererComponents)
			{
				component.enabled = false;
			}
			
			// Disable colliders:
			foreach (Collider component in colliderComponents)
			{
				component.enabled = false;
			}
		}



	}
	
	// Update is called once per frame
	void Update () {
		if(myDino.activeInHierarchy && TrackableEventHandler.rendered)
		{
			myDino.transform.localPosition = new Vector3(myDino.transform.localPosition.x-Time.deltaTime*15 , myDino.transform.localPosition.y,myDino.transform.localPosition.z);
			if(turtle.transform.localPosition.z < 60)
			{
				turtle.transform.localPosition = new Vector3(turtle.transform.localPosition.x , turtle.transform.localPosition.y,turtle.transform.localPosition.z+Time.deltaTime*15);
			}
			else
			{
				if(turtle.GetComponent<Animation>().IsPlaying ("Run"))
				{
					turtle.GetComponent<Animation>().Play ("Idle");
					turtle.transform.localEulerAngles = new Vector3(0,180,0);
				}
			}
			if(myDino.transform.localPosition.x < -30)
			{
				dinoWalk.Stop ();
				turtle.transform.localEulerAngles = new Vector3(0,180,0);
				if(PlayerPrefs.GetInt("Sound") == 1)
				{
					turtle.GetComponent<AudioSource>().loop = false;
					turtle.GetComponent<AudioSource>().clip = InGameSoundManager._instance.yippee;
					turtle.GetComponent<AudioSource>().Play();
				}
				turtle.GetComponent<Animation>().Play ("Success");
				turtleScream.Stop ();
				Invoke ("TurtleBack" , success.length );
				myDino.SetActive (false);
			}
		}
		else if(bringTurtleBack)
		{
			turtle.transform.localPosition = new Vector3(turtle.transform.localPosition.x , turtle.transform.localPosition.y,turtle.transform.localPosition.z-Time.deltaTime*10);
			if(turtle.transform.localPosition.z < -15)
			{
				bringTurtleBack = false;
				turtle.GetComponent<Animation>().Play ("Idle");
				VirtualRealityPanel._instance.ActivateUI ();
			}
		}
	}

	void TurtleBack()
	{
		bringTurtleBack = true;
		turtle.GetComponent<Animation>().Play ("Walk");
	}

	public void ResetAll()
	{
		if(turtle)
		{
			turtle.GetComponent<Animation>().Play ("Idle");
			turtle.transform.localPosition = new Vector3(-12.3f,10f,-15f);
			turtle.transform.localEulerAngles = new Vector3(0,180,0);
			if(VirtualAnimationHandler._instance.sleepParticles != null)
				VirtualAnimationHandler._instance.sleepParticles.Stop ();
			turtle.GetComponent<AudioSource>().loop = true;
			turtle.GetComponent<AudioSource>().Stop ();
		}
		bringTurtleBack = false;
		myDino.SetActive (false);
		VirtualAnimationHandler._instance.isJumping = false;
		dinoWalk.Stop ();
		dinoRoar.Stop ();

	}

//	void OnGUI()
//	{
//		if(TrackableEventHandler.rendered)
//		{
//			if(GUI.Button (new Rect(200,400,100,100),"Dino") && !myDino.activeInHierarchy)
//			{
//				myDino.SetActive (true);
//				dinoRoar.Play ();
//				dinoWalk.Play ();
//				turtleScream.Play ();
//				turtle.animation.Play ("Run");
//				turtle.transform.localEulerAngles = new Vector3(0,0,0);
//				myDino.transform.localPosition = new Vector3(41 , myDino.transform.localPosition.y,myDino.transform.localPosition.z);
//				VirtualRealityPanel._instance.DeactivateUI ();
//				VirtualAnimationHandler._instance.StopRevert();
//			}
//		}
//		else
//		{
//			if(turtle)
//				turtle.transform.localEulerAngles = new Vector3(0,180,0);
//			myDino.SetActive (false);
//			dinoWalk.Stop ();
//		}
//	}

	public void Dino()
	{
		if(!myDino.activeInHierarchy)
		{
			turtle.transform.localPosition = new Vector3(turtle.transform.localPosition.x,10f,turtle.transform.localPosition.z);
			myDino.SetActive (true);
			if(PlayerPrefs.GetInt("Sound") == 1)
			{
				dinoRoar.Play ();
				dinoWalk.Play ();
				turtleScream.Play ();
				turtle.GetComponent<AudioSource>().Stop();
				turtle.GetComponent<AudioSource>().loop = false;
			}
			turtle.GetComponent<Animation>().Play ("Run");
			turtle.transform.localEulerAngles = new Vector3(0,0,0);
			myDino.transform.localPosition = new Vector3(41 , myDino.transform.localPosition.y,myDino.transform.localPosition.z);
			VirtualRealityPanel._instance.DeactivateUI ();
			VirtualAnimationHandler._instance.StopRevert();
		}
	}

	void OnApplicationPause(bool isPaused)
	{
		Screen.orientation = ScreenOrientation.LandscapeLeft;

	}
}
