using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Prime31;
using UnityEngine.Advertisements;

public class InitializePlugins : MonoBehaviour
{
	RoomUIManager uiManager;
	public AudioClip buttonClick;
	public bool showUploadStatus = true;
	public static bool isRecording = false;
	private bool isPaused = false;
	private bool isRecordingFinished = false;
	private GUIText uploadStatusLabel;
	private Texture2D previousThumbnail;
	public static InitializePlugins _instance;

	#if UNITY_IPHONE
	private List<StoreKitProduct> _products;
	#endif

	public static bool once;
	// Use this for initialization

	void Awake ()
	{
		if (!PlayerPrefs.HasKey ("MyCoins")) {
			PlayerPrefs.SetInt ("MyCoins", 100);
		}
	}

	public bool IsRecordSupported ()
	{
		if (Everyplay.IsRecordingSupported () && Everyplay.IsSupported ()) {
			return true;
		} else {
			return false;
		}
	}

	void Start ()
	{
		_instance = this;

		if (!once) {
			if (uploadStatusLabel != null) {
				Everyplay.UploadDidStart += UploadDidStart;
				Everyplay.UploadDidProgress += UploadDidProgress;
				Everyplay.UploadDidComplete += UploadDidComplete;
			}
			
			Everyplay.RecordingStarted += RecordingStarted;
			Everyplay.RecordingStopped += RecordingStopped;
			
			//		Everyplay.ThumbnailReadyAtFilePath += ThumbnailReadyAtFilePath;
			CallFBInit ();
			#if UNITY_IPHONE
			// you cannot make any purchases until you have retrieved the products from the server with the requestProductData method
			// we will store the products locally so that we will know what is purchaseable and when we can purchase the products
			StoreKitManager.productListReceivedEvent += allProducts => {
				Debug.Log ("received total products: " + allProducts.Count);
				_products = allProducts;
			};
			
			// array of product ID's from iTunesConnect. MUST match exactly what you have there!
			var productIdentifiers = new string[] { "IAP_ID.200coins", "IAP_ID.500coins", "IAP_ID.1000coins" };
			StoreKitBinding.requestProductData (productIdentifiers);
			#else
			Debug.Log ("querying....!!");
			var key = "your public key from the Android developer portal here";

			GoogleIAB.init (key);
			var skus = new string[] { "IAP_ID.200coins", "IAP_ID.500coins", "IAP_ID.1000coins" };
			GoogleIAB.queryInventory (skus);
			#endif


			once = true;
			DontDestroyOnLoad (gameObject);
		} else {
			Destroy (gameObject);
		}

	}


	void LoadInterstitial ()
	{

	}


	#region Everyplay

	void Destroy ()
	{
		if (uploadStatusLabel != null) {
			Everyplay.UploadDidStart -= UploadDidStart;
			Everyplay.UploadDidProgress -= UploadDidProgress;
			Everyplay.UploadDidComplete -= UploadDidComplete;
		}
		
		Everyplay.RecordingStarted -= RecordingStarted;
		Everyplay.RecordingStopped -= RecordingStopped;
		
//		Everyplay.ThumbnailReadyAtFilePath -= ThumbnailReadyAtFilePath;
	}

	private void RecordingStarted ()
	{
		isRecording = true;
		isPaused = false;
		isRecordingFinished = false;
	}

	private void RecordingStopped ()
	{
		isRecording = false;
		isRecordingFinished = true;
	}

	private void CreateUploadStatusLabel ()
	{
		GameObject uploadStatusLabelObj = new GameObject ("UploadStatus", typeof(GUIText));
		
		if (uploadStatusLabelObj) {
			uploadStatusLabelObj.transform.parent = transform;
			uploadStatusLabel = uploadStatusLabelObj.GetComponent<GUIText> ();
			
			if (uploadStatusLabel != null) {
				uploadStatusLabel.anchor = TextAnchor.LowerLeft;
				uploadStatusLabel.alignment = TextAlignment.Left;
				uploadStatusLabel.text = "Not uploading";
			}
		}
	}

	private void UploadDidStart (int videoId)
	{
		uploadStatusLabel.text = "Upload " + videoId + " started.";
	}

	private void UploadDidProgress (int videoId, float progress)
	{
		uploadStatusLabel.text = "Upload " + videoId + " is " + Mathf.RoundToInt ((float)progress * 100) + "% completed.";
	}

	private void UploadDidComplete (int videoId)
	{
		uploadStatusLabel.text = "Upload " + videoId + " completed.";
		
		StartCoroutine (ResetUploadStatusAfterDelay (2.0f));
	}

