using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;

public class PushNotification : MonoBehaviour
{
	#if UNITY_IPHONE && !UNITY_EDITOR
	[DllImport ("__Internal")]
	private static extern void _AddNotification();
	
	public static void AddNotification()
	{
		_AddNotification();
	}
#endif


	#if UNITY_ANDROID
	
//	public ELANNotification notification;
	//Actions


//	int LastNotificationId = 0;
//	private void Toast() {
//		AndroidToast.ShowToastNotification ("Hello Toast", AndroidToast.LENGTH_LONG);
//	}
//	
//	private void Local() {
//		Debug.Log("notification");
////		LastNotificationId = AndroidNotificationManager.instance.ScheduleLocalNotification("Best Buddies", "You've generated more coins! Come back and play!", 21600*4);
//		LastNotificationId = AndroidNotificationManager.instance.ScheduleLocalNotification("Best Buddies", "You've generated more coins! Come back and play!", 60);
//		
//		
//	}
//	
//	private void LoadLaunchNotification (){
//		Debug.Log("load launch notification");
//		AndroidNotificationManager.instance.OnNotificationIdLoaded += OnNotificationIdLoaded;
//		AndroidNotificationManager.instance.LocadAppLaunchNotificationId();
//	}
//	
//	private void CancelLocal() {
//		AndroidNotificationManager.instance.CancelLocalNotification(LastNotificationId);
//	}
//	
//	private void CancelAll() {
//		AndroidNotificationManager.instance.CancelAllLocalNotifications();
//	}
//	
//
//	
//	
//	private void LocalNitificationsListExample() {
//		//		List<LocalNotificationTemplate> PendingNotofications;
//		//	PendingNotofications = AndroidNotificationManager.instance.LoadPendingNotifications();
//	}
//	
//
//	
//	
//
//	
//	private void OnNotificationIdLoaded (int notificationid){
//		Debug.Log("notificationid = "+notificationid);
//		if(notificationid > -1)
//		{
//			AndroidNative.showMessage ("Loaded", "You received 50 coins!");
//			//Add Coins here
//			PlayerPrefs.SetInt ("MyCoins" ,(PlayerPrefs.GetInt ("MyCoins")+50));
//		}
//	}


#endif
	// Use this for initialization
	void Start ()
	{

#if UNITY_IPHONE && !UNITY_EDITOR
		AddNotification ();
		Invoke("RegisterForNotif",5.0f);
#elif UNITY_ANDROID
//		LoadLaunchNotification();
#endif
	}

	#if UNITY_IPHONE
	void RegisterForNotif ()
	{

//		UnityEngine.iOS.NotificationServices.RegisterForLocalNotificationTypes (LocalNotificationType.Alert | LocalNotificationType.Badge | LocalNotificationType.Sound);

	}

	void ScheduleNotification ()
	{
		// schedule notification to be delivered in 24 hours
		UnityEngine.iOS.LocalNotification notif = new UnityEngine.iOS.LocalNotification ();
		notif.fireDate = DateTime.Now.AddHours (24);
		notif.alertBody = "You've generated more coins! Come back and play!";
		UnityEngine.iOS.NotificationServices.ScheduleLocalNotification (notif);
	}

	void OnGUI ()
	{
		if (notifReceived)
			GUI.Label (new Rect (100, 200, 100, 100), "notification received = " + notifReceived);
	}


	void OnPushReceived (string text)
	{
		Debug.Log ("Add Coins");
		notifReceived = true;
		PlayerPrefs.SetInt ("MyCoins", (PlayerPrefs.GetInt ("MyCoins") + 20));
		//UnitySendMessage("Plugins", "OnPushReceived", "Message to send");
	}


	bool notifReceived;

	void OnPushReceivedPopup (string text)
	{
		Debug.Log ("Add Coins");
		notifReceived = true;
		//UnitySendMessage("Plugins", "OnPushReceived", "Message to send");
		PlayerPrefs.SetInt ("MyCoins", (PlayerPrefs.GetInt ("MyCoins") + 20));
		if (GameObject.Find ("UI Root").GetComponent<MainScene> ()) {
			GameObject.Find ("UI Root").GetComponent<MainScene> ().ActivatePopup ("Congratulations \n You received 20 coins!", false);
		} else {
			GameObject.Find ("UI Root").GetComponent<RoomUIManager> ().ActivatePopup ("Congratulations \n You received 20 coins!", false);
		}
	}

	#endif

	int _fiveSecondNotificationId;

	void OnApplicationPause (bool isPause)
	{

		if (isPause) {
			// cancel all notifications first.
#if UNITY_IPHONE
			UnityEngine.iOS.NotificationServices.ClearLocalNotifications ();
			UnityEngine.iOS.NotificationServices.CancelAllLocalNotifications ();		
			ScheduleNotification ();
#elif UNITY_ANDROID
			_fiveSecondNotificationId = EtceteraAndroid.scheduleNotification( 259200, "Talking Best friends", "Your Pet Friends are waiting!Come back for more fun!", "Come back for more fun!", "five-second-note" );
			#endif 
			
		} else {
#if UNITY_IPHONE
			// clear all notifications.
			if (UnityEngine.iOS.NotificationServices.localNotificationCount > 0) { 
				Debug.Log ("MYLOG : Local notification count = " + UnityEngine.iOS.NotificationServices.localNotificationCount); 				
				Debug.Log (UnityEngine.iOS.NotificationServices.localNotifications [0].alertBody);
			}
			
			UnityEngine.iOS.NotificationServices.ClearLocalNotifications ();
			UnityEngine.iOS.NotificationServices.CancelAllLocalNotifications ();
#elif UNITY_ANDROID
			Debug.Log("cancel local notification");
			EtceteraAndroid.cancelNotification( _fiveSecondNotificationId );
			EtceteraAndroid.cancelAllNotifications();
			//			CancelAll();
#endif
		}
	}
	







}
