using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
	public static int coinsGenerated;
    #region PUBLIC_VARIABLES
		public static GameManager instance;

		public GameObject bird;

		public GameObject mainPanel;
		public GameObject gamePlayPanel;
		public GameObject gamePlayUI;
		public GameObject gamePause;
		public GameObject gameState;
		public GameObject instructionPanel;
		public GameObject highScorePanel;

		public TextMesh timeLable;
		public TextMesh timeLable2;

		public TextMesh distanceLable;
		public TextMesh distanceLable2;

		public TextMesh scoreLable;
		public TextMesh coinsLabel;
	public TextMesh scoreLabelBold , coinsLabelBold;
		public TextMesh hScoreLable;
		public TextMesh hScoreLabelBold;	
	public TextMesh instructionLabel,instructionLabelBold;
		public SpriteRenderer FadeInFadeOutImage;

		public List<GameObject> defaultList;

		public List<GameObject> generatedNest;

		public GameObject nest;

		public bool isPrevNestStatic;

		public bool isGameRunning;

		public float starTime;

		
    #endregion

    #region PRIVATE_VARIABLES
		private float gamePlayTime;	
		private float gamePlayDistance;
		private bool isInstructionOrHighScore = false;
		private bool isMainMenu = false;
		private bool isGamePlay = false;
		private bool isGamePaused = false;
		private bool isFadding = false;
		private bool isGameOver = false;
		
    #endregion

    #region UNITY_CALLBACKS
		void Awake ()
		{
				instance = this;
				InitConfig ();
		}
 	void Start()
	{
		instructionLabel.text = "TAP ON THE SCREEN \n TO MAKE THE FAIRY \n JUMP.";
		instructionLabelBold.text = instructionLabel.text;
	}
		void Update ()
		{
				if (isGameRunning) {
						UpdateTimer ();
						UpdateDistance ();
				}
				if (Input.GetKeyDown (KeyCode.Escape)) {
						if (!isFadding)
								BackButton ();
				}
		}
    #endregion

    #region PRIVATE_METHODS
		private void BackButton ()
		{
				if (isGamePlay) {
						OnPause ();
				} else if (isGamePaused) {
						OnContinue ();
				}
				if (isInstructionOrHighScore) {
						OnBackToMain ();
				}
				if (isGameOver)
						OnMenu ();
				
		}
    #endregion

    #region PUBLIC_METHODS    
		public void UpdateTimer ()
		{
				int val = (int)((Time.time - starTime) * 100);
				timeLable.text = "" + ((float)val / 100);
				timeLable2.text = "" + ((float)val / 100);

		}

		public void UpdateDistance ()
		{
				Vector3 pos1 = Vector3.zero;
				Vector3 pos2 = Vector3.zero;
				pos1.y = Bird.instance.initPos.y;
				pos2.y = Bird.instance.transform.position.y;
				distanceLable.text = "" + ((float)((int)(Vector3.Distance (pos1, pos2) * 100)) / 100);
				distanceLable2.text = "" + ((float)((int)(Vector3.Distance (pos1, pos2) * 100)) / 100);

		}

		public void InitConfig ()
		{
				bird.SetActive (true);
				for (int i = 3; i < generatedNest.Count; i++) {
						Destroy (generatedNest [i]);
				}
				generatedNest = new List<GameObject> ();
				foreach (GameObject obj in defaultList) {
						obj.GetComponent<Nest> ().isNestGenerated = false;
						generatedNest.Add (obj);
				}
				if (Bird.instance != null)
						Bird.instance.GetComponent<Rigidbody2D>().gravityScale = 1.3f;
		}

		public void GenerateNewNest ()
		{
				float y = generatedNest [generatedNest.Count - 1].transform.position.y;
				bool isStatic = Random.Range (0, 2) == 0 ? true : false;
				if (isPrevNestStatic) {
						isStatic = false;
				}
				float xPos = 0;
				if (isStatic) {
						xPos = Random.Range (-4.0f, 4.0f);
						isPrevNestStatic = true;
				}
				GameObject clone = (GameObject)Instantiate (nest, new Vector3 (xPos, y + 5.5f, 0), Quaternion.identity);
				Nest cloneNest = clone.GetComponent<Nest> ();
				if (!isStatic) {
						isPrevNestStatic = false;
						cloneNest.isStaticNest = false;
						cloneNest.amountOfDisplacement = 4;// Random.Range(1, 4);
						float maxrandSpeed = (generatedNest [generatedNest.Count - 1].transform.position.y) / 100;
						maxrandSpeed = Mathf.Clamp (maxrandSpeed, 0.5f, 5);
						cloneNest.speed = Random.Range (0.4f, maxrandSpeed);
						cloneNest.isMovingRight = Random.Range (0, 2) == 0 ? true : false;
				}
				clone.transform.parent = gamePlayPanel.transform;
				generatedNest.Add (clone);
				if (generatedNest.Count % 2 != 0)
						cloneNest.isCameraPoint = true;
		}

		public void GameComplete ()
		{
			isGameRunning = false;
			isGamePlay = false;
			isGameOver = true;
//		GoogleMobileAdsDemoScript._instance.ShowInterstitial ();
			int score = (int)(float.Parse (distanceLable.text) * 10);
			scoreLable.text = "You Score :\n " + score;
			scoreLabelBold.text = scoreLable.text; 
			SoundManager.instance.PlayLevelComplete ();
			gameState.SetActive (true);
			gamePlayUI.SetActive (false);
			CheckForScoreUpdate (score);

			int coinsAdded = (int)(score/ 100);
			coinsLabel.text = "Coins Won : \n" +coinsAdded;
			coinsLabelBold.text = coinsLabel.text;
			PlayerPrefs.SetInt ("MyCoins" , PlayerPrefs.GetInt("MyCoins")+coinsAdded);
			coinsGenerated+=coinsAdded;
		}


		public void CheckForScoreUpdate (int score)
		{
				if (PlayerPrefs.HasKey ("Score")) {
						if (PlayerPrefs.GetInt ("Score") < score)
								PlayerPrefs.SetInt ("Score", score);
				} else {
						PlayerPrefs.SetInt ("Score", score);
				}
		}
    #endregion

    #region GUI

		public void OnPlay ()
		{
				isGamePlay = true;
				isGameOver = false;
				GameObject[] newScreens = new GameObject[2]; 
				newScreens [0] = gamePlayPanel;
				newScreens [1] = gamePlayUI;
				GameObject[] oldScreens = new GameObject[1];
				oldScreens [0] = mainPanel;
				StartCoroutine (FadeIn (oldScreens, newScreens, true));
//				mainPanel.SetActive (false);
//				gamePlayPanel.SetActive (true);
//				gamePlayUI.SetActive (true);
//				Bird.instance.SetDefaultPosition ();
				MainCamera.instance.transform.position = new Vector3 (0, 0, -10);
				starTime = Time.time;
//				isGameRunning = true;
				InitConfig ();
		}

		public void OnInstruction ()
		{
				isInstructionOrHighScore = true;
				GameObject[] newScreens = new GameObject[1]; 
				newScreens [0] = instructionPanel;

				GameObject[] oldScreens = new GameObject[1];
				oldScreens [0] = mainPanel;	
				StartCoroutine (FadeIn (oldScreens, newScreens, false));
//				instructionPanel.SetActive (true);
//				mainPanel.SetActive (false);
		}

		public void OnPause ()
		{
			gamePlayPanel.SetActive (false);
			isGamePlay = false;
			isGamePaused = true;
			Time.timeScale = 0;
			gamePause.SetActive (true);
		}

		public void OnMenu ()
		{
				isMainMenu = true;
				isGamePaused = false;
				isGamePlay = false;
				isGameOver = false;
				isInstructionOrHighScore = false;
				
				Time.timeScale = 1;
				MainCamera.instance.transform.position = new Vector3 (0, 0, -10);
				
				GameObject[] newScreens = new GameObject[1]; 
				newScreens [0] = mainPanel;
				
				GameObject[] oldScreens = new GameObject[4];
				oldScreens [0] = gameState;				
				oldScreens [1] = gamePlayPanel;
				oldScreens [2] = gamePlayUI;
				oldScreens [3] = gamePause;
				StartCoroutine (FadeIn (oldScreens, newScreens, false));
		
//				gameState.SetActive (false);
//				gamePlayPanel.SetActive (false);
//				gamePlayUI.SetActive (false);
//				mainPanel.SetActive (true);
//				gamePause.SetActive (false);
		}

		public void OnContinue ()
		{
				Time.timeScale = 1;
				gamePause.SetActive (false);
				isGamePlay = true;
				gamePlayPanel.SetActive (true);
		}

		public void OnReplay ()
		{
				Time.timeScale = 1;
				if (Bird.instance != null)
						Bird.instance.StopCo ();
				gameState.SetActive (false);
				gamePause.SetActive (false);
				OnPlay ();
		}

		public void OnHighScore ()
		{
				isInstructionOrHighScore = true;
				isMainMenu = false;
				if (PlayerPrefs.HasKey ("Score")) 
				hScoreLable.text = "High Score:\n" + PlayerPrefs.GetInt ("Score");
				else
						hScoreLable.text = "High Score: " + 0;
		hScoreLabelBold.text = hScoreLable.text;	
				GameObject[] newScreens = new GameObject[1]; 
				newScreens [0] = highScorePanel;
		
				GameObject[] oldScreens = new GameObject[1];
				oldScreens [0] = mainPanel;	
				StartCoroutine (FadeIn (oldScreens, newScreens, false));
		
//				highScorePanel.SetActive (true);
//				mainPanel.SetActive (false);
		}

    
		public void OnBackToMain ()
		{
				isInstructionOrHighScore = false;
				isMainMenu = true;
				isGamePaused = false;
				isGamePlay = false;
				GameObject[] newScreens = new GameObject[1]; 
				newScreens [0] = mainPanel;
		
				GameObject[] oldScreens = new GameObject[2];
				oldScreens [0] = highScorePanel;				
				oldScreens [1] = instructionPanel;
				
				StartCoroutine (FadeIn (oldScreens, newScreens, false));
//				highScorePanel.SetActive (false);
//				instructionPanel.SetActive (false);
//				mainPanel.SetActive (true);
		}
    #endregion

    #region COROUTINES
		IEnumerator FadeIn (GameObject[] oldScreen, GameObject[] newScreen, bool isOnPlay)
		{
				isFadding = true;
				float i = 0f;
				while (i<1) {
						i += Time.deltaTime * 3;
						float alpha = Mathf.Lerp (0, 1, i);
						FadeInFadeOutImage.color = new Color (FadeInFadeOutImage.color.r, FadeInFadeOutImage.color.g, FadeInFadeOutImage.color.b, alpha);
						yield return 0;
				}
				
				for (int j=0; j<oldScreen.Length; j++)
						oldScreen [j].SetActive (false);
				for (int j=0; j<newScreen.Length; j++)
						newScreen [j].SetActive (true);
				
				StartCoroutine (FadeOut (isOnPlay));
				yield return 0;
		}
		IEnumerator FadeOut (bool isOnPlay)
		{
				if (isOnPlay) {
						Bird.instance.SetDefaultPosition ();
						isGameRunning = true;
				}
				float i = 0f;
				while (i<1) {
						i += Time.deltaTime * 3;
						float alpha = Mathf.Lerp (1, 0, i);
						FadeInFadeOutImage.color = new Color (FadeInFadeOutImage.color.r, FadeInFadeOutImage.color.g, FadeInFadeOutImage.color.b, alpha);						
						yield return 0;
				}				
				yield return 0;
				isFadding = false;				
		}
    #endregion

	public void GoBackToGame()
	{
		Application.LoadLevel (RoomUIManager.previousLoadedLevel);
	}

}
