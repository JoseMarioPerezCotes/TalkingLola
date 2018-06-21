using UnityEngine;
using System.Collections;

public class TvAlphabets : MonoBehaviour {

	int counter;
	public TextMesh myMesh;
	bool startCounter,stopCounter , once;
	float timer;
	string charVal;
	public static bool tvOn;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(startCounter)
		{
			timer+=Time.deltaTime;
			if(timer >= (0.8f))
			{
				timer = 0;
				IncrementCounter();
			}
		}
	}

	void OnMouseDown()
	{
		if(!AnimationHandler._instance.coinsPanelActive && !LoaderPanel.loaderActive  && !PopupPanel.popupPanelActive )
		{
			if(!once)
			{
				Destroy(myMesh.GetComponent<TweenRotation>());
				Destroy(myMesh.GetComponent<TweenScale>());
				myMesh.transform.localEulerAngles = new Vector3(0,39,0);
				myMesh.transform.localScale = Vector3.one;
				once = true;
			}
			if(!startCounter)
			{
				if(PlayerPrefs.GetInt("Sound") == 1)
				{
					GetComponent<AudioSource>().Play ();
				}
				StartCounter();
			}
			else
			{
				StopCounter();
			}
		}
	}
	/// <summary>
	/// Stops the counter.
	/// </summary>
	public void StopCounter()
	{
		tvOn = false;
		startCounter = false;
		myMesh.GetComponent<AudioSource>().Stop ();
		if(!AnimationHandler._instance.suspendActions)
		{
			AnimationHandler._instance.myCharacter.GetComponent<VoiceRecognitionRepeating>().SubsequentRecord ();
		}
		myMesh.text = "Hi!";
//			AnimationHandler._instance.RevertToIdle ();
	}
	
	/// <summary>
	/// Starts the counter.
	/// </summary>
	public void StartCounter()
	{
		tvOn = true;
		timer = 0.0f;
		counter = 1;
		if(myMesh)
			myMesh.text = "A";
		startCounter = true;
		if(PlayerPrefs.GetInt ("Sound") == 0)
			myMesh.GetComponent<AudioSource>().volume = 0;
		else
			myMesh.GetComponent<AudioSource>().volume = 1;
		myMesh.GetComponent<AudioSource>().Play ();
		AnimationHandler._instance.TvStopit ();
//		AnimationHandler._instance.myCharacter.animation.Play ("Idle");
	}
	/// <summary>
	/// Increments the counter.
	/// </summary>
	void IncrementCounter()
	{
		counter++;
		if(counter > 26)
		{
			StopCounter();
			myMesh.GetComponent<AudioSource>().Stop ();
		}
		GetChar();

	}

	void GetChar()
	{
		switch(counter)
		{
		case 1:
			charVal = "A";
			break;
		case 2:
			charVal = "B";
			break;
		case 3:
			charVal = "C";
			break;
		case 4:
			charVal = "D";
			break;
		case 5:
			charVal = "E";
			break;
		case 6:
			charVal = "F";
			break;
		case 7:
			charVal = "G";
			break;
		case 8:
			charVal = "H";
			break;
		case 9:
			charVal = "I";
			break;
		case 10:
			charVal = "J";
			break;
		case 11:
			charVal = "K";
			break;
		case 12:
			charVal = "L";
			break;
		case 13:
			charVal = "M";
			break;
		case 14:
			charVal = "N";
			break;
		case 15:
			charVal = "O";
			break;
		case 16:
			charVal = "P";
			break;
		case 17:
			charVal = "Q";
			break;
		case 18:
			charVal = "R";
			break;
		case 19:
			charVal = "S";
			break;
		case 20:
			charVal = "T";
			break;
		case 21:
			charVal = "U";
			break;
		case 22:
			charVal = "V";
			break;
		case 23:
			charVal = "W";
			break;
		case 24:
			charVal = "X";
			break;
		case 25:
			charVal = "Y";
			break;
		case 26:
			charVal = "Z";
			break;
		default:
			charVal = "Hi!";
			break;
		}
		myMesh.text = charVal;
	}
}