	private IEnumerator ResetUploadStatusAfterDelay (float time)
	{
		yield return new WaitForSeconds (time);
		uploadStatusLabel.text = "Not uploading";
	}

	private void ThumbnailReadyAtFilePath (string path)
	{
		// We are loading the thumbnail during the recording for demonstration purposes only.
		// Normally you should start the load after you have stopped the recording to make sure the rendering does not stutter.
//		Everyplay.LoadThumbnailFromFilePath(path, ThumbnailSuccess, ThumbnailError);
	}

	private void ThumbnailSuccess (Texture2D texture)
	{
		if (texture != null) {
			previousThumbnail = texture;
		}
	}

	private void ThumbnailError (string error)
	{
		Debug.Log ("Thumbnail loading failed: " + error);
	}

	#endregion


	public void MakePurchases (string productIdentifier)
	{
		#if UNITY_IPHONE

		StoreKitBinding.purchaseProduct (productIdentifier, 1);

		#elif UNITY_ANDROID

		GoogleIAB.purchaseProduct (productIdentifier, "1");
		#endif
	}

	void OnEnable ()
	{

		#if UNITY_IPHONE
//		Chartboost.didCompleteAppStoreSheetFlow += didCompleteAppStoreSheetFlow;
		// Listens to all the StoreKit events. All event listeners MUST be removed before this object is disposed!
		StoreKitManager.transactionUpdatedEvent += transactionUpdatedEvent;
		StoreKitManager.productPurchaseAwaitingConfirmationEvent += productPurchaseAwaitingConfirmationEvent;
		StoreKitManager.purchaseSuccessfulEvent += purchaseSuccessfulEvent;
		StoreKitManager.purchaseCancelledEvent += purchaseCancelledEvent;
		StoreKitManager.purchaseFailedEvent += purchaseFailedEvent;
		StoreKitManager.productListReceivedEvent += productListReceivedEvent;
		StoreKitManager.productListRequestFailedEvent += productListRequestFailedEvent;
		StoreKitManager.restoreTransactionsFailedEvent += restoreTransactionsFailedEvent;
		StoreKitManager.restoreTransactionsFinishedEvent += restoreTransactionsFinishedEvent;
		StoreKitManager.paymentQueueUpdatedDownloadsEvent += paymentQueueUpdatedDownloadsEvent;
		#elif UNITY_ANDROID
		
		// Listen to all events for illustration purposes
		GoogleIABManager.billingSupportedEvent += billingSupportedEvent;
		GoogleIABManager.billingNotSupportedEvent += billingNotSupportedEvent;
		GoogleIABManager.queryInventorySucceededEvent += queryInventorySucceededEvent;
		GoogleIABManager.queryInventoryFailedEvent += queryInventoryFailedEvent;
		GoogleIABManager.purchaseCompleteAwaitingVerificationEvent += purchaseCompleteAwaitingVerificationEvent;
		GoogleIABManager.purchaseSucceededEvent += purchaseSucceededEvent;
		GoogleIABManager.purchaseFailedEvent += purchaseFailedEvent;
		GoogleIABManager.consumePurchaseSucceededEvent += consumePurchaseSucceededEvent;
		GoogleIABManager.consumePurchaseFailedEvent += consumePurchaseFailedEvent;
//		
		#endif
	}

	
	void OnDisable ()
	{

		#if UNITY_IPHONE
		
		// Remove all the event handlers
		StoreKitManager.transactionUpdatedEvent -= transactionUpdatedEvent;
		StoreKitManager.productPurchaseAwaitingConfirmationEvent -= productPurchaseAwaitingConfirmationEvent;
		StoreKitManager.purchaseSuccessfulEvent -= purchaseSuccessfulEvent;
		StoreKitManager.purchaseCancelledEvent -= purchaseCancelledEvent;
		StoreKitManager.purchaseFailedEvent -= purchaseFailedEvent;
		StoreKitManager.productListReceivedEvent -= productListReceivedEvent;
		StoreKitManager.productListRequestFailedEvent -= productListRequestFailedEvent;
		StoreKitManager.restoreTransactionsFailedEvent -= restoreTransactionsFailedEvent;
		StoreKitManager.restoreTransactionsFinishedEvent -= restoreTransactionsFinishedEvent;
		StoreKitManager.paymentQueueUpdatedDownloadsEvent -= paymentQueueUpdatedDownloadsEvent;
//		Chartboost.didCompleteAppStoreSheetFlow -= didCompleteAppStoreSheetFlow;
		#elif UNITY_ANDROID
		
		// Remove all event handlers
		GoogleIABManager.billingSupportedEvent -= billingSupportedEvent;
		GoogleIABManager.billingNotSupportedEvent -= billingNotSupportedEvent;
		GoogleIABManager.queryInventorySucceededEvent -= queryInventorySucceededEvent;
		GoogleIABManager.queryInventoryFailedEvent -= queryInventoryFailedEvent;
		GoogleIABManager.purchaseCompleteAwaitingVerificationEvent += purchaseCompleteAwaitingVerificationEvent;
		GoogleIABManager.purchaseSucceededEvent -= purchaseSucceededEvent;
		GoogleIABManager.purchaseFailedEvent -= purchaseFailedEvent;
		GoogleIABManager.consumePurchaseSucceededEvent -= consumePurchaseSucceededEvent;
		GoogleIABManager.consumePurchaseFailedEvent -= consumePurchaseFailedEvent;
		
		
		#endif
	}


