using UnityEngine;
using System.Collections;
using MiniJSON;
using UnityEngine.Advertisements;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using Facebook.Unity;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class GetMoreCoins : MonoBehaviour
{
    private RewardBasedVideoAd rewardedVideo;

	RoomUIManager uiManager;
	public UILabel myCoinsLabel;
	// Use this for initialization

	void Start ()
	{
        rewardedVideo = RewardBasedVideoAd.Instance;

        rewardedVideo.OnAdLoaded += HandleOnAdLoaded;
        rewardedVideo.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        rewardedVideo.OnAdRewarded += HandleOnAdRewarded;
        rewardedVideo.OnAdClosed += HandleOnAdClosed;

        uiManager = GameObject.Find ("UI Root").GetComponent<RoomUIManager> ();
		myCoinsLabel.text = PlayerPrefs.GetInt ("MyCoins").ToString ();

	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Cancel ();
			Debug.Log ("pressed here");
		}
	}

	#region FB.Login()

	public void FBLogin ()
	{
        //		if(!LoaderPanel.loaderActive && !PopupPanel.popupPanelActive)
        //		{
        //			if(!PlayerPrefs.HasKey("FbLogged"))
        //			{
        //				uiManager.loaderPanel.SetActive (true);
        //				FB.Login("email",LoginCallback);
        //			}
        //			else
        //			{
        //				uiManager.ActivatePopup ("You are already logged in",false);
        //			}
        //		}       		
    }

    string lastResponse;

	void LoginCallback (IResult result)
	{
		uiManager.loaderPanel.SetActive (false);
		Debug.Log ("result = " + result.RawResult.ToString ());
//		if (result.Error != null)
//			lastResponse = "Error Response:\n" + result.Error;
//		else if (!FB.IsLoggedIn) {
//			lastResponse = "Login cancelled by Player";
//		} else {
//			lastResponse = "Login was successful!";
//			PlayerPrefs.SetInt ("FbLogged", 1);
//			uiManager.ActivatePopup ("Congratulations!! \nYou received 50 coins..!!", false);
//			StartCoroutine (IncrementCoins (PlayerPrefs.GetInt ("MyCoins"), 50));
//			PlayerPrefs.SetInt ("MyCoins", PlayerPrefs.GetInt ("MyCoins") + 50);
//		}
//	
//		Debug.Log ("login result  = " + lastResponse);
//
		if (result == null) {
			this.lastResponse = "Null Response\n";
			return;
		}
			
		// Some platforms return the empty string instead of null.
		if (!string.IsNullOrEmpty (result.Error)) {
			this.lastResponse = "Error - Check log for details";
			this.lastResponse = "Error Response:\n" + result.Error;
		} else if (result.Cancelled) {
			this.lastResponse = "Cancelled - Check log for details";
			this.lastResponse = "Cancelled Response:\n" + result.RawResult;
		} else if (!string.IsNullOrEmpty (result.RawResult)) {
			this.lastResponse = "Success - Check log for details";
			this.lastResponse = "Success Response:\n" + result.RawResult;
			Debug.Log ("login is done ");
			PlayerPrefs.SetInt ("FbLogged", 1);
			uiManager.ActivatePopup ("Congratulations!! \nYou received 50 coins..!!", false);
			StartCoroutine (IncrementCoins (PlayerPrefs.GetInt ("MyCoins"), 50));
			PlayerPrefs.SetInt ("MyCoins", PlayerPrefs.GetInt ("MyCoins") + 50);
		} else {
			this.lastResponse = "Empty Response\n";
		}
	}




	#endregion

	IEnumerator IncrementCoins (int myCoins, int amountIncreased)
	{
		int incrementDelta = (int)amountIncreased / 20;
		int finalCoins = myCoins + amountIncreased;
		float timer = 0.0f;
		while (myCoins < finalCoins) {
			while (timer < 0.6f) {
				timer += 0.02f;
				yield return 0;
			}
			myCoins += incrementDelta;
			myCoinsLabel.text = myCoins.ToString ();
			yield return 0;
		}
		myCoinsLabel.text = PlayerPrefs.GetInt ("MyCoins").ToString ();
	}


	IEnumerator IncrementCoinsVideo (int myCoins, int amountIncreased)
	{
		int incrementDelta = (int)amountIncreased / 20;
		int finalCoins = myCoins + amountIncreased;
		float timer = 0.0f;
		UILabel myCoinsLabel = GameObject.FindObjectOfType<GetMoreCoins> ().myCoinsLabel;
		while (myCoins < finalCoins) {
			while (timer < 0.6f) {
				timer += 0.02f;
				yield return 0;
			}
			myCoins += incrementDelta;
			myCoinsLabel.text = myCoins.ToString ();
			yield return 0;
		}
		myCoinsLabel.text = PlayerPrefs.GetInt ("MyCoins").ToString ();
	}


	public void ShowVideo ()
	{
//		if(!LoaderPanel.loaderActive && !PopupPanel.popupPanelActive)
//		{
//			if( Advertisement.isReady() ) {
//				// Show with default zone, pause engine and print result to debug log
//				Advertisement.Show(null, new ShowOptions {
//					pause = true,
//					resultCallback = result => {
//						Debug.Log(result.ToString());
//						if(result.ToString ().Contains("Finished"))
//						{
//							if(GameObject.FindObjectOfType<GetMoreCoins>().myCoinsLabel)
//								StartCoroutine (IncrementCoinsVideo( PlayerPrefs.GetInt ("MyCoins"), 30));
//							PlayerPrefs.SetInt ("MyCoins",(PlayerPrefs.GetInt ("MyCoins")+30) );
//						}
//					}
//				});
//			}
//			else
//			{
//				uiManager.ActivatePopup ("Video not available..!!",false);
//			}
//		}
	}

	public void Cancel ()
	{
		if (!LoaderPanel.loaderActive && !PopupPanel.popupPanelActive) {
			Destroy (gameObject);
			AnimationHandler._instance.coinsPanelActive = false;
			AnimationHandler._instance.mainScreenPanel.SetActive (true);
//			GoogleMobileAdsDemoScript._instance.ShowBanner ();
		}
	}

	public void Purchases ()
	{
//		if(!LoaderPanel.loaderActive && !PopupPanel.popupPanelActive)
//		{
//			GameObject purchasePanel = (GameObject)Instantiate (Resources.Load("Purchases"));
//			purchasePanel.transform.parent = transform.parent;
//			purchasePanel.transform.localScale = Vector3.one;
//			Destroy (gameObject);
//		}
	}

    public void LoadRewardedVideo()
    {
#if UNITY_EDITOR
        string adUnitsId = "Sin uso";
#elif UNITY_ANDROID
        string adUnitsId = "ca-app-pub-6113913621681882/1495078568";
#elif UNITY_IPHONE
        string adUnitsId = "ca-app-pub-6113913621681882/9500221749";
#else
        string adUnitsId = "Plataforma no reconocida";
#endif
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            //AdRequest request = new AdRequest.Builder().AddTestDevice("DE609E45-7DFF-4310-8347-CE3E1838509C").Build(); //Tablet
            AdRequest request = new AdRequest.Builder().AddTestDevice("04F6FEDD-A0D7-4154-88C3-BF17C3933478").Build(); //Lenovo
            //AddTestDevice("037ce47b-2924-4132-8969-5db5d5fa6274").Build(); //Huawei
            //AdRequest request = new AdRequest.Builder().AddTestDevice("ffbcf4dc-72d3-4a29-90d9-11c554abc6dd").Build(); //Samsung
            rewardedVideo.LoadAd(request, adUnitsId);
        }

    }
    public void ShowRewardedVideo()
    {
        if (rewardedVideo.IsLoaded())
        {
            rewardedVideo.Show();
        }
        

    }

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        ShowRewardedVideo();
    }
    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
       
    }
    public void HandleOnAdClosed(object sender, EventArgs args)
    {       

    }
    public void HandleOnAdRewarded(object sender, Reward args)
    {
    }
    private void OnDisable()
    {
        rewardedVideo.OnAdLoaded -= HandleOnAdLoaded;
        rewardedVideo.OnAdFailedToLoad -= HandleOnAdFailedToLoad;
        rewardedVideo.OnAdClosed -= HandleOnAdClosed;
        rewardedVideo.OnAdRewarded -= HandleOnAdRewarded;

    }
}

