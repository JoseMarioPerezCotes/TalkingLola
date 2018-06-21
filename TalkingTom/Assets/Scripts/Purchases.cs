using UnityEngine;
using System.Collections;

public class Purchases : MonoBehaviour {
	RoomUIManager uiManager;
	public UILabel myCoinsLabel;
	// Use this for initialization
	void Start () {
		uiManager = GameObject.Find("UI Root").GetComponent<RoomUIManager>();
		myCoinsLabel.text = PlayerPrefs.GetInt("MyCoins").ToString ();
	}
	
	// Update is called once per frame
	void Update () {
		#if UNITY_ANDROID
		if(Input.GetKeyDown (KeyCode.Escape))
		{
			Cancel();
		}
#endif
	}

	public void Buy200Coins()
	{
		if(!LoaderPanel.loaderActive && !PopupPanel.popupPanelActive)
		{
			InitializePlugins._instance.MakePurchases("IAP_ID.200coins");
			uiManager.loaderPanel.SetActive (true);
		}
	}

	public void Buy300Coins()
	{
		if(!LoaderPanel.loaderActive && !PopupPanel.popupPanelActive)
		{

		}
	}

	public void Buy500Coins()
	{
		if(!LoaderPanel.loaderActive && !PopupPanel.popupPanelActive)
		{

			InitializePlugins._instance.MakePurchases("IAP_ID.500coins");
			uiManager.loaderPanel.SetActive (true);
		}
	}

	public void Buy750Coins()
	{
		if(!LoaderPanel.loaderActive && !PopupPanel.popupPanelActive)
		{

		}
	}

	public void Buy1000Coins()
	{
		if(!LoaderPanel.loaderActive && !PopupPanel.popupPanelActive)
		{

			InitializePlugins._instance.MakePurchases("IAP_ID.1000coins");
			uiManager.loaderPanel.SetActive (true);
		}
	}

	public void Cancel()
	{
		if(!LoaderPanel.loaderActive && !PopupPanel.popupPanelActive)
		{
			Destroy (gameObject);
			AnimationHandler._instance.coinsPanelActive = false;
			AnimationHandler._instance.mainScreenPanel.SetActive (true);
//			GoogleMobileAdsDemoScript._instance.ShowBanner ();
		}
	}
}
