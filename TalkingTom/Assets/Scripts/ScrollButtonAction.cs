using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScrollButtonAction : MonoBehaviour {
	public float springStrength = 8.0f;
	private UIScrollView scrollView;
	private int NoOfScroll;
	private Vector3 startingScrollPosition;
	private UIGrid grid;

	GameObject Item;
	GameObject nextButton,prevButton;
	RoomUIManager uiManager;
	int CountClick;

	public int dogCost, turtleCost , owlCost; 
	public int moveToElement;
	public GameObject rightButton , leftButton;
	public GameObject playBtn,buyBtn;
	public UILabel myCoinsLabel;
	public static bool disableButtonPress;
	public static int characterNo;
	void OnEnable()
	{
		myCoinsLabel.text = PlayerPrefs.GetInt ("MyCoins").ToString ();

	}






	// Use this for initialization
	void Start () 
	{
//		moveToElement = 2;
		myCoinsLabel.text = PlayerPrefs.GetInt ("MyCoins").ToString ();
		CountClick = 0;
		uiManager = GameObject.Find("UI Root").GetComponent<RoomUIManager>();
		buyBtn.SetActive (false);
		scrollView = NGUITools.FindInParents<UIScrollView>(gameObject);
		grid = this.GetComponent<UIGrid>();
	//	grid.cellWidth = transform.GetComponentInChildren<UITexture> ().width;
		grid.cellWidth = Screen.width;
		grid.Reposition ();
		NoOfScroll = this.transform.childCount - 1;
		startingScrollPosition = scrollView.panel.cachedTransform.localPosition;

		float nextScroll = grid.cellWidth * moveToElement;
		Vector3 target = new Vector3( -nextScroll,0.0f,0.0f);
		MoveBy(target);
		CountClick = moveToElement;
		if(NoOfScroll == 0)
		{
			if(rightButton != null)
			{
				rightButton.SetActive(false);
				leftButton.SetActive(false);
			}
		}
		if (CountClick == 0)
		{
			if(leftButton != null)
				leftButton.SetActive(false);
		}
	}

	
	/// <summary>
	/// Scrolls until target position matches target panelAnchorPosition (may be the center of the panel, one of its sides, etc)
	/// </summary>	
	void MoveBy (Vector3 target)
	{
		if (scrollView != null && scrollView.panel != null)
		{
			// Spring the panel to this calculated position
			SpringPanel.Begin(scrollView.panel.cachedGameObject, startingScrollPosition + target, springStrength);
			disableButtonPress = true;
		}
	}


	#region Load Next Page 
	public void NextPage()
	{
		if(CountClick<NoOfScroll && !PopupPanel.popupPanelActive)
		{		
			CountClick++;
			UseOrPurchase();
			RightScroll ();
		}		
	}

	void RightScroll()
	{
		float nextScroll = grid.cellWidth * CountClick;
		Vector3 target = new Vector3( -nextScroll,0.0f, 0.0f);	
		MoveBy(target);
		if (CountClick >= (NoOfScroll))
		{
			if(rightButton != null)
				rightButton.SetActive(false);
		}
		else
		{
			if(rightButton != null)
				rightButton.SetActive(true);
		}
		
		if(NoOfScroll != 0 && CountClick > 0)
		{
			if(leftButton != null)
				leftButton.SetActive(true);
		}
	}

	
	#endregion

	#region Load Previous Page 
	public void PreviousPage()
	{
		if(CountClick>0  && !PopupPanel.popupPanelActive)
		{

			CountClick--;
			UseOrPurchase();
			LeftScroll ();
		}
	}

	void LeftScroll()
	{

		float nextScroll = grid.cellWidth * CountClick;
		Vector3 target = new Vector3( -nextScroll,0.0f,0.0f);			
		MoveBy(target);
		//Debug.Log(Item);
		if (CountClick <= 0)
		{
			if(leftButton != null)
				leftButton.SetActive(false);
		}
		else
		{
			if(leftButton != null)
				leftButton.SetActive(true);
		}
		
		if(NoOfScroll != 0 && CountClick <= (NoOfScroll))
		{
			if(rightButton != null)
				rightButton.SetActive(true);
		}

	}
	#endregion

	void UseOrPurchase()
	{
		if(CountClick == 1)
		{
            if (PlayerPrefs.HasKey(character[1]))//if(PlayerPrefs.HasKey("Owl"))
            {
				buyBtn.SetActive (false);
				playBtn.SetActive (true);
			}
			else
			{
				buyBtn.SetActive (true);
				buyBtn.GetComponentInChildren<UILabel>().text = dogCost.ToString ();
				playBtn.SetActive (false);
			}

		}
        else if (CountClick == 2)
        {
            if (PlayerPrefs.HasKey(character[2]))//if(PlayerPrefs.HasKey("Turtle"))
            {
                buyBtn.SetActive(false);
                playBtn.SetActive(true);
            }
            else
            {
                buyBtn.SetActive(true);
                buyBtn.GetComponentInChildren<UILabel>().text = owlCost.ToString();//turtleCost.ToString();
                playBtn.SetActive(false);
            }

        }
        else if(CountClick == 3)
		{
            if (PlayerPrefs.HasKey(character[3]))//if(PlayerPrefs.HasKey("Turtle"))
            {
				buyBtn.SetActive (false);
				playBtn.SetActive (true);
			}
			else
			{
				buyBtn.SetActive (true);
				buyBtn.GetComponentInChildren<UILabel>().text = turtleCost.ToString ();
				playBtn.SetActive (false);
			}
			
		}
		else
		{
			buyBtn.SetActive (false);
			playBtn.SetActive (true);
		}
	}

    [SerializeField]
    public List<string> character = new List<string>()
    {
        "Cat",
        "Dog",
        "Owl",
        "Turtle"
    };

    public void Play()
	{
		switch(CountClick)
		{
		case 0:
                characterNo = 0;//character.IndexOf(character[0]); //3;  //cat
			break;
		case 1:
			characterNo = 1;// character.IndexOf(character[1]); //0; // dog
			break;
		case 2:
			characterNo = 2; //character.IndexOf(character[2]); //1; // owl
			break;
		case 3:
			characterNo = 3; //character.IndexOf(character[3]); //2; // turtle
                break;
		}


		PlayerPrefs.SetInt("Character",characterNo);
		GameObject myCharacter = null;
        switch (characterNo)
        {
            case 0:
                myCharacter = (GameObject)Instantiate(Resources.Load(character[0]));//myCharacter = (GameObject)Instantiate(Resources.Load("Dog"));
                if (Application.loadedLevelName == "BedRoom")
			{
				myCharacter.transform.position = new Vector3(-0.86f,1.2f,2.350002f);
				myCharacter.transform.rotation = Quaternion.Euler(-62.00067f,152.1f,38.81095f);
			}
			break;
		case 1:
                myCharacter = (GameObject)Instantiate(Resources.Load(character[1]));//myCharacter = (GameObject)Instantiate(Resources.Load("Owl"));
                if (Application.loadedLevelName == "BedRoom")
			{
				myCharacter.transform.position = new Vector3(-0.3f,0.1800001f,2.74f);
				myCharacter.transform.rotation = Quaternion.Euler(0f,110f,0f);
			}
			break;
		case 2:
                myCharacter = (GameObject)Instantiate(Resources.Load(character[2]));//myCharacter = (GameObject)Instantiate(Resources.Load("Turtle"));
                if (Application.loadedLevelName == "BedRoom")
			{
				myCharacter.transform.position = new Vector3(1.3f,1.02f,3.029746f);
				myCharacter.transform.rotation = Quaternion.Euler(23.15633f,133.62f,-56.47394f);
			}
			break;
		case 3:
                myCharacter = (GameObject)Instantiate(Resources.Load(character[3]));//myCharacter = (GameObject)Instantiate(Resources.Load("Cat"));
                if (Application.loadedLevelName == "BedRoom")
			{
				myCharacter.transform.position = new Vector3(-0.856347f,0.730195f,2.74f);
				myCharacter.transform.rotation = Quaternion.Euler(-33.49133f,105.4364f,-12.82684f);
			}
			break;
		}

		AnimationHandler._instance.mainScreenPanel.SetActive(true);
		Destroy(AnimationHandler._instance.myCharacter);
		AnimationHandler._instance.myCharacter = myCharacter;
		AnimationHandler._instance.myCharacterDetectActions = myCharacter.GetComponent<DetectActions>();
		AnimationHandler._instance.suspendActions = false;
		AnimationHandler._instance.isSoapOnBody = false;
		if(Application.loadedLevelName == "BathRoom")
		{
			myCharacter.transform.position = new Vector3(1.72f,-4.31f,1);
			AnimationHandler._instance.characterGeneralPosInBathRoom = myCharacter.transform.position;
			if(characterNo < 3)
			{
				GameObject bathroomDoor = AnimationHandler._instance.door;
				bathroomDoor.transform.position = new Vector3(bathroomDoor.transform.position.z,1.68f,bathroomDoor.transform.position.z);
				bathroomDoor.transform.localScale = new Vector3(bathroomDoor.transform.localScale.x,1.28f,bathroomDoor.transform.localScale.z);
				bathroomDoor.GetComponent<ObjectMotion>().myOriginalPos = bathroomDoor.transform.position;
			}
		}
		else if(Application.loadedLevelName == "BedRoom")
		{
            

            myCharacter.GetComponent<Animation>().Play ("Sleep");
            Debug.Log("Foco");
			AnimationHandler._instance.bedroomLight.GetComponent<Light>().intensity = 0.1f;
			AnimationHandler._instance.sleepParticles.gameObject.SetActive (true);
			AnimationHandler._instance.sleepParticles.Play ();
			AnimationHandler._instance.StopRecording();
			if(PlayerPrefs.GetInt ("Sound") == 1)
			{
                AnimationHandler._instance.BedroomPosition();
                AnimationHandler._instance.GetComponent<AudioSource>().clip = InGameSoundManager._instance.snoring;
				AnimationHandler._instance.GetComponent<AudioSource>().loop = true;
				AnimationHandler._instance.GetComponent<AudioSource>().Play ();
			}
		}
		else if(Application.loadedLevelName == "PlayGround")
		{
			myCharacter.transform.position = new Vector3(0,-1.65f,0);
			AnimationHandler._instance.characterPosInPlayground = myCharacter.transform.position;
		}
		else if(Application.loadedLevelName == "Gym")
		{
			myCharacter.transform.position = new Vector3(0f,-2.1f,0.5f);
		}
		AnimationHandler._instance.RemoveExtraButtons ();
		AnimationHandler._instance.coinsPanelActive = false;
		gameObject.transform.parent.parent.gameObject.SetActive (false);
//		GoogleMobileAdsDemoScript._instance.ShowBanner ();
	}

	public void Purchase()
	{
		uiManager.ActivatePopup ("Do you want to buy the character?",true);
		uiManager.popUpPanel.GetComponent<PopupPanel>().okButton.GetComponent<UIButton>().onClick.Clear();
		uiManager.popUpPanel.GetComponent<PopupPanel>().okButton.GetComponent<UIButton>().onClick.Add(new EventDelegate(BuyCharacter));
	}

	void BuyCharacter()
	{
		uiManager.popUpPanel.SetActive (false);
		if(CountClick == 1)
		{
			if(PlayerPrefs.GetInt ("MyCoins") > dogCost)
			{
				StartCoroutine(DecrementCoins(PlayerPrefs.GetInt("MyCoins"), dogCost));
				PlayerPrefs.SetInt ("MyCoins", PlayerPrefs.GetInt ("MyCoins") - dogCost);
                PlayerPrefs.SetInt(character[1], 1); //PlayerPrefs.SetInt ("Dog",1);// PlayerPrefs.SetInt(character[1], 1);//
                uiManager.ActivatePopup(character[1]+"Purchased Successfully", false); // uiManager.ActivatePopup("Dog Purchased Successfully" , false);
                UseOrPurchase();
			}
			else
			{
				uiManager.ActivatePopup("You do not have enough coins to buy the character!" , false);
			}
		}
        else if (CountClick == 2)
        {
            if (PlayerPrefs.GetInt("MyCoins") > owlCost)
            {
                StartCoroutine(DecrementCoins(PlayerPrefs.GetInt("MyCoins"), owlCost));
                PlayerPrefs.SetInt("MyCoins", PlayerPrefs.GetInt("MyCoins") - owlCost);
                PlayerPrefs.SetInt(character[2], 1);// PlayerPrefs.SetInt ("Owl",1);// 
                uiManager.ActivatePopup("Owl Purchased Successfully", false);
                UseOrPurchase();
            }
            else
            {
                uiManager.ActivatePopup("You do not have enough coins to buy the character!", false);
            }
        }
        else if(CountClick == 3)
		{
			if(PlayerPrefs.GetInt ("MyCoins") > turtleCost)
			{
				StartCoroutine(DecrementCoins(PlayerPrefs.GetInt("MyCoins"),turtleCost));
				PlayerPrefs.SetInt ("MyCoins", PlayerPrefs.GetInt ("MyCoins") - turtleCost);
                PlayerPrefs.SetInt(character[3], 1); //PlayerPrefs.SetInt ("Turtle",1);// PlayerPrefs.SetInt(character[3], 1);
                uiManager.ActivatePopup("Turtle Purchased Successfully" , false);
				UseOrPurchase();
			}
			else
			{
				uiManager.ActivatePopup("You do not have enough coins to buy the character!" , false);
			}
		}
	}

	IEnumerator DecrementCoins(int myCoins , int amountDecreased)
	{
		int decrementDelta = (int)amountDecreased/20;
		int finalCoins = myCoins-amountDecreased;
		float timer = 0.0f;
		while(myCoins > finalCoins)
		{
			while(timer < 0.6f)
			{
				timer+=0.02f;
				yield return 0;
			}
			myCoins-=decrementDelta;
			myCoinsLabel.text = myCoins.ToString ();
			yield return 0;
		}
		myCoinsLabel.text = PlayerPrefs.GetInt ("MyCoins").ToString ();
	}
}