	#region Chartboost

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
	
	#if UNITY_IPHONE
	void didCompleteAppStoreSheetFlow ()
	{
		Debug.Log ("didCompleteAppStoreSheetFlow");
	}
	#endif
	#endregion


	IEnumerator IncrementCoins (int myCoins, int amountIncreased)
	{
		int incrementDelta = (int)amountIncreased / 20;
		int finalCoins = myCoins + amountIncreased;
		float timer = 0.0f;
		UILabel myCoinsLabel = GameObject.FindObjectOfType<Purchases> ().myCoinsLabel;
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
	#if UNITY_IPHONE
	
	void transactionUpdatedEvent (StoreKitTransaction transaction)
	{
		Debug.Log ("transactionUpdatedEvent: " + transaction);
	}

	
	void productListReceivedEvent (List<StoreKitProduct> productList)
	{
		Debug.Log ("productListReceivedEvent. total products received: " + productList.Count);
		
		// print the products to the console
		foreach (StoreKitProduct product in productList)
			Debug.Log (product.ToString () + "\n");
	}

	
	void productListRequestFailedEvent (string error)
	{
		Debug.Log ("productListRequestFailedEvent: " + error);
	}

	
	void purchaseFailedEvent (string error)
	{
		if (uiManager == null) {
			uiManager = GameObject.Find ("UI Root").GetComponent<RoomUIManager> ();
		}
		Debug.Log ("purchaseFailedEvent: " + error);
		uiManager.ActivatePopup ("Purchase Failed", false);
		uiManager.loaderPanel.SetActive (false);

	}

		
	void purchaseCancelledEvent (string error)
	{
		if (uiManager == null) {
			uiManager = GameObject.Find ("UI Root").GetComponent<RoomUIManager> ();
		}
		Debug.Log ("purchaseCancelledEvent: " + error);
		Time.timeScale = 1;
		uiManager.ActivatePopup ("Purchase Cancelled", false);
		uiManager.loaderPanel.SetActive (false);

	}

	
	
	void productPurchaseAwaitingConfirmationEvent (StoreKitTransaction transaction)
	{
		Debug.Log ("productPurchaseAwaitingConfirmationEvent: " + transaction);
	}

	
	void purchaseSuccessfulEvent (StoreKitTransaction transaction)
	{
		Debug.Log ("purchaseSuccessfulEvent: " + transaction);
		
		if (transaction.productIdentifier == "IAP_ID.200coins") {
			StartCoroutine (IncrementCoins (PlayerPrefs.GetInt ("MyCoins"), 200));
			PlayerPrefs.SetInt ("MyCoins", (PlayerPrefs.GetInt ("MyCoins") + 200));
		} else if (transaction.productIdentifier == "IAP_ID.500coins") {
			StartCoroutine (IncrementCoins (PlayerPrefs.GetInt ("MyCoins"), 500));
			PlayerPrefs.SetInt ("MyCoins", (PlayerPrefs.GetInt ("MyCoins") + 500));
			
		} else if (transaction.productIdentifier == "IAP_ID.1000coins") {
			StartCoroutine (IncrementCoins (PlayerPrefs.GetInt ("MyCoins"), 1000));
			PlayerPrefs.SetInt ("MyCoins", (PlayerPrefs.GetInt ("MyCoins") + 1000));

		}
		if (uiManager == null) {
			uiManager = GameObject.Find ("UI Root").GetComponent<RoomUIManager> ();
		}
		uiManager.ActivatePopup ("Purchase Successful", false);
		uiManager.loaderPanel.SetActive (false);
	}




	void restoreTransactionsFailedEvent (string error)
	{
		Debug.Log ("restoreTransactionsFailedEvent: " + error);
		

	}

	
	void restoreTransactionsFinishedEvent ()
	{
		Debug.Log ("restoreTransactionsFinished");
		
	}

	
	void paymentQueueUpdatedDownloadsEvent (List<StoreKitDownload> downloads)
	{
		Debug.Log ("paymentQueueUpdatedDownloadsEvent: ");
		foreach (var dl in downloads)
			Debug.Log (dl);
	}
	

	#elif UNITY_ANDROID
	
	void billingSupportedEvent ()
	{
		Debug.Log ("billingSupportedEvent");
	}

	
	void billingNotSupportedEvent (string error)
	{
		Debug.Log ("billingNotSupportedEvent: " + error);
	}

	
	void queryInventorySucceededEvent (List<GooglePurchase> purchases, List<GoogleSkuInfo> skus)
	{
		Debug.Log (string.Format ("queryInventorySucceededEvent. total purchases: {0}, total skus: {1}", purchases.Count, skus.Count));
		Prime31.Utils.logObject (purchases);
		Prime31.Utils.logObject (skus);
		
		for (int i = 0; i < purchases.Count; i++) {
			if (purchases.toJson ().Contains ("IAP_ID.200coins")) {
				GoogleIAB.consumeProduct ("IAP_ID.200coins");
			}

			if (purchases.toJson ().Contains ("IAP_ID.500coins")) {
				GoogleIAB.consumeProduct ("IAP_ID.500coins");
			}
			if (purchases.toJson ().Contains ("IAP_ID.1000coins")) {
				GoogleIAB.consumeProduct ("IAP_ID.1000coins");
				
			}
			
		}
		
	}

	
	void queryInventoryFailedEvent (string error)
	{
		Debug.Log ("queryInventoryFailedEvent: " + error);
	}

	
	void purchaseCompleteAwaitingVerificationEvent (string purchaseData, string signature)
	{
		Debug.Log ("purchaseCompleteAwaitingVerificationEvent. purchaseData: " + purchaseData + ", signature: " + signature);
	}

	
	void purchaseSucceededEvent (GooglePurchase purchase)
	{
		Debug.Log ("purchaseSucceededEvent: " + purchase);
		
		if (purchase.productId == "IAP_ID.200coins") {
			StartCoroutine (IncrementCoins (PlayerPrefs.GetInt ("MyCoins"), 200));
			PlayerPrefs.SetInt ("MyCoins", (PlayerPrefs.GetInt ("MyCoins") + 200));
			
		} else if (purchase.productId == "IAP_ID.500coins") {
			StartCoroutine (IncrementCoins (PlayerPrefs.GetInt ("MyCoins"), 500));
			PlayerPrefs.SetInt ("MyCoins", (PlayerPrefs.GetInt ("MyCoins") + 500));
			
		} else if (purchase.productId == "IAP_ID.1000coins") {
			StartCoroutine (IncrementCoins (PlayerPrefs.GetInt ("MyCoins"), 1000));
			PlayerPrefs.SetInt ("MyCoins", (PlayerPrefs.GetInt ("MyCoins") + 1000));
			
		}

		Debug.Log ("product id = " + purchase.productId);
		GoogleIAB.consumeProduct (purchase.productId);
		if (uiManager == null) {
			uiManager = GameObject.Find ("UI Root").GetComponent<RoomUIManager> ();
		}
		uiManager.ActivatePopup ("Purchase Successful", false);
		uiManager.loaderPanel.SetActive (false);

	}

	
	void purchaseFailedEvent (string error)
	{
		Debug.Log ("purchaseFailedEvent: " + error);
		if (uiManager == null) {
			uiManager = GameObject.Find ("UI Root").GetComponent<RoomUIManager> ();
		}
		Time.timeScale = 1;
		uiManager.ActivatePopup ("Purchase Cancelled", false);
		uiManager.loaderPanel.SetActive (false);
		
		
	}

	
	void consumePurchaseSucceededEvent (GooglePurchase purchase)
	{
		Debug.Log ("consumePurchaseSucceededEvent: " + purchase);
		
	}

	
	void consumePurchaseFailedEvent (string error)
	{
		Debug.Log ("consumePurchaseFailedEvent: " + error);

		
	}
	
	
	#endif
	
	

	#region FB.Init()

	private void CallFBInit ()
	{
		Facebook.Unity.FB.Init (OnInitComplete, OnHideUnity);
	}

	private void OnInitComplete ()
	{
		Debug.Log ("FB.Init completed: Is user logged in? " + Facebook.Unity.FB.IsLoggedIn);
	}

	private void OnHideUnity (bool isGameShown)
	{
		Debug.Log ("Is game showing? " + isGameShown);
	}

	#endregion

	// Update is called once per frame
	void Update ()
	{
	
	}
}
