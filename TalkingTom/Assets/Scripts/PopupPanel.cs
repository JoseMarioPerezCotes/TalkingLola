using UnityEngine;
using System.Collections;

public class PopupPanel : MonoBehaviour {
	public UILabel poupLabel;
	public GameObject okButton, cancelButton , okCancelParent, individualOk;
	public static bool popupPanelActive;
	// Use this for initialization
	void Start () {
	
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

	void OnEnable()
	{
		popupPanelActive = true;
	}

	void OnDisable()
	{
		popupPanelActive = false;
	}

	public void Cancel()
	{

		gameObject.SetActive (false);
	}


}
