using UnityEngine;
using System.Collections;

public class InstructionsPanel : MonoBehaviour {
	public static bool instructionPanelPresent;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		#if UNITY_ANDROID
		if(Input.GetKeyDown (KeyCode.Escape))
		{
			Cross ();
		}
		#endif
	}

	void OnEnable()
	{
		instructionPanelPresent = true;
	}

	void OnDisable()
	{
		instructionPanelPresent = false;
	}

	public void Cross()
	{
		gameObject.SetActive (false);
	}

	public void OpenImageUrl()
	{
		Application.OpenURL("http://www.bestbuddygames.com/wp-content/uploads/2015/05/bestbuddy.pdf");
	}
}
