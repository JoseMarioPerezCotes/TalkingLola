using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScrollVirtualReality : MonoBehaviour {
	public float springStrength = 8.0f;
	private UIScrollView scrollView;
	private int NoOfScroll;
	private Vector3 startingScrollPosition;
	private UIGrid grid;

	GameObject Item;
	GameObject nextButton,prevButton;
	RoomUIManager uiManager;
	int CountClick;

    public int dogCost, turtleCost, owlCost;
    public int moveToElement;
	public GameObject rightButton , leftButton;
	public GameObject playBtn,buyBtn;
	public UILabel myCoinsLabel;
	public static bool disableButtonPress;
	public static int characterNo;
	public GameObject popUpPanel;
	public GameObject characterParent;

	public DinoMove mydino;

	void OnEnable()
	{
		Destroy (mydino.turtle);
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
            if (PlayerPrefs.HasKey(character[1]))////if(PlayerPrefs.HasKey("Owl"))
            {
				buyBtn.SetActive (false);
				playBtn.SetActive (true);
			}
			else
			{
				buyBtn.SetActive (true);
				buyBtn.GetComponentInChildren<UILabel>().text = owlCost.ToString ();
				playBtn.SetActive (false);
			}

		}
		else if(CountClick == 2)
		{
            if (PlayerPrefs.HasKey(character[2]))//if(PlayerPrefs.HasKey("Turtle"))
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
        else if (CountClick == 3)
        {
            if (PlayerPrefs.HasKey(character[3]))//if(PlayerPrefs.HasKey("Turtle"))
            {
                buyBtn.SetActive(false);
                playBtn.SetActive(true);
            }
            else
            {
                buyBtn.SetActive(true);
                buyBtn.GetComponentInChildren<UILabel>().text = turtleCost.ToString();
                playBtn.SetActive(false);
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
		if( !PopupPanel.popupPanelActive)
		{
			switch(CountClick)
			{
			case 0:
                characterNo = 0;//character.IndexOf(character[0]);  // --- FOX
				ScrollButtonAction.characterNo = 0;//character.IndexOf(character[0]);
                    break;
			case 1:
                    characterNo = 1; //character.IndexOf(character[1]); //   --  PIG
                    ScrollButtonAction.characterNo = 1;// character.IndexOf(character[1]);
                    break;
			case 2:
				characterNo = character.IndexOf(character[2]); //     --- bull
				ScrollButtonAction.characterNo = character.IndexOf(character[2]);
                    break;
			case 3:
				characterNo = character.IndexOf(character[3]);  //  --- BUNNY
				ScrollButtonAction.characterNo = character.IndexOf(character[3]);
                    break;
//			case 0:
//				characterNo = 2;  // --- FOX
//				ScrollButtonAction.characterNo = 2;
//				break;
//			case 1:
//				characterNo = 0; //   --  PIG
//				ScrollButtonAction.characterNo = 0;
//				break;
//			case 2:
//				characterNo = 3; //     --- bull
//				ScrollButtonAction.characterNo = 1;
//				break;
//			case 3:
//				characterNo = 1; //  --- BUNNY
//				ScrollButtonAction.characterNo = 3;
//				break;
			}


			PlayerPrefs.SetInt("Character",ScrollButtonAction.characterNo);
			GameObject myCharacter = null;
	//		switch(characterNo)
//		{
//		case 0:
//			myCharacter = (GameObject)Instantiate(Resources.Load("DogVirtual"));
//			break;
//		case 1:
//			myCharacter = (GameObject)Instantiate(Resources.Load("CatVirtual"));
//
//			break;
//		case 2:
//			myCharacter = (GameObject)Instantiate(Resources.Load("TurtleVirtual"));
//
//			break;
//		case 3:
//				myCharacter = (GameObject)Instantiate(Resources.Load("OwlVirtual"));
//			break;
//		}


			switch(ScrollButtonAction.characterNo)
			{
			case 0:
				myCharacter = (GameObject)Instantiate(Resources.Load(character[0])); //(GameObject)Instantiate(Resources.Load("DogVirtual"));
                    Debug.LogError(character[0]);
				break;
			case 1:
				myCharacter = (GameObject)Instantiate(Resources.Load(character[1])); //(GameObject)Instantiate(Resources.Load("OwlVirtual"));
                    Debug.LogError(character[1]);
				break;
			case 2:
				myCharacter = (GameObject)Instantiate(Resources.Load(character[2])); //(GameObject)Instantiate(Resources.Load("TurtleVirtual"));
                    Debug.LogError(character[2]);
				break;
			case 3:
				myCharacter = (GameObject)Instantiate(Resources.Load(character[3])); // (GameObject)Instantiate(Resources.Load("CatVirtual"));
                    Debug.LogError(character[3]);
				break;
			}

			myCharacter.transform.parent = characterParent.transform;
			myCharacter.transform.localPosition = new Vector3(-12.3f,10f,-15);
			myCharacter.transform.localRotation = Quaternion.Euler(0f,180f,0f);
			myCharacter.transform.localScale = new Vector3(5,5,5);
			if(!TrackableEventHandler.rendered)
			{
				Renderer[] rendererComponents = myCharacter.GetComponentsInChildren<Renderer>(true);
				Collider[] colliderComponents = myCharacter.GetComponentsInChildren<Collider>(true);
				
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
			Destroy (mydino.turtle);
			gameObject.transform.parent.parent.gameObject.SetActive (false);
            mydino.turtle = myCharacter;
                //TIRAR PEDOS EN REALIDAD AUMENTADA
            if (characterNo <= 1  )//if(characterNo == 3)
            {
				Debug.Log("characterNo =" + characterNo);
				VirtualRealityPanel._instance.otherAnimations.SetActive (false);
				VirtualRealityPanel._instance.catAnimations.SetActive (true);
			}
//			if(characterNo == 2 || characterNo == 1)
//			{
//				Debug.Log("characterNo =" + characterNo);
//				VirtualRealityPanel._instance.otherAnimations.SetActive (false);
//				VirtualRealityPanel._instance.catAnimations.SetActive (true);
//			}
			else
			{
				Debug.Log("characterNo =" + characterNo);

				VirtualRealityPanel._instance.otherAnimations.SetActive (true);
				VirtualRealityPanel._instance.catAnimations.SetActive (false);
			}
			if(TrackableEventHandler.rendered)
				VirtualRealityPanel._instance.uiPanel.SetActive (true);
			VirtualRealityPanel._instance.backButton.SetActive (true);
		}
	}

	public void Purchase()
	{
		ActivatePopup ("Do you want to buy the character?",true);
		popUpPanel.GetComponent<PopupPanel>().okButton.GetComponent<UIButton>().onClick.Clear();
		popUpPanel.GetComponent<PopupPanel>().okButton.GetComponent<UIButton>().onClick.Add(new EventDelegate(BuyCharacter));
	}

	void BuyCharacter()
	{
		popUpPanel.SetActive (false);
		if(CountClick == 1)
		{
			if(PlayerPrefs.GetInt ("MyCoins") >= owlCost)
			{
				StartCoroutine(DecrementCoins(PlayerPrefs.GetInt("MyCoins"),owlCost));
				PlayerPrefs.SetInt ("MyCoins", PlayerPrefs.GetInt ("MyCoins") - owlCost);
                PlayerPrefs.SetInt(character[1], 1); //PlayerPrefs.SetInt ("Owl",1);
                ActivatePopup("Character Purchased Successfully" , false);
				UseOrPurchase();
			}
			else
			{
				ActivatePopup("You do not have enough coins to buy the character!" , false);
			}
		}
        else if (CountClick == 2)
        {
            if (PlayerPrefs.GetInt("MyCoins") >= turtleCost)
            {
                StartCoroutine(DecrementCoins(PlayerPrefs.GetInt("MyCoins"), turtleCost));
                PlayerPrefs.SetInt("MyCoins", PlayerPrefs.GetInt("MyCoins") - turtleCost);
                PlayerPrefs.SetInt(character[2], 1);//PlayerPrefs.SetInt("Turtle", 1);
                ActivatePopup("Character Purchased Successfully", false);
                UseOrPurchase();
            }
            else
            {
                ActivatePopup("You do not have enough coins to buy the character!", false);
            }
        }
        else if(CountClick == 3)
		{
			if(PlayerPrefs.GetInt ("MyCoins") >= turtleCost)
			{
				StartCoroutine(DecrementCoins(PlayerPrefs.GetInt("MyCoins"),turtleCost));
				PlayerPrefs.SetInt ("MyCoins", PlayerPrefs.GetInt ("MyCoins") - turtleCost);
                PlayerPrefs.SetInt(character[3], 1); //PlayerPrefs.SetInt ("Turtle",1);
				ActivatePopup("Character Purchased Successfully" , false);
				UseOrPurchase();
			}
			else
			{
				ActivatePopup("You do not have enough coins to buy the character!" , false);
			}
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
